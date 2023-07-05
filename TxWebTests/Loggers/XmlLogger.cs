using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TxWebTests.HeaderUtils;
using TxWebTests.Interfaces;
using TxWebTests.Spreadsheet;
using XmlExecutor.DataTypes;
using static TxWebTests.Spreadsheet.SpreadsheetManager;

namespace TxWebTests.Loggers
{
    public class XmlLogger : IWebTestsLogger
    {

        private readonly string _folderName;
        private Stopwatch _scriptsTimer;
        private XmlDocument _currentScenarioXmlDocument;
        private XmlNode _currentScenarioNode;
        private Stopwatch _watch;
        private ScenarioAction _fullScenarioAction;
        private Exception _exception;
        private int _parsedCounter;
        //Invalid scripts concatenated in this field 
        private readonly ICollection<string> _invalidFiles = new List<string>();
        //For OnDemanScreenshot 
        private List<string> _screenshotsOnDemand = new List<string>();

        //For TxPerformance
        private Dictionary<string, List<double>> _funcTimer = new Dictionary<string, List<double>>();

        public XmlLogger(string folderName)
        {
            _currentScenarioXmlDocument = new XmlDocument();
            _currentScenarioNode = _currentScenarioXmlDocument.CreateElement("ScenarioReport");
            _folderName = folderName;
            _scriptsTimer = Stopwatch.StartNew();
        }
        public void LogAfter(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            _watch.Stop();
            double seconds = TimeSpan.FromMilliseconds(_watch.ElapsedMilliseconds).TotalSeconds;

            if (_funcTimer.ContainsKey(signature.Name))
            {
                _funcTimer[signature.Name].Add(seconds);
            }
            else
            {
                _funcTimer[signature.Name] = new List<double>() { seconds };
            }
        }

        public void LogBefore(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            _watch = Stopwatch.StartNew();
            ReportScenarioAction(callingTree, signature);
        }

        public void LogError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception exception)
        {
            _exception = exception;
        }

        public void ReportScenarioNameAndStatus(string status, string name, TestData testData, TimeSpan testTime)
        {
            XmlNode testDataNode = _currentScenarioXmlDocument.CreateElement("TestData");

            XmlNode bugRefNode = _currentScenarioXmlDocument.CreateElement("BugRef");
            bugRefNode.InnerText = testData.TestBugRef.ToString();
            testDataNode.AppendChild(bugRefNode);

            XmlNode testDescriptionNode = _currentScenarioXmlDocument.CreateElement("Description");
            testDescriptionNode.InnerText = testData.TestDescription.ToString();
            testDataNode.AppendChild(testDescriptionNode);

            XmlNode testRunningIssueNode = _currentScenarioXmlDocument.CreateElement("RunningIssue");
            testRunningIssueNode.InnerText = testData.TestRunningIssue != null ? testData.TestRunningIssue.ToString() : "";
            testDataNode.AppendChild(testRunningIssueNode);

            XmlNode testDependenciesNode = _currentScenarioXmlDocument.CreateElement("Dependencies");
            if (testData.TestDependencies.Count > 0)
            {
                foreach (var testDependency in testData.TestDependencies)
                    if (testDependency != "testDependencies")
                    {
                        XmlNode testDependencyNode = _currentScenarioXmlDocument.CreateElement("Dependency");
                        testDependencyNode.InnerText = testDependency;
                        testDependenciesNode.AppendChild(testDependencyNode);
                    }
            }
            testDataNode.AppendChild(testDependenciesNode);

            _currentScenarioNode.AppendChild(testDataNode);

            XmlNode statusNode = _currentScenarioXmlDocument.CreateElement("Status");
            statusNode.InnerText = status;
            _currentScenarioNode.AppendChild(statusNode);

            XmlNode nameNode = _currentScenarioXmlDocument.CreateElement("Name");
            nameNode.InnerText = name;
            _currentScenarioNode.AppendChild(nameNode);

            XmlNode timeNode = _currentScenarioXmlDocument.CreateElement("Time");
            timeNode.InnerText = testTime.Seconds.ToString();
            _currentScenarioNode.AppendChild(timeNode);

            XmlNode actionsNode = _currentScenarioXmlDocument.CreateElement("Actions");
            actionsNode.InnerText = _fullScenarioAction.ToString();
            _currentScenarioNode.AppendChild(actionsNode);

            if (_exception != null)
            {
                XmlNode exceptionNode = _currentScenarioXmlDocument.CreateElement("Exception");
                exceptionNode.InnerText = _exception.ToString();
                _currentScenarioNode.AppendChild(exceptionNode);

            }

            _currentScenarioXmlDocument.AppendChild(_currentScenarioNode);
            _currentScenarioXmlDocument.Save($"{name}.xml");
            Reset();
        }

