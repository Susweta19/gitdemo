using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using TxSelenium.GenericTests.TxCalendar;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests
{
    public class GTTxCalendar : WERefreshed
    {

        //  public IWebDriver Driver { get; private set; }
        public GTTxCalendar(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            GetDriver().WaitForAjax();
        }
        ///<summmary>
        /// Save..
        ///</Summery>
        [TxAction("ReadCurrentMonth", "")]
        public string ReadCurrentMonth()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"idDivCalendarResourceManager\")]//span[@class=\"dhtmlxcalendar_month_label_month\"]"), this).GetElement().Text;
        }
        [TxAction("ReadCurrentYear", "")]
        public string ReadCurrentYear()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"idDivCalendarResourceManager\")]//span[@class=\"dhtmlxcalendar_month_label_year\"]"), this).GetElement().Text;
        }
        [TxAction("ReadCurrentDate", "")]
        public string ReadCurrentDate()
        {
            return GetDriver().WaitForElement(By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell_month_date_today\") or contains(@class,\"dhtmlxcalendar_cell_month_date\")]//div"), this).GetElement().Text;
        }

        //[TxAction("GetCurrentMonth", "GetCurrentMonth in three letter")]
        //public string GetCurrentMonth()
        //{
        //    return DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture).Substring(0, 3);
        //}

        [TxAction("ReadCalenderHeader", "")]
        public ActionColl<string> ReadCalenderHeader()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_cal_header\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"dhx_scale_bar\"]"));
            ICollection<string> CalenderHeaders = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                CalenderHeaders.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(CalenderHeaders);
        }

        [TxAction("GetCurrentWeekDays", "")]
        public ActionColl<int> GetCurrentWeekDays(string date = "")
        {
            DateTime monday;
            if (date == "")
                monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            else
                monday = Convert.ToDateTime(date).AddDays(-((int)Convert.ToDateTime(date).DayOfWeek) + 1);

            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day)
                .ToList();
            return new ActionColl<int>(daysThisWeek);
        }

        [TxAction("GetCurrentWeekDaysMonth", "")]
        public ActionColl<string> GetCurrentWeekDaysMonth(string date = "")
        {
            List<string> monthList = new List<string>();
            DateTime monday;
            if (date == "")
                monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            else
                monday = Convert.ToDateTime(date).AddDays(-((int)Convert.ToDateTime(date).DayOfWeek) + 1);

            monthList = Enumerable.Range(0, 7)
                   .Select(d => monday.AddDays(d).ToString("MMMM", CultureInfo.InvariantCulture))
                   .ToList();
            return new ActionColl<string>(monthList);
        }
        [TxAction("ReadWeekNumber", "")]
        public string ReadWeekNumber()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"dhxtoolbar_float_left\")]//div[@class=\"dhx_toolbar_text\"][1]"), this).GetElement().Text.Split('#').Last();
        }

        [TxAction("ReadNameOfView", "")]
        public string ReadNameOfView()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivTxToolbarButtonsRight\")]//div[@class=\"dhx_toolbar_text\"]//b"), this).GetElement().Text;
        }
        [TxAction("GetCurrentWeekNumber", "")]
        public string GetCurrentWeekNumber()
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday).ToString();
        }
        [TxAction("IsViewFullScreen", "")]
        public bool IsViewFullScreen()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idDivCellBManager\") and contains(@style,\"display: none;\")]"), this.GetElement());
        }
        [TxAction("ExpandCollapseCalender", "")]
        public void ExpandCollapseCalender()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"/24x24-wMainOptions.png\")]"), this);
        }
        [TxAction("ManageDate", "")]
        public WDate ManageDate()
        {
            return new WDate(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivCalendarResourceManager\")]/div[contains(@class,\"dhtmlxcalendar_material\") and not(contains(@style,\"display: none\"))]"), this);
        }
        [TxAction("Today", "")]
        public void Today()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-user-clock.png\")]"));
            GetDriver().WaitForAjax();
        }
        [TxAction("GoToPreviousWeek", "")]
        public void GoToPreviousWeek()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-chevron-circle-left.png\")]"), this);
            GetDriver().WaitForAjax();
        }
        [TxAction("GoToNextWeek", "")]
        public void GoToNextWeek()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-chevron-circle-right.png\")]"), this);
        }
        [TxAction("IsCurrentMonthDaysDisplayed", "")]
        public bool IsCurrentMonthDaysDisplayed()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivCellCManager\")]"), this).GetElement().FindElements(By.XPath(".//td[not(starts-with(@class,\"dhx_before \")) and not(starts-with(@class,\"dhx_after \"))]//div[@class=\"dhx_month_head\"]"));
            ICollection<string> totaldays = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                totaldays.Add(element.ElementAt(i).Text);

            List<int> actualdays = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToList();
            if (totaldays.Count == actualdays.Count())
            {
                for (int j = 0; j < totaldays.Count; j++)
                {
                    string actualDay = actualdays[j].ToString();
                    if (actualDay.Length < 2)
                        actualDay = "0" + actualDay;
                    if (!actualDay.Equals(totaldays.ElementAt(j)))
                        return false;
                }
                return true;
            }
            return false;
        }

        [TxAction("IsMonthDaysDisplayed", "")]
        public bool IsMonthDaysDisplayed(int year, int month)
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivCellCManager\")]"), this).GetElement().FindElements(By.XPath(".//td[not(starts-with(@class,\"dhx_before \")) and not(starts-with(@class,\"dhx_after \"))]//div[@class=\"dhx_month_head\"]"));
            ICollection<string> totaldays = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                totaldays.Add(element.ElementAt(i).Text);

            List<int> actualdays = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();
            if (totaldays.Count == actualdays.Count())
            {
                for (int j = 0; j < totaldays.Count; j++)
                {
                    string actualDay = actualdays[j].ToString();
                    if (actualDay.Length < 2)
                        actualDay = "0" + actualDay;
                    if (!actualDay.Equals(totaldays.ElementAt(j)))
                        return false;
                }
                return true;
            }
            return false;
        }

        [TxAction("SelectCalenderView", "")]
        public void SelectCalenderView(string viewMode)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"arwimg\"]"), this);
            GetDriver().ClickAt(By.XPath(".//div[@class=\"btn_sel_text\" and text()=\"" + viewMode + "\"]"));
        }

        [TxAction("AddTaskInCalender", ".....")]
        public WDValidatedWindow<AddTask> AddTaskInCalender(int ColumNumber, int rowNumber = 0, bool allowed = true)
        {
            WERefreshed dropElem;
            WERefreshed dragElem;
            if (rowNumber > 0)
            {
                dragElem = GetDriver().WaitForElement(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//tr[" + rowNumber + "]/td[" + ColumNumber + "]"), this);
                if (ColumNumber > 1)
                    dropElem = GetDriver().WaitForElement(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//tr[" + rowNumber + "]/td[" + (ColumNumber - 1) + "]"), this);
                else
                    dropElem = GetDriver().WaitForElement(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//tr[" + rowNumber + "]/td[" + (ColumNumber + 1) + "]"), this);
            }
            else
            {
                dragElem = GetDriver().WaitForElement(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//div[starts-with(@class,\"dhx_scale_holder\")][" + ColumNumber + "]"), this);
                if (ColumNumber > 1)
                    dropElem = GetDriver().WaitForElement(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//div[starts-with(@class,\"dhx_scale_holder\")][" + (ColumNumber - 1) + "]"), this);
                else
                    dropElem = GetDriver().WaitForElement(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//div[starts-with(@class,\"dhx_scale_holder\")][" + (ColumNumber + 1) + "]"), this);
            }
            GetDriver().DragAndDrop(dragElem, dropElem);
            GetDriver().WaitForAjax();

            if (allowed)
            {
                GetDriver().WaitForAjax();
                AddTask addtask = new AddTask(GetDriver(), WForm.WriteFormDiv);
                return new WDValidatedWindow<AddTask>(GetDriver(), addtask);
            }
            return null;
        }

        [TxAction("AddTaskInCalenderForReadOnlyMode", ".....")]
        public void AddTaskInCalenderForReadOnlyMode(int ColumNumber)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[starts-with (@class,\"dhx_cal_data\")]//div[starts-with(@class,\"dhx_scale_holder\")][" + ColumNumber + "]"), this);
        }

        //[TxAction("ReadTime", ".....")]
        //public string ReadTime(string ColumNumber)
        //{
        //    return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_cal_data\"]//div[starts-with(@class,\"dhx_scale_holder\")][" + ColumNumber + "]//div[@class=\"dhx_event_move dhx_title\"]")).GetElement().Text;
        //}

        [TxAction("ReadTaskName", ".....")]
        public string ReadTaskName(string ColumNumber)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_cal_data\"]//div[starts-with(@class,\"dhx_scale_holder\")][" + ColumNumber + "]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"dhx_body\"]")).Last().Text;
        }

        [TxAction("GetDayOfWeek", ".....")]
        public int GetDayOfWeek(string date)
        {
            DateTime currentDateTime = Convert.ToDateTime(date);
            int week = (int)currentDateTime.DayOfWeek;

            return week;
        }



        [TxAction("DoubleClickOnTask", ".....")]
        public void DoubleClickOnTask(string taskId)
        {
            Sleep(5);
            GetDriver().WaitForAjax();
            GetDriver().DoubleClickAt(By.XPath(".//div[@class=\"dhx_body\"and text()=\"" + taskId + "\"]"), this);
          //  GetDriver().WaitForAjax();
            Sleep(20);
        }

        [TxAction("ReturnAddTaskInCalender", ".....")]
        public WDValidatedWindow<AddTask> ReturnAddTaskInCalender()
        {
            //GetDriver().DoubleClickAt(By.XPath(".//div[@class=\"dhx_cal_data\"]//div[starts-with(@class,\"dhx_scale_holder\")][" + ColumNumber + "]"), this);
            AddTask addtask = new AddTask(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<AddTask>(GetDriver(), addtask);
        }

        [TxAction("ExpandCollaspeList", ".....")]
        public void ExpandCollaspeList(string listName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + listName + "\"]/../following-sibling::div[@class=\"dhx_cell_hdr_arrow\"]"), this);
        }

        [TxAction("ManageListBox", ".....")]
        public WMultipleLink ManageListBox(string listName)
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//span[text()=\"" + listName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/following-sibling::div[@class=\"dhx_cell_cont_acc\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
        }

        [TxAction("IsTaskNamePresent", ".....")]
        public bool IsTaskNamePresent(string ColumNumber, string taskName)
        {
            Console.WriteLine("Task name : " + taskName);
            bool aa= GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_cal_data\"]//div[starts-with(@class,\"dhx_scale_holder\")]//div[@class=\"dhx_body\" and text()=\"" + taskName + "\"]"), this.GetElement());
            int i = 0;
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_cal_data\"]//div[starts-with(@class,\"dhx_scale_holder\")]//div[@class=\"dhx_body\" and text()=\"" + taskName + "\"]"), this.GetElement());
        }

        [TxAction("ReadCalenderSummery", ".....")]
        public string ReadCalenderSummery()
        {
            string summery = "";
            ICollection<IWebElement> elems = GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxtoolbar_float_left\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"dhx_toolbar_text\" and not(contains(@style,\"display: none\"))]"));
            foreach (IWebElement elem in elems)
                summery += elem.Text;
            return summery;
        }

        [TxAction("ReadTime", ".....")]
        public string ReadTime(string ColumNumber, string taskName="")
        {

            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_cal_data\"]//div[starts-with(@class,\"dhx_scale_holder\")]//div[text()=\"" + taskName + "\"]/..//div[@class=\"dhx_event_move dhx_title\"]")).GetElement().Text;
        }
        [TxAction("ReadScaleHour", ".....")]
        public string ReadScaleHour(string row)
        {
           return GetDriver().WaitForElement(By.XPath(".//*[@class=\"dhx_scale_hour\"]/..//div["+row+"]")).GetElement().Text;
        }

        [TxAction("ClickOnTask", ".....")]
        public WDValidatedWindow<AddTask> ClickOnTask(string ColumNumber, string taskName)
        {
            GetDriver().WaitForAjax();
            Sleep(5);
            GetDriver().DoubleClickAt(By.XPath(".//div[@class=\"dhx_body\"and text()=\"" + taskName + "\"]"), this);
            GetDriver().WaitForAjax();
          //  Sleep(10);
            AddTask addtask = new AddTask(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<AddTask>(GetDriver(), addtask);
        }
    }
}
