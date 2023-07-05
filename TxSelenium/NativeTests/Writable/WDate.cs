using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XmlExecutor.DataTypes;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Writable
{
    public class WDate : WERefreshed
    {
        #region Calendar_Elements
        public static By Dynamic_Calendar_Interface = By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]");
        public static By Static_Calendar_Interface = By.XPath(".//div[@class=\"dhtmlxcalendar_material\" and not(contains(@style,\"display: none\"))]");

        private By Today_Button = By.XPath(".//span[@class=\"dhtmlxcalendar_label_today\"]");
        private By Clear_Button = By.XPath(".//span[@class=\"dhtmlxcalendar_label_clear\"]");
        private By Next_YearList_Button = By.XPath(".//td[contains(@class,\"dhtmlxcalendar_selector_cell_right\")]");
        private By Previous_YearList_Button = By.XPath(".//td[contains(@class,\"dhtmlxcalendar_selector_cell_left\")]");
        private By Prev_Button = By.XPath(".//div[@class=\"dhtmlxcalendar_month_cont\"]/ul/li/div[1]");
        private By Next_Button = By.XPath(".//div[@class=\"dhtmlxcalendar_month_cont\"]/ul/li/div[2]");
        private By OK_Button = By.XPath(".//button[text() = \"Done\"] | .//button[text() = \"Ok\"]");

        private By Month_Label = By.XPath(".//span[starts-with(@class,\"dhtmlxcalendar_month_label_month\")]");
        private By Year_Label = By.XPath(".//span[@class=\"dhtmlxcalendar_month_label_year\"]");
        private By Hour_Label = By.XPath(".//span[@class='dhtmlxcalendar_label_hours']");
        private By Minutes_Label = By.XPath(".//span[@class='dhtmlxcalendar_label_minutes']");
        private By Highlighted_Day = By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month_date\")]//div[(contains(@class,\"dhtmlxcalendar_label\"))]");

        private By Month_List_Item = By.XPath(".//div[starts-with(@class,\"dhtmlxcalendar_area_selector_month\")]//li");
        private By Enabled_Day_Item = By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell_month_date_today\")]/..//li");
        private By WeekDays_List_Item = By.XPath(".//div[@class=\"dhtmlxcalendar_days_cont\"]//li[starts-with(@class,\"dhtmlxcalendar_cell\") and not(contains(@class,\"_wn\"))]");

        private By cursorHour = By.XPath(".//div[starts-with(@class,\"ui_tpicker_hour_slider ui-sli\")]");
        private By cursorMin = By.XPath(".//div[starts-with(@class,\"ui_tpicker_minute_slider ui-sli\")]");
        private By cursorSec = By.XPath(".//div[starts-with(@class,\"ui_tpicker_second_slider ui-sli\")]");
        #endregion

        public WDate(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        #region Private_Methods
        private string GetDayItem(string date)
        {
            return ".//li[@class=\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month_date\" or @class=\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month_date dhtmlxcalendar_cell_month_date_today\"]//div[text()=\"" + date + "\"]";
        }

        private string GetMonthItem(string month)
        {
            return ".//div[@class=\"dhtmlxcalendar_area_selector_month\"]//li[text()=\"" + month + "\"]";
        }
        
        private string GetYearItem_ByYear(string year)
        {
            return ".//div[@class=\"dhtmlxcalendar_area_selector_year\"]//li[text()=\"" + year + "\"]";
        }

        private string GetYearItem_ByIndex(int lineIndex = 1,int yearIndex=1)
        {
            return ".//div[@class=\"dhtmlxcalendar_area_selector_year\"]/ul["+lineIndex+"]/li[" + yearIndex + "]";
        }

        private string GetHourItem(string hour)
        {
            return ".//div[@class='dhtmlxcalendar_area_selector_hours']//li[text()=\"" + hour + "\"]";
        }

        private string GetMinuteItem(string minute)
        {
            return ".//div[@class='dhtmlxcalendar_area_selector_minutes']//li[text()=\"" + minute + "\"]";
        }

        private string GetDisabledWeekDayItem(string day)
        {
            return ".//li[contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month_dis\") or contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_dis\")]//*[text()=\"" + day + "\"]";
        }

        private DateTime GetPreviousWeekDay(DateTime date, DayOfWeek dow)
        {
            int currentDay = (int)date.DayOfWeek, gotoDay = (int)dow;
            return date.AddDays(-7).AddDays(gotoDay - currentDay);
        }

        private DateTime GetNextWeekDay(DateTime date, DayOfWeek dow)
        {
            int currentDay = (int)date.DayOfWeek, gotoDay = (int)dow;
            return date.AddDays(7).AddDays(gotoDay - currentDay);
        }
        #endregion

        [TxAction("Today", "")]
        public void Today()
        {
            GetDriver().ClickAt(Today_Button, this);
        }

        [TxAction("ReadMonthNames", "Reads all the month names from Calender")]
        public ActionColl<string> ReadMonthNames()
        {
            GetDriver().ClickAt(Month_Label);
            List<IWebElement> monthName = this.GetElement().FindElements(Month_List_Item).ToList();
            List<string> month = new List<string>();
            for (int i = 0; i < monthName.Count; i++)
                month.Add(monthName.ElementAt(i).Text);
            return new ActionColl<string>(month);
        }

        [TxAction("Clear", "")]
        public void Clear()
        {
            GetDriver().ClickAt(Clear_Button, this);
        }

        [TxAction("IsCurrentDateSelected", "Checks weather the current date is selected in the calender or not")]
        public bool IsCurrentDateSelected(string currentDate)
        {
            return GetDriver().IsElementPresent(By.XPath(GetDayItem(currentDate)));
        }
        
        [TxAction("Month", "Select the month in the calendar.")]
        public void Month(string month, string cultureName = "en-GB")
        {
            month = month.All(Char.IsDigit) ? new CultureInfo(cultureName).DateTimeFormat.GetMonthName(Convert.ToInt32(month)).Substring(0, 3) : month;
            GetDriver().ClickAt(Month_Label, this);
            GetDriver().ClickAt(By.XPath(GetMonthItem(month)), this);
        }

        [TxAction("Year", "Select the year in the calendar.")]
        public void Year(string year)
        {
            GetDriver().ClickAt(Year_Label, this);
            int minYear = Convert.ToInt32(GetDriver().WaitForElement(By.XPath(GetYearItem_ByIndex()),this).GetElement().Text);
            int maxYear = minYear + 11;
            while (!(minYear <= Convert.ToInt32(year) && Convert.ToInt32(year) <= maxYear))
            {
                if (minYear < Convert.ToInt32(year))
                {
                    GetDriver().ClickAt(Next_YearList_Button, this);
                    minYear = minYear + 12;
                    maxYear = maxYear + 12;
                }
                else
                {
                    GetDriver().ClickAt(Previous_YearList_Button, this);
                    minYear = minYear - 12;
                    maxYear = maxYear - 12;
                }
            }
            GetDriver().ClickAt(By.XPath(GetYearItem_ByYear(year)), this);
        }
        
        [TxAction("Hour", "Select the day in the calendar between 00 -> 23.")]
        public void Hour(string hour)
        {
            GetDriver().ClickAt(Hour_Label, this);
            GetDriver().ClickAt(By.XPath(GetHourItem(hour)), this);
        }

        [TxAction("Minute", "Select the day in the calendar between 00 -> 55.")]
        public void Minute(string minute)
        {
            GetDriver().ClickAt(Minutes_Label, this);
            GetDriver().ClickAt(By.XPath(GetMinuteItem(minute)), this);
        }

        [TxAction("Prev", "Click on prev button.")]
        public void Prev()
        {
            GetDriver().ClickAt(Prev_Button, this);
        }

        [TxAction("Next", "Click on next button.")]
        public void Next()
        {
            GetDriver().ClickAt(Next_Button, this);
        }

        [TxAction("Ok", "Click on done button.")]
        public void Done()
        {
            GetDriver().ClickAt(OK_Button, this);
        }

        [TxAction("ReadEnabledWeekDays", "Reads all the enabled dates in calender")]
        public ActionColl<string> ReadEnabledWeekDays()
        {
            List<IWebElement> dates = this.GetElement().FindElements(Enabled_Day_Item).ToList();
            List<string> weekdays = new List<string>();
            for (int i = 0; i < dates.Count; i++)
                weekdays.Add(dates.ElementAt(i).Text);
            return new ActionColl<string>(weekdays);
        }

        [TxAction("IsWeekDaysTranslated", "")]
        public bool IsWeekDaysTranslated(string cultureName = "en-GB")
        {
            bool translated = true;
            CultureInfo culture = new CultureInfo(cultureName);
            List<string> actualDays = new List<string>() {
                culture.DateTimeFormat.GetDayName(DayOfWeek.Monday).ToString().Substring(0, 2).ToLower(),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Tuesday).ToString().Substring(0, 2).ToLower(),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Wednesday).ToString().Substring(0, 2).ToLower(),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Thursday).ToString().Substring(0, 2).ToLower(),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Friday).ToString().Substring(0, 2).ToLower(),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Saturday).ToString().Substring(0, 2).ToLower(),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Sunday).ToString().Substring(0, 2).ToLower()};

            ICollection<IWebElement> element = this.GetElement().FindElements(WeekDays_List_Item);
            List<string> weekdays = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                weekdays.Add(element.ElementAt(i).Text.ToLower());

            for (int i = 0; i < element.Count; i++)
                if (weekdays[i] != actualDays[i].ToLower())
                {
                    translated = false;
                    break;
                }
            return translated;
        }

        [TxAction("ReadDisplayedYear", "")]
        public string ReadDisplayedYear()
        {
            return GetDriver().WaitForElement(Year_Label, this).GetElement().Text;
        }

        [TxAction("ReadDisplayedMonth", "")]
        public string ReadDisplayedMonth()
        {
            return GetDriver().WaitForElement(Month_Label, this).GetElement().Text;
        }

        [TxAction("ReadDisplayedDate", "")]
        public string ReadDisplayedDate()
        {
            return GetDriver().WaitForElement(Highlighted_Day, this).GetElement().Text;
        }

        [TxAction("SelectPreviousDay", "To Select Previous Day")]
        public void SelectPreviousDay(string numDay)
        {
            int day = Convert.ToInt32(numDay);
            int date = DateTime.Today.AddDays(-day).Day;
            if (day <= 30)
            {
                if (day > DateTime.Today.Day)
                    Prev();
                Day(Convert.ToString(date));
            }
            else
                throw new Exception("Date should be less than 30 days or equal to 30");
        }
        
        [TxAction("IsWeekDayDisabled", "Reads all the enabled dates in calender")]
        public bool IsWeekDayDisabled(string day)
        {
            return GetDriver().IsElementPresent(By.XPath(GetDisabledWeekDayItem(day)));
        }

        [TxAction("SelectPreviousFriday", "To Select Previous Friday")]
        public void SelectPreviousFriday()
        {
            DateTime fridayDate = GetPreviousWeekDay(DateTime.Now, DayOfWeek.Friday);
            if (fridayDate.Month < DateTime.Now.Month)
                Prev();
            Day(fridayDate.Day.ToString());
        }
        
        [TxAction("IsWeekDaysDisabledByWeekNumber", "Reads all the enabled dates in calender")]
        public bool IsWeekDaysDisabledByWeekNumber(string day, string weekNumber)//need to change
        {
            int week = Convert.ToInt32(weekNumber);
            if (week > 0)
                return GetDriver().IsElementPresent(By.XPath(".//li[@class=\"dhtmlxcalendar_cell_wn\"]//*[ text()=\"" + weekNumber + "\"]//li[contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month_dis\")]//*[text()=\"" + day + "\"]"));
            else
                return GetDriver().IsElementPresent(By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell_month_date_today\")]/ancestor::ul//li[contains(@class,\"_dis\")]//*[text()=\"" + day + "\"]"));
        }

        [TxAction("Day", "Select the day in the calendar between 1 -> 31.")]
        public void Day(string day)//need to change
        {
            bool isElemPresent = GetDriver().IsElementPresent(By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month\") and not(contains(@class,\"_dis\"))]/div"), this.GetElement());
            WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\" ]|.//*[@id=\"ui-datepicker-div\"]"));
            if (isElemPresent)
            {
                if (this.FrameBy != null)
                    ele = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\" ]|.//*[@id=\"ui-datepicker-div\"]"), null, this.FrameBy);
                GetDriver().ClickAt(By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month\")]/div[text()=\"" + day + "\"]"), ele);
            }
            else
                GetDriver().ClickAt(By.XPath(".//li[contains(@class,\"dhtmlxcalendar_cell dhtmlxcalendar_cell_month\")]/div[text()=\"" + day + "\"]"), ele);
        }
    }
}
/*
 * /// <summary>
        /// 
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        [TxAction("DragTimeMouse", "Drag The Time Using Mouse Movement")]
        public void DragTime(int hour, int minute, int second)
        {
            //Dragging For Hour
            IWebElement cursorh = GetDriver().WaitForElement(cursorHour, this).GetElement();
            GetDriver().ClickAndHold(cursorh);
            int h = EffectiveTime()[0];
            int i = 1;
            if (h > hour)
            {
                while (h != hour)
                {
                    GetDriver().MouseMove(-i, 0);
                    h = EffectiveTime()[0];
                }
            }
            else
            {
                while (h != hour)
                {
                    GetDriver().MouseMove(i, 0);
                    h = EffectiveTime()[0];
                }
            }
            GetDriver().Release();
            // Dragging For Minute       
            IWebElement cursorm = GetDriver().WaitForElement(cursorMin, this).GetElement();
            GetDriver().ClickAndHold(cursorm);
            int m = EffectiveTime()[1];
            if (m > minute)
            {
                while (m != minute)
                {
                    GetDriver().MouseMove(-i, 0);
                    m = EffectiveTime()[1];
                }
            }
            else
            {
                while (m != minute)
                {
                    GetDriver().MouseMove(i, 0);
                    m = EffectiveTime()[1];
                }
            }

            GetDriver().Release();
            // Dragging For Second
            IWebElement cursors = GetDriver().WaitForElement(cursorSec, this).GetElement();
            GetDriver().ClickAndHold(cursors);
            int s = EffectiveTime()[2];
            if (s > second)
            {
                while (s != second)
                {
                    GetDriver().MouseMove(-i, 0);
                    s = EffectiveTime()[2];
                }
            }
            else
            {
                while (s != second)
                {
                    GetDriver().MouseMove(i, 0);
                    s = EffectiveTime()[2];
                }
            }
            GetDriver().Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] EffectiveTime()
        {
            string time = GetDriver().WaitForElement(By.ClassName("ui_tpicker_time"), this).GetElement().Text;
            string[] seperatedTime = time.Split(':');

            int[] seperatedTimeConv = { Convert.ToInt32(seperatedTime[0]), Convert.ToInt32(seperatedTime[1]), Convert.ToInt32(seperatedTime[2]) };

            return seperatedTimeConv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        [TxAction("DragTime", "Using Arrow Key Press")]
        public void DragTimePrerssKey(int hour, int minute, int second)
        {
            //Hour
            GetDriver().ClickAt(cursorHour);
            for (int i = 100; i > 0; i--)
                GetDriver().PressKey(Keys.Left);
            for (int i = 0; i < hour; i++)
                GetDriver().PressKey(Keys.Right);
            //Minute
            GetDriver().ClickAt(cursorMin);
            for (int i = 100; i > 0; i--)
                GetDriver().PressKey(Keys.Left);
            for (int i = 0; i < minute; i++)
                GetDriver().PressKey(Keys.Right);
            //Second
            GetDriver().ClickAt(cursorSec);
            for (int i = 100; i > 0; i--)
                GetDriver().PressKey(Keys.Left);
            for (int i = 0; i < second; i++)
                GetDriver().PressKey(Keys.Right);

        }

    */