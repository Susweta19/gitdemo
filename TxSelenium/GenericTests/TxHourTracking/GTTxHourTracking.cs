using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using OpenQA.Selenium;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.Windows;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTTxHourTracking : WERefreshed
    {
        public GTTxHourTracking(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        #region TxHourTracking_Sections_And_Buttons
        public static By ToolbarSection = By.XPath(".//div[starts-with(@id,'idDivMainToolbarTxHourTracking')]");
        public static By Toolbar_Buttons_Section = By.XPath(".//div[starts-with(@id,\"idDivTxToolbarButtons\")]");
        public static By NavigationLayoutSection = By.XPath(".//div[starts-with(@id,'idDivCellBTxHourTracking') and not(contains(@style,\"display: none\"))]");
        public static By TimeTrackingInterface = By.XPath(".//div[starts-with(@id,'idDivGridTxHourTracking')]");
        public static readonly By fullWindowBy = By.XPath(".//div[@class=\"dhxlayout_cont\"]/ancestor::div[@class=\"dhx_cell_cont_wins\"]");
        public static readonly By Full_Window_Buttons = By.XPath(".//input[@type=\"button\"]");
        public static By HourTrackingInterface_Window_And_Tab = By.XPath(".//div[@class=\"dhxlayout_cont\"]/ancestor::div[@class=\"dhx_cell_cont_wins\"] | .//div[@class=\"clDivMainTxHourTrackingLayout dhxlayout_base_material\"]");
        public static By Time_Exportation_Interface = By.XPath(".//div[starts-with(@id,\"idDivTimeExportationLayout\")]");
        #endregion

        [TxAction("IsNavigationLayoutDisplayed", "")]
        public bool IsNavigationLayoutDisplayed()
        {
            return !GetDriver().WaitForElement(GTToolBar.ExpandCollapseNavigationLayout_ByImg, this).GetElement().GetAttribute("class").Contains("dhxtoolbar_btn_pres");
        }

        [TxAction("TxHourTrackingToolBar", "used for hour tracking")]
        public GTToolBar TxHourTrackingToolBar()
        {
            return new GTToolBar(GetDriver(), ToolbarSection, this);
        }

        [TxAction("ManageTask", "used managing task")]
        public GTManageTask ManageTask()
        {
            return new GTManageTask(GetDriver(), TimeTrackingInterface, this);
        }

        [TxAction("ManageNavigationLayout", "use for Manage Calendar")]
        public GTManageNavigationLayout ManageNavigationLayout()
        {
            return new GTManageNavigationLayout(GetDriver(), NavigationLayoutSection, this);
        }

        [TxAction("ReadAllButtons", "Reads all button name present")]
        public ActionColl<string> ReadAllButtons()
        {
            List<IWebElement> fields = GetDriver().WaitForElement(HourTrackingInterface_Window_And_Tab, this).GetElement().FindElements(GTToolBar.ToolbarButton_each).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                buttonNames.Add(fields.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(buttonNames);
        }
        
        [TxAction("ReadAllButtonsV2", "")]
        public ActionColl<string> ReadAllButtonsV2()
        {
            List<IWebElement> buttons = GetDriver().WaitForElement(fullWindowBy , this).GetElement().FindElements(Full_Window_Buttons).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < buttons.Count; i++)
                buttonNames.Add(buttons.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(buttonNames);
        }
        
        [TxAction("ReadToolbarButtonsInNewTab", "Reads all button name present")]
        public ActionColl<string> ReadToolbarButtonsInNewTab()
        {
            List<IWebElement> fields = GetDriver().WaitForElement(Toolbar_Buttons_Section, this).GetElement().FindElements(GTToolBar.ToolbarButton_each).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                buttonNames.Add(fields.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(buttonNames);
        }

        [TxAction("ReadToolbarButtonsInWindow", "Reads all button name present")]
        public ActionColl<string> ReadToolbarButtonsInWindow()
        {
            List<IWebElement> fields = GetDriver().WaitForElement(ToolbarSection, this).GetElement().FindElements(GTToolBar.ToolbarButton_each).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                buttonNames.Add(fields.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(buttonNames);
        }

        [TxAction("ReadWeekRange", "Reads all button name present")]
        public string ReadWeekRange()
        {
            return GetDriver().WaitForElement(GTToolBar.WeekRange, this).GetElement().Text;
        }
    }
}
