using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using TxSelenium.GenericTests.TxTableView;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTTimeExportation : WERefreshed
    {
        public GTTimeExportation(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        #region TxHourTracking_TimeExportationInterface_Sections_And_Buttons
        public static By Refresh_Button = By.XPath(".//img[contains(@src,\"Refresh.png\")]|.//img[contains(@src,\"bRefresh.png\")]");
        public static By Start_Date_Calendar_Button = By.XPath(".//div[starts-with(@id,\"idLabelTimeExportationDateFrom\")]/ancestor::div[contains(@class,\"DivTimeExportationDateLine\")]//img[contains(@src,\"calendar.png\")]");
        public static By End_Date_Calendar_Button = By.XPath(".//div[starts-with(@id,\"idLabelTimeExportationDateTo\")]/ancestor::div[contains(@class,\"DivTimeExportationDateLine\")]//img[contains(@src,\"calendar.png\")]");
        public static By Start_Date_Remove_Button = By.XPath(".//div[starts-with(@id,\"idLabelTimeExportationDateFrom\")]/ancestor::div[contains(@class,\"DivTimeExportationDateLine\")]//img[contains(@src,\"False.png\")]");
        public static By End_Date_Remove_Button = By.XPath(".//div[starts-with(@id,\"idLabelTimeExportationDateTo\")]/ancestor::div[contains(@class,\"DivTimeExportationDateLine\")]//img[contains(@src,\"False.png\")]");
        public static By Time_Exportation_TableView_Result = By.XPath(".//div[starts-with(@id,\"idDivTimeExportationTableView\")]");
        public static By Number_Of_Filtered_Tasks = By.XPath(".//h4[starts-with(@id,\"idHeaderTimeExportationResultTaskNb\")]");
        public static By Number_Of_Task_Selected = By.XPath(".//h4[starts-with(@id,\"idHeaderTimeExportationResultSelTaskNb\")]");
        #endregion

        [TxAction("Refresh", "Refresh")]
        public void Refresh()
        {
            GetDriver().ClickAt(Refresh_Button, this);
            WaitFrameAjax();
        }

        [TxAction("ManageStartDate", ".....")]
        public WDate ManageStartDate()
        {
            GetDriver().WaitForElement(Start_Date_Calendar_Button, this).GetElement().Click();
            return new WDate(GetDriver(), WDate.Dynamic_Calendar_Interface);
        }

        [TxAction("ManageEndDate", ".....")]
        public WDate ManageEndtDate()
        {
            GetDriver().WaitForElement(End_Date_Calendar_Button, this).GetElement().Click();
            return new WDate(GetDriver(), WDate.Dynamic_Calendar_Interface);
        }

        [TxAction("RemoveStartDate", ".....")]
        public void RemoveStartDate()
        {
            GetDriver().WaitForElement(Start_Date_Remove_Button, this).GetElement().Click();
        }

        [TxAction("RemoveEndDate", ".....")]
        public void RemoveEndDate()
        {
            GetDriver().WaitForElement(End_Date_Remove_Button, this).GetElement().Click();
        }

        [TxAction("TxTableViewResult", ".....")]
        public GTTxTableView TxTableViewResult()
        {
            return new GTTxTableView(GetDriver(), Time_Exportation_TableView_Result, this);
        }
        
        [TxAction("ReadNumberOfTaskFiltered", "Reads the value for number of task filtered")]
        public string ReadNumberOfTaskFiltered()
        {
            return GetDriver().WaitForElement(Number_Of_Filtered_Tasks).GetElement().Text;
        }

        [TxAction("ReadNumberOfTaskSelected", "Reads the value for number of task selected")]
        public string ReadNumberOfTaskSelected()
        {
            return GetDriver().WaitForElement(Number_Of_Task_Selected).GetElement().Text;
        }

        [TxAction("ManageLinkBox", "")]
        public WMultipleLink ManageLinkBox(string linkName, Boolean alreadyOpen = false)
        {
            WERefreshed linkDiv = GetDriver().WaitForElement(By.XPath(GTManageNavigationLayout.GetLinkBox(linkName)), this);
            if (!alreadyOpen)
                GetDriver().ClickAt(GTManageNavigationLayout.Expand_Collapse_LinkBox_Arrow_Button, linkDiv);
           
            return new WMultipleLink(GetDriver(), By.ClassName("dhx_cell_cont_acc"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, linkDiv);//should be in WMultipleLink
        }
    }
}
