using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using System.Threading;
using XmlExecutor.DataTypes;
using System.Globalization;
using TxSelenium.Utils;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTManageTask : WERefreshed
    {
        public GTManageTask(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("AddTask", "")]
        public WDValidatedWindow<GTAddTask> AddTask(int index = 0, string user = "")
        {
            if (user != "")
                GetDriver().ClickAt(By.XPath(".//span[@id=\"nodeval\"and text()=\"" + user + "\"]/ancestor::tr//img[contains(@src,\"AddObject.png\") and not(contains(@style,\"display: none\"))]"), this);
            else
            {
                GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"-AddObject.png\") and @idrow= \"" + index + "\"]"),this);
            }
            GetDriver().WaitForAjax();
            GTAddTask addtask = new GTAddTask(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"), this);
            return new WDValidatedWindow<GTAddTask>(GetDriver(), addtask);

        }

        [TxAction("TaskContextMenu", "")]
        public GTTaskContextMenu TaskContextMenu(string rowIndex = "", string colIndex = "2", string taskName = "")
        {
            WERefreshed element;
            if (rowIndex == "")
            {
                if (taskName != "")
                {
                    element = GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + colIndex + "]"), this);
                }
                else
                {
                    element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//span[@id='nodeval']"), this);
                }
            }
            else
            {
                if (colIndex.Contains("+"))
                {
                    string[] datas = colIndex.Split('+');
                    int col = 0;
                    foreach (string data in datas)
                        col += Convert.ToInt32(data);
                    colIndex = col.ToString();
                }
                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + colIndex + "]"), this);

            }
            Sleep(2);
            GetDriver().RightClick(element);
            Sleep(1);
            if (!GetDriver().IsElementPresent(By.XPath(".//div[@class and not(contains(@style,\"display: none\"))]//table[@class='dhtmlxMebu_SubLevelArea_Tbl']/tbody")))
            {
                GetDriver().RightClick(element);
                Sleep(1);
            }
            return new GTTaskContextMenu(GetDriver(), By.XPath(".//div[@class and not(contains(@style,\"display: none\"))]//table[@class='dhtmlxMebu_SubLevelArea_Tbl']/tbody"));
        }

        [TxAction("RightClickOnCurrentDateCell", "Right clicks on the cell under current date")]
        public GTTaskContextMenu RightClickOnCurrentDateCell(int rowNumber, string taskName = null)
        {

            WERefreshed element;
            int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
            if (taskName != null)
            {
                element = GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + taskName + "\"]/ancestor::tr//td[" + (columnNumber + 3) + "]"), this);
            }
           // SelectRow(taskName);
           else
                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowNumber + "]//td[" + (columnNumber + 3) + "]"), this);
            GetDriver().RightClick(element);
            return new GTTaskContextMenu(GetDriver(), By.XPath(".//div[@class and not(contains(@style,\"display: none\"))]//table[@class='dhtmlxMebu_SubLevelArea_Tbl']/tbody"));
        }

        [TxAction("RightClickOnCellBeforeCurrentDate", "Right clicks on the cell before current date")]
        public GTTaskContextMenu RightClickOnCellBeforeCurrentDate(int rowNumber)
        {

            WERefreshed element;
            int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
            element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowNumber + "]//td[" + ((columnNumber + 3) - 1) + "]"), this);
            GetDriver().RightClick(element);
            return new GTTaskContextMenu(GetDriver(), By.XPath(".//div[@class and not(contains(@style,\"display: none\"))]//table[@class='dhtmlxMebu_SubLevelArea_Tbl']/tbody"));
        }

        [TxAction("FillCellBeforeCurrentDate", "Writes in the cell before current date")]
        public WEGInput FillCellBeforeCurrentDate(int rowNumber)
        {
            int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
            GetDriver().ClickAt(By.XPath(".//tr[contains(@class,'_material')][" + rowNumber + "]//td[" + ((columnNumber + 3) - 1) + "]"));
            return new WEGInput(GetDriver(), By.XPath(".//tr[contains(@class,'_material')][" + rowNumber + "]//td[" + ((columnNumber + 3) - 1) + "]/input"));
        }

        [TxAction("RemoveTask", "")]
        public void RemoveTask(string taskName)
        {
            GetDriver().ClickAt(By.XPath(".//span[@id=\"nodeval\" and text()=\"" + taskName + "\"]/../../..//img[@class=\"clRemoveTxHourTrackingTask clIcon\"]"), this);
            WaitForAjax();
        }

        [TxAction("SelectRow", "")]
        public void SelectRow(string taskName)
        {
            GetDriver().ClickAt(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]/td[2]"), this);
        }

        [TxAction("IsRowSelected", "")]
        public bool IsRowSelected(string rowName)
        {
            bool res = GetDriver().IsElementPresent(By.XPath(".//tr[contains(@class,'rowselected')]/td[@title='" + rowName + "']"));
            return res;
        }

        [TxAction("IsTaskPresent", "Checks weather a task is present or not")]
        public bool IsTaskPresent(string taskName, int index = 1)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//tr[not(contains(@style,\"display: none;\"))]//span[@id=\"nodeval\" and text()=\"" + taskName + "\"][" + index + "]"), this.GetElement());
        }

        [TxAction("WriteHour", "")]
        public WEGInput WriteHour(string taskName, int colNum)
        {
            GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"objbox\"]//input"), this);
        }

        [TxAction("Normal", "")]
        public WEGInput Normal(string taskName, int colNum, bool click = true)
        {
            if (click)
            {
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
                Thread.Sleep(2000);
            }
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@class,\"clDivTimeTypesLine\")]/input[@stimetype='ttMachNorm']"));
        }

        [TxAction("NormalWork", "")]
        public WEGInput NormalWork(string taskName, int colNum, bool click = true)
        {
            if (click)
            {
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
                Thread.Sleep(2000);
            }
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_material\" and not(contains(@style,\"display: none;\"))]//label[text()=\"Normal Work:\"]/..//input"));
        }

        [TxAction("ReadCommentByTaskName", "")]
        public string ReadCommentByTaskName(string taskName, int colIndex)
        {
            WERefreshed element = GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + colIndex + "]"));
            Sleep(1);
            GetDriver().MouseOver(element);
            Sleep(1);
            string res = element.GetElement().GetAttribute("title");
            return res;
        }

        [TxAction("DelayedByCustomer", "")]
        public WEGInput DelayedByCustomer(string taskName, int colNum, bool click = true)
        {
            if (click)
            {
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
                Thread.Sleep(2000);
            }
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_material\" and not(contains(@style,\"display: none;\"))]//label[text()=\"Delay by Customer:\"]/../input"));
        }

        [TxAction("DelayedByLab", "")]
        public WEGInput DelayedByLab(string taskName, int colNum, bool click = true)
        {
            if (click)
            {
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
                Thread.Sleep(2000);
            }
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_material\" and not(contains(@style,\"display: none;\"))]//label[text()=\"Delay by Laboratory:\"]/../input"));
        }

        [TxAction("WriteUnderCurrentDateCell", "")]
        public WEGInput WriteUnderCurrentDateCell(string taskName, bool click = true)
        {
            if (click)
            {

                int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + (columnNumber + 3) + "]"), this);
                Thread.Sleep(2000);
            }
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@class,\"clDivTimeTypesLine\")]/input[@stimetype='ttMachNorm']"));
        }

        [TxAction("Maintenance", "")]
        public WEGInput Maintenance(string taskName, int colNum, bool click = true)
        {
            if (click)
            {
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
                Thread.Sleep(2000);
            }
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@class,\"clDivTimeTypesLine\")]/input[@stimetype='ttMachineMaintenance']"));
        }

        [TxAction("ReadCell", "")]
        public string ReadCell(int colIndex, int rowIndex = 0, string taskName = "")
        {
            string data;
            if (taskName != "")
            {
                WERefreshed element = GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + colIndex + "]"), this);
                data = element.GetElement().Text;
            }
            else
            {
                WERefreshed element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + colIndex + "]"), this);
                data = element.GetElement().Text;
            }

            return data;
        }


        [TxAction("ReadCellAfterDuplication", "")]
        public bool ReadCellAfterDuplication(int rowIndex, string duplicatedata)
        {
            int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
            for (int i = columnNumber; i < 7; i++)
                if (!ReadCell((i + 3), rowIndex).Equals(duplicatedata))
                    return false;

            if (columnNumber > 1)
                for (int i = columnNumber; i < 7; i++)
                    if (!ReadCell((i + 3), rowIndex).Equals(duplicatedata))
                        return false;
            return true;
        }

        [TxAction("ReadCellAfterDeletion", "")]
        public void ReadCellAfterDeletion(int rowIndex)
        {
            WERefreshed element;
            string data;
            int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
            for (int i = columnNumber; i <= 7; i++)
            {
                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + (i + 1) + "]"));
                data = element.GetElement().Text;
                data.Equals("");
            }
        }

        [TxAction("IfTaskPresent", "")]
        public void IfTaskPresent(string taskName = null, int index = 1)
        {
            WaitForAjax();
            int numberOfTaskPresent = this.GetElement().FindElements(By.XPath(".//tr[not(contains(@style,\"display: none;\"))]//img[contains(@src,\"-DeleteObject.png\")]")).Count;
            if (numberOfTaskPresent > 0)
            {
                if (taskName != null)
                {
                    if (IsTaskPresent(taskName))
                    {
                        RemoveTaskByName(taskName);
                        new WESPopUp(GetDriver()).ClosePopup(true);
                    }
                }
                else
                {
                    List<IWebElement> deleteButton = this.GetElement().FindElements(By.XPath(".//span[@id=\"nodeval\"]/ancestor::tr//img[contains(@src,\"DeleteObject.png\")]")).ToList();
                    foreach (IWebElement elem in deleteButton)
                    {
                        elem.Click();
                        new WESPopUp(GetDriver()).ClosePopup(true);
                        WaitForAjax();
                    }
                    WaitForAjax();
                }
                WaitForSaving();
            }
        }


        [TxAction("DeleteAllPresentTask", "")]
        public void DeleteAllPresentTask()
        {
            WaitForAjax();
            List<IWebElement> deleteButtons = this.GetElement().FindElements(By.XPath(".//tr[not(contains(@style,\"display: none;\"))]//img[contains(@src,\"-DeleteObject.png\") and not(contains(@style,\"display: none;\"))]")).ToList();
            if (deleteButtons.Count > 0)
            {
                foreach (IWebElement deleteButton in deleteButtons)
                {
                    deleteButton.Click();
                    new WESPopUp(GetDriver()).ClosePopup(true);
                    WaitForAjax();
                }
                GetDriver().ClickAt(By.XPath(".//div[@title=\"Save view times\"]"));
                Sleep(1);
            }
        }

        [TxAction("RemoveTaskByName", "")]
        public void RemoveTaskByName(string taskName, int index = 1)
        {
            GetDriver().ClickAt(By.XPath(".//span[@id=\"nodeval\" and text()=\"" + taskName + "\"][" + index + "]/ancestor::tr//img[contains(@src,\"-DeleteObject.png\")]"), this);
            WaitForAjax();
        }

        [TxAction("ReadMessage", "")]
        public string ReadMessage()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_popup_material\"and contains(@style,\"visibility: visible;\")]//div[starts-with(@id,\"dhxpopup_node\")]//label[not(contains(@class,\"cl\"))]")).GetElement().Text;
        }

        [TxAction("ReadViewDetailsInfo", "Reads the message appeared Object is Focused in Teexma")]
        public string ReadViewDetailsInfo()
        {
           string text= GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"alert alert-info alert-dismissible\")]")).GetElement().Text;
            text = text.Split('\n').Last();
            return text;
        }

        [TxAction("IsCurrentWeekDaysDisplayed", "")]
        public bool IsCurrentWeekDaysDisplayed()
        {
            List<string> currentWeekdays = new List<string>();
            DateTime monday;
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            if (dayOfWeek != 0)
                monday = DateTime.Now.AddDays(-dayOfWeek + 1);
            else
                monday = DateTime.Now.AddDays(-7);
            var daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d).Day)
                .ToList();
            int i = GetDriver().IsElementPresent(By.XPath(".//td[4]//h3"), this.GetElement()) ? 4 : 5;
            for (int j = i; j < (i + 7); j++)
            {
                WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//td[" + j + "]//h3"), this);
                currentWeekdays.Add(elem.GetElement().Text);
            }
            //to check two list have same data or not
            for (int k = 0; k < currentWeekdays.Count; k++)
                if (currentWeekdays[k] != daysThisWeek[k].ToString())
                    return false;
            return true;
        }


        [TxAction("IsPreviousWeekDaysDisplayed", "")]
        public bool IsPreviousWeekDaysDisplayed()
        {
            List<string> currentWeekdays = new List<string>();
            DateTime previousMonday;
            if ((int)DateTime.Now.DayOfWeek != 0)
                previousMonday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 - 7);//1 for sunday to monday and 7 for monday to monday
            else
                previousMonday = DateTime.Now.AddDays(-15);//on sunday,7 for monday to monday and 7 for monday to monday
            var daysPreviousWeek = Enumerable.Range(0, 7)
                .Select(d => previousMonday.AddDays(d).Day)
                .ToList();
            int i = GetDriver().IsElementPresent(By.XPath(".//td[4]//h3"), this.GetElement()) ? 4 : 5;
            for (int j = i; j < (i + 7); j++)
            {
                WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//td[" + j + "]//h3"), this);
                currentWeekdays.Add(elem.GetElement().Text);
            }
            //to check two list have same data or not
            for (int k = 0; k < currentWeekdays.Count; k++)
                if (currentWeekdays[k] != daysPreviousWeek[k].ToString())
                    return false;
            return true;
        }

        [TxAction("IsNextWeekDaysDisplayed", "")]
        public bool IsNextWeekDaysDisplayed()
        {
            List<string> currentWeekdays = new List<string>();
            DateTime nextMonday;
            if ((int)DateTime.Now.DayOfWeek != 0)
                nextMonday = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek) + 1 + 7);//1 for sunday to monday and 7 for monday to monday
            else
                nextMonday = DateTime.Now.AddDays(7);
            var daysNextWeek = Enumerable.Range(0, 7)
                .Select(d => nextMonday.AddDays(d).Day)
                .ToList();
            int i = GetDriver().IsElementPresent(By.XPath(".//td[4]//h3"), this.GetElement()) ? 4 : 5;
            for (int j = i; j < (i + 7); j++)
            {
                WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//td[" + j + "]//h3"), this);
                currentWeekdays.Add(elem.GetElement().Text);
            }
            //to check two list have same data or not
            for (int k = 0; k < currentWeekdays.Count; k++)
                if (currentWeekdays[k] != daysNextWeek[k].ToString())
                    return false;
            return true;
        }
        [TxAction("ReadComment", "")]
        public string ReadComment(int rowIndex, int colIndex, bool byDayNumber = false)
        {
            //I replaced waitforelement by new werefreshed because elem is not displayed in dom
            WaitForAjax();
            WERefreshed element;
            if (byDayNumber)
                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + ((int)DateTime.Now.DayOfWeek + 4) + "]"));
            else
                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + colIndex + "]"));
            Sleep(2);

            string res = element.GetElement().GetAttribute("title");
            return res;
        }

        [TxAction("ReadCommentBytaskName", "")]
        public string ReadCommentBytaskName(string taskName, int colIndex)
        {
            WERefreshed element=GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + colIndex + "]"));
            Sleep(1);
            GetDriver().MouseOver(element);
            Sleep(1);
            string res = element.GetElement().GetAttribute("title");
            return res;
        }

        [TxAction("ClickOnCell", "To manage time cell in time tracking interface")]
        public void ClickOnCell(string taskName, int colIndex)
        {
            WERefreshed timeCell = GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + colIndex + "]"));
           
                GetDriver().ClickAt(timeCell);
           
        }
        [TxAction("ReadSavingPopUp", "")]
        public string ReadSavingPopUp()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhtmlx_message_area\"]//div[@class=\"dhtmlx-info dhtmlx-info\"]//div")).GetElement().Text;
        }

        [TxAction("WaitForSaving", "")]
        public void WaitForSaving()
        {
            try
            {
                GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhtmlx_message_area\"]//div[@class]//div"));
            }
            catch (Exception)
            {
                Console.WriteLine("unable to get the savings popup");
            }
        }


        [TxAction("WriteonSelectedTimeCell", "Writes on selected time cell")]
        public WEGInput WriteonSelectedTimeCell()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"objbox\"]//input"), this);
        }

        [TxAction("IsWeekNumberDisplayed", "Checks weather all the week number is displayed or not")]
        public bool IsWeekNumberDisplayed()
        {
            int k = 0;
            List<string> weekNumber = new List<string>();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            for (int i = 5; i < 15; i++)
            {
                IWebElement elem = GetDriver().FindElement(this.GetElement(), By.XPath(".//td[" + i + "]//h3"));
                GetDriver().ScrollToElement(elem);
                weekNumber.Add(elem.Text);
            }

            //to check two list have same data or not
            for (int i = weekNumber.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(weekNumber.ElementAt(i)) == weekNum)
                {
                    weekNum = Convert.ToInt32(weekNumber.ElementAt(i));
                }
                else
                {
                    if (weekNum - 1 <= 0)
                    {
                        weekNum = 53;
                        if (Convert.ToInt32(weekNumber.ElementAt(i)) == 53)
                        {

                            weekNum = Convert.ToInt32(weekNumber.ElementAt(i));
                            k++;
                        }
                    }

                    else if (Convert.ToInt32(weekNumber.ElementAt(i)) == (weekNum - 1))
                    {

                        weekNum = Convert.ToInt32(weekNumber.ElementAt(i));
                        k++;
                    }
                    else
                        return false;
                }
            }
            return true;
        }
        //[TxAction("IsWeekNumberDisplay", "")]
        //public bool  IsWeekNumberDisplay()
        //{
        //    List<string> weekNumber = new List<string>();
        //    List<string> weekRange = new List<string>();
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        // for(int i = 14; i > 4; i--)
        //    {
        //        IWebElement elem = GetDriver().FindElement(this.GetElement(), By.XPath(".//td[" + i + "]//h3"));
        //        GetDriver().ScrollToElement(elem);
               
        //        weekNumber.Add(elem.Text);

        //    }
        //    for (int i = 10; i >0; i--)
        //    {

        //      weekRange.Add(weekNum.ToString());
        //        weekNum--;

        //    }

        //    if (weekRange.ToString() == weekNumber.ToString())
        //        return true;
        //    else
        //        return false;
        //}

        
        [TxAction("IsCellEditabled", "Checks weather the column is displayed or not")]
        public bool IsCellEditabled(string taskName, string weeknumber)
        {
            //column index starts from 1
            Sleep(1);
            return GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + weeknumber + "]")).GetElement().GetAttribute("class").Contains("GridCellWhite");
        }

        [TxAction("IsTimeCellValidated", "Checks weather the cell is validated or not")]
        public bool IsTimeCellValidated(string resourceName, string weeknumber)
        {
            //column index starts from 1
            IWebElement cell = GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + resourceName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[contains(@class,\"clGridCell\")][" + weeknumber + "]"), this).GetElement();
            bool res = cell.GetAttribute("class").Contains("TimeCellValidated");
            return res;
        }


        [TxAction("IsCommentPresent", "Checks weather a Comment is present or not")]
        public bool IsCommentPresent(int rowIndex, int colIndex)
        {
            string element = GetDriver().FindElement(By.XPath(".//div[starts-with(@class,'objbox')]//tr[@class][" + rowIndex + "]/td[" + colIndex + "]")).GetAttribute("class");
            return element.Contains("clTxHourTrackingComment");
        }

        [TxAction("IsAddTaskButtonPresent", "")]
        public bool IsAddTaskButtonPresent(string resourceName)
        {
            Sleep(1);
            return GetDriver().IsElementPresent(By.XPath(".//span[@id=\"nodeval\" and text()=\"" + resourceName + "\"]/ancestor::tr[contains(@class,\"_material\")]//img[contains(@src,\"AddObject.png\")and not(contains(@style,\"display: none\"))]"), this.GetElement());
        }


        [TxAction("IsDeleteTaskButtonPresent", "")]
        public bool IsDeleteTaskButtonPresent(string taskName)
        {
            Sleep(1);
            return GetDriver().IsElementPresent(By.XPath(".//span[@id=\"nodeval\" and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//img[contains(@src,\"DeleteObject.png\") and not(contains(@style,\"display: none\"))]"), this.GetElement());
        }

        [TxAction("ScrollToCellNumber", "ScrollToCellNumber")]
        public void ScrollToCellNumber(string taskName, int cellNumber)
        {
            IWebElement elem = new WERefreshed(GetDriver(), By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + cellNumber + "]")).GetElement();
            GetDriver().ScrollToElement(elem);
        }

        [TxAction("ManageTimeCellinPopup", "")]
        public WEGInput ManageTimeCellinPopup(string taskName, int colNum, string sTimeType, bool click = true)
        {
            if (click)
            {
                GetDriver().ClickAt(By.XPath(".//span[@id='nodeval'and text()='" + taskName + "']/../../..//td[" + colNum + "]"), this);
                Sleep(2);
            }
            return new WEGInput(GetDriver(), By.XPath($".//div[starts-with(@class,\"clDivTimeTypesLine\")]/input[@stimetype='{sTimeType}']"));
        }
        #region TimeCell
        [TxAction("ManageTimeCell", "To manage time cell in time tracking interface")]
        public WEGInput ManageTimeCell(string taskName, int colIndex, bool click = true)
        {
            WERefreshed timeCell = GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"nodeval\") and text()=\"" + taskName + "\"]/ancestor::tr[contains(@class,\"_material\")]//td[" + colIndex + "]"), this);
            if (click)
                GetDriver().ClickAt(timeCell);
            return new WEGInput(GetDriver(), By.XPath("//input"), timeCell);
        }
        #endregion
    }
}

//[TxAction("ReadCellAfterDuplicationForChangedValue", "")]
//public void ReadCellAfterDuplicationForChangedValue(int rowIndex, string duplicateddata, string newDuplicatedData)
//{
//    WERefreshed element;
//    string data;
//    int columnNumber = Convert.ToInt32(DateTime.Now.DayOfWeek);
//    if (columnNumber.Equals(2))
//    {
//        for (int i = 0; i < 6; i++)
//        {
//            if (i == 0)
//            {
//                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + ((columnNumber + 3) + i) + "]"));
//                data = element.GetElement().Text;
//                data.Equals(duplicateddata);
//            }
//            else
//            {
//                element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + ((columnNumber + 3) + i) + "]"));
//                data = element.GetElement().Text;
//                data.Equals(newDuplicatedData);
//            }
//        }

//    }
//    else
//    {
//        for (int i = 0; i < columnNumber; i++)
//        {
//            element = GetDriver().WaitForElement(By.XPath(".//tr[contains(@class,'_material')][" + rowIndex + "]//td[" + ((columnNumber + 3) - i) + "]"));
//            data = element.GetElement().Text;
//            data.Equals(newDuplicatedData);
//        }
//    }

//}
