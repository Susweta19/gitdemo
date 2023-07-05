using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TxWebTests.Spreadsheet
{
    public class TxPerfFeedback
    {

        private SpreadsheetManager spreadsheet;
        private BatchUpdateRequest batchupdaterequest;
        private BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest;
        private int sheetId;

        private List<String> functioNames = new List<string>();
        private List<String> scriptRefNumbers = new List<string>();
        private List<List<double>> timeTakenByExecutions = new List<List<double>>();

        private List<String> failedScripts = new List<string>();

        public TxPerfFeedback(SpreadsheetManager spreadsheet)
        {
            this.spreadsheet = spreadsheet;
            this.batchupdaterequest = new BatchUpdateRequest(spreadsheet.Service, spreadsheet.Config.SpreadSheetId);

            sheetId = new Random().Next();
            GetExecutionData();
            if (timeTakenByExecutions.Count > 2)
            {
                batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
                batchUpdateSpreadsheetRequest.Requests = new List<Request>();

                AddSheet("SummaryReport");
                DesignSheet();
                WriteRequest();
                ConditionalformatingRequest();
                ConditionalformatingRequestForFailedScripts();
                ExecuteRequests();
            }
            else
                Console.WriteLine("Sorry !!!!, There is very few data to make a performance Summary");
        }

        private void GetExecutionData()
        {
            GetAllFailedScripts();
            GetFunctionNameAndScriptReff();
            GetTotalExecutionData();
        }

        private void WriteRequest()
        {
            WriteValueHeader();
            WriteAllDataRequest();
        }


        private void GetTotalExecutionData()
        {
            IList<IList<Object>> values1 = null;

            String range1 = "1.TxPerformances!A3:A100";
            SpreadsheetsResource.ValuesResource.GetRequest request1 = spreadsheet.Service.Spreadsheets.Values.Get(spreadsheet.Config.SpreadSheetId, range1);
            ValueRange response1 = request1.Execute();
            values1 = response1.Values;

            for (int i = 1; i < 10; i++)
            {
                IList<IList<Object>> values = null;
                List<Double> time = new List<double>();
                try
                {
                    String range = "" + i + ".TxPerformances!E3:E100";
                    SpreadsheetsResource.ValuesResource.GetRequest request = spreadsheet.Service.Spreadsheets.Values.Get(spreadsheet.Config.SpreadSheetId, range);
                    ValueRange response = request.Execute();
                    values = response.Values;
                }
                catch (Exception)
                {
                    // Nothing problem. All sheet data are recorded. No new sheet Found
                    break;
                }

                int index = 0;
                foreach (var row in values)
                {
                    if (row.Count != 0)
                    {
                        if (failedScripts.Contains(values1.ElementAt(index)[0].ToString()))
                        {
                            time.Add(-1.00);
                        }
                        else
                        {
                            String[] data = row[0].ToString().Split('\n');
                            foreach (String value in data)
                            {
                                try
                                {
                                    //  Console.WriteLine(value.ToString());
                                    String val = value.Split(':')[2];
                                    int length = val.Length;
                                    val = val.Substring(1, length - 1);
                                    double timeTaken = -1.0;
                                    try
                                    {
                                        timeTaken = Double.Parse(val);
                                        time.Add(timeTaken);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Can not convert with , : " + e.ToString());
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("****Other Error=" + e.ToString());
                                    time.Add(-1.00);
                                }
                            }
                        }
                    }
                    else
                        time.Add(-1.00);
                    index++;
                }
                timeTakenByExecutions.Add(time);
            }
        }

        //TODO get sheet name from spradsheet
        public void GetFunctionNameAndScriptReff()
        {
            try
            {
                //This block is for getting ScriptReffNumbers
                String range1 = "1.TxPerformances!A3:E100";
                SpreadsheetsResource.ValuesResource.GetRequest request1 = spreadsheet.Service.Spreadsheets.Values.Get(spreadsheet.Config.SpreadSheetId, range1);
                ValueRange response1 = request1.Execute();
                IList<IList<Object>> values1 = response1.Values;

                //This block is for getting ExecutionData and FunctionNames
                String range2 = "1.TxPerformances!E3:E100";
                SpreadsheetsResource.ValuesResource.GetRequest request2 = spreadsheet.Service.Spreadsheets.Values.Get(spreadsheet.Config.SpreadSheetId, range2);
                ValueRange response2 = request2.Execute();
                IList<IList<Object>> values2 = response2.Values;
                int index = 0;
                foreach (var row in values2)
                {
                    string scriptRefNumber = values1.ElementAt(index)[0].ToString();
                    if (failedScripts.Contains(scriptRefNumber))
                    {
                        functioNames.Add("Failed Script Found ");
                        scriptRefNumbers.Add(scriptRefNumber);
                    }
                    else
                    {
                        String[] data = row[0].ToString().Split('\n');
                        foreach (String value in data)
                        {
                            functioNames.Add(value.Split(':')[1]); // For getting function Names
                            scriptRefNumbers.Add(scriptRefNumber); // For getting ref number
                        }
                    }
                    index++;
                }
            }
            catch (Exception e)
            {
                // No Such Spreadsheet Found
                Console.WriteLine(e.ToString());
            }
        }

        private void GetAllFailedScripts()
        {
            for (int i = 1; i < 6; i++)
            {
                try
                {
                    String range1 = i + ".TxPerformances!A3:C100";
                    SpreadsheetsResource.ValuesResource.GetRequest request1 =
                        spreadsheet.Service.Spreadsheets.Values.Get(spreadsheet.Config.SpreadSheetId, range1);
                    ValueRange response1 = request1.Execute();
                    IList<IList<Object>> values1 = response1.Values;
                  
                    foreach (var row in values1)
                    {
                        if (row[2].ToString().Contains("-->Failed"))
                        {
                            if (!failedScripts.Contains(row[0].ToString()))
                                failedScripts.Add(row[0].ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    //Nothing. All data Recoded
                    break;
                }
            }

        }

        //TODO Factorize this func
        private void AddSheet(string DBName)
        {
            AddSheetRequest addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = DBName;
            addSheetRequest.Properties.SheetId = sheetId;
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });

            UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetHeaderRowFormat() } };
            GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 0, RowIndex = 1, SheetId = sheetId };
            updateCellRequest.Start = coordiante;
            updateCellRequest.Fields = "*";
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });

            DimensionRange dimensionFirstColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 0, EndIndex = 1 };
            UpdateDimensionPropertiesRequest updateDimProp = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionFirstColumn, Properties = spreadsheet.GetDimensionProperties(100) };
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp });

            DimensionRange dimensionSecondColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 1, EndIndex = 2 };
            UpdateDimensionPropertiesRequest updateDimProp2 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionSecondColumn, Properties = spreadsheet.GetDimensionProperties(200) };
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp2 });

            DimensionRange dimensionThridColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 2, EndIndex = 3 };
            UpdateDimensionPropertiesRequest updateDimProp3 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionThridColumn, Properties = spreadsheet.GetDimensionProperties(170) };
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp3 });

            DimensionRange dimensionFourthColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 3, EndIndex = 4 };
            UpdateDimensionPropertiesRequest updateDimProp4 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionFourthColumn, Properties = spreadsheet.GetDimensionProperties(170) };
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp4 });

            DimensionRange dimensionFifthColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 4, EndIndex = 5 };
            UpdateDimensionPropertiesRequest updateDimProp5 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionFifthColumn, Properties = spreadsheet.GetDimensionProperties(170) };
            batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp5 });

            if (timeTakenByExecutions.Count >= 5)
            {
                DimensionRange dimensionSixthColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 5, EndIndex = 6 };
                UpdateDimensionPropertiesRequest updateDimProp6 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionSixthColumn, Properties = spreadsheet.GetDimensionProperties(170) };
                batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp6 });

                DimensionRange dimensionSeventhColumn = new DimensionRange() { Dimension = "COLUMNS", SheetId = sheetId, StartIndex = 6, EndIndex = 7 };
                UpdateDimensionPropertiesRequest updateDimProp7 = new UpdateDimensionPropertiesRequest() { Fields = "*", Range = dimensionSeventhColumn, Properties = spreadsheet.GetDimensionProperties(170) };
                batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateDimensionProperties = updateDimProp7 });
            }

        }


        private void DesignSheet()
        {
            for (int i = 0; i < functioNames.Count; i++)
            {
                UpdateCellsRequest updateCellRequest = new UpdateCellsRequest() { Rows = new List<RowData> { GetScenarioRowFormat() } };
                GridCoordinate coordiante = new GridCoordinate() { ColumnIndex = 0, RowIndex = i + 2, SheetId = sheetId };
                updateCellRequest.Start = coordiante;
                updateCellRequest.Fields = "*";
                batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { UpdateCells = updateCellRequest });
            }
        }

        private void WriteValueHeader()
        {
            if (timeTakenByExecutions.Count >= 5)
                batchupdaterequest.StoreRawValuesRequests((new List<object>() { "Ref/Name", "Function Name", "1.TxPerformances", "2.TxPerformances", "3.TxPerformances", "4.TxPerformances", "5.TxPerformances" }), "SummaryReport" + "!A2");
            else
                batchupdaterequest.StoreRawValuesRequests((new List<object>() { "Ref/Name", "Function Name", "1.TxPerformances", "2.TxPerformances", "3.TxPerformances" }), "SummaryReport" + "!A2");
        }

        private void WriteAllDataRequest()
        {
            if (timeTakenByExecutions.Count >= 5)
                for (int i = 0; i < functioNames.Count; i++)
                    batchupdaterequest.StoreRawValuesRequests((new List<object>() { scriptRefNumbers.ElementAt(i), functioNames.ElementAt(i), timeTakenByExecutions.ElementAt(0).ElementAt(i), timeTakenByExecutions.ElementAt(1).ElementAt(i), timeTakenByExecutions.ElementAt(2).ElementAt(i), timeTakenByExecutions.ElementAt(3).ElementAt(i), timeTakenByExecutions.ElementAt(4).ElementAt(i) }), "SummaryReport" + "!A" + (i + 3) + "");
            else
                for (int i = 0; i < functioNames.Count; i++)
                    batchupdaterequest.StoreRawValuesRequests((new List<object>() { scriptRefNumbers.ElementAt(i), functioNames.ElementAt(i), timeTakenByExecutions.ElementAt(0).ElementAt(i), timeTakenByExecutions.ElementAt(1).ElementAt(i), timeTakenByExecutions.ElementAt(2).ElementAt(i) }), "SummaryReport" + "!A" + (i + 3) + "");

        }
        private void ConditionalformatingRequest()
        {
            for (int rowNumber = 3; rowNumber <= scriptRefNumbers.Count + 3; rowNumber++)
            {
                try
                {
                    Color bgcolor = new Color()
                    {
                        Red = 0.84f,
                        Green = 0.38f,
                        Blue = 0.40f
                    };
                    CellFormat format = new CellFormat() { BackgroundColor = bgcolor };
                    ConditionValue cv = new ConditionValue()
                    {
                        UserEnteredValue = "=if(MULTIPLY(AVERAGE($C$" + rowNumber + ":$G$" + rowNumber + ");0,25)<MINUS(MAX($C$" + rowNumber + ":$G$" + rowNumber + ");MIN($C$" + rowNumber + ":$G$" + rowNumber + "));TRUE;FALSE)"
                    };
                    IList<ConditionValue> cvl = new List<ConditionValue> { cv };
                    BooleanCondition condition = new BooleanCondition();
                    condition.Type = "CUSTOM_FORMULA";
                    condition.Values = cvl;
                    BooleanRule booleanRule = new BooleanRule() { Condition = condition, Format = format };

                    GridRange range = new GridRange()
                    {
                        SheetId = sheetId,
                        StartColumnIndex = 2,
                        EndColumnIndex = 7,
                        StartRowIndex = rowNumber - 1,
                        EndRowIndex = rowNumber
                    };
                    ConditionalFormatRule rule = new ConditionalFormatRule()
                    {
                        Ranges = new List<GridRange> { range },
                        BooleanRule = booleanRule,
                    };
                    AddConditionalFormatRuleRequest formatReq = new AddConditionalFormatRuleRequest()
                    {
                        Rule = rule
                    };
                    batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { AddConditionalFormatRule = formatReq });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    break;
                }
            }
        }


        private void ConditionalformatingRequestForFailedScripts()
        {
            for (int rowNumber = 3; rowNumber <= scriptRefNumbers.Count + 3; rowNumber++)
            {
                try
                {
                    Color bgcolor = new Color()
                    {
                        Red = 1.0f,
                        Green = 0.38f,
                        Blue = 0.40f
                    };
                    CellFormat format = new CellFormat() { BackgroundColor = bgcolor };
                    ConditionValue cv = new ConditionValue()
                    {
                        UserEnteredValue = "=IF(RegExMatch($B$" + rowNumber + "" + ";\"Failed Script Found\");TRUE;FALSE)"
                    };
                    IList<ConditionValue> cvl = new List<ConditionValue> { cv };
                    BooleanCondition condition = new BooleanCondition();
                    condition.Type = "CUSTOM_FORMULA";
                    condition.Values = cvl;
                    BooleanRule booleanRule = new BooleanRule() { Condition = condition, Format = format };

                    GridRange range = new GridRange()
                    {
                        SheetId = sheetId,
                        StartColumnIndex = 1,
                        EndColumnIndex = 7,
                        StartRowIndex = rowNumber - 1,
                        EndRowIndex = rowNumber
                    };
                    ConditionalFormatRule rule = new ConditionalFormatRule()
                    {
                        Ranges = new List<GridRange> { range },
                        BooleanRule = booleanRule,
                    };
                    AddConditionalFormatRuleRequest formatReq = new AddConditionalFormatRuleRequest()
                    {
                        Rule = rule
                    };
                    batchupdaterequest.BatchUpdateSpreadsheetRequest.Requests.Add(new Request { AddConditionalFormatRule = formatReq });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    break;
                }
            }
        }


        private void ExecuteRequests()
        {
            spreadsheet.ExecuteRequest(batchupdaterequest.ExecuteRowFormatRequests());//1
            spreadsheet.ExecuteRequest(batchupdaterequest.ExecuteRAWValuesRequests());//2
        }

        private RowData GetScenarioRowFormat()
        {
            RowData rowData = new RowData();
            Color color = new Color();

            color.Red = 0.57f;
            color.Green = 0.80f;
            color.Blue = 0.45f;

            CellData cellRef = new CellData();
            CellFormat cellFormat = new CellFormat();
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "CENTER";
            cellFormat.VerticalAlignment = "MIDDLE";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 13 };
            cellFormat.WrapStrategy = "WRAP";
            cellRef.UserEnteredFormat = cellFormat;

            CellData cellDataFunctionName = new CellData();
            CellFormat cellFormatActions = new CellFormat();
            cellFormatActions.TextFormat = new TextFormat() { Italic = true, FontSize = 12 };
            cellFormatActions.BackgroundColor = color;
            cellFormatActions.WrapStrategy = "WRAP";
            cellFormatActions.VerticalAlignment = "MIDDLE";
            cellDataFunctionName.UserEnteredFormat = cellFormatActions;

            CellData cellDataExecutionExec = new CellData();
            CellFormat cellFormatComments = new CellFormat();
            cellFormatComments.TextFormat = new TextFormat() { FontSize = 12 };
            cellFormatComments.BackgroundColor = color;
            cellFormatComments.WrapStrategy = "WRAP";
            cellFormatComments.VerticalAlignment = "MIDDLE";
            cellDataExecutionExec.UserEnteredFormat = cellFormatComments;

            if (timeTakenByExecutions.Count >= 5)
                rowData.Values = new List<CellData> { cellRef, cellDataFunctionName, cellDataExecutionExec, cellDataExecutionExec, cellDataExecutionExec, cellDataExecutionExec, cellDataExecutionExec };
            else
                rowData.Values = new List<CellData> { cellRef, cellDataFunctionName, cellDataExecutionExec, cellDataExecutionExec, cellDataExecutionExec };
            return rowData;
        }

        private RowData GetHeaderRowFormat()
        {
            int cellNumber;
            RowData rowData = new RowData();
            CellData cellData = new CellData();
            CellFormat cellFormat = new CellFormat();
            Color color = new Color() { Red = 0.89f, Green = 0.92f, Blue = 0.96f };
            cellFormat.BackgroundColor = color;
            cellFormat.HorizontalAlignment = "Center";
            cellFormat.TextFormat = new TextFormat() { Bold = true, FontSize = 13 };
            cellData.UserEnteredFormat = cellFormat;
            rowData.Values = new List<CellData>();

            if (timeTakenByExecutions.Count >= 5)
                cellNumber = 7;
            else
                cellNumber = 5;

            for (int i = 0; i < cellNumber; i++)
                rowData.Values.Add(cellData);
            return rowData;
        }
    }
}
