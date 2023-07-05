using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.Utils
{
    public class CommonMethods
    {
        [TxAction("GetDayOfWeek", ".....")]
        public int GetDayOfWeek(string date)
        {
            DateTime currentDateTime = Convert.ToDateTime(date);
            int week = (int)currentDateTime.DayOfWeek;

            return week;
        }

        [TxAction("GetCurrentWeekNumber", "")]
        public string GetCurrentWeekNumber(bool twodigit = false)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if (weekNum < 10 && twodigit)
            {
                string weekNumber = "0" + weekNum.ToString();
                return weekNumber;
            }
            else
                return weekNum.ToString();
        }

        [TxAction("GetNextOrPreviousWeekNumber", "")]
        public string GetNextOrPreviousWeekNumber(bool next = true)
        {
           
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
         
            string nam;
            if (next)

            {
                weekNum = weekNum + 1;
              nam = weekNum.ToString();
                return nam;
            }
            else
            {
               // weekNum = Convert.ToInt32(nam);
                weekNum = weekNum-1;
                nam = weekNum.ToString();
                return nam;
            }
        }

        [TxAction("GetCurrentDayOfWeek", "Current Day of week")]
        public int GetCurrentDayOfWeek()
        {
            return Convert.ToInt32(DateTime.Now.DayOfWeek);
        }

        [TxAction("CurrentShortDay", "Current Day of week")]
        public string CurrentShortDay(string cultureName = "en-GB",int daylenth=3)
        {
            CultureInfo culture = new CultureInfo(cultureName);
         
            return culture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek).ToString().Substring(0, daylenth);
        }

        [TxAction("CurrentMonth", "Current Month")]
        public string CurrentMonth(string cultureName = "en-GB", int monthLength = 3)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            if (monthLength == -1)
                return culture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            else
                return culture.DateTimeFormat.GetMonthName(DateTime.Now.Month).Substring(0, monthLength);

        }

        [TxAction("GetNextMonthName", "Next Month")]
        public string GetNextMonthName(string cultureName = "en-GB", int monthLength = 3)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            string monthName = culture.DateTimeFormat.GetMonthName((DateTime.Now.Month == 12) ? 1 : (DateTime.Now.Month) + 1);
            return monthLength == -1 ? monthName : monthName.Substring(0, monthLength);
        }

        [TxAction("GetPreviousMonthName", "Previous Month")]
        public string GetPreviousMonthName(string cultureName = "en-GB", int monthLength = 3)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            return monthLength == -1 ? culture.DateTimeFormat.GetMonthName((DateTime.Now.Month == 1) ? 12 : (DateTime.Now.Month) - 1) : culture.DateTimeFormat.GetMonthName((DateTime.Now.Month == 1) ? 12 : (DateTime.Now.Month) - 1).Substring(0, monthLength);
        }

        [TxAction("CurrentWeekFirstDay", "")]
        public string CurrentWeekFirstDay()
        {
            return DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.DayOfWeek) + 1).Day.ToString();
        }

        [TxAction("CurrentWeekLastDay", "Get Next Month day")]
        public string CurrentWeekLastDay()
        {
            return DateTime.Now.AddDays(7 - Convert.ToInt32(DateTime.Now.DayOfWeek)).Day.ToString();
        }

        [TxAction("CurrentWeekLastDayMonth", "Get Next Month day")]
        public string CurrentWeekLastDayMonth(string cultureName = "en-GB", int index = 3)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetMonthName(DateTime.Now.AddDays(7 - Convert.ToInt32(DateTime.Now.DayOfWeek)).Month).Substring(0, index);
        }

        [TxAction("CurrentWeekFirstdayMonth", "Get Next Month day")]
        public string CurrentWeekFirstdayMonth(string cultureName = "en-GB", int index = 3)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetMonthName(DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.DayOfWeek) + 1).Month).Substring(0, index);
        }

        [TxAction("GetNextDays", "Get Next Month day")]
        public string GetNextDays(int days = 1)
        {
            return DateTime.Now.AddDays(days).Day.ToString();
        }

        [TxAction("GetRequiredYear", "Gets the year reqd")]
        public string GetRequiredYear(int index)
        {
            var date = DateTime.Now.Year + (index);
            return  date.ToString();
        }

        [TxAction("GetPreviousWeekDays", "")]
        public ActionColl<int> GetPreviousWeekDays(bool twodegitday = false)
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 - 7);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day)
                .ToList();
            return new ActionColl<int>(daysThisWeek);
        }

        [TxAction("GetPreviousWeekDays2", "")]
        public ActionColl<string> GetPreviousWeekDays2(bool twodegitday = false)
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 - 7);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day.ToString())
                .ToList();
            return new ActionColl<string>(daysThisWeek);
        }

        [TxAction("GetNextWeekDays", "")]
        public ActionColl<int> GetNextWeekDays()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 + 7);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day)
                .ToList();
            var a = DateTime.Now.DayOfWeek;
            return new ActionColl<int>(daysThisWeek);
        }


        [TxAction("GetNextWeekDaysTwoDegit", "")]
        public ActionColl<string> GetNextWeekDaysTwoDegit()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 + 7);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day.ToString().Length > 1 ? monday.AddDays(d).Day.ToString() : "0" + monday.AddDays(d).Day.ToString())
                .ToList();
            var a = DateTime.Now.DayOfWeek;

            return new ActionColl<string>(daysThisWeek);
        }

        [TxAction("GetPrevioustWeekDaysTwoDegit", "")]
        public ActionColl<string> GetPrevioustWeekDaysTwoDegit()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 - 7);
            var daythisweek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day.ToString().Length > 1 ? monday.AddDays(d).Day.ToString() : "0" + monday.AddDays(d).Day.ToString()).ToList();
            return new ActionColl<string>(daythisweek);
        }

        [TxAction("GetPreviousWeekDaysMonth", "")]
        public ActionColl<string> GetPreviousWeekDaysMonth()
        {
           

                DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 - 7);
                var monthList = Enumerable.Range(0, 7)
                        .Select(d => monday.AddDays(d).ToString("MMMM", CultureInfo.InvariantCulture))
                        .ToList();
              
        

            return new ActionColl<string>(monthList);
        }
       

        [TxAction("GetNextWeekDaysMonth", "")]
        public ActionColl<string> GetNextWeekDaysMonth()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 + 7);
            var monthList = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).ToString("MMMM", CultureInfo.InvariantCulture))
                .ToList();
            return new ActionColl<string>(monthList);
        }

        [TxAction("GetRequiredDate", "")]
        public string GetRequiredDate(int dayIndex = 1)
        {
            return DateTime.Now.AddDays(dayIndex).ToShortDateString();
        }

        [TxAction("GetCurrentWeekDays", "")]
        public ActionColl<string> GetCurrentWeekDays(string date = "")
        {
            DateTime monday;
            if (date == "")
                monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            else
                monday = Convert.ToDateTime(date).AddDays(-((int)Convert.ToDateTime(date).DayOfWeek) + 1);

            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day.ToString())
                .ToList();
            return new ActionColl<string>(daysThisWeek);
        }

        [TxAction("GetCurrentWeekDaysTwoDegit", "")]
        public ActionColl<string> GetCurrentWeekDaysTwoDegit()
        {
            DateTime monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            var daythisweek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day.ToString().Length > 1 ? monday.AddDays(d).Day.ToString() : "0" + monday.AddDays(d).Day.ToString()).ToList();
            return new ActionColl<string>(daythisweek);
        }

        [TxAction("GetCurrentWeekDaysMonth", "")]
        public ActionColl<string> GetCurrentWeekDaysMonth(string date = "", int length = 0)
        {
            List<string> monthList = new List<string>();
            DateTime monday;
            if (date == "")
                monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            else
                monday = Convert.ToDateTime(date).AddDays(-((int)Convert.ToDateTime(date).DayOfWeek) + 1);
            if (length > 0)
                monthList = Enumerable.Range(0, 7)
                       .Select(d => monday.AddDays(d).ToString("MMMM", CultureInfo.InvariantCulture).Substring(0, length))
                       .ToList();
            else
                monthList = Enumerable.Range(0, 7)
                         .Select(d => monday.AddDays(d).ToString("MMMM", CultureInfo.InvariantCulture))
                         .ToList();
            return new ActionColl<string>(monthList);
        }

        [TxAction("GetCurrentWeekDaysYear", "")]
        public ActionColl<string> GetCurrentWeekDaysYear(string date = "")
        {
            List<string> yearList = new List<string>();
            DateTime monday;
            if (date == "")
                monday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1);
            else
                monday = Convert.ToDateTime(date).AddDays(-((int)Convert.ToDateTime(date).DayOfWeek) + 1);
           
            yearList = Enumerable.Range(0, 7)
                        .Select(d => monday.AddDays(d).ToString("yyyy", CultureInfo.InvariantCulture))
                        .ToList();
            return new ActionColl<string>(yearList);
        }

        [TxAction("SubstringData", "*****")]
        public string SubstringData(string data, int index)
        {
            return data.Substring(0, index);
        }

        [TxAction("MakeHashTag", "*****")]
        public string MakeHashTag(ActionColl<string> datas)
        {
            string result = "";
            foreach (string data in datas)
                result += data;
            return result;
        }

        [TxAction("GetNextDate", "Get Next Full Date")]
        public string GetNextDate(int days = 1, string format = "dd/MM/yyyy")
        {
            if(format.Equals("d"))
                return DateTime.Now.AddDays(days).Date.ToString(format+" ").Trim();
            else if(format.Equals("M"))
                return DateTime.Now.AddDays(days).Date.ToString(format + " ").Trim();
            else
                return DateTime.Now.AddDays(days).Date.ToString(format).Replace('-', '/');
        }
        [TxAction("GetCurrentHour", "Current 24-hour clock hour, with a leading Zero")]
        public string GetCurrentHour()
        {
            return DateTime.Now.ToString("HH");
        }
        [TxAction("GetDayOfWeekinString", ".....")]
        public string GetDayOfWeekinString(string date = "")
        {
            DateTime Cdate = (date == "") ? DateTime.Now : Convert.ToDateTime(date);
            return Cdate.DayOfWeek.ToString();
        }
    }
}
