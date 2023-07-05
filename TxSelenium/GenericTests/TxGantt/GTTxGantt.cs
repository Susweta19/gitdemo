using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.GenericTests.TxGantt;
using System.Collections.Generic;
using XmlExecutor.DataTypes;
using System.Linq;
using System;

namespace TxSelenium.GenericTests
{
    public class GTTxGantt : WERefreshed
    {

        public GTTxGantt(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        #region TxGantt_Main_Toolbar_Buttons

        #endregion

        [TxAction("ManageTask", "")]
        public ManageTask ManageTask(bool taskno = false)
        {
            if (!taskno)
            {
                return new ManageTask(GetDriver(), By.XPath(".//div[@column_id=\"text\"]/../.."));
            }
            else
            {
                return new ManageTask(GetDriver(), By.XPath(".//div[@column_id=\"name\"]/../.."));
            }
        }
        [TxAction("ManageTask1", "")]
        public ManageTask ManageTask1()
        {
            return new ManageTask(GetDriver(), By.XPath(".//div[@class=\"gantt_grid\"]"));
        }

        [TxAction("SelectUser", "")]
        public void SelectUser(string userName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[text()=\""+userName+"\"]/.."));
        }

        [TxAction("Refresh", "")]
        public void Refresh()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Refresh\"]//img"),this);
        }

        [TxAction("SelectSpecificProject", "Select the specific project(s)")]
        public WDValidatedWindow<WMultipleLink> SelectSpecificProject()
        {
            WMultipleLink selectProject = new WMultipleLink(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"), null, WSingleLink.entitySelectorWLinkBy);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), selectProject);
        }

        [TxAction("ProjectSelection", "Choose My Project or Specific Projects.")]
        public void ProjectSelection(int optionNo)
        {
            GetDriver().ClickAt(By.XPath(".//select[@id=\"load_selection\"]/option[" + optionNo + "]"), this);
        }

        [TxAction("ToDay", "Specify the current day")]
        public void ToDay()
        {
            GetDriver().ClickAt(By.Id("id_btn_Today"), this);
        }

        [TxAction("Export", "Export to a specific format.")]
        public void Export(string to)
        {
            GetDriver().ClickAt(By.Id("id_btn_Export"), this);
            GetDriver().ClickAt(By.XPath(".//div[contains(text(),\"" + to + "\")]"), this);
        }

        [TxAction("Calender", "Choose year/month/day wise.")]
        public void Calender(int index)
        {
            GetDriver().ClickAt(By.XPath(".//select[@id=\"scales_menu\"]/option[" + index + "]"), this);
        }

        [TxAction("Sort", "Sort in asc or desc order.")]
        public void Sort(int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_grid_scale\"]/div[" + columnIndex + "]"), this);
        }

        [TxAction("ManageGanttView", "")]
        public GanttView ManageGanttView()
        {
            return new GanttView(GetDriver(), By.XPath(".//div[@class=\"gantt_task\"]"));
        }
        [TxAction("SelectScale", "")]
        public void SelectScale(string option)
        {
            GetDriver().WaitForElement(By.XPath(".//div[@title=\"Scale\"]//div[@class=\"arwimg\"]"), this).GetElement().Click();
            GetDriver().ClickAt(By.XPath(".//table[@class=\"buttons_cont\"]//div[@class=\"btn_sel_text\"and text()=\"" + option + "\"]"));
        }
        [TxAction("SelectTheObjectToDisplay", "")]
        public void SelectTheObjectToDisplay(string option)
        {
            GetDriver().WaitForElement(By.XPath(".//div[@title=\"Select the Objects to display\"]//div[@class=\"arwimg\"]"), this).GetElement().Click();
            GetDriver().ClickAt(By.XPath(".//table[@class=\"buttons_cont\"]//div[@class=\"btn_sel_text\"and text()=\"" + option + "\"]"));
            WaitForAjax();
            Sleep(1);
        }
        [TxAction("SelectPaging", "")]
        public void SelectPaging(string option)
        {
            GetDriver().WaitForElement(By.XPath(".//div[@title=\"Paging scale\"]//div[@class=\"arwimg\"]"), this).GetElement().Click();
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhx_toolbar_poly_material dhxtoolbar_icon\")and not(contains(@style,\"display: none;\"))]//tr[not(contains(@style,\"display: none;\"))]//div[@class=\"btn_sel_text\"and text()=\"" + option + "\"]"), this);
            WaitForAjax();
            Sleep(1);
        }
        [TxAction("ClickNow", "")]
        public void ClickNow()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-user-clock.png\")]"), this);
            Sleep(1);
        }
        [TxAction("NextOrPreviousPage", "")]
        public void NextOrPreviousPage(bool next = true)
        {
            if (next)
                GetDriver().WaitForElement(By.XPath(".//div[@title=\"Next page\"]"), this).GetElement().Click();
            else
            {
                GetDriver().WaitForElement(By.XPath(".//div[@title=\"Previous page\"]"), this).GetElement().Click();
            }
            Sleep(1);
        }
        [TxAction("OpenOrCloseBranches", "")]
        public void OpenOrCloseBranches(bool open = true)
        {
            if (open)
                GetDriver().ClickAt(By.XPath(".//div[@title=\"Close all branches\"]"), this);
            else
            {
                GetDriver().ClickAt(By.XPath(".//div[@title=\"Open all branches\"]"), this);
            }
        }
        [TxAction("ClickOnTogglelegend", "")]
        public void ClickOnTogglelegend()
        {
            GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"fal_fa-question-square.png\")]"), this).GetElement().Click();
        }
        //[TxAction("IsTogglelegendCollasped", "")]
        //public bool IsTogglelegendCollasped()
        //{
        //    return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_popup_material\"and contains(@style,\"display: none;\")]"));
        //}
        [TxAction("ReadToggleName", "")]
        public ActionColl<string> ReadToggleName()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"mainContainerPopup\")]//div[starts-with(@id,\"contentPopup\")]"), this).GetElement().FindElements(By.XPath(".//span"));
            ICollection<string> ToggleName = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                ToggleName.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(ToggleName);
        }
        [TxAction("IsToggleDisplay", "")]
        public bool IsToggleDisplay()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_popup_material\"and not(contains(@style,\"display: none;\"))]"));
        }
        [TxAction("Search", "")]
        public WEGInput Search()
        {
            GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"fal_fa-search.png\")]"), this).GetElement().Click();
            return new WEGInput(GetDriver(), By.Id("inputSearch"), this);
        }
        [TxAction("ReadSearch", "")]
        public string ReadSearch()
        {
            return GetDriver().WaitForElement(By.Id("searchResults"), this).GetElement().Text;
        }
        [TxAction("RightClickOnTask", "")]
        public ManageRightOption RightClickOnTask(string index, string optionToMove)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"gantt_grid_data\"]//div[contains(@class,\"gantt_row \")][" + index + "]"), this);
            GetDriver().RightClick(elem);
            WERefreshed elem1 = new WERefreshed(GetDriver(), By.XPath(".//div[text()=\"" + optionToMove + "\"]/../.."));
            GetDriver().MouseOver(elem1, 10, 10);
            return new ManageRightOption(GetDriver(), By.XPath(".//div[@id=\"polygon_dhxId_XmHlbdeXpccY_sortLevel\"]"));
        }
        [TxAction("RightClickOnTaskByName", "")]
        public ManageRightOption RightClickOnTaskByName(string name, string optionToMove)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + name + "\"]/.."), this);
            GetDriver().RightClick(elem);
            WERefreshed elem1 = new WERefreshed(GetDriver(), By.XPath(".//div[text()=\"" + optionToMove + "\"]/../.."));
            GetDriver().MouseOver(elem1, 10, 10);
            GetDriver().Release();
            return new ManageRightOption(GetDriver(), By.XPath(".//div[@id=\"polygon_dhxId_XmHlbdeXpccY_sortLevel\"]"));
        }
        [TxAction("AddTask", "")]
        public void AddTask()
        {
            GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_def\"and not(contains(@style,\"display: none;\"))]//img[contains(@src,\"AddChildObject.png\")]|.//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_def\"and not(contains(@style,\"display: none;\"))]//img[contains(@src,\"Add.png\")]")).GetElement().Click();
        }

        [TxAction("ReturnObjectForm", "Passes Teexma in write mode.")]
        public WDValidatedWindow<WForm> ReturnObjectForm()
        {
            // GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"24x24-Add.png\")]"), this).GetElement().Click();
            WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }
        [TxAction("ChoseContact", "select manual selection")]
        public WDValidatedWindow<WMultipleLink> ChoseContact()
        {
            WMultipleLink wegcheckbox = new WMultipleLink(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), wegcheckbox);
        }
        [TxAction("ReadSavingMessage", "Read Saving Message")]
        public string ReadSavingMessage()
        {
           
                return GetDriver().WaitForElement(By.XPath(".//div[@class=\"gantt_message_area dhtmlx_message_area\"]//div[@class=\"gantt-info dhtmlx-info gantt-warning dhtmlx-warning\"]//div")).GetElement().Text;
           
        }
        [TxAction("Save", "Click on Save button")]
        public void Save()
        {
            GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"fal_fa-save.png\")]"), this).GetElement().Click();
        }
        [TxAction("ReadNumberOfGrid", "Count the number of grid")]
        public int ReadNumberOfGrid()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxlayout_cont\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"gantt_task_scale\"]"));
            int grid = element.Count;
            return grid;
        }
        [TxAction("IsAddTaskWindowPresent", "Count the number of grid")]
        public bool IsAddTaskWindowPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhxwin_active\"]"));
        }
        [TxAction("SelectGroup", "")]
        public void SelectGroup(string option)
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Group by\" or @title=\"Regrouper par\"]//div[@class=\"arwimg\"]"), this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhx_toolbar_poly_material dhxtoolbar_icon\")and not(contains(@style,\"display: none;\"))]//tr[not(contains(@style,\"display: none;\"))]//div[@class=\"btn_sel_text\"and text()=\"" + option + "\"]"), this);
        }
        [TxAction("FocusedObject", "")]
        public string FocusedObject()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"gantt_row\") and contains(@class,\"searchFocus\")]//div[@class=\"gantt_tree_content\"]")).GetElement().Text;
        }
        [TxAction("NextSearchObject", "")]
        public void NextSearchObject()
        {
            GetDriver().ClickAt(By.XPath(".//button[@id=\"nextResult\"]"));
        }
        [TxAction("ReadToolbarButtonsInWindow", "Reads all button name present")]
        public ActionColl<string> ReadToolbarButtonsInWindow()
        {

            List<IWebElement> fields = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"dhxtoolbar_float_left\")]")).GetElement().FindElements(By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_def\"]")).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                buttonNames.Add(fields.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(buttonNames);


        }
        [TxAction("HorizontalScroll", "")]
        public void HorizontalScroll(string position)
        {
            IWebElement ele = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"gantt_hor_scroll\")]")).GetElement();
            GetDriver().ScrollByPosition(ele, position, "0");
        }
        [TxAction("MachineViewButton", "")]
        public void MachineViewButton()
        {
            GetDriver().WaitForElement(By.XPath("//img[contains(@src,\"external-link-alt.png\")]")).GetElement().Click();
        }
        [TxAction("ClickOnTogglecriticalpath", "")]
        public void ClickOnTogglecriticalpath()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-route.png\")]"), this);
        }

        [TxAction("ClickOnToggleresourcespanel", "")]
        public void ClickOnToggleresourcespanel()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-angle\")]"), this);
        }
        [TxAction("IsButtonPresent", "***.")]
        public bool IsButtonPresent(string buttonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@title,\"" + buttonName + "\")]"), this.GetElement());
        }
        [TxAction("AssignMoreResource", "")]
        public WDValidatedWindow<WMultipleLink> AssignMoreResource()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Define my resource selection\"]"), this);
            WMultipleLink multiplelink = new WMultipleLink(GetDriver(), WForm.WriteFormDiv, WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), multiplelink);
        }
    }
}
