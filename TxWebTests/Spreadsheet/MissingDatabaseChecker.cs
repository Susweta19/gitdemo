using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TxWebTests.Config;

namespace TxWebTests.Spreadsheet
{
    public class MissingDatabaseChecker
    {
        string spreadsheetIdForActualDBList = "1hQwY0XeQgsxCIxiJHQL8Syh467a9tTenqxIpUDelp1c";
        SheetsService service;
        SpreadsheetConfig config = null;
        string emailBody = "";
        string currentVersion;
        string databaseParameter = "";

        private List<String> actualDBList = new List<string>();
        private List<String> currentDBList = new List<string>();
        private List<String> missingDBList = new List<string>();
        private List<String> ccAddressList = new List<string>();
        private string releaseNumber;

        public MissingDatabaseChecker(SpreadsheetConfig config,SheetsService service,string version,string releaseNumber)
        {
            try {
                this.service = service;
                this.config = config;
                this.currentVersion = version;
                this.releaseNumber = releaseNumber;

                //GetCurrentDBList();
                //GetActualDBList();
                //if (actualDBList.Count != currentDBList.Count)
                //    CompareDBList();
                //PrepareEmailBody();
                //ccAddressList.Add("anubhab.maity@bassetti-group.com");
                //ccAddressList.Add("dhirendra.singh@bassetti-group.com");

                //SendEmail("anubhab.maity@bassetti-group.com", ccAddressList);
                //SendEmail("anthony.portier@bassetti.fr");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is some problem with Missing Datbase Checking\n Error:"+e.ToString());
            }
        }

        private void GetActualDBList()
        {
            String range = "DBLIST_"+ currentVersion + "!A3:A";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetIdForActualDBList, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> responseData = response.Values;
            foreach (var value in responseData)
                actualDBList.Add(value[0].ToString().Split('.').Last());
        }

        private void GetCurrentDBList()
        {
            String range = "Total Overview!A3:A";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(config.SpreadSheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> responseData = response.Values;
            foreach (var value in responseData)
                currentDBList.Add(value[0].ToString().Split('.').Last());
        }

        public List<String> CompareDBList()
        {
            GetCurrentDBList();
            GetActualDBList();
            if (actualDBList.Count != currentDBList.Count)
                foreach (string dbName in actualDBList)
                    if (!IsDatabasePresent(dbName))
                    {
                        missingDBList.Add(dbName);
                        databaseParameter += dbName + ",";
                    }
            return missingDBList;
        }

        private bool IsDatabasePresent(string databaseName)
        {
            foreach (string dbName in currentDBList)
                if (dbName.Equals(databaseName))
                    return true;
            return false;
        }

        private void PrepareEmailBody()
        {
            emailBody += "Hello,\n";
            if (missingDBList.Count > 0)
            {
                emailBody += "\nNumber of missing database is : " + missingDBList.Count + "\n\nList of missing database : \n";
                for (int i = 0; i < missingDBList.Count; i++)
                    emailBody += (i+1) + "." + missingDBList[i] + "\n\n";
                emailBody += "DataBase Parameter :" + databaseParameter + "\n\n";
            }
            else
                emailBody += "There is no missing database.";
            emailBody += "\nSpreadSheet : https://docs.google.com/spreadsheets/d/" + config.SpreadSheetId+"/";
            emailBody += "\n\nThanks & Regards\nAnubhab Maity\nTesting Team\nContact:anubhab.maity@bassetti-group.com";
        }

        private void SendEmail(string recipient, List<string> ccMailAddress = null)
        {
            try
            {
                var fromAddress = new MailAddress("seleniumtest.bassetti@gmail.com", "Missing DB List");
                var toAddress = new MailAddress(recipient);
                const string fromPass = "Bas1669!";
                string subject = "Missing Database List For ";
                if (!String.IsNullOrEmpty(releaseNumber))
                    subject += "." + releaseNumber;
                else
                    subject += currentVersion;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPass)
                };

                MailMessage message = new MailMessage(fromAddress, toAddress);
                message.Body = emailBody;
                message.Subject = subject;

                if (ccMailAddress!=null)
                    foreach (var data in ccMailAddress)
                        message.CC.Add(data);

                smtp.Send(message);
                Console.WriteLine("Missing Db Email Sent to--> " + toAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("There is a problem with sending mail to " + recipient + " . Error : " + e.ToString());
            }
        }
    }
}
