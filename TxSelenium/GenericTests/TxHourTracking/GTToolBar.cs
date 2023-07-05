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

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTToolBar : WERefreshed
    {
        public GTToolBar(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        #region TxHourTracking_Main_Toolbar_Buttons_And_Sections
        public static By ExpandCollapseNavigationLayout_ByImg = By.XPath(".//img[contains(@src,\"MainOptions.png\")]/..");
        public static By ExpandNavigationLayout_ByDiv = By.XPath(".//div[starts-with(@class,\"dhx_toolbar_btn dhxtoolbar_btn_pres\") and @title=\"Expand / Collapse the main menu\"]");
        public static By CollapseNavigationLayout_ByDiv = By.XPath(".//div[starts-with(@class,\"dhx_toolbar_btn dhxtoolbar_btn_def\") and @title=\"Expand / Collapse the main menu\"]");
        public static By WeekRange = By.XPath(".//div[starts-with(@class,\"dhx_toolbar_text\") and not(contains(@style,\"display: none\"))]");
        public static By ToolbarButton_each = By.XPath(".//div[contains(@class,\"dhx_toolbar_btn dhxtoolbar_btn\")and not(contains(@style,\"display: none\"))]");
        public static By TodayButton = By.XPath(".//div[starts-with(@class,\"dhx_toolbar_btn\") and contains(@title,\"Today\")]");
        public static By PreviousWeekButton = By.XPath(".//div[@title=\"Previous week\"]/img[contains(@src,\"fal_fa-chevron-circle-left-black.png\") or contains(@src,\"fal_fa-chevron-circle-left.png\")]");
        public static By NextWeekButton = By.XPath(".//div[@title='Next week']/img[contains(@src,\"fal_fa-chevron-circle-right-black.png\") or contains(@src,\"fal_fa-chevron-circle-right.png\")]");
        public static By Open_Time_Exportation_Interface_Button = By.XPath(".//img[contains(@src,\"24x24-wExportTime.png\")] | //img[contains(@src,\"24x24-ExportTime.png\")]");
        public static By Validate_View_Times_Button = By.XPath(".//div[@title=\"Validate view times\"]|.//div[@title=\"Valider les temps de la vue\"]");
        public static By Cancel_View_Times_Validation_Button = By.XPath(".//div[@title=\"Cancel view times validation\"]");
        public static By Save_View_Times_Button = By.XPath(".//div[@title=\"Save view times\"]");
        #endregion

        [TxAction("ExpandCalender", "used to Expand Calender")]
        public GTManageNavigationLayout ExpandCalender()
        {
            GetDriver().ClickAt(ExpandNavigationLayout_ByDiv, this);
            return new GTManageNavigationLayout(GetDriver(), By.XPath(".//div[starts-with(@id,'idDivCellBTxHourTracking')]"), this);
        }
        [TxAction("SaveViewTimes", "Used for Save View Time")]
        public void SaveViewTimes()
        {
            GetDriver().ClickAt(Save_View_Times_Button, this);
        }
        [TxAction("ReadSavingTime", "Used for Check Saving Time")]
        public bool ReadSavingTime(string time)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_toolbar_text\" and starts-with(text(),\"Last save: " + time + "\")]"), this.GetElement());
        }
        [TxAction("CollapseCalender", "used to Collapse Calender")]
        public void CollapseCalender()
        {
            GetDriver().ClickAt(CollapseNavigationLayout_ByDiv, this);
        }

        [TxAction("Today", "used for Today")]
        public void Today()
        {
            GetDriver().ClickAt(TodayButton, this);
            WaitForAjax();
        }

        [TxAction("PreviousWeek", "used for PreviousWeek")]
        public void PreviousWeek()
        {
            GetDriver().ClickAt(PreviousWeekButton, this);
        }

        [TxAction("NextWeek", "used for Next Week")]
        public void NextWeek()
        {
            GetDriver().ClickAt(NextWeekButton, this);
        }

        [TxAction("TimeExportationInWindow", "TO Export Time Details")]
        public WDWindow<GTTimeExportation> TimeExportationInWindow()
        {
            GetDriver().ClickAt(Open_Time_Exportation_Interface_Button, this);
            GTTimeExportation timeExportation = new GTTimeExportation(GetDriver(), GTTxHourTracking.Time_Exportation_Interface);
            return new WDWindow<GTTimeExportation>(GetDriver(), timeExportation);
        }

        [TxAction("TimeExportationInNewTab", "TO Export Time Details")]
        public GTTab<GTTimeExportation> TimeExportationInNewTab()
        {
            GetDriver().ClickAt(Open_Time_Exportation_Interface_Button, this);
            GTTimeExportation timeExportation = new GTTimeExportation(GetDriver(), By.TagName("body"));
            return new GTTab<GTTimeExportation>(GetDriver(), timeExportation);
        }

        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string imageName)
        {
            return GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"" + imageName + ".png\")]/.."), this).GetElement().GetAttribute("class").Contains("_dis");
        }

        [TxAction("ValidateViewTIme", "used for ValidateViewTIme")]
        public void ValidateViewTIme()
        {
            GetDriver().ClickAt(Validate_View_Times_Button, this);
        }

        [TxAction("CancelViewTimeValidation", "used for CancelViewTimeValidation")]
        public void CancelViewTimeValidation()
        {
            GetDriver().ClickAt(Cancel_View_Times_Validation_Button, this);
        }

        [TxAction("IsButtonPresent", "***.")]
        public bool IsButtonPresent(string buttonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@title,\"" + buttonName + "\") and not(contains(@style,\"display: none\"))]"), this.GetElement());
        }

        //*********************NOT IN USE***********************

        //[TxAction("DisplayMode", "")]
        //public void DisplayMode()
        //{
        //    GetDriver().ClickAt(By.XPath(".//div[@class='dhx_toolbar_arw dis']"), this);
        //}

        //[TxAction("Save", "")]
        //public void Save()
        //{
        //    GetDriver().ClickAt(By.XPath(".//div[@class='dhx_toolbar_btn def' and @title='Save']"), this);
        //}
    }
}
