using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.Utils;
using System.Threading;
using System;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTTxCharts : WERefreshed
    {
        public GTTxCharts(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("ChartsSettings", "Select the specific project(s)")]
        public WDValidatedWindow<GTConfiguration> ChartsSettings()
        {
            GetDriver().ClickAt(ElemByPictureName.Structure);
            GetDriver().WaitForAjax();
            GTConfiguration configure = new GTConfiguration(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTConfiguration>(GetDriver(), configure);
        }

        [TxAction("NewChart", "Reset the Chart")]
        public WESPopUp NewChart()
        {
            GetDriver().ClickAt(ElemByPictureName.NewButton, this);
            GetDriver().WaitForAjax();
            return new WESPopUp(GetDriver());
        }

        //IsAnyPointPresent

        [TxAction("IsAnyPointPresentInChart", "***")]
        public bool IsAnyPointPresentInChart()
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[contains(@class,'highcharts-tracker') and @visibility='visible'and @clip-path='none']//*[@fill]"),this.GetElement());
        }


        [TxAction("NumberOfPointsInChart", "***")]
        public int NumberOfPointsInChart()
        {
            Thread.Sleep(4000);
            try
            {
                return this.GetElement().FindElements(By.XPath(".//*[contains(@class,'highcharts-tracker')]//*[starts-with(@class,\"highcharts-point\")]")).Count;
            }
            catch(Exception)
            {
                int point = GetDriver().WaitForElement(By.XPath(".//*[starts-with(@class,'highcharts-tracker')]/../..")).GetElement().FindElements(By.XPath(".//*[contains(@class,\"highcharts-point\")]")).Count;
                return point;
            }
        }
        

        [TxAction("SaveCurves", "Save the Curves")]
        public WDValidatedWindow<GTSaveCurves> SaveCurve()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"wSave.png\")]"), this);
            GetDriver().WaitForAjax();
            GTSaveCurves save = new GTSaveCurves(GetDriver(), By.ClassName("dhxwin_active"));
            return new WDValidatedWindow<GTSaveCurves>(GetDriver(), save);
        }

        [TxAction("ExportChart", "Click on ExportChart")]
        public WDValidatedWindow<GTExportChart> ExportChart()
        {
            GetDriver().ClickAt(ElemByPictureName.ExportBisButton, this);
            GetDriver().WaitForAjax();
            GTExportChart exportchart = new GTExportChart(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTExportChart>(GetDriver(), exportchart);
        }

        [TxAction("SelectCurve", "Selects Curve based on ID")]
        public void SelectCurve(string curveName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@id='idChartsList']//div[starts-with(@class,'chartSeries')]//span[text()=\"" + curveName + "\"]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("IsCurveSelected", "Selects Curve based on ID")]
        public bool IsCurveSelected(string curveName)
        {
           return GetDriver().IsElementPresent(By.XPath(".//div[@id='idChartsList']//div[starts-with(@class,'chartSeries selected')]//span[text()=\"" + curveName + "\"]"), this.GetElement());
        }

        [TxAction("AddNewCurve", "Click on Add New Curve")]
        public WDValidatedWindow<GTAddNewCurve> AddNewCurve()
        {
            Sleep(2);
            GetDriver().ClickAt(ElemByPictureName.AddObject, this);
            GetDriver().WaitForAjax();
         bool ele=   GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idChartCreatorMainTabbar\")]"));
            if (ele == false)
            {
                GetDriver().ClickAt(ElemByPictureName.AddObject, this);
                GetDriver().WaitForAjax();
            }
            GTAddNewCurve addnewCurve = new GTAddNewCurve(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTAddNewCurve>(GetDriver(), addnewCurve);
        }
        [TxAction("AddNewCurve2", "Click on Add New Curve")]
        public WDValidatedWindow<GTAddNewCurve> AddNewCurve2()
        {
            Sleep(2);
            GetDriver().ClickAt(ElemByPictureName.AddObject, this);
            GetDriver().WaitForAjax();
            GTAddNewCurve addnewCurve = new GTAddNewCurve(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTAddNewCurve>(GetDriver(), addnewCurve);
        }

        [TxAction("ModifyCurve", "Click on modify Curve")]
        public WDValidatedWindow<GTAddNewCurve> ModifyCurve()
        {
            GetDriver().ClickAt(ElemByPictureName.ModifyObject, this);
            GetDriver().WaitForAjax();
            GTAddNewCurve addnewCurve = new GTAddNewCurve(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTAddNewCurve>(GetDriver(), addnewCurve);
        }

        [TxAction("DeleteCurve", "Click on delete Curve")]
        public WESPopUp DeleteCurve()
        {
            GetDriver().ClickAt(ElemByPictureName.DeleteObject);
            GetDriver().WaitForAjax();
            return new WESPopUp(GetDriver());
        }

        [TxAction("Interpolation", "Click on Calculate an interpolation Button")]
        public WDWindow<GTInterpolation> Interpoltion()
        {
            GetDriver().ClickAt(ElemByPictureName.Unit, this);
            GetDriver().WaitForAjax();
            GTInterpolation interpolation = new GTInterpolation(GetDriver(), By.ClassName("dhxwin_active"));
            return new WDWindow<GTInterpolation>(GetDriver(), interpolation);
        }

        [TxAction("Regression", "Click on Calculate an Regression Button")]
        public WDValidatedWindow<GTRegression> Regression()
        {
            GetDriver().ClickAt(ElemByPictureName.DataMining, this);
            GetDriver().WaitForAjax();
            GTRegression regression = new GTRegression(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTRegression>(GetDriver(), regression);
        }

        [TxAction("Curves", "Select a curve in the list.")]
        public void Curves(string curveName)
        {
            GetDriver().ClickAt(new ByChained(By.Id("idDivTxChartsCurvesList"), By.XPath(".//div[@text = \"" + curveName + "\"]")), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("ResetZoom", "Click on ResetZoom")]
        public void ResetZoom()
        {
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@class,'highcharts-button')]//*[text()='Reset zoom']"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("SelectGraphCurve", "Select Curves in the graph part.")]
        public void SelectGraphCurve(string curveName)
        {
            GetDriver().ClickAt(By.XPath(".//span[@class=\"seriesName\"and text()=\""+ curveName + "\"]"), this);
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// Zoom on the graph according absicca point
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        [TxAction("ZoomOnGraph", "Select Curves in the graph part.")]
        public void ZoomOnGraph(int x1, int x2)
        {
            WERefreshed position1 = GetDriver().WaitForElement(By.XPath(".//*[name()='svg']//*[name() = 'g']//*[contains(@class,'highcharts-markers highcharts')]//*[name()='path']["+x1+"]"), this);
            WERefreshed position2 = GetDriver().WaitForElement(By.XPath(".//*[name()='svg']//*[name() = 'g']//*[contains(@class,'highcharts-markers highcharts')]//*[name()='path']["+x2+"]"), this);

            GetDriver().ClickAndHold(position1.GetElement());
            GetDriver().MouseOver(position2);
            GetDriver().Release();
        }

        [TxAction("IsButtonPresent", "***.")]
        public bool IsButtonPresent(string buttonImageName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,'"+buttonImageName+".png')]"),this.GetElement());
        }

        [TxAction("SelectChartName", "***.")]
        public void SelectChartName(int index)
        {
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@class,'highcharts-legend-item')]["+index+"]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("IsChartDeactivated", "***.")]
        public bool IsChartDeactivated(int index)
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[starts-with(@class,'highcharts-legend-item')]["+index+"]//*[@stroke='#CCC']"), this.GetElement());
        }
        [TxAction("clickserries", "Graph Style selected")]
        public void clickserries()
        {
            GetDriver().ClickAt(By.XPath(".//*[@transform='translate(8,3)']"), this);
            GetDriver().WaitForAjax();
            // return GetDriver().IsElementPresent(By.XPath(".//*[@style='color:#CCC;font-size:12px;font-weight:bold;cursor:pointer;fill:#CCC;']"));
        }
        [TxAction("Checkseriesenabled", "Graph Style selected")]
        public bool Checkseriesenabled(int index)
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[starts-with(@class,'highcharts-legend-item')]["+index+"]//*[@style='color:#333333;font-size:12px;font-weight:bold;cursor:pointer;fill:#333333;']"));
        }
        [TxAction("IsSeriesListPresent", "***.")]
        public bool IsSeriesListPresent()
        {
            return GetDriver().IsElementPresent(By.Id("idChartsListContainer"),this.GetElement());
        }


        [TxAction("NumberOfGraph", "***.")]
        public int NumberOfGraph()
        {
            // return GetDriver().FindElement(By.XPath(".//div[@id='idChartsList']").FindElements(By.XPath(".//img[@src='images/Charts/icons/points_ligne.png']").)
            return GetDriver().WaitForElement(By.XPath(".//div[@id='idChartsList']"), this).GetElement().FindElements(By.XPath(".//img[@src='images/Charts/icons/points_ligne.png']")).Count;
        }
        [TxAction("IsSeriesNamePresent", "***.")]
        public bool IsSeriesNamePresent(string SERIESNAME,bool startsWith=false)
        {
            if(startsWith)
                return GetDriver().IsElementPresent(By.XPath(".//div [@id='idChartsList']//span[starts-with(text(),\""+ SERIESNAME + "\")]"), this.GetElement());
            return GetDriver().IsElementPresent(By.XPath(".//div [@id='idChartsList']//span[text()='"+SERIESNAME+"']"), this.GetElement());
        }
        [TxAction("IsChartsHeaderPresent", "***.")]
        public bool IsChartsHeaderPresent(string headerName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@id=\"idChartsListHeader\"]//span[text()=\"" + headerName + "\"]"), this.GetElement());
        }

        [TxAction("ReadChartPointColor", "read any curve color")]
        public string ReadChartPointColor()//(".//*[contains(@class,\"-series\") and contains(@class,\"highcharts-tracker\")]//*[@fill][1]")
        {
            GetDriver().WaitForAjax();

            WERefreshed point = GetDriver().WaitForElement(By.XPath(".//*[contains(@class,\"-series\") and contains(@class,\"highcharts-tracker\")]//*[@fill][1]"));
            Sleep(2);
            //   return   GetDriver().WaitForElement(By.XPath(".//*[contains(@class,\"-series\") and contains(@class,\"highcharts-tracker\")]//*[@fill][1]"));
            /*string aa = point.GetElement().GetAttribute("fill");
            int i = 0;*/
            return point.GetElement().GetAttribute("fill");
        }

        [TxAction("DoubleClickOnChart", "DoubleClickOnChart")]
        public GTTab<GTTxCharts> DoubleClickOnChart()
        {
            WaitForAjax();
            GetDriver().DoubleClickAt(this.ElemIdentifier, this.Parent);
            GTTxCharts txchart = new GTTxCharts(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxCharts>(GetDriver(), txchart);
        }

        [TxAction("NumberOfCurveDisplayed", "NumberOfCurveDisplayed")]
        public int NumberOfCurveDisplayed()
        {
            WaitForAjax();
            return this.GetElement().FindElements(By.XPath(".//*[starts-with(@class,\"highcharts-markers highcharts-series-\")]")).Count;
        }

        [TxAction("IsNamePresent", "***")]
        public bool IsNamePresent(string chartName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[text()=\"" + chartName + "\"]"), this.GetElement());
        }

    }
   
}
    



