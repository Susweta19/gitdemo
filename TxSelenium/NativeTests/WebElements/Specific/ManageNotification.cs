using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.WebElements.Specific
{
    public class ManageNotification
    {
        private TxWebDriver Driver;
        public ManageNotification(TxWebDriver driver)
        {
            Driver = driver;
        }

        
        [TxAction("ReadNotificationFromNotificationBar", "")]
        public string ReadNotificationFromNotificationBar(string datanumber, bool expand= false)
        {
            if(expand)
            {
                Driver.ClickAt(By.XPath(".//img[contains(@src,\"24x24-wNotifications.png\")]|.//img[contains(@src,\"24x24-wNotificationsSolid.png\")]"));
                Driver.ClickAt(By.XPath(".//img[contains(@src,\"24x24-Expand.png\")]"));
                Thread.Sleep(2000);
                string aa = Driver.WaitForElement(By.XPath(".//div[starts-with(@id,\"notif\")and @data-number=\"" + datanumber + "\"]")).GetElement().Text.Replace("\r", "").Replace("\n", "");
                aa = aa.Replace("\\", "");
                Driver.ClickAt(By.XPath(".//img[contains(@src,\"24x24-wNotifications.png\")]|.//img[contains(@src,\"24x24-wNotificationsSolid.png\")]"));
                return aa;
            }
            else
            {
                Driver.ClickAt(By.XPath(".//img[contains(@src,\"24x24-wNotifications.png\")]|.//img[contains(@src,\"24x24-wNotificationsSolid.png\")]"));
                Thread.Sleep(2000);
                string aa = Driver.WaitForElement(By.XPath(".//div[starts-with(@id,\"notif\")and @data-number=\"" + datanumber + "\"]")).GetElement().Text.Replace("\r", "").Replace("\n", "");
                aa = aa.Replace("\\", "");
                Driver.ClickAt(By.XPath(".//img[contains(@src,\"24x24-wNotifications.png\")]|.//img[contains(@src,\"24x24-wNotificationsSolid.png\")]"));
                return aa;
            }            
        }

        [TxAction("ReadToastMessage", "")]
        public string ReadToastMessage()
        {
            Driver.WaitForAjax();
            try
            {
                WERefreshed message = Driver.WaitForElement(By.XPath(".//div[starts-with(@id,\"toaster\")] | .//div[contains(@class,\"dhtmlx-info dhtmlx-info\")]/div"));
                return message.GetElement().Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable To Get Toast Message.\nException : "+ex.ToString());
                return "Unable To Get Toast Message.";
            }
        }

        [TxAction("ReadNotificationText", "")]
        public string ReadNotificationText(bool byclick=false)
        {
            if (byclick)
            {
                Thread.Sleep(5000);
                Driver.WaitForElement(By.XPath(".//img[contains(@src,\"wNotificationsSolid.png\")] | .//img[contains(@src,\"wNotifications.png\")]")).GetElement().Click();
                return Driver.WaitForElement(By.XPath("//div[contains(@class,\"ToolbarNotifText\")]")).GetElement().Text.Replace("/", "");
            }
            else
            return Driver.WaitForElement(By.XPath("//div[contains(@class,\"ToolbarNotifText\")]")).GetElement().Text.Replace("/","");
        }

        [TxAction("DeleteAllNotification", "")]
        public void DeleteAllNotification()
        {

            Driver.WaitForElement(By.XPath(".//span[contains(@id,'notifDeleteAll1')]")).GetElement().Click();
        }

        [TxAction("IsNoNotificationPresent", "***")]
        public bool IsNoNotificationPresent()
        {
            Driver.WaitForElement(By.XPath(".//img[contains(@src,\"wNotificationsSolid.png\")] | .//img[contains(@src,\"wNotifications.png\")]")).GetElement().Click();
            return Driver.IsElementPresent(By.XPath(".//span[not(contains(@style,\"display: none;\")) and text()=\"No notification\"]"));
        }

    }
}
