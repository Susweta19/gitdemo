using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxIndicator
{
    public class GTSave: WERefreshed
    {
        private static readonly By closeButton = By.XPath(".//input[@value=\"Close\"]");
        private static readonly By saveButton = By.XPath(".//input[contains(@id, \"idBtnValidate\")]");
        public GTSave(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("EntityType", "Select Entity Type")]
        public WEGDHtmlxComboBox EntityType()
        {
            return new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[@class=\"clDivComboIndicator\"]/label[text()=\"Object Type:\"]/..//div[contains(@class,\"dhxcombo_material\")]"), this);
        }

        [TxAction("Characteristics", "Characteristics dropdown list")]
        public WEGDHtmlxComboBox Characteristics()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[@class=\"clDivComboIndicator\"]/label[text()=\"Attributes:\"]/..//div[contains(@class,\"dhxcombo_material\")]"), this);
        }
        
        [TxAction("Object", "Select object from objectlist")]
        public WMultipleLink Treelink()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[contains(@id,\"idDivSaveObjects\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("Compressimages", "Compress the image")]
        public WEGCheckBox Compressimages()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[contains(@id,\"idDivCheckbox\")]"), this);
        }

        [TxAction("Save", "Compress the image")]
        public void Save()
        {
            GetDriver().ClickAt(GTSave.saveButton, this);
        }

        [TxAction("Close", " Click save button")]
        public void close()
        {
            GetDriver().ClickAt(GTSave.closeButton, this);
        }
    }
}
