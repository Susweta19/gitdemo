namespace TxWebTests.Config
{
    public class SpreadsheetConfig
    {
        public string SpreadSheetId { get; private set; }
        public bool PrepareSpreadSheet { get; private set; }
        public string DriveFolderId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spreadSheetId"></param>
        /// <param name="prepareSpreadSheet"></param>
        /// <param name="driveFolderId"></param>
        public SpreadsheetConfig(string spreadSheetId, bool prepareSpreadSheet = false, string driveFolderId = null)
        {
            SpreadSheetId = spreadSheetId;
            PrepareSpreadSheet = prepareSpreadSheet;
            DriveFolderId = driveFolderId;
        }
    }
}
