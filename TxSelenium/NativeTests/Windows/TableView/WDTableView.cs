using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDTableView : WERefreshed
    {
        //To select the linear or classic tree
        private static readonly By treeDiv = By.XPath(".//div[contains(@id, \"table\") and not(contains(@style, \"display: none\"))]");

        public static By GetHeaderBy(string headerName)
        {
            return new ByChained(treeDiv, By.XPath(".//div[@class=\"xhdr\"]//div[text()=\"" + headerName + "\"]"));
        }

        public WDTableView(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// Select the selection of an entiy with the specified path.
        /// </summary>
        /// <param name="path">the path to the specified entity</param>
        [TxAction("EntityTableView", "Select the selection box of one entity identified by the path leading to it.")]
        public WMultipleLink EntityTableView()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivTableViewContent\")]"), WESTree.expandByTableViewTree, WSingleLink.entitySelectorTableViewBy, this);
        }

        //TODO this function properly.  
        /// <summary>
        /// Click on an header to sort the table view.
        /// </summary>
        /// <param name="headerName">the name to the specified header</param>
        [TxAction("ReadValue", "close the table view")]
        public string ReadValue(int colIndex, int rowIndex)
        {
            WERefreshed value = GetDriver().WaitForElement(new ByChained(treeDiv, By.XPath(".//div[@class = \"objbox\"]//tr[" + rowIndex + "]//td[" + colIndex + "]")), this);
            return value.GetElement().Text;
        }

        /// <summary>
        /// click on an header to sort the table view.
        /// </summary>
        /// <param name = "headername" > the name to the specified header</param>
        [TxAction("SelectHeader", "Click on the header selected to sort the table view.")]
        public void Headers(string headerName)
        {
            WERefreshed table = GetDriver().WaitForElement(new ByChained(treeDiv, By.ClassName("xhdr")), this);
            GetDriver().ScrollHorizontal(table.GetElement(), WDTableView.GetHeaderBy(headerName), this);
        }
    }
}
