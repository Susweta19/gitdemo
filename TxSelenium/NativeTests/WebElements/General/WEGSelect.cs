using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements
{
    /// <summary>
    /// class managing select element in html
    /// </summary>
    /// //TODO Make it wecheckbox read and write
    public class WEGSelect : WERefreshed
    {
        private static By GetObjectTypeBy(string value)
        {
            return By.XPath(".//span[text()=\"" + value + "\"]");
        }

        public WEGSelect(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            base.WaitForElement();
        }

        [TxAction("SelectByText", "Selects an element based on the displayed text.")]
        public void SelectByText(string text)
        {
            SelectElement select = new SelectElement(GetElement());
            select.SelectByText(text);
        }

        [TxAction("SelectByValue", "Selects an element based on its associated value.")]
        public void SelectByValue(string value)
        {
            SelectElement select = new SelectElement(GetElement());
            select.SelectByValue(value);
        }

        /// <summary>
        /// Manage the cmb_principale in WERequirementSet and WEMultiCriteria
        /// </summary>
        /// <param name="text"></param>
        public void SelectCMB(string text)
        {
            string cmbTitle = GetDriver().WaitForElement(new ByChained(By.Id("cmb_titre"), By.TagName("span")), this).GetElement().Text;
            //We check if the cmb selected is also what we want (TODO For the comboBox)
            if (cmbTitle != text)
            {
                GetDriver().ClickAt(By.Id("cmb_fleche"), this);
                GetDriver().ClickAt(GetObjectTypeBy(text), this);
            }
        }
    }
}
