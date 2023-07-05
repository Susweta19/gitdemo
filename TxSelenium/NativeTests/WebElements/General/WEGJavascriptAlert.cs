using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements.General
{
    public class WEGJavascriptAlert 
    {
        TxWebDriver driver;

        public WEGJavascriptAlert(TxWebDriver driver)
        {
            this.driver = driver;
        }

        [TxAction("Accept", "")]
        public void Accept()
        {
            IAlert alert = driver.Driver.SwitchTo().Alert();
            alert.Accept();
        }

        [TxAction("Dismiss", "")]
        public void Dismiss()
        {
            driver.Driver.SwitchTo().Alert().Dismiss();
        }

        [TxAction("ReadMessage", "")]
        public string ReadMessage()
        {
            return driver.Driver.SwitchTo().Alert().Text;
        }

        [TxAction("SendKey", "")]
        public void SendKey(string value)
        {
            driver.Driver.SwitchTo().Alert().SendKeys(value);
        }
    }
}
