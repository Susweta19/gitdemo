using OpenQA.Selenium;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;

namespace TxSelenium.GenericTests.TxObject
{
    public class GTTxObject : WERefreshed
    {
        public GTTxObject(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("Objects", "")]
        public GTTxTableView Objects()
        {
            return new GTTxTableView(GetDriver(), By.Id("tableViewModifyObjects"), this);
        }

        [TxAction("ChangeTab", "")]
        public void ChangeTab(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxtabbar_tab_text\") and text()=\"" + tabName + "\"]"), this);
        }

        [TxAction("Caracteristiques", "")]
        public WMultipleLink Caracteristiques()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"moTabAttributes\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("EntitySource", "")]
        public WMultipleLink EntitySource()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"moTabOptions\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("IsEntitySourceDisabled", "")]
        public bool IsEntitySourceDisabled()
        {
            return !GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"moToolbarSourceObject\") and @class]"), this.GetElement());
        }

        [TxAction("NumberOfForms", "")]
        public void NumberOfForms(int index)
        {
            if (index == 0)
                GetDriver().ClickAt(By.Id("moRadioButtonNForm"), this);
            else
                GetDriver().ClickAt(By.Id("moRadioButton1Form"), this);
        }

        [TxAction("OneFormPerEntity", "")]
        public WEGCheckBox OneFormPerEntity()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"moRadioButtonNForm\")]"), this);
        }

        [TxAction("OneFormForAllObject", "")]
        public WEGCheckBox OneFormForAllObject()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"moRadioButton1Form\")]"), this);
        }

        [TxAction("IsTabPresent", "")]
        public bool IsTabPresent(string tabName)
        {
            return !GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@class,\"dhxtabbar_tab_text\") and text()=\"" + tabName + "\"]/parent::div[contains(@style,\"visibility: hidden\")]"), this.GetElement());
        }
    }
}
