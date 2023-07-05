using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.GenericTests.TxTableView;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTAddTask : WERefreshed
    {
        public GTAddTask(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("SelectTask", "")]
        public WMultipleLink SelectTask()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivHourTrackingTreeToolbar\")]/.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("SelectTaskV2", "")]
        public GTTxTableView SelectTaskV2()
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//div[starts-with(@id,\"idTaskSelectionTableViewWidget\")]"), this);
        }

        [TxAction("IsTaskDisabled", "")]
        public bool IsTaskDisabled(string taskName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//span[text()=\"" + taskName + "\" and contains(@style,'color: lightgray;')] | .//span[@class=\"cellLinkValue\" and text()=\"" + taskName + "\"]/ancestor::tr//img[contains(@src,\"chk1_dis.gif\")]"), this.GetElement());
        }

        [TxAction("SelectDate", "")]
        public WDate SelectDate()
        {
            WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"calendar.png\")]"), this);
            WaitForAjax();
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));
        }

        [TxAction("WriteDate", "")]
        public WEGInput WriteDate()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@id='idInpCalendarAddTask']"), this);
        }

        [TxAction("RemoveDate", "")]
        public void RemoveDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class='clDivIconRemoveCCalendar']"), this);
        }

        [TxAction("WriteDuration", "")]
        public WEGInput WriteDuration()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idInpDurationAddTask\")]"), this);
        }

        [TxAction("WriteComment", "")]
        public WEGInput WriteComment()
        {
            return new WEGInput(GetDriver(), By.XPath(".//textarea[contains(@id,\"idTextareaTxHourTracking\")]"), this);
        }

        [TxAction("PreviousWeekTask", "")]
        public void PreviousWeekTask()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id=\"idBtnImportTasks\"]"), this);
        }

        [TxAction("ReadMessageForDuration", "")]
        public string ReadMessageForDuration()
        {
            return GetDriver().WaitForElement(By.XPath(".//tr[@class=\"dhxnode\"]//div[starts-with(@id,\"dhxpopup_node\")]/child::label")).GetElement().Text;
        }
        [TxAction("IsDateFillDisabled", "")]
        public bool IsDateFillDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[starts-with(@id,\"idInpCalendarAddTask\")and @disabled]"), this.GetElement());
        }

        [TxAction("IsDurationDisabled", "")]
        public bool IsDurationDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[starts-with(@id,\"idInpDurationAddTask\")and @disabled]"), this.GetElement());
        }
        [TxAction("IsValidateButtonDisabled", "")]
        public bool IsValidateButtonDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[@id=\"idBtnValidate\"and @disabled=\"disabled\"]"), this.GetElement());
        }
        [TxAction("IsCommentSectionDisabled", "")]
        public bool IsCommentSectionDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//textarea[starts-with(@id,\"idTextareaTxHourTracking\")and @disabled]"), this.GetElement());
        }
        [TxAction("ReturnTableView", "")]
        public GTTxTableView ReturnTableView()
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//fieldset[@class=\"clTaskSelectionFieldset\"]"));
        }

        [TxAction("ExpandCollapseFolder", "")]
        public void ExpandCollapseFolder(string folderName, bool alreadyOpen = false)
        {
            if (alreadyOpen)
                GetDriver().ClickAt(By.XPath(".//span[text()=\"" + folderName + "\"]/ancestor::tr[@idnode]//div[contains(@style,\"minus.gif\")]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//span[text()=\"" + folderName + "\"]/ancestor::tr[@idnode]//div[contains(@style,\"plus.gif\")]"), this);
        }

        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            bool result = false;
            string[] buttonList = buttonName.Split("|");
            foreach (string btnName in buttonList)
            {
                bool res = GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idDivBtns\")]//input[@value=\"" + btnName + "\" and @disabled=\"disabled\"] | .//div[starts-with(@id,\"idDivBtns\")]//input[@title=\"" + btnName + "\" and @disabled=\"disabled\"]"), this.GetElement());
                if (res)
                {
                    result = res;
                }
            }
            return result;
        }

        [TxAction("Search", "Tree search.")]
        public void Search(string research, bool popup = true)
        {
            WEGInput input;
            try
            {
                input = new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDiv\") and contains(@id,\"Toolbar\") and not(contains(@id,\"MainToolbar\")) and not(contains(@style,\"display: none\"))  and starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"] | .//input[starts-with(@id,\"inp_search_\")]"), this);
            }
            catch (Exception)
            {
                input = new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,\"IdFilterToolbar\") and starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"]|.//div[starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"]"), this);
                Console.WriteLine("Exception handled within Search method...");
            }

            input.Fill(research);
            input.Enter();

            if (popup)
                if (research.Length == 1)
                    new WESPopUp(GetDriver()).ClosePopup(true);
        }
    }

}
