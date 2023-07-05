using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using System.Threading;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using TxSelenium.GenericTests.TxCalendar;

namespace TxSelenium.GenericTests.TxGantt
{
    public class ManageTask : WERefreshed
    {
        public ManageTask(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("IsProjectPresent", "")]
        public bool IsProjectPresent(string ProjectName)
        {
            IWebElement elem;
            try
            {
                elem = GetDriver().FindElement(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + ProjectName + "\"]"));
                GetDriver().ScrollToElement(elem);
                return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + ProjectName + "\"]"));
            }
            catch (Exception)
            {
                return false;
            }
        }

        [TxAction("CollapseEntity", "")]
        public void CollapseEntity(string ProjectName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\""+ ProjectName + "\"]/../div[contains(@class,\"gantt_tree_icon gantt_close\")]"),this);
        }
        [TxAction("ExpandEntity", "")]
        public void ExpandEntity(string ProjectName)
        {
            try
            {
                GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + ProjectName + "\"]/../div[contains(@class,\"gantt_tree_icon gantt_open\")]"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Already opened");
            }
        }

        [TxAction("scrollToRow", "")]
        public void scrollToRow(int row)
        {
            IWebElement ele = GetDriver().WaitForElement(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[contains(@class,\"gantt_row\")][" + row + "]"), this).GetElement();
            GetDriver().ScrollToBottom(ele);
        }
        [TxAction("ClickOnSearchButton", "")]
        public void ClickOnSearchButton()
        {
            GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"fal_fa-search.png\")]")).GetElement().Click();

        }
        [TxAction("Search", "")]
        public void Search(string value)
        {
            GetDriver().WaitForElement(By.Id("inputSearch")).GetElement().SendKeys(value + Keys.Enter);
        }
        [TxAction("ReadTaskName", "")]
        public ActionColl<string> ReadTaskName()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[@class=\"gantt_grid_data\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"gantt_tree_content\"]"));
            ICollection<string> TaskName = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                TaskName.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(TaskName);
        }
        [TxAction("IsProjectCollasped", "")]
        public bool IsProjectCollasped(string projectName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[text()=\"" + projectName + "\"]/..//div[@class=\"gantt_tree_icon gantt_open\"]"));
        }
        [TxAction("SelectProject", "")]
        public void SelectProject(string projectName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + projectName + "\"]/.."));
        }

        [TxAction("SelectEntity", "")]
        public void SelectEntity(string SelectEntity)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + SelectEntity + "\"]/.."),this);
        }

        [TxAction("DoubleClickOnProject", "")]
        public void DoubleClickOnProject(string projectName)
        {
            Sleep(5);
            GetDriver().DoubleClickAt(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[@class=\"gantt_tree_content\"and text()=\"" + projectName + "\"]"));
            Sleep(5);
        }
        [TxAction("DoubleClickOnTask", "Gets the handle for the main entity tree.")]
        public void DoubleClickOnTask(string projectName, string taskName)
        {
            bool collasped = GetDriver().IsElementPresent(By.XPath(".//div[text()=\"" + projectName + "\"]/..//div[@class=\"gantt_tree_icon gantt_open\"]"), this.GetElement());
            if (collasped)
            {
                GetDriver().ClickAt(By.XPath(".//div[text()=\"" + projectName + "\"]/..//div[@class=\"gantt_tree_icon gantt_open\"]"), this);
                Thread.Sleep(1000);
                GetDriver().DoubleClickAt(By.XPath(".//div[@class=\"gantt_tree_content\"and text()=\"" + taskName + "\"]/.."), this);
            }
            else
            {
                WaitForAjax();
                GetDriver().DoubleClickAt(By.XPath(".//div[@class=\"gantt_tree_content\"and text()=\"" + taskName + "\"]/.."), this);
            }           

        }

        [TxAction("ReturnAddTask", "Gets the handle for the main entity tree.")]
        public WDValidatedWindow<AddTask> ReturnAddTask()
        {
            AddTask wfrom = new AddTask(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<AddTask>(GetDriver(), wfrom);
        }

        [TxAction("ReturnAddTask1", "Gets the handle for the main entity tree.")]
        public WDValidatedWindow<WForm> ReturnAddTask1()
        {
            WForm wfrom = new WForm(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<WForm>(GetDriver(), wfrom);
        }

        [TxAction("SelectTask", "Gets the handle for the main entity tree.")]
        public void SelectTask(string projectName, string taskName)
        {
            bool collasped = GetDriver().IsElementPresent(By.XPath(".//div[text()=\"" + projectName + "\"]/..//div[@class=\"gantt_tree_icon gantt_open\"]"), this.GetElement());
            if (collasped)
            {
                GetDriver().ClickAt(By.XPath(".//div[text()=\"" + projectName + "\"]/..//div[@class=\"gantt_tree_icon gantt_open\"]"), this);
                Thread.Sleep(1000);
                GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_tree_content\"and text()=\"" + taskName + "\"]"), this);
            }
            else
            {
                GetDriver().ClickAt(By.XPath(".//div[@class=\"gantt_tree_content\"and text()=\"" + taskName + "\"]"), this);
            }

        }
        [TxAction("CreateLinkToTask", "")]
        public void CreateLinkToTask(string sourceTaskName, string destinationName)
        {
            IWebElement source = GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + sourceTaskName + "\"]/../..//div[@class=\"gantt_link_control task_right task_end_date\"]//div")).GetElement();
            //div[@class=\"gantt_link_point\"]")).GetElement();
            WERefreshed deste = GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + destinationName + "\"]/../..//div[@class=\"gantt_link_control task_right task_end_date\"]"));
            ////div[@class=\"gantt_link_point\"]"));

            GetDriver().ClickAndHold(source);
            GetDriver().MouseOver(deste);
           
            GetDriver().Release();
            
            Sleep(2);

        }
        [TxAction("AddLink", "okNoCancel = ok, no or cancel")]
        public void AddLink( bool okOrCancel = false)
        {
            if (okOrCancel)
            {
                GetDriver().ClickAt(By.Id("idBtnValidate"));
                }
            else
            {
                GetDriver().ClickAt(By.Id("idBtnCancel"));
            }
           
               
        }
        [TxAction("RightClickOnTask", "")]
        public ManageRightOption RightClickOnTask(string taskName, string optionToMove)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"gantt_tree_content\"and text()=\"" + taskName + "\"]"), this);
            GetDriver().RightClick(elem);
            WERefreshed elem1 = new WERefreshed(GetDriver(), By.XPath(".//div[text()=\"" + optionToMove + "\"]/../.."));
            GetDriver().MouseOver(elem1, 10, 10);
            return new ManageRightOption(GetDriver(), By.XPath(".//div[@id=\"polygon_dhxId_XmHlbdeXpccY_sortLevel\"]"));
        }
        [TxAction("RightClickOnTask1", "")]
        public ManageRightOption RightClickOnTask1(string taskName, string option, string optionToMove = null)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"gantt_tree_content\"and text()=\"" + taskName + "\"]"), this);
            GetDriver().RightClick(elem.GetElement(), 10, 10);

            if (optionToMove != null)
            {
                WERefreshed elem1 = new WERefreshed(GetDriver(), By.XPath(".//div[text()=\"" + optionToMove + "\"]/../.."));
                GetDriver().MouseOver(elem1, 10, 10);
                return new ManageRightOption(GetDriver(), By.XPath(".//div[@id=\"polygon_dhxId_XmHlbdeXpccY_sortLevel\"]"));
            }
            else
            {
                GetDriver().ClickAt(By.XPath(".//div[@class=\"sub_item_text\" and text()=\"" + option + "\"]/../.."));
                return null;
            }
        }
        [TxAction("SelectedEntity", "")]
        public string SelectedEntity()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"gantt_grid_data\"]//div[contains(@class,\"gantt_row gantt_selected gantt_selected gantt_row_project\")]//div[@class=\"gantt_tree_content\"]"), this).GetElement().Text;
        }
        [TxAction("ReadTaskColor", "")]
        public string ReadTaskColor(string taskName)
        {
            string data = GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + taskName + "\"]/ancestor::div[@task_id]")).GetElement().GetAttribute("Style");
            string[] datas = data.Split(';');
            foreach (string d in datas)
                if (d.Contains("background-color"))
                    data = d.Split(':').Last().Trim();
            return data;
        }
        [TxAction("ReadResourceName", "")]
        public ActionColl<string> ReadResourceName()
        {
            //ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"gantt_grid_head_cell gantt_grid_head_name\")]/ancestor::div[@class=\"gantt_grid\"]//div[@class=\"gantt_grid_data\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"gantt_tree_content\"]"));
            ICollection<IWebElement> element = this.GetElement().FindElements(By.XPath(".//div[contains(@class,'gantt_cell_tree')]//div[@class='gantt_tree_content']"));
            ICollection<string> ResourceName = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                ResourceName.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(ResourceName);
        }
        [TxAction("ReadStartDate", "")]
        public string ReadStartDate(string taskName)
        {
            return GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + taskName + "\"]/../..//div[contains(@class,\"gantt_side_content gantt_left\")]")).GetElement().Text.Trim();
        }

        [TxAction("ReadEndDate", "")]
        public string ReadEndDate(string taskName)
        {
            return GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + taskName + "\"]/../..//div[contains(@class,\"gantt_side_content gantt_right\")]")).GetElement().Text.Trim();
        }
    }

}
