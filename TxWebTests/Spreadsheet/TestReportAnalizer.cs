using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TxWebTests.Config;

namespace TxWebTests.Spreadsheet
{
    public class TestReportAnalizer
    {
        SpreadsheetConfig config = null;
        SheetsService service;
        //Raw Data Block
        string SpreadsheetTitle = "";
        private List<String> DatabaseName = new List<string>();
        private List<int> numberofFailed = new List<int>();
        private List<int> numberofSuccess = new List<int>();
        private List<int> numberOfTotalScripts = new List<int>();
        private List<List<String>> ValidateBugRef = new List<List<string>>();
        private List<List<String>> NotValidateBugRef = new List<List<string>>();

        //Analysed Data Block
        private List<String> ConflictedBugRefDetails = new List<string>();
        private List<Double> FailedRatioPerDB = new List<Double>();
        private List<Double> FailedRatioAllDB = new List<Double>();
        private int TotalNumberOfScriptsRun;
        private int TotalNumberOfScriptsSuccess;
        private List<List<string>> DatabaseNameWithMostFailedNumber = new List<List<string>>();
        private List<List<string>> DatabaseInfoWithMostFailedRatio = new List<List<string>>();
        private string mailBody = "No data Found";

        public TestReportAnalizer(SpreadsheetConfig config, SheetsService service)
        {
            this.config = config;
            this.service = service;

            try
            {
                GetDataFromSheet();
                AnalysBugRefConflict();
                GetNumberOfTotalScriptsRun();
                GetMaxFailedNumberDBinfo(numberofFailed);
                CalculateFailedRatio();
                GetMaxFailedRatioDBinfo(FailedRatioPerDB);
                createResult();
                mailBody = createBodyForMail();
                SendEmail("anubhab.maity@bassetti-group.com");
                SendEmail("anthony.portier@bassetti.fr");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is a problem with TestReport Analysis\n" + e.ToString());
            }
        }

        /// <summary>
        /// For getting all Raw Data from Spreadsheet
        /// </summary>
        private void GetDataFromSheet()
        {
            try
            {
                GetSpreadsheetTitle();
                String range1 = "Total Overview!A3:H100";
                SpreadsheetsResource.ValuesResource.GetRequest request1 = service.Spreadsheets.Values.Get(config.SpreadSheetId, range1);
                ValueRange response1 = request1.Execute();
                IList<IList<Object>> values1 = response1.Values;
                foreach (var value in values1)
                {
                    string bugValidated = "";
                    try
                    {
                        bugValidated = value[5].ToString();
                    }
                    catch (Exception)
                    {
                        //Nothing to worry. May be the cell is blank;
                    }

                    string bugNotValidated = "";
                    try
                    {
                        bugNotValidated = value[6].ToString();
                    }
                    catch (Exception)
                    {
                        //Nothing to worry. May be the cell is blank;
                    }

                    DatabaseName.Add(value[0].ToString());
                    try
                    {
                        numberofFailed.Add(int.Parse(value[2].ToString()));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on numberofFailed column(" + value[2].ToString() + ")");
                        numberofFailed.Add(0);
                    }
                    try
                    {
                        numberofSuccess.Add(int.Parse(value[3].ToString()));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on numberofSuccess column(" + value[3].ToString() + ")");
                        numberofSuccess.Add(0);
                    }
                    try
                    {
                        numberOfTotalScripts.Add(int.Parse(value[4].ToString()));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on numberOfTotalScripts column(" + value[4].ToString() + ")");
                        numberOfTotalScripts.Add(0);
                    }
                    try
                    {
                        ValidateBugRef.Add(GetBugRef(bugValidated));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on ValidateBugRef column(" + bugValidated + ")");
                        ValidateBugRef.Add(new List<string>() { "0" });
                    }
                    try
                    {
                        NotValidateBugRef.Add(GetBugRef(bugNotValidated));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on NotValidateBugRef column(" + bugNotValidated + ")");
                        NotValidateBugRef.Add(new List<string>() { "0" });
                    }
                }
            }
            catch (Exception e)
            {
                // No Such Spreadsheet Found
                Console.WriteLine(e.ToString());
                throw new Exception("There is a problem with getting Data from spreadsheet");
            }
        }

        /// <summary>
        /// Analys the Bug Conflicts.
        /// </summary>

