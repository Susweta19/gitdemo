using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TxSelenium.Loggers;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows
{
    public class WDInterface : IDisposable
    {

        private static readonly By password = By.Id("idPassword");
        private static readonly By login = By.XPath(".//div[@id=\"idDivSelectUsername\"]");
        private static readonly By langage = By.XPath(".//div[@id=\"selectBoxLanguage\"]");
        private static readonly By loginButton = By.Id("idBtnConnect");
        private static readonly By loginInfo = By.ClassName("info");
        private static readonly By mainWindowBy = By.ClassName("dhxlayout_cont");

        public TxWebDriver Driver { get; private set; }

        public WDInterface(TxWebDriver driver)
        {
            this.Driver = driver;
        }

        /// <summary>
        /// Fills out the login page and logs in.
        /// </summary>
        /// <param name="login">the login</param>
        /// <param name="password">the password</param>
        [TxAction("Login", "Fills the login form an validates it.")]
        public void Login(string login, string password, string langage = null)
        {
            new WEGDHtmlxComboBox(Driver, WDInterface.login).SelectOption(login);
            new WEGInput(Driver, WDInterface.password).Fill(password);
            LogsService.Log("Login : Selected option", Loggers.LogLevel.TRACE);
            LogsService.Log("Login : Filled password", Loggers.LogLevel.TRACE);

            if (langage != null)
            {
                new WEGDHtmlxComboBox(Driver, WDInterface.langage).SelectOption(langage);
                LogsService.Log("Login : Set langage", Loggers.LogLevel.TRACE);
            }

            Driver.ClickAt(WDInterface.loginButton);
            LogsService.Log("Login : Clicked on Login button", Loggers.LogLevel.TRACE);
           
            if (!Driver.WaitForElementToDisapear(WDInterface.password))
            {
                LogsService.Log("Login : Password element did not disapear", Loggers.LogLevel.WARNING);
                IWebElement info = Driver.WaitForElement(WDInterface.loginInfo).GetElement();
                LogsService.Log("Login : Got info element", Loggers.LogLevel.WARNING);
                throw new Exception(info.Text);
            }
            Thread.Sleep(2000);
            Driver.WaitForAjax();
        }

        /// <summary>
        /// Gets the main window object.
        /// </summary>
        /// <returns>the main window object</returns>
        [TxAction("MainWindow", "The main interface handle.")]
        public WDMainWindow GetMainWindow()
        {
            Driver.SwitchToContent();
            Driver.WaitForAjax();
            Driver.Driver.Manage().Window.Maximize();
            return new WDMainWindow(this.Driver, WDInterface.mainWindowBy);
        }


        public void Dispose()
        {
            ((IDisposable)Driver).Dispose();
        }
    }
}
