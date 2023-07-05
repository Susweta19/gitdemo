using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TxWebTests.Config;

namespace TxWebTests.Spreadsheet
{
    public class ArchiveSheet
    {
        private SheetsService Service;
        private BatchUpdateRequest Batchupdaterequest;
        private DriveService DrivesService;
        SpreadsheetConfig config;

        string NewSpreadsheetTitle;
        string spreadsheetId;
        string ReleaseNumber;
        bool isAdminTest;
        string TagName;
        string newSheetId;

        string ArchiveFolderId = "1c7XJJqgvKU62bQ-nEX0s71HLkmI1zeJJ";
        string sheetName_TestProgress = "AutomationReport";
        string spreadsheetId_TestProgress = "1umoHJOsY0hRoTN0a_4LcG-A_hJgoFOaPs4U7gaUXOdQ";
        string spreadsheetIdStandardDBList = "1hQwY0XeQgsxCIxiJHQL8Syh467a9tTenqxIpUDelp1c";
        string versionName = "4.3.0";

        public ArchiveSheet(SheetsService Service, DriveService DrivesService, SpreadsheetConfig config, string ReleaseNumber, bool isAdminTest,string TagName="")
        {
            spreadsheetId = config.SpreadSheetId;
            this.Service = Service;
            this.DrivesService = DrivesService;
            this.config = config;
            this.ReleaseNumber = ReleaseNumber;
            this.isAdminTest = isAdminTest;
            this.TagName = TagName;
            Batchupdaterequest = new BatchUpdateRequest(Service, spreadsheetId);
            ArchiveFolderId = GetArchiveFolderId();
            //CopySpreadsheet();
            //WriteTestProgressDetails();
        }

        public void RenameSheets(string newSpSheetId)
        {
            Batchupdaterequest = new BatchUpdateRequest(Service, newSpSheetId);
            UpdateSpreadsheetPropertiesRequest updateSpreadSheetProperties = new UpdateSpreadsheetPropertiesRequest
            {
                Properties = new SpreadsheetProperties() { Title = "[" + (isAdminTest ? "Admin" : "User") + "] Selenium Test Auto " + (TagName.Length > 0 ? "[" + TagName + "]." : "") + ReleaseNumber + " " + DateTime.Now.ToString() + " [Copied]" },
                Fields = "Title"
            };
            Batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request() { UpdateSpreadsheetProperties = updateSpreadSheetProperties });
            ExecuteRequest(Batchupdaterequest.ExecuteRowFormatRequests());
        }

