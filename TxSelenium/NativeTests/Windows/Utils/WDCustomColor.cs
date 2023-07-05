using OpenQA.Selenium;
using System;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using OpenQA.Selenium.Support.PageObjects;

namespace TxSelenium.NativeTests.Windows.Utils
{
    public class WDCustomColor : WERefreshed
    {
        By elem;
        public WDCustomColor(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
            elem = elemIdentifier;
        }
        public By path(string ColorName)
        {
            return By.XPath(".//label[starts-with(@id,'mceu') and text()=\""+ ColorName + "\"]/../input");
        }

        [TxAction("Red", "Set Red Color.")]
        public WEGInput SetRed()
        {
            return new WEGInput(GetDriver(), path("R"), this);
        }

        [TxAction("Green", "Set Green Color.")]
        public WEGInput SetGreen()
        {
            return new WEGInput(GetDriver(), path("G"), this);
        }

        [TxAction("Blue", "Set Blue Color.")]
        public WEGInput SetBlue()
        {
            return new WEGInput(GetDriver(), path("B"), this);
        }

        /// <summary>
        /// Top value 0 for down and 100 for up
        /// </summary>
        /// <param name="top"></param>
        [TxAction("MoveColorPicker", "Set Custom Color")]
        public void MoveColorPicker(double top)
        {
            IWebElement button = GetDriver().WaitForElement(new ByChained(By.ClassName("mce-colorpicker-h"), By.ClassName("mce-colorpicker-h-marker"))).GetElement();
            int Position = Convert.ToInt32(2.2 * top);
            GetDriver().ClickAndHold(button);
            GetDriver().MouseMove(0, 225);
            GetDriver().Release();
            GetDriver().ClickAndHold(button);
            GetDriver().MouseMove(0, -Position);
            GetDriver().Release();
        }

        [TxAction("Validate", "Click on OK or cancel")]
        public void Validate(bool Validate = true)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"mce-container-body\"]"), this);
            if (Validate)
                GetDriver().ClickAt(By.XPath(".//button/span[text()=\"Ok\"]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//button/span[text()=\"Cancel\"]"), this);
        }
    }
}