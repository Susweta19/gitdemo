using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TxSelenium;
using TxSelenium.Loggers;
using TxSelenium.Utils;
using TxWebTests.Config;
using TxWebTests.HeaderUtils;
using TxWebTests.Interfaces;
using TxWebTests.Loggers;
using TxWebTests.Spreadsheet;
using XmlExecutor;
using XmlExecutor.DataTypes;
using XmlExecutor.Factories;

namespace TxWebTests
{
    public class ParsedTests
    {
        private Configuration _config;
        private AttributeUtils _attributeUtils;
        private ICollection<IWebTestsLogger> _loggers;

        public ParsedTests(AttributeUtils attributeUtils, Configuration config)
        {
            _config = config;
            _attributeUtils = attributeUtils;
            if (_config.VerboseMode)
            {
                ILogger consoleLogger = new TxWConsoleLogger(TxSelenium.Loggers.LogLevel.TRACE);
                LogsService.AddLogger(consoleLogger);
            }
        }

        public void RunTests()
        {
            ActionsParser parser = new ActionsParser(_attributeUtils, typeof(TestManager), _config.StringsToReplace);
            ICollection<ActionsExecutor> executors;
            using (StreamWriter fileWriter = new StreamWriter(_config.FileParseErrorPath))
            {
                ParseErrorLogger logParseError = delegate (string fileName, Exception e)
                {
                    fileWriter.Write(fileName + " " + e.Message + "\r\n");
                    _config.Spreadsheet?.CountError(fileName);
                    _config.XmlLogger?.CountError(fileName);
                };

                executors = parser.ParseFolder(_config.TestsFolderPath, logParseError);
            }
            using (StreamWriter failedWriter = new StreamWriter(_config.FileFailedPath))
            using (StreamWriter successWriter = new StreamWriter(_config.FileSuccessPath))
            {
                try
                {
                    Stopwatch executionTime = new Stopwatch();
                    TimeSpan executionSpan = TimeSpan.Zero;
                    int executorIndex = 1;
                    foreach (ActionsExecutor executor in executors)
                    {
                        bool passed = false;
                        int retryCount = 0;
                        while (! passed && (executor.FailedAction == null || executor.FailedAction == "") && retryCount < 6)
                        {
                            //The executor failed before being able to do anything it can be restarted safely
                            //High retry count since it's almost certain that the problem is on the webdriver side
                            executionTime.Start();
                            passed = RunTest(executor);
                            executionTime.Stop();
                            executionSpan = executionTime.Elapsed;
                            executionTime.Reset();
                            retryCount++;
                        }

                        if (!passed && (executor.FailedAction == null || executor.FailedAction == ""))
                        {
                            Console.WriteLine("Failed to start the test : " + executor.Name);
                        }

                        int firstLoginRetryCount = 0;
                        //With the new dockerized TEEXMA it might be necessary to wait for TxUpgrade to finish
                        while (!passed && (executor.FailedAction == "Login") && executorIndex == 1 && firstLoginRetryCount < 5)
                        {
                            //The executor failed before being able to log in it can be restarted safely
                            //Limited restart count for that case since it's more often a server side problem
                            Console.WriteLine("Failed on first login retrying ...");
                            firstLoginRetryCount++;
                            Thread.Sleep(firstLoginRetryCount * 10000);
                            executionTime.Start();
                            passed = RunTest(executor);
                            executionTime.Stop();
                            executionSpan = executionTime.Elapsed;
                            executionTime.Reset();
                        }

                        if (! passed && (executor.FailedAction == "Login") && executorIndex == 1)
                        {
                            File.WriteAllText("/src/logs/LoginFailed.txt", "login failed");
                            //Failed to access TEEXMA after multiple retries it must be because
                            //of an installation problem it's no use to execute the remaining tests.
                            throw new Exception("TEEXMA login failure suspected faulty installation.");
                        }

                        //Any test can have a problem with the login step if TEEXMA takes too much time to answer
                        if ( ! passed && (executor.FailedAction == "Login" || executor.FailedAction == "SelectOT"))
                        {
                            //The executor failed before being able to log in it can be restarted safely
                            //Limited restart count for that case since it's more often a server side problem
                            Console.WriteLine("Failed on login retrying ...");
                            Thread.Sleep(10000);
                            executionTime.Start();
                            passed = RunTest(executor);
                            executionTime.Stop();
                            executionSpan = executionTime.Elapsed;
                            executionTime.Reset();
                        }
                        Header header = null;
                        IList headers = null;
                        TestData currentTestData = null;
                        if (executor.ExtractedData.TryGetValue(TestManager.headerExtractName, out headers))
                        {
                            header = (Header)headers[0];
                            currentTestData = header.TestData;
                        }

                        if (_config.Spreadsheet != null)
                            if (executorIndex == 1 && _config.Spreadsheet.Config.PrepareSpreadSheet)
                                _config.Spreadsheet.WriteBrowserDetails(_config.browserName, _config.browserVersion,_config.webDriverVersion);

                        if (_config.XmlLogger != null && executorIndex == 1)
                        {
                            _config.XmlLogger.WriteBrowserDetails(_config.browserName, _config.browserVersion, _config.webDriverVersion);
                        }

                        executorIndex++;
                        if (passed)
                        {
                            successWriter.Write(executor.Name + "\r\n");
                            //_config.Spreadsheet?.ReportScenarioNameAndStatus(SpreadsheetManager.ScenarioStatus.Success, executor.Name, currentTestData);
                            _config.XmlLogger?.ReportScenarioNameAndStatus("Passed", executor.Name, currentTestData, executionSpan);

                        }
                        else
                        {
                            string runningIssue = "";
                            string bugRef = "";
                            if (currentTestData != null)
                            {
                                //Todo - To remove 
                                runningIssue = !string.IsNullOrEmpty(currentTestData.TestRunningIssue) && _config.DetailsMode ? "Running Issue : " + currentTestData.TestRunningIssue : "";
                                bugRef = currentTestData.TestBugRef != 0 && _config.DetailsMode ? "Bug Reference : " + currentTestData.TestBugRef : "";
                                _config.XmlLogger?.ReportScenarioNameAndStatus("Failed", executor.Name, currentTestData, executionSpan);
                            }
                            else
                            {
                                _config.Spreadsheet?.ReportExecutionError(executor.Name);
                                _config.XmlLogger?.ReportExecutionError(executor.Name);
                            }

                            failedWriter.Write($"{executor.Name + " failed on : " + executor.FailedAction,-45} {bugRef,-30} {runningIssue,-60}" + "\r\n");

                        }
                        Console.WriteLine("Test " + executor.Name + " passed : " + passed.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    _config.XmlLogger?.WriteResults();
                }
            }
        }

        private bool RunTest(ActionsExecutor executor)
        {
            try
            {
                using (TxLogger txLogger = new TxLogger(executor.Name + ".log"))
                using (TxWebDriver driver = new TxWebDriver(_config.StartBrowser(), _config.ServerURL, _config.Language))
                {
                    Console.WriteLine("Test name : " + executor.Name);

                    _loggers = new List<IWebTestsLogger> { txLogger };

                    if (_config.Spreadsheet != null)
                        _loggers.Add(new GoogleSheetLogger(_config.Spreadsheet));

                    if (_config.XmlLogger != null)
                        _loggers.Add(_config.XmlLogger);

                    if (_config.VerboseMode ==  true)
                        _loggers.Add(new ConsoleLogger());

                    TestManager testManager = new TestManager(driver);
                    executor.BeforeExec += LogBefore;
                    executor.AfterExec += LogAfter;
                    executor.ExecError += LogError;
                    bool passed = executor.Execute(testManager);
                    if ((!passed) && _config.ScreenshotOnError)
                    {
                        try
                        {
                            Screenshot screenshot = driver.TakeScreenShot();
                            screenshot.SaveAsFile(executor.Name + "ErrorScreenShot.png", ScreenshotImageFormat.Png);
                            Console.WriteLine("Screenshot Taken");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("There is some while taking screenshot.");
                            Console.WriteLine("Error : " + ex.ToString());
                            Console.WriteLine("Inner Exception : " + ex.InnerException);
                        }
                        _config.GoogleDrive?.UploadRequest(executor.Name + "ErrorScreenShot.png");
                    }
                    if (!passed)
                    {
                        //var logs = driver.Driver.Manage().Logs.GetLog(LogType.Browser);
                        //Console.WriteLine("\n**********Browser Console Start**********\n");
                        //foreach (var log in logs)
                        //{
                        //    Console.WriteLine(log.Timestamp + "-->" + log.Level + "--->" + log.Message + "\n");
                        //}
                        //Console.WriteLine("\n**********Browser Console End**********\n");

                    }
                    return passed;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            } finally
            {
                if (LinuxUtils.IsLinux)
                {
                    //Killing every process when in the CI case to avoid failures due to 
                    Process[] chromeProcesses = Process.GetProcessesByName("chrome");
                    Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
                    foreach (Process process in chromeProcesses)
                    {
                        Console.WriteLine("Killing a chrome process");
                        if ( ! process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit();
                            Console.WriteLine("Killed a chrome process HasExited : " + process.HasExited);
                            process.Dispose();
                        }
                    }

                    foreach (Process process in chromeDriverProcesses)
                    {
                        Console.WriteLine("Killing a chromedriver process");
                        if ( ! process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit();
                        }
                        Console.WriteLine("Killed a chromedriver process HasExited : " + process.HasExited);
                    }
                }
            }
            return false;
        }

        private void LogBefore(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            foreach (IWebTestsLogger logger in _loggers)
                logger.LogBefore(instance, callingTree, signature);
        }

        private void LogAfter(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            foreach (IWebTestsLogger logger in _loggers)
                logger.LogAfter(instance, callingTree, signature);
        }

        private void LogError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e)
        {
            foreach (IWebTestsLogger logger in _loggers)
                logger.LogError(instance, callingTree, signature, e);
        }
    }
}
