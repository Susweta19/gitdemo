using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using TxWebTests.Config;
using TxWebTests.HeaderUtils;
using TxWebTests.Spreadsheet;
using static TxWebTests.Spreadsheet.SpreadsheetManager;

namespace GSheetWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isAdminTest = true;
            bool isTagVersion = false;
            string releaseNumber = "";
            string tagName = "";
            string jobNumber = "";
            string jobDuration = "";
            bool prepareSpreadSheet = true;

            string folderPath = args[0];

            string[] testsFolders = Directory.GetDirectories(folderPath);

            XmlDocument configDoc = new XmlDocument();
            string configPath = Path.Combine(folderPath, "gSheetConfig.xml");
            configDoc.Load(configPath);
            bool screenshotOnError = configDoc.SelectSingleNode("//ScreenshotOnError|//screenshotOnError").InnerText.ToLower() == "true";
            string spreadSheetId = configDoc.SelectSingleNode("//SpreadSheetId|//spreadSheetId").InnerText;
            string driveFolderId = configDoc.SelectSingleNode("//DriveFolderId|//driveFolderId").InnerText;

            if (configDoc.SelectSingleNode("//PrepareSpreadSheet|//prepareSpreadSheet") != null)
                prepareSpreadSheet = configDoc.SelectSingleNode("//PrepareSpreadSheet|//prepareSpreadSheet").InnerText.ToLower() == "true";

            if (configDoc.SelectSingleNode("//IsAdminTest|//isAdminTest") != null)
                isAdminTest = configDoc.SelectSingleNode("//IsAdminTest|//isAdminTest").InnerText.ToLower() == "true";

            if (configDoc.SelectSingleNode("//IsTagVersion|//isTagVersion") != null)
                isTagVersion = configDoc.SelectSingleNode("//IsTagVersion|//isTagVersion").InnerText.ToLower() == "true";

            if (configDoc.SelectSingleNode("//ReleaseNumber|//releaseNumber") != null)
                releaseNumber = configDoc.SelectSingleNode("//ReleaseNumber|//releaseNumber").InnerText;

            if (configDoc.SelectSingleNode("//TagName|//tagName") != null)
                tagName = configDoc.SelectSingleNode("//TagName|//tagName").InnerText;

            if (configDoc.SelectSingleNode("//JobNumber|//jobNumber") != null)
                jobNumber = configDoc.SelectSingleNode("//JobNumber|//jobNumber").InnerText;

            if (configDoc.SelectSingleNode("//JobDuration|//jobDuration") != null)
                jobDuration = configDoc.SelectSingleNode("//JobDuration|//jobDuration").InnerText;

            string lastDatabaseName = testsFolders[testsFolders.Length - 1];
            bool wroteBrowserDetails = false;
            foreach (var testsFolder in testsFolders)
            {
                string browserDetailsPath = Path.Combine(testsFolder, "BrowserDetails.xml");

                string dbName = Path.GetFileName(testsFolder);
                SpreadsheetConfig spreadsheetConfig = new SpreadsheetConfig(spreadSheetId, prepareSpreadSheet, driveFolderId);
                SpreadsheetManager spreadsheetManager = new SpreadsheetManager(spreadsheetConfig, dbName, releaseNumber, isAdminTest, isTagVersion, tagName, lastDatabaseName, jobNumber, jobDuration);

                GoogleDriveManager googleDrive = null;
                if (screenshotOnError && spreadsheetConfig.DriveFolderId != null)
                {
                    googleDrive = new GoogleDriveManager(spreadsheetManager);
                }

                string[] testFiles = Directory.GetFiles(testsFolder, "*.xml");
                foreach (var testFilePath in testFiles)
                {
                    string fileName = Path.GetFileName(testFilePath);
                    if (fileName != "BrowserDetails.xml" && fileName != "TotalTime.xml" && fileName != "ParseFailures.xml")
                    {
                        try
                        {
                            uploadTestResults(testFilePath, spreadsheetManager);
                            string errorScreenshotPath = testsFolder + "\\" + fileName.Split('.')[0] + "ErrorScreenShot.png";
                            if (googleDrive!=null && File.Exists(errorScreenshotPath))
                            {
                                //need to change THalder
                                googleDrive.UploadRequest(errorScreenshotPath);
                            }
                        } catch
                        {
                            Console.WriteLine($"Failed to upload results for file {fileName}");
                        }
                    }
                }

                string totalTimePath = Path.Combine(testsFolder, "TotalTime.xml");
                if (File.Exists(totalTimePath))
                {
                    XmlDocument totalTimeDoc = new XmlDocument();
                    totalTimeDoc.Load(totalTimePath);
                    string totalTimeString = totalTimeDoc.SelectSingleNode("//TotalTime").InnerText;
                    long totalTime = long.Parse(totalTimeString);
                    spreadsheetManager.DataBaseResults(totalTime);
                    spreadsheetManager.WriteOverviewValue(totalTime);
                }
                Thread.Sleep(5000);

                if (!wroteBrowserDetails && File.Exists(browserDetailsPath))
                {

                    XmlDocument browserDetails = new XmlDocument();
                    browserDetails.Load(browserDetailsPath);
                    string browserName = browserDetails.SelectSingleNode("//BrowserName").InnerText;
                    string version = browserDetails.SelectSingleNode("//Version").InnerText;
                    string webDriverVersion = browserDetails.SelectSingleNode("//WebDriverVersion").InnerText;
                    spreadsheetManager.WriteBrowserDetails(browserName, version, webDriverVersion);
                    wroteBrowserDetails = true;
                    prepareSpreadSheet = false;
                }
            }
        }

        static void uploadTestResults(string testFilePath, SpreadsheetManager spreadsheetManager)
        {
            XmlDocument testResults = new XmlDocument();
            testResults.Load(testFilePath);

            string name = testResults.SelectSingleNode("//Name").InnerText;
            string statusString = testResults.SelectSingleNode("//Status").InnerText;
            ScenarioStatus status;
            switch (statusString)
            {
                case "Passed":
                    status = ScenarioStatus.Success;
                    break;
                case "Failed":
                    status = ScenarioStatus.Failed;
                    break;
                case "Error":
                    status = ScenarioStatus.Error;
                    break;
                default:
                    throw new Exception("failed to parse status");
            }
            if (status == ScenarioStatus.Error)
            {
                spreadsheetManager.ReportExecutionError(name);
            }
            else
            {
                string bugRefString = testResults.SelectSingleNode("//BugRef").InnerText;
                int bugRef = int.Parse(bugRefString);
                string description = testResults.SelectSingleNode("//Description").InnerText;
                string runningIssue = testResults.SelectSingleNode("//RunningIssue").InnerText;

                XmlNodeList dependencyNodes = testResults.SelectNodes("//Dependency");
                ICollection<string> dependencies = new List<String>();
                foreach (XmlNode dependencyNode in dependencyNodes)
                {
                    dependencies.Add(dependencyNode.InnerText);
                }

                string actions = testResults.SelectSingleNode("//Actions").InnerText;
                if (testResults.SelectSingleNode("//Exception") != null)
                {
                    string exception = testResults.SelectSingleNode("//Exception").InnerText;
                    spreadsheetManager.ReportException(exception);
                }

                //If there is no time node time is -1
                string timeString = testResults.SelectSingleNode("//Time")?.InnerText;
                if (string.IsNullOrEmpty(timeString)) timeString = "-1";
                int time = int.Parse(timeString);
                TestData testData = new TestData(0, dependencies, description, runningIssue, bugRef);
                spreadsheetManager.ReportScenarioNameAndStatus(status, name, testData, actions, time);
            }
        }
    }
}