        public void WriteBrowserDetails(string browserName, String version, string webDriverVersion)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode browserDetailsNode = xmlDocument.CreateElement("BrowserDetails");
            XmlNode browserNameNode = xmlDocument.CreateElement("BrowserName");
            browserNameNode.InnerText = browserName;
            browserDetailsNode.AppendChild(browserNameNode);
            XmlNode versionNode = xmlDocument.CreateElement("Version");
            versionNode.InnerText = version;
            browserDetailsNode.AppendChild(versionNode);
            XmlNode webDriverVersionNode = xmlDocument.CreateElement("WebDriverVersion");
            webDriverVersionNode.InnerText = webDriverVersion;
            browserDetailsNode.AppendChild(webDriverVersionNode);
            xmlDocument.AppendChild(browserDetailsNode);
            xmlDocument.Save("BrowserDetails.xml");
        }

        internal void ReportExecutionError(string name)
        {
            XmlNode nameNode = _currentScenarioXmlDocument.CreateElement("Name");
            nameNode.InnerText = name;
            _currentScenarioNode.PrependChild(nameNode);

            XmlNode statusNode = _currentScenarioXmlDocument.CreateElement("Status");
            statusNode.InnerText = "Error";
            _currentScenarioNode.PrependChild(statusNode);
            _currentScenarioXmlDocument.AppendChild(_currentScenarioNode);
            _currentScenarioXmlDocument.Save($"{_folderName}.xml");
            Reset();
        }

        internal void WriteResults()
        {
            if (_invalidFiles.Count > 0)
            {
                XmlDocument _parseErrorsXmlDocument = new XmlDocument();
                var invalidFilesNode = _parseErrorsXmlDocument.CreateElement("InvalidFiles");
                _parseErrorsXmlDocument.AppendChild(invalidFilesNode);
                foreach (var invalidFile in _invalidFiles)
                {
                    var invalidFileNode = _parseErrorsXmlDocument.CreateElement("InvalidFile");
                    invalidFileNode.InnerText = invalidFile;
                    invalidFilesNode.AppendChild(invalidFileNode);
                }
                _parseErrorsXmlDocument.Save("ParseFailures.xml");
            }
            XmlDocument totalTimeDocument = new XmlDocument();
            var totalTimeNode = totalTimeDocument.CreateElement("TotalTime");
            _scriptsTimer.Stop();
            totalTimeNode.InnerText = _scriptsTimer.ElapsedMilliseconds.ToString();
            totalTimeDocument.AppendChild(totalTimeNode);
            totalTimeDocument.Save("TotalTime.xml");
        }

        /// <summary>
        /// Increase the parse counter and write script name
        /// </summary>
        /// <param name="fileName"></param>
        public void CountError(string fileName)
        {
            _invalidFiles.Add(fileName.Split('\\').Last());
            _parsedCounter++;
        }

        private void Reset()
        {
            _fullScenarioAction = null;
            _currentScenarioNode = null;
            _currentScenarioXmlDocument = new XmlDocument();
            _currentScenarioNode = _currentScenarioXmlDocument.CreateElement("ScenarioReport");
        }

        /// <summary>
        /// According the log event you build a string containing
        /// allthe actions till the scenario failed or not
        /// </summary>
        /// <param name="node"></param>
        /// <param name="signature"></param>
        private void ReportScenarioAction(TxNode<MethodCall> node, MethodSignature signature)
        {
            if (_fullScenarioAction == null)
            {
                _fullScenarioAction = new ScenarioAction();
                //add first action
                _fullScenarioAction.AddAction(node, signature);
            }
            else
            {
                _fullScenarioAction.AddAction(node, signature);
            }
            if (node.Content.Name == "TakeScreenshotNow")
            {
                foreach (var value in node.Content.CallValues.Values)
                    _screenshotsOnDemand.Add(value.Value.ToString());
            }
        }
    }
}
