using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTManageDate : WDate
    {
        public GTManageDate(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        ////enum Months { Jan,Feb,Mar,Arp,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec};

        ////[TxAction("SelectPreviousDay", "To Select Previous Day")]
        ////public void SelectPreviousDay(int numDay)
        ////{
        ////    int date = DateTime.Today.AddDays(-numDay).Day;
        ////    if (numDay <= 30)
        ////    {
        ////        if (numDay > DateTime.Today.Day)
        ////            Prev();
        ////            Day(Convert.ToString(numDay));
        ////    }
        ////    else
        ////        throw new Exception("Date should be less than 30 days or equal to 30");
        ////}

        //[TxAction("ReadDay", "")]
        //public string ReadDay()
        //{
        //    WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//li[@class='dhtmlxcalendar_cell dhtmlxcalendar_cell_month_date']"), this);
        //    string res = elem.GetElement().Text;
        //    return res;
        //}

        

        //[TxAction("ReadYear", "")]
        //public string ReadYear()
        //{
        //    WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//span[@class='dhtmlxcalendar_month_label_year']"), this);
        //    string res = elem.GetElement().Text;
        //    return res;
        //}

        //[TxAction("ReadMonth", "")]
        //public string ReadMonth()
        //{
        //    WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//span[@class='dhtmlxcalendar_month_label_month']"), this);
        //    string res = elem.GetElement().Text;
        //    return res;
        //}

        ////[TxAction("Today", "Click on TOday in Calender")]
        ////public void Today()
        ////{
        ////    GetDriver().ClickAt(By.XPath(".//span[starts-with(@class,\"dhtmlxcalendar_label_today\")]"), this);
           
        ////}
    }
}

