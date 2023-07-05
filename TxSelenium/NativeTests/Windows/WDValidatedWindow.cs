using TxSelenium.NativeTests.WebElements;
using OpenQA.Selenium;
using XmlExecutor.Attributes;
using System.Threading;
using TxSelenium.Utils;
using System;

namespace TxSelenium.NativeTests.Windows
{
	public class WDValidatedWindow<T>  : WDWindow<T> where T : WERefreshed
	{
		private By okBy;
		private By cancelBy;

        public WDValidatedWindow(TxWebDriver driver, T content, WERefreshed parent=null, By frameby=null)
            : base(driver, content,parent,frameby)
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
		public void ValidateOk(bool validate = true, bool multipleWindowActive = false,int windowIndex=2)
        {
            //To fix the units problem 
            //TODO Explore more this problem
            Thread.Sleep(1000);
			By buttonBy = validate ? okBy : cancelBy;
            if (multipleWindowActive)
            {
                WERefreshed lastWindow = new WERefreshed(GetDriver(), By.XPath(".//*[@class = \"dhtmlx_window_active\"] " +
                    "| .//*[@class = \"dhxwin_active\"]["+windowIndex+"]"));
                GetDriver().ClickAt(buttonBy, lastWindow);
            }
            else
            {
                try
                {
                    GetDriver().ClickAt(buttonBy, this);
                }
                catch (Exception)
                {
                    if (!IsWindowMaximized())
                    {
                        MinMaxedWindow();
                        GetDriver().FindElement(this.GetElement(), buttonBy).Click();
                    }
                    else
                    {
                        GetDriver().Driver.Manage().Window.Maximize();
                        GetDriver().FindElement(this.GetElement(), buttonBy).Click();
                    }
                }
                
            }
            GetDriver().WaitForAjax();
        }

        [TxAction("ReadHeaderName", "")]
        public string ReadHeaderName(bool currentwindow=true)
        {
            WaitForAjax();
            if (currentwindow)
            {
                WaitForAjax();
              return  GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxwin_active\"][2]//div[contains(@class,\"dhxwin_text_inside\")]")).GetElement().Text;
            }
            else
                return GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhxwin_text_inside\")]")).GetElement().Text;
        }

        [TxAction("Suivant", "Closes the window by clicking on Ok or Cancel or the equivalent.")]
        public void Suivant()
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"next\")]"), this);
        }

        [TxAction("Delete", "To delete all information")]
        public void Delete()
        {
            GetDriver().ClickAt(By.Id("delete"), this);
        }

        [TxAction("ChangeObjectLocking", "To Validate all information")]
        public void ChangeObjectLocking()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id='lockObject']"), this);
        }
        [TxAction("IsCountDownStarted", "")]
        public bool IsCountDownStarted()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,'idDivCountDown')]"), this.GetElement());
        }
        [TxAction("Validate", "To Validate all information")]
        public void Validate()
        {
            GetDriver().ClickAt(By.XPath(".//input[@value='Validate']"), this);
        }

        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//*[@value=\"" + buttonName + "\" and @disabled]"), this.GetElement());
        }
        [TxAction("Save", "To Validate all information")]
        public void Save()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id='idBtnValidate']"), this);
        }
    }
}

