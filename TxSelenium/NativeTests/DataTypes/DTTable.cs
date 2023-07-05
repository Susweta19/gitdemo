using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.DataTypes
{
    public class DTTable : IReadableData
    {
        private string[,] data;
        WERefreshed parent;
        public DTTable(string[,] data, WERefreshed parent)
        {
            this.data = data;
            this.parent = parent;
        }

        [TxAction("GetValue", "Gets the value in the cell with the specified coordinates")]
        public string GetValue(int rowIndex, int colIndex)
        {
            return data[rowIndex + 1, colIndex - 1];
        }

        [TxAction("GetLink", "Gets the value in the cell with the specified coordinates")]
        public WEGLink GetLink(int rowIndex, int colIndex)
        {
            return new WEGLink(parent.GetDriver(), By.XPath(".//table/tbody/tr[" + rowIndex + "]/td[" + (colIndex - 1) + "]/a |.//table/tbody/tr[" + rowIndex + "]/td[" + (colIndex - 1) + "]"), parent);
        }
    }
}
