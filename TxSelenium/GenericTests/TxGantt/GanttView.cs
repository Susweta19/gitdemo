using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using System.Globalization;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxGantt
{
    public class GanttView : WERefreshed
    {
        public GanttView(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("ReadTomorrowDaysMonthYear", "")]
        public bool ReadTomorrowDaysMonthYear(string cultureName = "en-GB")
        {
            CultureInfo culture = new CultureInfo(cultureName);
            culture.DateTimeFormat.GetMonthName(DateTime.Now.AddDays(1).Month).ToString();
            var text = GetDriver().WaitForElement(By.XPath(".//div[@class=\"gantt_scale_line day_scale\"]//div[starts-with(@class,\"gantt_scale_cell\")][2]"), this).GetElement().Text;
            string aa = culture.DateTimeFormat.GetMonthName(DateTime.Now.AddDays(1).Month).ToString();
            List<string> data = new List<string>();
            foreach (string date in text.Split(' '))
            {
                data.Add(date);
            }
            string TomorrowDatewithouttime = DateTime.Now.AddDays(1).ToString("MM/dd/yyy");
            List<string> datas = new List<string>();

            foreach (string date in TomorrowDatewithouttime.Split('/'))
            {
                datas.Add(date);
            }
            if (data[0] == datas[1] && data[1] == aa && data[2] == datas[2])

            {
                return true;
            }
            else
                return false;

        }
        [TxAction("GetcurrentWeekDays", "")]
        public ActionColl<int> GetcurrentWeekDays()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day)
                .ToList();
            var a = DateTime.Now.DayOfWeek;
            return new ActionColl<int>(daysThisWeek);
        }


        [TxAction("GetCurrentWeekMonth", "")]
        public ActionColl<string> GetCurrentWeekMonth()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            var monthList = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).ToString("MMMM", CultureInfo.InvariantCulture))
                .ToList();
            return new ActionColl<string>(monthList);

        }


        [TxAction("WeekNumberInYear", "")]
        public int WeekNumberInYear()
        {
            int weekNum = DateTime.Now.DayOfYear / 7;
            return weekNum;
        }


        [TxAction("ReadGanttHeader", "")]
        public ActionColl<string> ReadGanttHeader()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[@class=\"gantt_scale_line day_scale\"]"), this).GetElement().FindElements(By.XPath(".//div[starts-with(@class,\"gantt_scale\")]"));
            ICollection<string> GanttHeader = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                GanttHeader.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(GanttHeader);
        }

        [TxAction("ReadCurrentDate", "")]
        public string ReadCurrentDate()
        {
            /*IWebElement elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class='gantt_scale_cell subscale clNow']")).GetElement();
            GetDriver().ScrollToElement(elem);*/
            return GetDriver().WaitForElement(By.XPath(".//div[@class='gantt_scale_cell subscale clNow']")).GetElement().Text;
        }
        [TxAction("ReadGanttFirstHeader", "Read Header, First One")]
        public ActionColl<string> ReadGanttFirstHeader()
        {
            WaitForAjax();
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"gantt_task_scale\")]/div[contains(@class,\"gantt_scale_line\")][1]"), this).GetElement().FindElements(By.XPath(".//div[starts-with(@class,\"gantt_scale\")]"));
            ICollection<string> GanttHeader = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                GanttHeader.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(GanttHeader);
        }
        [TxAction("ReadGanttSecondHeader", "Read Header, Second One")]
        public ActionColl<string> ReadGanttSecondHeader()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"gantt_task_scale\")]/div[contains(@class,\"gantt_scale_line\")][2]"), this).GetElement().FindElements(By.XPath(".//div[starts-with(@class,\"gantt_scale\")]"));
            ICollection<string> GanttHeader = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                GanttHeader.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(GanttHeader);
        }

    }
}
