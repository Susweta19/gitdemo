using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDAddAssoLink : WERefreshed
    {
        public WDAddAssoLink(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            :base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("AddTreeLink", "Selects an entity.")]
        public WMultipleLink TreeLink()
        {
            GetDriver().WaitForAjax();
            return new WMultipleLink(GetDriver(), By.XPath(".//*[starts-with(@id,\"idDivMain\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("Ok", "Ok Button to fill the formulary.")]
        public WDValidatedWindow<WForm> Ok(bool OneForm = false)
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idBtnValidate\")]"), this);

            if (OneForm == true)
                new WESPopUp(GetDriver()).ClosePopup(true);

            WForm linkCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), linkCreator);
        }
    }
}
