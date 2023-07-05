using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows.Extraction
{
    public class WDAutomaticPublication : WERefreshed
    {
        public WDAutomaticPublication(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        //TODO maybe return form
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TxAction("FileName", "Export data")]
        public WEGInput FileName()
        {
            return new WEGInput(GetDriver(), By.Id("idTextQueryString"), this);
        }

        [TxAction("Ok", "Export data")]
        public WDValidatedWindow<WForm> OkButton()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxwin_active\")]//input[@id='idBtnValidate']"));

            WForm form = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), form);
        }
    }
}
