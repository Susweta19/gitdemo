using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using System;
using System.Globalization;

namespace TxSelenium.NativeTests.Windows.Utils
{
    public class WDModificationDate : WERefreshed
    {
        public WDModificationDate(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("FromDate", "Select From Date")]
        public WDate FromDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"divBoundFilter\"]/div[1]//img[contains(@src,\"calendar.png\")]"));
            //return new WDate(GetDriver(), By.X("ui-datepicker-div"));
            return new WDate(GetDriver(), By.XPath(".//div[contains(@class,\"dhtmlxcalendar_in_input\")]"));
        }

        [TxAction("ToDate", "Select To Date")]
        public WDate To()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"divBoundFilter\"]/div[2]//img[contains(@src,\"calendar.png\")]"));
            return new WDate(GetDriver(), By.XPath(".//div[contains(@class,\"dhtmlxcalendar_in_input\")]"));
        }

        [TxAction("Ok", "Click On Ok")]
        public void Ok()
        {
            GetDriver().ClickAt(By.XPath(".//button[@id=\"popupButtonValid\" and text()=\"Ok\"]"));
        }

        [TxAction("Cancel", "Click On Cancel")]
        public void Cancel()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"divBoundFilter\"]/button[text()=\"Cancel\"]"));
        }

        [TxAction("Delete", "Click On Delete")]
        public void Delete()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"divBoundFilter\"]/button[text()=\"Delete\"]"));
        }

        [TxAction("GetCurrentMonth", "GetCurrentMonth in three letter")]
        public string GetCurrentMonth()
        {
            return DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture).Substring(0, 3);
        }
    }
}
