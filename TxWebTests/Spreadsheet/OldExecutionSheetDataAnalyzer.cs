using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TxWebTests.Config;

namespace TxWebTests.Spreadsheet
{
    public class OldExecutionSheetDataAnalyzer
    {
        SpreadsheetConfig config;
        SheetsService service;
        public BatchUpdateRequest batchupdaterequest;

        private List<String> DatabaseNameOfCurrentSheet = new List<string>();
        private List<int> numberofFailedOfCurrentSheet = new List<int>();

        string oldSpreadsheetID = "";
        private List<String> DatabaseNameOfArchiveSheet = new List<string>();
        private List<int> numberofFailedOfArchiveSheet = new List<int>();

        public OldExecutionSheetDataAnalyzer(SpreadsheetConfig config, SheetsService service)
        {
            this.config = config;
            this.service = service;
            this.batchupdaterequest = new BatchUpdateRequest(service, config.SpreadSheetId);
            GetDataFromCurrentSheet();
            GetDataFromArchiveSheet();
            CompareData();
            var RowFormatReq = batchupdaterequest.ExecuteRowFormatRequests();
            var RawValueReq = batchupdaterequest.ExecuteRAWValuesRequests();

            if (!(RowFormatReq == null | RowFormatReq == null))
            {
                ExecuteRequest(RowFormatReq);
                ExecuteRequest(RawValueReq);
            }

        }

        private RowData GetBGcolor(int diff)
        {
            RowData rowData = new RowData();
            Color color = new Color();
            if (diff > 20)
            {
                color.Red = 1.0f;
                color.Green = 0.00f;
                color.Blue = 0.00f;
            }

            if (diff <= 20 && diff > 10)
            {

                color.Red = 1.0f;
                color.Green = 0.45f;
                color.Blue = 0.45f;
            }

            if (diff <= 10 && diff > 5)
            {
                color.Red = 1.0f;
                color.Green = 0.65f;
                color.Blue = 0.21f;
            }

            if (diff <= 5 && diff > 0)
            {
                color.Red = 1.0f;
                color.Green = 0.97f;
                color.Blue = 0.26f;

            }

            if (diff < 0)
            {
                color.Red = .40f;
                color.Green = 1.0f;
                color.Blue = 0.55f;
            }

            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "CENTER";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, Italic = false, FontSize = 14 };
            cellFormat.WrapStrategy = "WRAP";
            cellData.UserEnteredFormat = cellFormat;

            rowData.Values = new List<CellData> { cellData };
            return rowData;
        }

        private void CompareData()
        {
            try
            {
                foreach (var CurrentDbName in DatabaseNameOfCurrentSheet.Select((value, index) => new { index, value }))
                {
                    foreach (var OldDBName in DatabaseNameOfArchiveSheet.Select((value, index) => new { index, value }))
                    {
                        if (CurrentDbName.value.Split('.')[1].Equals(OldDBName.value.Split('.')[1]))
                        {
                            int failedDiff = numberofFailedOfCurrentSheet[CurrentDbName.index] - numberofFailedOfArchiveSheet[OldDBName.index];
                            ChangeBGColorOfCell(failedDiff, CurrentDbName.index, CurrentDbName.value);
                            break;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is a problem with Compare Data betwwen old and current spreadsheet" + e.ToString());
            }
        }

        private void ChangeBGColorOfCell(int failedDifference, int index, String dbName)
        {
            if (failedDifference != 0)
            {
                UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetBGcolor(failedDifference) } };
                GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 0, RowIndex = index + 2, SheetId = 0 };
                updateCellRequest.Start = coordiante;
                updateCellRequest.Fields = "*";
                batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
                batchupdaterequest.StoreRawValuesRequests((new List<object>() { dbName }), "Total Overview" + "!A" + (index + 3) + "");
            }
            //  Console.WriteLine("failedDifference="+ failedDifference+ "--->   DbName"+range);
        }


        private void GetDataFromCurrentSheet()
        {
            try
            {
                String range1 = "Total Overview!A3:H100";
                String range2 = "Total Overview!I1";

                //For getting all data from current sheet
                SpreadsheetsResource.ValuesResource.GetRequest request1 = service.Spreadsheets.Values.Get(config.SpreadSheetId, range1);
                ValueRange response1 = request1.Execute();
                IList<IList<Object>> values1 = response1.Values;

                //For getting Id of old sheet
                SpreadsheetsResource.ValuesResource.GetRequest request2 = service.Spreadsheets.Values.Get(config.SpreadSheetId, range2);
                ValueRange response2 = request2.Execute();
                IList<IList<Object>> values2 = response2.Values;

                foreach (var value in values2)
                {
                    try
                    {
                        oldSpreadsheetID = value[0].ToString().Split(':')[1];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to get Old spreadsheet ID\n" + e.ToString());
                        throw new Exception("There is a problem to get Old Spreadsheet ID");
                    }
                }

                foreach (var value in values1)
                {
                    DatabaseNameOfCurrentSheet.Add(value[0].ToString());
                    try
                    {
                        numberofFailedOfCurrentSheet.Add(int.Parse(value[2].ToString()));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on numberofFailed column(" + value[2].ToString() + ")");
                        numberofFailedOfCurrentSheet.Add(0);
                    }

                }
            }
            catch (Exception e)
            {
                // No Such Spreadsheet Found
                Console.WriteLine(e.ToString());
                throw new Exception("There is a problem with getting Data from Current spreadsheet");
            }
        }


        private void GetDataFromArchiveSheet()
        {
            try
            {
                String range1 = "Total Overview!A3:H100";

                //For getting all data from  sheet
                SpreadsheetsResource.ValuesResource.GetRequest request1 = service.Spreadsheets.Values.Get(oldSpreadsheetID, range1);
                ValueRange response1 = request1.Execute();
                IList<IList<Object>> values1 = response1.Values;


                foreach (var value in values1)
                {
                    DatabaseNameOfArchiveSheet.Add(value[0].ToString());
                    try
                    {
                        numberofFailedOfArchiveSheet.Add(int.Parse(value[2].ToString()));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Worng Data Found on numberofFailed column(" + value[2].ToString() + ")");
                        numberofFailedOfArchiveSheet.Add(0);
                    }

                }
            }
            catch (Exception e)
            {
                // No Such Spreadsheet Found
                Console.WriteLine(e.ToString());
                throw new Exception("There is a problem with getting Data from Old spreadsheet");
            }
        }
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
    }
}
