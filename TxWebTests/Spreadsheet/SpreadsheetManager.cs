using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using System.IO;
using XmlExecutor.DataTypes;
using System.Linq;
using System.Text;
using TxSelenium.Utils;
using TxWebTests.Config;
using TxWebTests.HeaderUtils;
using System.Reflection;
using Google.Apis.Drive.v3;
using System.Xml;
using System.Globalization;

namespace TxWebTests.Spreadsheet
{
    /// <summary>
    /// Class reporting information execution in google sheet
    /// </summary>
    public class SpreadsheetManager
    {
        #region Fields
        //Counters
        private int _failedCounter = 0;
        private int _successCounter = 0;
        private int _parsedCounter = 0;

        //List of validated and invalidated bug referenced
        private readonly ICollection<int> _validatedBugRefList = new List<int>();
        private readonly ICollection<int> _invalidatedBugRefList = new List<int>();

        //List of JustFailed scripts
        private readonly ICollection<string> _justFailedScripts = new List<string>();

        //Invalid scripts concatenated in this field 
        private readonly ICollection<string> _invalidFiles = new List<string>();

        //For Record the scripts need to write valid Ref Number
        private readonly List<string> _refNeedToCorrect = new List<string>();
        
        public BatchUpdateRequest Batchupdaterequest;

        //Fields for managing the spreadsheet
        public string DataBaseName;
        private readonly int _sheetId;
        public SheetsService Service;

        //Used in the config file
        public SpreadsheetConfig Config { get; private set; }
        

        //For TxPerformance
        public Dictionary<string, List<double>> FuncTimer = new Dictionary<string, List<double>>();

        //For OnDemanScreenshot 
        private List<string> screenshotsOnDemand = new List<string>();
        public string CurrentFolderId;
        
        private string _exception = null;

        private string _spSheetId;


        // For new Modification
        private string ReleaseNumber = "ReleaseNumberNotFound";//Default Value when ReleaseNumber=null;
        private Boolean IsAdminTest;
        private bool IsTagVersion;
        private string lastDatabaseName;
        public DriveService DrivesService; //For handling spreadsheet folder(Where to paste the spreadsheet).
        string ArchiveFolderId = "1c7XJJqgvKU62bQ-nEX0s71HLkmI1zeJJ";// 430 folder Id parent Archive Folder 
        string OldSpreadsheetTitle;//This one is for keeping old spreadsheet Title same as previous
        string versionName;
        string spreadsheetIdStandardDBList = "1hQwY0XeQgsxCIxiJHQL8Syh467a9tTenqxIpUDelp1c";
        string TagName = "";
        private string _jobNumber;
        private string _jobDuration;
        public string NewSpreadsheetId;

        public enum ScenarioStatus
        {
            Success,
            Failed,
            JustFailed,
            Error
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dataBaseName"></param>
        public SpreadsheetManager(SpreadsheetConfig config, string dataBaseName, string ReleaseNumber, bool isAdminTest,
            bool IsTagVersion, string TagName, string lastDatabaseName, string jobNumber, string jobDuration)
        {
            _spSheetId = config.SpreadSheetId;
            this.Service = SetSheetService();
            this.DrivesService = new GoogleDriveManager().SetDriveService();
            this.Batchupdaterequest = new BatchUpdateRequest(Service, _spSheetId);
            this.Config = config;
            this.lastDatabaseName = lastDatabaseName;
            IsAdminTest = isAdminTest;
            this.IsTagVersion = IsTagVersion;
            this.TagName = TagName;
            this.ReleaseNumber = ReleaseNumber;
            this.TagName = TagName;
            this._jobNumber = jobNumber;
            this._jobDuration = jobDuration;

            //ArchiveSheet archiveSheet=new ArchiveSheet(Service, DrivesService, Config, ReleaseNumber, IsAdminTest, TagName);
            //NewSpreadsheetId = archiveSheet.CopySpreadsheet();
            //archiveSheet.WriteTestProgressDetails();

            //SpreadsheetConfig newConfig = new SpreadsheetConfig(String.IsNullOrEmpty(NewSpreadsheetId) ? _spSheetId : NewSpreadsheetId);
            //new MissingDatabaseChecker(newConfig, Service, "430", this.ReleaseNumber);
            //new TestReportAnalizer(newConfig, Service);
            //new OldExecutionSheetDataAnalyzer(newConfig, Service);


            this._sheetId = new Random().Next();
            if (config.PrepareSpreadSheet)
            {
               InitSpreadSheet();
            }
            this.DataBaseName = GetRowNumber("Total Overview") == 0 ? 1 + "." + dataBaseName : (GetRowNumber("Total Overview") - 1).ToString() + "." + dataBaseName;
            AddSheet(this.DataBaseName);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// According the log event you build a string containing
        /// allthe actions till the scenario failed or not
        /// </summary>
        /// <param name="node"></param>
        /// <param name="signature"></param>
        public void ReportScenarioAction(TxNode<MethodCall> node, MethodSignature signature)
        {
            if (node.Content.Name == "TakeScreenshotNow")
            {
                foreach (var value in node.Content.CallValues.Values)
                    screenshotsOnDemand.Add(value.Value.ToString());
            }
        }

        public void ReportException(string exception)
        {
            _exception = exception;
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

        public void WriteBrowserDetails(string browserName, String version, string webDriverVersion)
        {
            string sheetName = "";
            string SpreadsheetPrefix = "Admin";
            if (!IsAdminTest)
                SpreadsheetPrefix = "User";
            sheetName = "[" + SpreadsheetPrefix + "]" + "Selenium Test Auto(" + browserName + ") " + ReleaseNumber + " " + DateTime.Now.ToString();
            if (IsTagVersion)
                sheetName = "[" + SpreadsheetPrefix + "]" + "Selenium Test Auto(" + browserName + ") " + ReleaseNumber + " [" + TagName + " ] " + DateTime.Now.ToString();

            if (string.IsNullOrEmpty(browserName))
                browserName = "Sorry, I was  unable to get the name";
            if (string.IsNullOrEmpty(version))
                version = "Sorry, I was  unable to get the version";
            Batchupdaterequest.StoreRawValuesRequests(new List<object>() { "Browser Details" }, "Total Overview" + "!J1");
            Batchupdaterequest.StoreRawValuesRequests(new List<object>() { "BrowserName :" + browserName + "\nBrowserVersion :" + version + "\nDriverVersion : " + webDriverVersion }, "Total Overview" + "!J2");

            UpdateSpreadsheetPropertiesRequest updateSpreadSheetProperties = new UpdateSpreadsheetPropertiesRequest()
            {
                Properties = new SpreadsheetProperties()
                {
                    Title = sheetName
                }
            };
            updateSpreadSheetProperties.Fields = "Title";
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateSpreadsheetProperties = updateSpreadSheetProperties });

        }

