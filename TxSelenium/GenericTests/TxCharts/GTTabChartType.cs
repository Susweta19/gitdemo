using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.Writable;
using TxSelenium.NativeTests.Windows;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTTabChartType : WERefreshed
    {
        private WERefreshed sideItemBarParent;
        public GTTabChartType(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
            this.sideItemBarParent = new WERefreshed(GetDriver(),By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]"), this);
        }

        [TxAction("SelectChartType", "SelectChartType")]
        public void SelectChartType(string typeName)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"sideBarItem\") and text()='"+typeName+"']"),this);
        }

        [TxAction("IsChartTypeSelected", "IsChartTypeSelected")]
        public bool IsChartTypeSelected(string typeName)

        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class, \"active\") and text()='" + typeName + "']"), this.GetElement());
        }

        [TxAction("ManageSeriesTable", "ManageSeriesTable")]
        public GTManageSeriesTable ManageSeriesTable()
        {
            return new GTManageSeriesTable(GetDriver(),By.XPath(".//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[starts-with(@class,'chartDataGrid')]"), sideItemBarParent);
        }

        [TxAction("NumberOfPointInChart", "To check number of point in chart ")]
        public int NumberOfPointInChart()
       
            {
                //return GetDriver().WaitForElement(By.XPath(".//*[starts-with(@class,'highcharts-markers highcharts-tracker') and @visibility='visible' and not(@clip-path='none')]|.//*[starts-with(@class,'highcharts-series highcharts-tracker') and @visibility='visible'and not(@clip-path='none')]"), this).GetElement().FindElements(By.XPath(".//*[@stroke-width='1']")).Count;
                return GetDriver().WaitForElement(By.XPath(".//*[contains(@class,\"highcharts-markers highcharts\")]"), this).GetElement().FindElements(By.XPath(".//*[contains(@class,\"highcharts-point\")]")).Count;
            }
 
        [TxAction("ManagePreview", "ManagePreview")]
        public GTManagePreview ManagePreview()
        {
            return new GTManagePreview(GetDriver(),By.XPath(".//div[contains(@style,'display: block;')]//div[starts-with(@id,'previewContainer')]"),this);
        }

        [TxAction("ManageAppearance", "ManageAppearance")]
        public GTFunctionAppearance ManageAppearance()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[text()='Appearance']"), sideItemBarParent);
            return new GTFunctionAppearance(GetDriver(),By.XPath(".//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[starts-with(@id ,\"chartAppearance\") and not(contains(@id,\"Advanced\"))]"), sideItemBarParent);
        }
        [TxAction("ManageAdvanced", "******")]
        public GTFunctionAdvanced ManageAdvanced()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[@class=\"dhxtabbar_tab_text\" and text()=\"Advanced\"]"), sideItemBarParent);
            return new GTFunctionAdvanced(GetDriver(),By.XPath(".//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[@class='dhx_cell_tabbar' and contains(@style,'visible')]//div[starts-with(@id ,\"chartAppearanceAdvanced\")]"), sideItemBarParent);
        }


        [TxAction("IsSeriescolour", "****")]
        public bool IsSeriescolour(string Colourvalue)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class='objbox']//td[@title='1'][@bgcolor='" + Colourvalue + "']|.//div[@class='objbox']//td[@title='Colonne texte'][@bgcolor='" + Colourvalue + "']"), this.GetElement());
        }


        [TxAction("IscurveDisabled", "*****")]
        public bool IscurveDisabled(string curveName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class='sidebarSideItem disabled' and text()='" + curveName + "']"), this.GetElement());
        }


        [TxAction("IsButtonDisabled", "*****")]
        public bool IsButtonDisabled(string buttonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[@disabled='disabled'][@value='"+ buttonName + "']"));
        }

        [TxAction("IsAdvanceApprencePresent", "ManageAdvanced")]
        public bool IsAdvanceApprencePresent(string Name)
        {
           return GetDriver().IsElementPresent(By.XPath(".//div[@class='dhxtabbar_tab_text' and text()=\""+ Name + "\"]")); 
        }

        [TxAction("ChangeCurve", "")]
        public void ChangeCurve(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhxtabbar_tab_text\") and text()=\"" + tabName + "\"]"), this);
        }

    }
}
