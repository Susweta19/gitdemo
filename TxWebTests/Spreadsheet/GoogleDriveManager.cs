using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TxSelenium.Utils;

namespace TxWebTests.Spreadsheet
{
    public class GoogleDriveManager
    {
        private readonly DriveService _service;
        private readonly string _dbName = null;
        private string _folderId = null;
        private readonly SpreadsheetManager _spreadsheet;
        public GoogleDriveManager()
        {
            //do nothing
        }

        /// <summary>
        /// This Constuctor is for OnDemandScreenshot Upload
        /// </summary>
        public GoogleDriveManager(Boolean ForUploadOnDemandScreenshot, SpreadsheetManager _spreadsheet)
        {
            this._spreadsheet = _spreadsheet;
        }

        public GoogleDriveManager(SpreadsheetManager spreadsheet)
        {
            this._service = SetDriveService();
            this._dbName = spreadsheet.DataBaseName;
            this._spreadsheet = spreadsheet;
            if (spreadsheet.Config.PrepareSpreadSheet)
                DeletePreviousData();
            CreateFolder();
        }

        public DriveService SetDriveService()
        {
            string[] scopes = { DriveService.Scope.Drive };

            string clientIdPath = LinuxUtils.IsLinux ? "./client_id.json" : ".\\client_id.json";
            ServiceAccountCredential credential;
            using (FileStream stream = new FileStream(clientIdPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes).UnderlyingCredential as ServiceAccountCredential;
            }

            Console.WriteLine("Drive Service Connecting...");
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
            return service;
        }

        public void CreateFolder()
        {
            if (string.IsNullOrEmpty(_folderId))
            {
                try
                {
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = _dbName,
                        MimeType = "application/vnd.google-apps.folder",
                        Parents = new List<string> { _spreadsheet.Config.DriveFolderId },
                    };

                    var request = _service.Files.Create(fileMetadata);
                    request.SupportsAllDrives = true;
                    request.Fields = "id";
                    var file = request.Execute();
                    _folderId = file.Id;
                    _spreadsheet.CurrentFolderId= _folderId;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Delete previousdata is InitSpreadsheet is True
        /// </summary>
        public void DeletePreviousData()
        {
            try
            {
                FilesResource.ListRequest listRequest = _service.Files.List();
                listRequest.PageSize = 100;
                listRequest.Fields = "nextPageToken, files(id, name)";
                listRequest.Q = "mimeType='application/vnd.google-apps.folder' and '" + _spreadsheet.Config.DriveFolderId + "' in parents";
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                foreach (Google.Apis.Drive.v3.Data.File file in files)
                    _service.Files.Delete(file.Id).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void UploadRequest(string filePath)
        {
            if (System.IO.File.Exists(filePath.ToString()))
            {
                try
                {
                    Google.Apis.Drive.v3.Data.File body =
                        new Google.Apis.Drive.v3.Data.File
                        {
                            Name = System.IO.Path.GetFileName(filePath.ToString()),
                            Description = "Test Description"
                        };
                    string mimeType = "image/png";
                    body.MimeType = mimeType;
                    body.Parents = new List<string> { _folderId };
                    byte[] byteArray = System.IO.File.ReadAllBytes(filePath);
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, mimeType);
                    request.SupportsTeamDrives = true;
                    request.Upload();
                    Google.Apis.Drive.v3.Data.File file = request.ResponseBody;
                    int rowIndex = _spreadsheet.GetRowNumber(_dbName);
                    string fileName = filePath.Split('\\').Last();
                    _spreadsheet.Batchupdaterequest.StoreUserEnteredValuesRequests(new List<object>() { "=HYPERLINK(\"https://drive.google.com/open?id=" + file.Id + "\";\"" + fileName + "\")" }, "" + _dbName + "" + "!G" + rowIndex + "");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Console.WriteLine("Screenshot not found may be because of driver issue, " + filePath);
            }
        }

        public void UploadOnDemandScrenshotRequests(string range, string filePath)
        {
            if (System.IO.File.Exists(filePath.ToString()))
            {
                try
                {
                    Google.Apis.Drive.v3.Data.File body =
                        new Google.Apis.Drive.v3.Data.File
                        {
                            Name = System.IO.Path.GetFileName(filePath.ToString()),
                            Description = "Test Description"
                        };
                    string mimeType = "image/png";
                    body.MimeType = mimeType;
                    body.Parents = new List<string> { _spreadsheet.CurrentFolderId };
                    byte[] byteArray = System.IO.File.ReadAllBytes(filePath);
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                    FilesResource.CreateMediaUpload request = _spreadsheet.DrivesService.Files.Create(body, stream, mimeType);
                    request.SupportsTeamDrives = true;
                    request.Upload();
                    Google.Apis.Drive.v3.Data.File file = request.ResponseBody;

                    _spreadsheet.Batchupdaterequest.StoreUserEnteredValuesRequests(new List<object>() { "=HYPERLINK(\"https://drive.google.com/open?id=" + file.Id + "\";\"" + filePath + "\")" }, range);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Console.WriteLine("Screenshot not found may be because of driver issue, " + filePath);
            }
        }
    }
}