        public void ReportExecutionError(string scenarioName)
        {
            UpdateScenarioRowFormat(ScenarioStatus.Error);
            //To know where writing the script informations
            int rowIndex = GetRowNumber(DataBaseName) + 1;

            string startingRange = DataBaseName + "!A" + rowIndex;
            Batchupdaterequest.StoreRawValuesRequests(new List<object>() { scenarioName, "Execution error", "An error occured before this test could start", "Execution error", "Execution error" }, startingRange);

            Batchupdaterequest.RowIndex = Batchupdaterequest.RowIndex + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="scenarioName"></param>
        /// <param name="scriptData"></param>
        public void ReportScenarioNameAndStatus(ScenarioStatus status, string scenarioName, TestData scriptData, string fullScenarioAction, int time)
        {
            if (status == ScenarioStatus.Failed &&
                scriptData.TestBugRef == 0 &&
                scriptData.TestRunningIssue == null)
                status = ScenarioStatus.JustFailed;

            UpdateScenarioRowFormat(status);
            //To know where writing the script informations
            int rowIndex = GetRowNumber(DataBaseName) + 1;

            string startingRange = DataBaseName + "!A" + rowIndex;


            if (string.IsNullOrEmpty(fullScenarioAction))
                fullScenarioAction = "Error happens before actions \n please check stacktrace";

            else if (status == ScenarioStatus.Failed || status == ScenarioStatus.JustFailed)
            {
                fullScenarioAction += "-->Failed";
                if (_exception != null)
                    fullScenarioAction += "\n" + "Exception: " + _exception.ToString().Replace("Expected value not found\n\t\t", "");
                if ((scriptData.TestRunningIssue != null && scriptData.TestBugRef == 0))
                    _refNeedToCorrect.Add(scenarioName);
            }

            if (status == ScenarioStatus.Failed)
            {
                _failedCounter++;
                if (!_invalidatedBugRefList.Contains(scriptData.TestBugRef) && scriptData.TestBugRef != 0)
                    _invalidatedBugRefList.Add(scriptData.TestBugRef);
            }
            else if (status == ScenarioStatus.Success)
            {
                _successCounter++;
                if (!_validatedBugRefList.Contains(scriptData.TestBugRef) && scriptData.TestBugRef != 0)
                    _validatedBugRefList.Add(scriptData.TestBugRef);
            }
            else
            {
                _failedCounter++;
                _justFailedScripts.Add(scenarioName);
            }

            string testBugRef = null;
            if (!scriptData.TestBugRef.ToString().Equals("0"))
                testBugRef = scriptData.TestBugRef.ToString();

            string commentCell = null;

            if (!string.IsNullOrEmpty(scriptData.TestRunningIssue) && scriptData.TestRunningIssue != "testRunningIssue")
                commentCell += "Test Running Issue : " + scriptData.TestRunningIssue;

            if (scriptData.TestDependencies.Count > 0)
            {
                commentCell += "\n\nTestDependencies : \n";
                foreach (var testDependencies in scriptData.TestDependencies)
                    if (testDependencies != "testDependencies")
                        commentCell += "Dependent On : " + testDependencies + "\n";
            }

            if (scriptData.TestFuncTimer != null && scriptData.TestFuncTimer.Count > 0)
                if (!scriptData.TestFuncTimer.Contains("testFuncTimer"))
                    commentCell = ManageFunctionTimer(scriptData.TestFuncTimer);

            Batchupdaterequest.StoreRawValuesRequests(new List<object>() { scenarioName, scriptData.TestDescription, fullScenarioAction, testBugRef, commentCell, time}, startingRange);

            Batchupdaterequest.RowIndex = Batchupdaterequest.RowIndex + 1;

            //For screenshotsOnDemand list
            if (screenshotsOnDemand.Count > 0)
                ManageTakeScreenshotNowAction(rowIndex);
        }

        private void ManageTakeScreenshotNowAction(int rowIndex)
        {
            char column = 'H';
            GoogleDriveManager googleDriverManager = new GoogleDriveManager(true, this);

            for (int i = 7; i < (screenshotsOnDemand.Count + 7); i++)
            {
                UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetScenarioRowFormatForOnDemandScreenshot() } };
                GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = i, RowIndex = rowIndex - 1, SheetId = _sheetId };
                updateCellRequest.Start = coordiante;
                updateCellRequest.Fields = "*";
                Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
            }
            DimensionRange dimensionOnDemandScreenshotColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = _sheetId, StartIndex = 7, EndIndex = screenshotsOnDemand.Count + 7 };
            UpdateDimensionPropertiesRequest updateDimPropForOnDemandScreenshot = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionOnDemandScreenshotColumn, Properties = GetDimensionProperties(200) };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimPropForOnDemandScreenshot });

            foreach (string screenshotName in screenshotsOnDemand)
            {
                string range = DataBaseName + "!" + column + rowIndex;
                googleDriverManager.UploadOnDemandScrenshotRequests(range, screenshotName + ".png");
                column = (char)((int)column + 1);
            }
            screenshotsOnDemand.Clear();
        }

        private string ManageFunctionTimer(ICollection<string> funcList)
        {
            StringBuilder times = new StringBuilder();
            foreach (var item in funcList)
            {
                if (this.FuncTimer.ContainsKey(item.ToString()))
                {
                    foreach (var elem in this.FuncTimer[item.ToString()])
                    {
                        times.AppendLine("Function : " + item.ToString() + "  time : " + elem);
                    }
                }
            }
            //need to clear dictionnary after every scenario
            this.FuncTimer.Clear();
            return times.ToString().TrimEnd();
        }

        /// <summary>
        /// Write counter results for every
        /// database in the first sheet
        /// </summary>
        /// <param name="totalTime">The total time it took to execute the tests in milliseconds</param>
        public void WriteOverviewValue(long totalTime)
        {
            UpdateOverviewRowFormat();
            int rowNb = 3;
            //When there is no data, means for the first write of overview rowIndex should be 3;
            if (GetRowNumber("Total Overview") != 0)
                rowNb = GetRowNumber("Total Overview") + 1;

            double failedReportCounter = _failedCounter + _parsedCounter;

            if (_parsedCounter != 0)
            {
                UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetOverViewRowFormatForInvalidScript() } };
                GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 2, RowIndex = rowNb - 1, SheetId = 0 };
                updateCellRequest.Start = coordiante;
                updateCellRequest.Fields = "*";
                Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
            }

            Batchupdaterequest.StoreRawValuesRequests(new List<object>() { DataBaseName,
                Math.Round(TimeSpan.FromMilliseconds(totalTime).TotalMinutes,4),
                                                                           failedReportCounter,
                                                                           _successCounter,
                                                                           (_failedCounter + _successCounter + _parsedCounter) },
                                                                           "Total Overview!A" + rowNb);
            if (_validatedBugRefList.Count != 0)
                Batchupdaterequest.StoreRawValuesRequests(new List<object>() { CollToString<int>(_validatedBugRefList) }, "Total Overview!F" + rowNb);
            if (_invalidatedBugRefList.Count != 0)
                Batchupdaterequest.StoreRawValuesRequests(new List<object>() { CollToString<int>(_invalidatedBugRefList) }, "Total Overview!G" + rowNb);
            if (_justFailedScripts.Count != 0)
            {
                string justFailedStringFormat = "";
                foreach (string value in _justFailedScripts)
                    justFailedStringFormat += value + ",";
                Batchupdaterequest.StoreRawValuesRequests(new List<object>() { justFailedStringFormat }, "Total Overview!H" + rowNb);
            }
            if (_refNeedToCorrect.Count != 0)
            {
                //debug code
                string refNeedToCorrectScriptsName = CollToString(_refNeedToCorrect);
                Batchupdaterequest.StoreRawValuesRequests(new List<object>() { "Need to Modify Ref Number\n" + refNeedToCorrectScriptsName }, "Total Overview!K" + rowNb + "");
            }
            else
                Batchupdaterequest.StoreRawValuesRequests(new List<object>() { "" }, "Total Overview!K" + rowNb + "");

            ExecuteRequest(Batchupdaterequest.ExecuteRowFormatRequests());//1
            if (Batchupdaterequest.ExecuteUserEnteredValuesRequests() != null)
                ExecuteRequest(Batchupdaterequest.ExecuteUserEnteredValuesRequests());//2
            ExecuteRequest(Batchupdaterequest.ExecuteRAWValuesRequests());//3


            if (DataBaseName.Contains("5.TxPerformances"))
                new TxPerfFeedback(this);
            if ((DataBaseName.Contains("WriteForm") && (!DataBaseName.Equals("1.WriteForm"))))
            {
                ArchiveSheet archiveSheet = new ArchiveSheet(Service, DrivesService, Config, ReleaseNumber, IsAdminTest, TagName);
                NewSpreadsheetId = archiveSheet.CopySpreadsheet();
                
                if(String.IsNullOrEmpty(NewSpreadsheetId))
                {
                    archiveSheet.WriteTestProgressDetails();
                    archiveSheet.RenameSheets(NewSpreadsheetId);
                    SpreadsheetConfig newConfig = new SpreadsheetConfig(NewSpreadsheetId);
                    new TestReportAnalizer(newConfig, Service);
                    new OldExecutionSheetDataAnalyzer(newConfig, Service);
                    //new MissingDatabaseChecker(newConfig, Service, "430", this.ReleaseNumber);
                }
                else
                {
                    new TestReportAnalizer(Config, Service);
                    new OldExecutionSheetDataAnalyzer(Config, Service);
                    //new MissingDatabaseChecker(Config, Service, "430", this.ReleaseNumber);
                }
            }
        }
        /// <summary>
        /// Write the final statistics for every 
        /// new database added
        /// </summary>
        /// <param name="totalTime">The total time it took to execute the tests in milliseconds</param>
        public void DataBaseResults(long totalTime)
        {
            UpdateCellFormatDatabaseResults();

            double seconds = TimeSpan.FromMilliseconds(totalTime).TotalMinutes;

            string invalidFileNames = _invalidFiles.Count != 0 ? CollToString<string>(_invalidFiles) : "None";
            string results = "Time : " + seconds + "\n" + "Failed : " + _failedCounter + "\n" + "Success : " + _successCounter + "\n" + "List of Invalid Scripts : \n" + invalidFileNames;
            Batchupdaterequest.StoreRawValuesRequests((new List<object>() { results }), DataBaseName + "!C1");
        }
        #endregion

        #region Private Methods

        private string GetArchiveFolderId()
        {
            string archivedFolderId = null;
            String range = "ArchiveFolderId" + "!A2:B";
            SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(spreadsheetIdStandardDBList, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> responseData = response.Values;
            foreach (var value in responseData)
            {
                if (IsTagVersion)
                {
                    if (value[0].ToString() == "Tag")
                        archivedFolderId = value[1].ToString();
                }
                else if (value[0].ToString() == versionName)
                    archivedFolderId = value[1].ToString();
            }
            return archivedFolderId;
        }

        /// <summary>
        /// Authentification to the service (API = SheetsService)  
        /// </summary>
        /// <returns></returns>
        private SheetsService SetSheetService()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets };

            string clientIdPath = LinuxUtils.IsLinux ? "./client_id.json" : ".\\client_id.json";
            ServiceAccountCredential credential;
            using (var stream = new FileStream(clientIdPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes).UnderlyingCredential as ServiceAccountCredential;
            }

            Console.WriteLine("SheetService Connecting...");
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
            return service;
        }

        private bool IsVersionSame(string releseNumber)
        {
            SpreadsheetsResource.GetRequest request = Service.Spreadsheets.Get(Config.SpreadSheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet spSheet = ExecuteRequest(request) as Google.Apis.Sheets.v4.Data.Spreadsheet;
            OldSpreadsheetTitle = spSheet.Properties.Title;
            Console.WriteLine("Old Spreadsheet is : " + OldSpreadsheetTitle);
            this.ReleaseNumber = releseNumber;
            return (spSheet.Properties.Title.Contains(releseNumber));
        }

        private void CopySpreadsheet()
        {
            SpreadsheetsResource.GetRequest req = Service.Spreadsheets.Get(Config.SpreadSheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet spSheet = ExecuteRequest(req) as Google.Apis.Sheets.v4.Data.Spreadsheet;
            OldSpreadsheetTitle = spSheet.Properties.Title;
            Console.WriteLine("Old Spreadsheet is : " + OldSpreadsheetTitle);

            Google.Apis.Drive.v3.Data.File file;
            try
            {
                var sheetMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = OldSpreadsheetTitle,
                    MimeType = "application/vnd.google-apps.spreadsheet",
                    Parents = new List<string> { ArchiveFolderId },
                };

                var request = DrivesService.Files.Copy(sheetMetadata, _spSheetId);
                request.SupportsTeamDrives = true;
                request.Fields = "id";
                file = request.Execute();
                Console.WriteLine("Old Spreadsheet is copied to Archive Folder with Id:" + file.Id);
                Batchupdaterequest.StoreUserEnteredValuesRequests(new List<object>() { "=HYPERLINK(\"https://docs.google.com/spreadsheets/d/" + file.Id + "\";\"Last Execution Sheet, ID:" + file.Id + "\")" }, "Total Overview" + "!I1");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is some problem with make a copy of  the old Spreadsheet" + e.ToString());
            }
        }
        /// <summary>
        /// Intialize the spread sheet before starting a new sesion test.
        /// </summary>
        private void InitSpreadSheet()
        {
            Console.WriteLine("Spreadsheet Initializing");
            //Create list of request

            //Need Some Informations of the spreadsheet;
            SpreadsheetsResource.GetRequest request = Service.Spreadsheets.Get(_spSheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet spSheet = ExecuteRequest(request) as Google.Apis.Sheets.v4.Data.Spreadsheet;

            UpdateSpreadsheetPropertiesRequest updateSpreadSheetProperties = new UpdateSpreadsheetPropertiesRequest
            {
                Properties = new SpreadsheetProperties() { Title = "[" + (IsAdminTest ? "Admin" : "User") + "] Selenium Test Auto " + (TagName.Length > 0 ? "[" + TagName + "]." : "") + ReleaseNumber + " " + DateTime.Now.ToString() },
                Fields = "Title"
            };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateSpreadsheetProperties = updateSpreadSheetProperties });


            //TODO Froze the second row
            //GridProperties gridProperties = new GridProperties();
            //gridProperties.FrozenRowCount = 2;
            //UpdateSheetPropertiesRequest updateSheetPropertiesRequest = new UpdateSheetPropertiesRequest() { Properties = new SheetProperties() { SheetId = 0, Title = "Total Overview" , GridProperties = gridProperties, Index = 2 } };               
            //updateSheetPropertiesRequest.Fields = "GridProperties";
            //batchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateSheetProperties = updateSheetPropertiesRequest });
            // service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, Config.SpreadSheetId).Execute();

            //Add an deleteRequest for each sheet of the spreadsheet excpet the first one (sheetId = 0)

            foreach (Sheet sheet in spSheet.Sheets.Except(new List<Sheet> { spSheet.Sheets[0] }))
            {
                int? sheetIdToDelete = sheet.Properties.SheetId;
                DeleteSheetRequest deletesheetrequest = new DeleteSheetRequest { SheetId = sheetIdToDelete };
                Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { DeleteSheet = deletesheetrequest });
            }
            string firstSheetTitle = spSheet.Sheets[0].Properties.Title;
            int rowNumber = GetRowNumber(firstSheetTitle);

            BatchClearValuesRequest batchClearValuesRequest = new BatchClearValuesRequest { Ranges = new List<string> { firstSheetTitle + "!A1:H1000" } };

            //Execute clear first sheet request 
            ExecuteRequest(Service.Spreadsheets.Values.BatchClear(batchClearValuesRequest, _spSheetId));

            // This one is for init TotalOverView Background
            Request updateCellsRequest = new Request()
            {
                RepeatCell = new RepeatCellRequest()
                {
                    Range = new GridRange()
                    {
                        SheetId = 0,
                    },
                    Cell = new CellData()
                    {
                        UserEnteredFormat = new CellFormat() { BackgroundColor = new Color() { Red = 1.00f, Green = 1.00f, Blue = 1.00f } }
                    },
                    Fields = "UserEnteredFormat(BackgroundColor)"
                }
            };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(updateCellsRequest);

            UpdateOverviewHeaderFormat();
            WriteOverviewHeaderValue();
            ExecuteRequest(Service.Spreadsheets.BatchUpdate(Batchupdaterequest.BatchUpdateSpreadsheetRequest, _spSheetId));
            ExecuteRequest(Batchupdaterequest.ExecuteUserEnteredValuesRequests());
            ExecuteRequest(Batchupdaterequest.ExecuteRAWValuesRequests());
            Batchupdaterequest = new BatchUpdateRequest(Service, _spSheetId);
        }

        /// <summary>
        /// Update five properties 
        /// of the first sheet (Total Overview)
        /// </summary>
        private void UpdateOverviewHeaderFormat()
        {
            //BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            //batchUpdateSpreadsheetRequest.Requests = new List<Request>();

            UpdateCellsRequest cellRequest1 = new UpdateCellsRequest() { Rows = new List<RowData> { GetOverViewFirstRowFormat() } };
            GridCoordinate coordiante1 = new GridCoordinate() { ColumnIndex = 0, RowIndex = 0, SheetId = 0 };
            cellRequest1.Start = coordiante1;
            cellRequest1.Fields = "*";


            UpdateCellsRequest cellRequest2 = new UpdateCellsRequest() { Rows = new List<RowData> { GetHeaderRowFormat(8) } };
            GridCoordinate coordiante2 = new GridCoordinate() { ColumnIndex = 0, RowIndex = 1, SheetId = 0 };
            cellRequest2.Start = coordiante2;
            cellRequest2.Fields = "*";

            UpdateSheetPropertiesRequest sheetPropertiesRequest =
                new UpdateSheetPropertiesRequest
                {
                    Properties = new SheetProperties() { SheetId = 0, Title = "Total Overview" },
                    Fields = "Title"
                };

            UpdateDimensionPropertiesRequest dimensionPropertiesRequest1 = new UpdateDimensionPropertiesRequest
            {
                Properties = GetDimensionProperties(230),
                Range = new DimensionRange() { Dimension = "COLUMNS", StartIndex = 0, EndIndex = 2, SheetId = 0 },
                Fields = "*"
            };

            UpdateDimensionPropertiesRequest dimensionPropertiesRequest2 = new UpdateDimensionPropertiesRequest
            {
                Properties = GetDimensionProperties(170),
                Range = new DimensionRange() { Dimension = "COLUMNS", StartIndex = 2, EndIndex = 5, SheetId = 0 },
                Fields = "*"
            };

            UpdateDimensionPropertiesRequest dimensionPropertiesRequest3 = new UpdateDimensionPropertiesRequest
            {
                Properties = GetDimensionProperties(150),
                Range = new DimensionRange() { Dimension = "COLUMNS", StartIndex = 5, EndIndex = 8, SheetId = 0 },
                Fields = "*"
            };
            UpdateDimensionPropertiesRequest dimensionPropertiesRequest4 = new UpdateDimensionPropertiesRequest
            {
                Properties = GetDimensionProperties(150),
                Range = new DimensionRange() { Dimension = "COLUMNS", StartIndex = 8, EndIndex = 10, SheetId = 0 },
                Fields = "*"
            };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateCells = cellRequest1 });
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateCells = cellRequest2 });
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateSheetProperties = sheetPropertiesRequest });
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateDimensionProperties = dimensionPropertiesRequest1 });
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateDimensionProperties = dimensionPropertiesRequest2 });
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateDimensionProperties = dimensionPropertiesRequest3 });
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateDimensionProperties = dimensionPropertiesRequest4 });
            //batchupdaterequest.ExecuteRowFormatRequests();
            //ExecuteRequest(service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, Config.SpreadSheetId));
        }

        /// <summary>
        /// Write inside the two first line
        /// of the first sheet.
        /// </summary>
        private void WriteOverviewHeaderValue()
        {
            const string dataBaseName = "Total Overview";
            Batchupdaterequest.StoreUserEnteredValuesRequests(new List<object>() { "= \"Total Databases = \" & NBVAL(A3: A100)", "=\"Total Time = \" & ROUND(SUM(B3:B100)/60;2) & \" h\"", "=\"Total Failed = \" & SUM(C3:C100)", "=\"Total Success = \" & SUM(D3:D100)", "=\"Total Scripts = \" & SUM(E3:E100)", "=to_percent(SUM(D3:D)/SUM(E3:E))", $"Job Duration={_jobDuration}", $"Job Number={_jobNumber}" }, "" + dataBaseName + "" + "!A1");
            Batchupdaterequest.StoreRawValuesRequests(new List<object>() { "Database", "Execution Time (mn)", "Failed Scripts", "Success Scripts", "Total Scripts", "Bugs Validated", "Bugs Not Validated", "Just Failed Scripts" }, "" + dataBaseName + "" + "!A2");
            // Batchupdaterequest.StoreUserEnteredValuesRequests(new List<object>() { "Database", "Execution Time (mn)", "Failed Scripts", "Success Scripts", "Total Scripts", "Bugs Validated", "Bugs Not Validated", "Just Failed Scripts" }, "" + dataBaseName + "" + "!A2");
        }

        /// <summary>
        /// To add a sheet in an existing SpreadSheet
        /// </summary>
        private void AddSheet(string dbName)
        {
            //Create list of request
            //BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            //batchUpdateSpreadsheetRequest.Requests = new List<Request>();

            //Request to add a sheet
            AddSheetRequest addSheetRequest = new AddSheetRequest
            {
                Properties = new SheetProperties
                {
                    Title = dbName,
                    SheetId = _sheetId
                }
            };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });

            //TODO Request to froze the second row
            //GridProperties gridProperties = new GridProperties();
            //gridProperties.FrozenRowCount = 2;
            //UpdateSheetPropertiesRequest updateSheetPropertiesRequest = new UpdateSheetPropertiesRequest() { Properties = new SheetProperties() { SheetId = 0, Title = "Total Overview" , GridProperties = gridProperties, Index = 2 } };               
            //updateSheetPropertiesRequest.Fields = "GridProperties";
            //batchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateSheetProperties = updateSheetPropertiesRequest });
            //service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, Config.SpreadSheetId).Execute();

            //Request to update the format of the 4 first cell in first range
            UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetHeaderRowFormat(6) } };
            GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 0, RowIndex = 1, SheetId = _sheetId };
            updateCellRequest.Start = coordiante;
            updateCellRequest.Fields = "*";
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });

            //Request to update the dimension of the first and second column 
            DimensionRange dimensionSecondColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = _sheetId, StartIndex = 0, EndIndex = 2 };
            UpdateDimensionPropertiesRequest updateDimProp = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionSecondColumn, Properties = GetDimensionProperties(250) };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp });

            //Request to update the dimension of the third column 
            DimensionRange dimensionThridColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = _sheetId, StartIndex = 2, EndIndex = 3 };
            UpdateDimensionPropertiesRequest updateDimProp2 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionThridColumn, Properties = GetDimensionProperties(500) };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp2 });

            //Dimension of bug ref
            DimensionRange dimensionFourthColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = _sheetId, StartIndex = 3, EndIndex = 4 };
            UpdateDimensionPropertiesRequest updateDimProp4 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionFourthColumn, Properties = GetDimensionProperties(150) };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp4 });

            //Dimension of comments column
            DimensionRange dimensionFifthColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = _sheetId, StartIndex = 4, EndIndex = 5 };
            UpdateDimensionPropertiesRequest updateDimProp3 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionFifthColumn, Properties = GetDimensionProperties(500) };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp3 });

            DimensionRange dimensionSixthColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = _sheetId, StartIndex = 5, EndIndex = 6 };
            UpdateDimensionPropertiesRequest updateDimProp6 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionSixthColumn, Properties = GetDimensionProperties(200) };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp6 });

            // ExecuteRequest(service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, Config.SpreadSheetId));
            WriteValueHeader();
        }

        /// <summary>
        /// To Write the header (2nd line) of every 
        /// sheet except the first one.
        /// </summary>
        private void WriteValueHeader()
        {
            Batchupdaterequest.StoreRawValuesRequests((new List<object>() { "Ref/Name", "Description", "Actions", "BugRef", "Comments", "Time (s)", "Screenshot" }), "" + DataBaseName + "" + "!A2");
        }

        /// <summary>
        /// Update the color and the cell format of row according 
        /// ScenarioStatus : Failed or Succes
        /// </summary>
        /// <param name="type"></param>
        private void UpdateScenarioRowFormat(ScenarioStatus type, int i = 1)
        {
            //BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            //batchUpdateSpreadsheetRequest.Requests = new List<Request>();

            UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetScenarioRowFormat(type) } };
            GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 0, RowIndex = GetRowNumber(DataBaseName), SheetId = _sheetId };
            updateCellRequest.Start = coordiante;
            updateCellRequest.Fields = "*";
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
        }

        private string CollToString<T>(ICollection<T> collection, string indentation = null)
        {
            StringBuilder result = new StringBuilder();
            foreach (T item in collection)
                result.AppendLine(indentation + item.ToString());

            return result.ToString().TrimEnd();
        }

        /// <summary>
        /// return the first row without value in it
        /// </summary>
        /// <param name="DBName"></param>
        /// <returns></returns>
        public int GetRowNumber(string DBName)
        {
            if (DBName == "Total Overview")
            {
                try
                {
                    string range = "" + DBName + "!A1:H1000";
                    SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(_spSheetId, range);
                    ValueRange response = ExecuteRequest(request) as ValueRange;
                    return response.Values != null ? response.Values.Count : 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return 0;
                }
            }
            else
                return Batchupdaterequest.RowIndex;
        }

        /// <summary>
        /// Update the format of the database row report
        /// </summary>
        private void UpdateOverviewRowFormat()
        {

            int rowNb = 2;
            //When there is no data, means for the first fill  of OverviewRowFormat, rowIndex should be 2;
            if (GetRowNumber("Total Overview") != 0)
                rowNb = GetRowNumber("Total Overview");

            UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetOverviewRowFormat() } };
            GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 0, RowIndex = rowNb, SheetId = 0 };
            updateCellRequest.Start = coordiante;
            updateCellRequest.Fields = "*";
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
        }

        /// <summary>
        /// Update format of the C1 cell
        /// </summary>
        private void UpdateCellFormatDatabaseResults()
        {

            UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetDatabaseResultsRowFormat() } };
            GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 2, RowIndex = 0, SheetId = _sheetId };
            updateCellRequest.Start = coordiante;
            updateCellRequest.Fields = "*";
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
        }
        #endregion

        #region Execute Request Method
        /// <summary>
        /// Function sending all request using reflection
        /// BatchUpdateRequest,BatchClearRequest,GetRequest
        /// </summary>
        /// <param name="request"></param>
        public object ExecuteRequest(object request, int i = 1)
        {
            string requestName = request.GetType().Name;
            try
            {
                //Same method to execute all type of request.
                MethodInfo method = request.GetType().GetMethod("Execute");
                return method.Invoke(request, new object[] { });
            }
            catch (Exception e)
            {
                Console.WriteLine(requestName + " Failed(" + i + ") because : " + e.Message);
                Console.WriteLine(e.StackTrace);
                if (e.InnerException != null)
                    Console.WriteLine(e.InnerException.ToString());
                //we resend the request at least twice if it fails.
                if (i < 3)
                {
                    i++;
                    return ExecuteRequest(request, i);
                }
                else
                {
                    Console.WriteLine("After resending the request " + i + " times, it fails!");
                    return null;
                }
            }
        }
        #endregion

        #region Format Spreadsheet Methods
        private RowData GetDatabaseResultsRowFormat()
        {
            RowData rowData = new RowData();
            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.HorizontalAlignment = "CENTER";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 12 };
            cellData.UserEnteredFormat = cellFormat;
            rowData.Values = new List<CellData> { cellData };
            return rowData;
        }

        private RowData GetOverViewFirstRowFormat()
        {
            RowData rowData = new RowData();
            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            Color color = new Color() { Red = 0.8f, Green = 0.9f, Blue = 0.7f };
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "Center";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 13 };
            cellData.UserEnteredFormat = cellFormat;

            //This one is for Last ExecutionSheetURL; //TODO
            CellData cellDataSmallFont = new CellData();
            CellFormat cellFormat2 = new CellFormat();
            cellFormat2.BackgroundColor = color;
            cellFormat2.HorizontalAlignment = "Center";
            cellFormat2.VerticalAlignment = "MIDDLE";
            cellFormat2.TextFormat = new TextFormat() { Bold = true, FontSize = 7 };
            cellDataSmallFont.UserEnteredFormat = cellFormat2;

            rowData.Values = new List<CellData> { cellData, cellData, cellData, cellData, cellData, cellData, cellData, cellData };
            return rowData;
        }

        private RowData GetHeaderRowFormat(int cellNumber)
        {
            RowData rowData = new RowData();
            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            Color color = new Color() { Red = 0.89f, Green = 0.92f, Blue = 0.96f };
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "Center";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 13 };
            cellData.UserEnteredFormat = cellFormat;
            rowData.Values = new List<CellData>();

            for (int i = 0; i < cellNumber; i++)
                rowData.Values.Add(cellData);
            return rowData;
        }

        private RowData GetOverviewRowFormat()
        {
            RowData rowData = new RowData();
            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.HorizontalAlignment = "CENTER";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 14 };
            cellData.UserEnteredFormat = cellFormat;
            rowData.Values = new List<CellData> { cellData, cellData, cellData, cellData, cellData, cellData, cellData, cellData };
            return rowData;
        }

        private RowData GetOverViewRowFormatForInvalidScript()
        {
            RowData rowData = new RowData();
            Color color = new Color();
            color.Red = 1.0f;
            color.Green = 0.00f;
            color.Blue = 0.05f;

            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "CENTER";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, Italic = true, FontSize = 15 };
            cellFormat.WrapStrategy = "WRAP";
            cellData.UserEnteredFormat = cellFormat;

            rowData.Values = new List<CellData> { cellData };
            return rowData;
        }
        private RowData ResetOverViewBackground()
        {
            RowData rowData = new RowData();
            Color color = new Color();
            color.Red = 0.00f;
            color.Green = 0.00f;
            color.Blue = 0.00f;

            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.BackgroundColor = color;
            //  cellFormat.HorizontalAlignment = "CENTER";
            //   cellFormat.VerticalAlignment = "MIDDLE";
            // cellFormat.TextFormat = new TextFormat() {FontSize = 15 };
            //  cellFormat.WrapStrategy = "WRAP";
            cellData.UserEnteredFormat = cellFormat;

            rowData.Values = new List<CellData> { cellData };
            return rowData;
        }
        private RowData GetScenarioRowFormat(ScenarioStatus status)
        {
            RowData rowData = new RowData();
            Color color = new Color();

            //For ScreenShot BackGround
            Color colorScreenshot = new Color();

            if (status == ScenarioStatus.Success)
            {
                color.Red = 0.57f;
                color.Green = 0.80f;
                color.Blue = 0.45f;

                //For ScreenShot BackGround
                colorScreenshot.Red = 1.0f;
                colorScreenshot.Green = 1.0f;
                colorScreenshot.Blue = 1.0f;
            }
            else if (status == ScenarioStatus.Failed)
            {
                color.Red = 0.84f;
                color.Green = 0.38f;
                color.Blue = 0.40f;

                //For ScreenShot BackGround
                colorScreenshot.Red = 0.73f;
                colorScreenshot.Green = 0.74f;
                colorScreenshot.Blue = 0.76f;
            }
            else if (status == ScenarioStatus.JustFailed)
            {
                color.Red = 0.82f;
                color.Green = 0.65f;
                color.Blue = 0.12f;

                //For ScreenShot BackGround
                colorScreenshot.Red = 0.73f;
                colorScreenshot.Green = 0.74f;
                colorScreenshot.Blue = 0.76f;
            }
            else
            {
                color.Red = 0.84f;
                color.Green = 0.2f;
                color.Blue = 1.0f;

                //For ScreenShot BackGround
                colorScreenshot.Red = 0.84f;
                colorScreenshot.Green = 0.2f;
                colorScreenshot.Blue = 1.0f;

            }

            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "CENTER";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 13 };
            cellFormat.WrapStrategy = "WRAP";
            cellData.UserEnteredFormat = cellFormat;

            CellData cellDataActions = new CellData();
            CellFormat cellFormatActions = new CellFormat();
            cellFormatActions.TextFormat = new TextFormat() { Italic = true, FontSize = 10 };
            cellFormatActions.BackgroundColor = color;
            cellFormatActions.VerticalAlignment = "MIDDLE";
            cellDataActions.UserEnteredFormat = cellFormatActions;

            CellData cellDataComments = new CellData();
            CellFormat cellFormatComments = new CellFormat();
            cellFormatComments.TextFormat = new TextFormat() { FontSize = 14 };
            cellFormatComments.BackgroundColor = color;
            cellFormatComments.WrapStrategy = "WRAP";
            cellFormatComments.VerticalAlignment = "MIDDLE";
            cellDataComments.UserEnteredFormat = cellFormatComments;

            CellData cellDataScreenShot = new CellData();
            CellFormat cellFormatScreenShot = new CellFormat();
            cellFormatScreenShot.TextFormat = new TextFormat() { FontSize = 13 };
            cellFormatScreenShot.BackgroundColor = colorScreenshot;
            cellFormatScreenShot.HorizontalAlignment = "CENTER";
            cellFormatScreenShot.WrapStrategy = "WRAP";
            cellFormatScreenShot.VerticalAlignment = "MIDDLE";
            cellDataScreenShot.UserEnteredFormat = cellFormatScreenShot;

            rowData.Values = new List<CellData> { cellData, cellData, cellDataActions, cellData, cellDataComments, cellData, cellDataScreenShot };
            return rowData;
        }

        private RowData GetScenarioRowFormatForOnDemandScreenshot()
        {
            RowData rowData = new RowData();
            CellData cellDataScreenShot = new CellData();
            CellFormat cellFormatScreenShot = new CellFormat();
            cellFormatScreenShot.TextFormat = new TextFormat() { FontSize = 13 };
            cellFormatScreenShot.HorizontalAlignment = "CENTER";
            cellFormatScreenShot.WrapStrategy = "WRAP";
            cellFormatScreenShot.VerticalAlignment = "MIDDLE";
            cellDataScreenShot.UserEnteredFormat = cellFormatScreenShot;

            rowData.Values = new List<CellData> { cellDataScreenShot };
            return rowData;
        }

        public DimensionProperties GetDimensionProperties(int pixelSize)
        {
            DimensionProperties dimensionPorperties = new DimensionProperties();
            dimensionPorperties.PixelSize = pixelSize;
            return dimensionPorperties;
        }
        #endregion
    }
}
