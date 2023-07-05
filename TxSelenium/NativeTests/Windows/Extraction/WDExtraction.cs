using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows.Extraction;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Windows
{
    public class WDExtraction : WERefreshed
    {
        private By closeby;
        public WDExtraction(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            this.closeby = Translator.GetButtonByValue(driver, Translator.closeButton);
        }

        [TxAction("Filter", "Select a filter")]
        public WEGDHtmlxComboBox Filtre()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[starts-with(@id,\"idDivComboOTTxExtraction\")] |.//*[contains(@id,\"idDivComboOT\")]"), this);
        }

        [TxAction("Extractions", "Select a standard extractions")]
        public WEGDHtmlxComboBox Extractions()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboExtractionsTxExtraction\")]"), this);
        }

        [TxAction("Objects", "Select entity(ies)")]
        public WMultipleLink TreeLink()
        {
            By elemIdentifier = By.XPath(".//fieldset[@class=\"clFieldSetObjectsTxExtraction\"]|.//*[contains(@id,\"id_div_cell_objects\")] | .//div[@class=\"clDivObjectsTxExtraction\"]");
            return new WMultipleLink(GetDriver(), elemIdentifier, WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("ConvertToPDF", "Convert to pdf")]
        public WEGCheckBox Convert()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"idCheckConvertPDF\")]"), this);
        }

        [TxAction("Extract", "Click on extract button")]
        public WDValidatedWindow<WDAutomaticPublication> Extract(bool automaticPublication = false)
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"extract\")] | .//input[@id=\"idBtnValidate\"]"), this);
            GetDriver().WaitForAjax();
            Sleep(5);
            if (automaticPublication)
            {
                WDAutomaticPublication autoPublication = new WDAutomaticPublication(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhxwin_active\")]"));
                return new WDValidatedWindow<WDAutomaticPublication>(GetDriver(), autoPublication);
            }
            else
            {
                return null;
            }
        }

        [TxAction("IsButtonDisabled", "Click on disable button")]
        public bool IsButtonDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[contains(@disabled,'disabled')]"), this.GetElement());
        }

        [TxAction("Close", "Click on Close button")]
        public void Close()
        {
            GetDriver().ClickAt(closeby, this);
        }
    }
}
