using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows.Utils
{
    public class WDImportDataTable : WERefreshed
    {

        public WDImportDataTable(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("TransposeTable", "manage transpose table checkbox")]
        public WEGCheckBox TransposeTable()
        {
            return new WEGCheckBox(GetDriver(), By.Id("idCheckboxTransposeArrayImport"), this);
        }

        [TxAction("Close", "click on the close button")]
        public void Close()
        {
            GetDriver().ClickAt(By.XPath(".//input[@value=\"Close\"]"), this);
        }

        [TxAction("Table", "click on the close button")]
        public WTable Table()
        {
            return new WTable(GetDriver(), By.Id("idDivGridArrayImport"), this);
        }

        [TxAction("Seprator", "click on the seprator button")]
        public WEGDHtmlxComboBox Seprator()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboSeparatorArrayImport\")]"), this);
        }

        [TxAction("ImportFromFile", "click on the import file button")]
        public void ImportFromFile(string path)
        {
            WERefreshed button = GetDriver().WaitForElement(By.XPath(".//input[starts-with(@id,\"idBtnImportFromFile\")]"), this);
            GetDriver().UploadFile(button, path);
        }
    }
}