        //TODO Should be More Optimised. Ask Tandrima
        private void AnalysBugRefConflict()
        {
            for (int dbIndexBugValidated = 0; dbIndexBugValidated < ValidateBugRef.Count; dbIndexBugValidated++)
            {
                foreach (string validatedRefNumber in ValidateBugRef[dbIndexBugValidated])
                {
                    if (validatedRefNumber == "" || validatedRefNumber == "-1")
                        continue;
                    for (int dbIndexBugNotValidated = 0; dbIndexBugNotValidated < NotValidateBugRef.Count; dbIndexBugNotValidated++)
                    {
                        foreach (string notValidatedRefNumber in NotValidateBugRef[dbIndexBugNotValidated])
                        {
                            if (notValidatedRefNumber == "" || notValidatedRefNumber == "-1")
                                continue;
                            if (validatedRefNumber.Equals(notValidatedRefNumber))
                            {
                                if (dbIndexBugValidated == dbIndexBugNotValidated)
                                    ConflictedBugRefDetails.Add("Bug Reference Conflict Found with ID " + validatedRefNumber + " on Database --> " + DatabaseName[dbIndexBugValidated].Split('.')[1]);
                                else
                                    ConflictedBugRefDetails.Add("Bug Reference Conflict Found with ID " + validatedRefNumber + " on Database --> " + DatabaseName[dbIndexBugValidated].Split('.')[1] + " and " + DatabaseName[dbIndexBugNotValidated].Split('.')[1]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Count Number of Total Scripts
        /// </summary>
        private void GetNumberOfTotalScriptsRun()
        {
            foreach (int number in numberOfTotalScripts)
                TotalNumberOfScriptsRun += number;

            foreach (int number in numberofSuccess)
                TotalNumberOfScriptsSuccess += number;
        }

        /// <summary>
        /// GetMaxFailedNumberDBinfo
        /// </summary>
        /// <param name="List"></param>
        private void GetMaxFailedNumberDBinfo(List<int> List)
        {
            //Duplicate the list for not overwritting old data
            List<int> List1 = new List<int>(List);
            List<String> dbName = new List<string>();
            List<String> noOfFailed = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                int maxVal = -1;
                int location = 0;
                foreach (var data in List1.Select((value, index) => new { index, value }))
                {
                    if (data.value > maxVal)
                    {
                        maxVal = data.value;
                        location = data.index;
                    }
                }
                dbName.Add(DatabaseName[location].Split('.')[1]);
                noOfFailed.Add(maxVal.ToString());
                List1[location] = -1;
            }
            DatabaseNameWithMostFailedNumber.Add(dbName);
            DatabaseNameWithMostFailedNumber.Add(noOfFailed);
        }

        /// <summary>
        /// Calculate failed Ratio per DB
        /// </summary>
        private void CalculateFailedRatio()
        {
            for (int i = 0; i < numberofFailed.Count; i++)
            {
                if (numberOfTotalScripts[i] >= 10 && numberOfTotalScripts[i] != 0)
                {
                    double value = ((double)numberofFailed[i] / (double)numberOfTotalScripts[i]) * 100;
                    FailedRatioPerDB.Add(Math.Truncate(value * 100) / 100);
                }
                else
                    FailedRatioPerDB.Add(-1.00);//If no of Scripts less than 10. We will not calculate for it.
            }
        }

        /// <summary>
        /// Get Ratio report per DB
        /// </summary>
        /// <param name="list1"></param>
        private void GetMaxFailedRatioDBinfo(List<Double> list1)
        {
            List<Double> list = new List<double>(list1);
            List<String> dbName = new List<string>();
            List<String> ratioOfFailed = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                double maxVal = -1;
                int location = 0;
                foreach (var data in list.Select((value, index) => new { index, value }))
                {
                    if (data.value > maxVal)
                    {
                        maxVal = data.value;
                        location = data.index;
                    }
                }

                dbName.Add(DatabaseName[location].Split('.')[1]);
                ratioOfFailed.Add(maxVal.ToString());
                list[location] = -1;
            }
            DatabaseInfoWithMostFailedRatio.Add(dbName);
            DatabaseInfoWithMostFailedRatio.Add(ratioOfFailed);
        }


        private void createResult()
        {
            Console.WriteLine("Spreadsheet Title-->" + SpreadsheetTitle);
            Console.WriteLine("Total number of Scripts run-->" + TotalNumberOfScriptsRun);
            Console.WriteLine("Total number of Scripts Failed-->" + (TotalNumberOfScriptsRun - TotalNumberOfScriptsSuccess));
            Console.WriteLine("Total Number of Script Success-->" + TotalNumberOfScriptsSuccess);

            double SuccessRatio = (double)(TotalNumberOfScriptsSuccess) / (double)TotalNumberOfScriptsRun * 100;
            Console.WriteLine("Total Success percentage-->" + SuccessRatio);

            Console.WriteLine("Max failed DB list--->");
            GetTopFaildDBInfo(DatabaseNameWithMostFailedNumber);

            Console.WriteLine("Max failed ratio DB list--->");
            GetTopFaildRatioDBInfo(DatabaseInfoWithMostFailedRatio);
            foreach (string data in ConflictedBugRefDetails)
                Console.WriteLine(data);
        }

        private string GetTopFaildDBInfo(List<List<string>> DatabaseInfo)
        {
            string dbInfo = "";
            for (int i = 0; i < 5; i++)
            {
                dbInfo += "\n\"" + DatabaseInfo[0][i] + "\" " + DatabaseInfo[1][i] + " Failed";
                Console.WriteLine("Database Name-->" + DatabaseInfo[0][i] + " Failed -->" + DatabaseInfo[1][i]);
            }
            return dbInfo;
        }

        private string GetTopFaildRatioDBInfo(List<List<string>> DatabaseInfo)
        {
            string dbInfo = "";
            for (int i = 0; i < 5; i++)
            {
                dbInfo += "\n\"" + DatabaseInfo[0][i] + "\" " + DatabaseInfo[1][i] + " % Failed";
                Console.WriteLine("Database Name-->" + DatabaseInfo[0][i] + " Failed -->" + DatabaseInfo[1][i] + " %");
            }
            return dbInfo;
        }

        private List<string> GetBugRef(string bugRefs)
        {
            List<string> bugRefValidated = new List<string>();

            foreach (var refNumber in bugRefs.Split('\n'))
                bugRefValidated.Add(refNumber);
            return bugRefValidated;
        }

        private object ExecuteRequest(object request, int i = 1)
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

        private void GetSpreadsheetTitle()
        {
            SpreadsheetsResource.GetRequest request = service.Spreadsheets.Get(config.SpreadSheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet spSheet = ExecuteRequest(request) as Google.Apis.Sheets.v4.Data.Spreadsheet;
            SpreadsheetTitle = spSheet.Properties.Title;
        }


        private string createBodyForMail()
        {

            String Body = "Hi,\n\n";
            Body += "Total number of Database run-->" + DatabaseName.Count;
            Body += "\nTotal number of Scripts run-->" + TotalNumberOfScriptsRun;
            Body += "\nTotal number of Scripts Failed-->" + (TotalNumberOfScriptsRun - TotalNumberOfScriptsSuccess);
            Body += "\nTotal Number of Script Success-->" + TotalNumberOfScriptsSuccess;

            double SuccessRatio = (double)(TotalNumberOfScriptsSuccess) / (double)TotalNumberOfScriptsRun * 100;
            SuccessRatio = (Math.Truncate(SuccessRatio * 100) / 100);

            Body += "\nTotal Success Percentage-->" + SuccessRatio + " %\n";
            Body += "\n**********Top failed Database list**********";
            Body += GetTopFaildDBInfo(DatabaseNameWithMostFailedNumber);
            Body += "\n\n**********Top failed Percentage per Database**********";
            Body += GetTopFaildRatioDBInfo(DatabaseInfoWithMostFailedRatio);

            Body += "\n\n**********Conflicted Bug Ref List**********";
            if (ConflictedBugRefDetails.Count != 0)
            {
                foreach (string data in ConflictedBugRefDetails)
                    Body += "\n" + data;
            }
            else
            {
                Body += "\nNO Conflict found!!!";
            }
            
            Body += "\n\nFeedback File-->" + @"https://docs.google.com/spreadsheets/d/" + config.SpreadSheetId + "//";
            Body += "\n\nRegards\nAnubhab Maity\nTesting Team\nContact:anubhab.maity@bassetti-group.com";
            return Body;
        }
        /// <summary>
        /// 
        /// TODO Optimize the code using Google API.
        /// </summary>
        /// <param name="recipient"></param>
        private void SendEmail(string recipient)
        {
            SmtpClient smtp;
            try
            {
                var fromAddress = new MailAddress("seleniumtest.bassetti@gmail.com", "Auto Generated Test Analysis Report");
                var toAddress = new MailAddress(recipient);
                const string fromPass = "Bas1669!";
                string subject = "" + SpreadsheetTitle;
                string body = mailBody;

                smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPass)
                };

                MailMessage message = new MailMessage(fromAddress, toAddress);
                message.Body = body;
                message.Subject = subject;
                smtp.Send(message);
                Console.WriteLine("Missin Db Email Sent to-->" + toAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("There is a problem with sending mail to "+recipient+" . Error : " + e.ToString());
            }
        }
    }
}