        private string GetArchiveFolderId()
        {
            string archivedFolderId = null;
            String range = "ArchiveFolderId" + "!A2:B";
            SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(spreadsheetIdStandardDBList, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> responseData = response.Values;
            foreach (var value in responseData)
            {
                if (TagName.Length>0)
                {
                    if (value[0].ToString() == "Tag")
                        archivedFolderId = value[1].ToString();
                }
                else if (value[0].ToString() == versionName)
                    archivedFolderId = value[1].ToString();
            }
            return archivedFolderId;
        }

        public void WriteTestProgressDetails()
        {
            //In Test Progress
            try
            {
                Batchupdaterequest = new BatchUpdateRequest(Service, spreadsheetId_TestProgress);
                SpreadsheetsResource.ValuesResource.GetRequest requestTestProgress = Service.Spreadsheets.Values.Get(spreadsheetId_TestProgress, sheetName_TestProgress + "!B1:J");
                ValueRange spSheetTestProgress = ExecuteRequest(requestTestProgress) as ValueRange;

                int rowNumber = spSheetTestProgress.Values.Count + 1;
                //newSheetId = "1Xd5FuFxS1lQtG7vj1NvGFTi04qvwPujKGKaDeWmfaFs";
                SpreadsheetsResource.ValuesResource.GetRequest requestTotalOverview = Service.Spreadsheets.Values.Get(newSheetId, "Total Overview!A1:Z1");
                ValueRange spSheetTotalOverview = ExecuteRequest(requestTotalOverview) as ValueRange;

                string totalDB = spSheetTotalOverview.Values.ElementAt(0).ElementAt(0).ToString().Split('=').Last().Trim();
                string totalTime = spSheetTotalOverview.Values.ElementAt(0).ElementAt(1).ToString().Split('=').Last().Trim();
                string totalFailedScript = spSheetTotalOverview.Values.ElementAt(0).ElementAt(2).ToString().Split('=').Last().Trim();
                string totalSuccessScript = spSheetTotalOverview.Values.ElementAt(0).ElementAt(3).ToString().Split('=').Last().Trim();
                string totalScript = spSheetTotalOverview.Values.ElementAt(0).ElementAt(4).ToString().Split('=').Last().Trim();

                string successRate = "=to_percent(SUM(D" + rowNumber + ")/SUM(C" + rowNumber + "))";
                string sheetLink = "=HYPERLINK(\"https://docs.google.com/spreadsheets/d/" + newSheetId + "\";\"LINK\")";

                Batchupdaterequest.StoreUserEnteredValuesRequests(
                    new List<object>() {DateTime.Now.ToShortDateString(),(TagName.Length>0?"["+TagName+"]"+ ReleaseNumber:ReleaseNumber),
                        totalScript, totalSuccessScript, totalFailedScript, totalDB, totalTime,
                        (isAdminTest?"Admin Test":"User Test")+(TagName.Length>0?" ["+TagName+"]":""),successRate, sheetLink }, 
                    sheetName_TestProgress + "!A" + rowNumber);

                SpreadsheetConfig newConfig = new SpreadsheetConfig(newSheetId);
                MissingDatabaseChecker missingDB=new MissingDatabaseChecker(newConfig, Service, "430", this.ReleaseNumber);
                var missingDBList = missingDB.CompareDBList();

                string missingDatabase = "";
                foreach (var db in missingDBList)
                    missingDatabase = missingDatabase + db + ",";
                Batchupdaterequest.StoreUserEnteredValuesRequests(
                    new List<object>() { missingDatabase, missingDBList.Count},
                    sheetName_TestProgress + "!L" + rowNumber);

                ExecuteRequest(Batchupdaterequest.ExecuteUserEnteredValuesRequests()) ;
            }
            catch (Exception e)
            {
                Console.WriteLine("There is some problem : " + e.ToString());
            }
        }

        public string CopySpreadsheet()
        {
            SpreadsheetsResource.GetRequest req = Service.Spreadsheets.Get(spreadsheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet spSheet = ExecuteRequest(req) as Google.Apis.Sheets.v4.Data.Spreadsheet;
            NewSpreadsheetTitle = spSheet.Properties.Title;
            Console.WriteLine("New Spreadsheet is : " + NewSpreadsheetTitle);

            Google.Apis.Drive.v3.Data.File file;
            try
            {
                var sheetMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = NewSpreadsheetTitle,
                    MimeType = "application/vnd.google-apps.spreadsheet",
                    Parents = new List<string> { ArchiveFolderId },
                };

                var request = DrivesService.Files.Copy(sheetMetadata, spreadsheetId);
                request.Fields = "id";
                request.SupportsTeamDrives = true;
                file = request.Execute();
                Console.WriteLine("Old Spreadsheet is copied to Archive Folder with Id:" + file.Id);
                newSheetId = file.Id;
                Batchupdaterequest.StoreUserEnteredValuesRequests(new List<object>() { "=HYPERLINK(\"https://docs.google.com/spreadsheets/d/" + file.Id + "\";\"Last Execution Sheet, ID:" + file.Id + "\")" }, "Total Overview" + "!I1");
                ExecuteRequest(Batchupdaterequest.ExecuteUserEnteredValuesRequests());
                return newSheetId;
            }
            catch (Exception e)
            {
                Console.WriteLine("There is some problem with make a copy of  the old Spreadsheet" + e.ToString());
                return null;
            }
        }

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
    }
}
