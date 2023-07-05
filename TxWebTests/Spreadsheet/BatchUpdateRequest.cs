using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace TxWebTests.Spreadsheet
{
    public class BatchUpdateRequest
    {
        private readonly string _spsheetId;

        //Default rowIndex is 2. 0 and 1 is reserved for Headers.
        public int RowIndex { get; set; } = 2;

        private readonly SheetsService _service;

        //Container of all Raw Data with range
        private readonly List<ValueRange> _rawListRange;

        //Container of all userEntered/Formula Data with range
        private readonly List<ValueRange> _userEnteredListRange;

        //Container of all row formating or design request
        private IList<Request> _requestsOfScenarioRowFormat;

        public BatchUpdateSpreadsheetRequest BatchUpdateSpreadsheetRequest;

        public BatchUpdateRequest(SheetsService service, string spsheetId)
        {
            this._service = service;
            this._spsheetId = spsheetId;

            _rawListRange = new List<ValueRange>();
            _userEnteredListRange = new List<ValueRange>();

            _requestsOfScenarioRowFormat = new List<Request>();

            BatchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest {Requests = new List<Request>()};

        }

        /// <summary>
        /// Store all Raw values with the starting Range 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="range"></param>
        public void StoreRawValuesRequests(List<object> values, string range)
        {
            ValueRange valueRange = new ValueRange
            {
                Range = range,
                Values = new List<IList<object>> {values}
            };
            _rawListRange.Add(valueRange);
        }

        /// <summary>
        /// Write all RAW data in spreadsheet in a single Execute Request
        /// </summary>
        public object ExecuteRAWValuesRequests()
        {
            BatchUpdateValuesRequest valueRequest = new BatchUpdateValuesRequest
            {
                Data = _rawListRange,
                ValueInputOption = "RAW"
            };
            return _service.Spreadsheets.Values.BatchUpdate(valueRequest, _spsheetId);
        }

        /// <summary>
        /// Store all USER_ENTERED/ Formula  values with the starting Range 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="range"></param>
        public void StoreUserEnteredValuesRequests(List<object> values, string range)
        {
            ValueRange valueRange = new ValueRange
            {
                Range = range,
                Values = new List<IList<object>> {values}
            };
            _userEnteredListRange.Add(valueRange);
        }

        /// <summary>
        ///  /// Write all USER_ENTERED/Formula data in spreadsheet in a single Execute Request
        /// </summary>
        public object ExecuteUserEnteredValuesRequests()
        {
            if (_userEnteredListRange.Count != 0)
            {
                BatchUpdateValuesRequest valueRequest = new BatchUpdateValuesRequest
                {
                    Data = _userEnteredListRange,
                    ValueInputOption = "USER_ENTERED"
                };
                return _service.Spreadsheets.Values.BatchUpdate(valueRequest, _spsheetId);
            }
            else
                return null;
        }

        /// <summary>
        /// Execute all Row Formating or design request in a single execute request
        /// </summary>
        public object ExecuteRowFormatRequests()
        {
            return _service.Spreadsheets.BatchUpdate(BatchUpdateSpreadsheetRequest, _spsheetId);
        }
    }
}
