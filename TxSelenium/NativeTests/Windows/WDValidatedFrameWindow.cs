using OpenQA.Selenium;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Windows
{
    public class WDValidatedFrameWindow<T> : WDFramedWindow<T> where T : WERefreshed
    {
        private By okBy;
        private By cancelBy;

        public WDValidatedFrameWindow(TxWebDriver driver, T content, WERefreshed parent, By frameBy)
            : base(driver, content, parent, frameBy)
        {           
            this.okBy = Translator.GetButtonByValue(driver, Translator.validateButton);
            this.cancelBy = Translator.GetButtonByValue(driver, Translator.cancelButton);
        }

        /// <summary>
        /// Closes the window and switch the driver's focus back to the main frame.
        /// </summary>
        /// <param name="validate">
        /// if false the cancel button is used 
        /// if true the other one is used (Ok, Valider ...)
        /// </param>
        [TxAction("Ok", "Closes the window by clicking on Ok or Cancel or the equivalent.")]
        public void Ok(bool validate = true)
        {
            By buttonBy = validate ? okBy : cancelBy;

            GetDriver().ClickAt(buttonBy, this);
            //maybe don't need!!!
            //Driver.SwitchToContent();
            GetDriver().WaitForAjax();
        }
        [TxAction("Validate", "To Validate all information")]
        public void Validate()
        {
            //GetDriver().FindElement(By.XPath(".//input[@value='Validate']|.//input[@id=\"idBtnValidate\"]")).Click();
            GetDriver().ClickAt(By.XPath(".//input[@value='Validate' or @id=\"validate\" or @value=\"Valider\"]|.//input[@id=\"idBtnValidate\"]"), this);//Validate
        }
        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//*[@value=\"" + buttonName + "\" and @disabled]"), this.GetElement());
        }
    }
}