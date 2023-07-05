using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTRoutingTable : WERefreshed
    {
        public GTRoutingTable(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        By FixedColumns = By.XPath(".//div[starts-with(@id,\"cgrid\")]");
        By DynamicColumns = By.XPath(".//div[starts-with(@id,\"cgrid\")]/following-sibling::div[1]");

        By TranposeButton = By.XPath(".//img[contains(@src,\"-Random.png\")]");

        private By GetCellXPath(int row, int col)
        {
            return By.XPath(".//tr[@class][" + row + "]//td[not(contains(@style,\"display: none\"))][" + col + "]");
        }

        [TxAction("ClickOnCell", "")]
        public void ClickOnCell(int row, int col, bool fixedColumn)
        {
            GetDriver().WaitForAjax();
            WERefreshed parent = GetDriver().WaitForElement(fixedColumn ? FixedColumns : DynamicColumns, this);
            GetDriver().ClickAt(GetCellXPath(row, col), parent);
        }

        [TxAction("ReadCellValue", "")]
        public string ReadCellValue(int row, int col, bool fixedColumn)
        {
            GetDriver().WaitForAjax();
            WERefreshed parent = GetDriver().WaitForElement(fixedColumn ? FixedColumns : DynamicColumns, this);
            return GetDriver().WaitForElement(GetCellXPath(row, col), parent).GetElement().Text;
        }

        [TxAction("TranposeTable", "")]
        public void TranposeTable()
        {
            GetDriver().ClickAt(TranposeButton, this.Parent);
        }


    }


}
