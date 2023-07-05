using OpenQA.Selenium;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements
{
    /// <summary>
    /// class to manage attribute mail and url, DTText doesn't work anymore.
    /// </summary>
    public class WEGMailUrl : WERefreshed
    {
        public WEGMailUrl(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
            base.WaitForElement();
        }

        [TxAction("ManageInput", "Manage the input when it's available")]
        public WEGInput ManageInput()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@class,\"ui-autocomplete-input\")]"), this);
        }

        [TxAction("Delete", "Delete the value according index 1 by defaut.")]
        public void Delete(int index = 1)
        {
            GetDriver().ClickAt(By.XPath(".//li[" + index + "]//span[contains(@class , \"ui-icon-close\")]"), this);
        }

        [TxAction("EmailEnvelop", "Click on this button to open mail")]
        public void EmailEnvelop()
        {
            GetDriver().ClickAt(ElemByPictureName.SendMail, this);
        }
        [TxAction("IsEnvelopButtonPresent", "")]
        public bool IsEnvelopButtonPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,\"SendEmail.png\")]"), this.GetElement());
        }
    }
}
