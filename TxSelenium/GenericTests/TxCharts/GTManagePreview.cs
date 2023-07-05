using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTManagePreview : WERefreshed
    {
        public GTManagePreview(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        //TODO chartPreview part(till 27_06_2018 nothing is coming)

        [TxAction("OrdinateUnitCombobox", "OrdinateUnitCombobox")]
        public WEGDHtmlxComboBox OrdinateUnitCombobox()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,'ordinateUnitCombo')]"), this);
        }

        [TxAction("ModifyOrdinateUnitList", "ModifyOrdinateUnitList")]
        public GTModifyUnitList ModifyOrdinateUnitList()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'chartOrdinateUnit')]//img[contains(@src,'ModifyObject.png')]"), this);
            return new GTModifyUnitList(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_popup_material\") and not(contains(@style,\"display: none\"))]//div[starts-with(@id,\"chartUnitSelector\") ]"));
        }

        [TxAction("AbscissaUnitCombobox", "AbscissaUnitCombobox")]
        public WEGDHtmlxComboBox AbscissaUnitCombobox()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,'abscissaUnitCombo')]"), this);
        }

        [TxAction("ModifyAbscissaUnitList", "ModifyAbscissaUnitList")]
        public GTModifyUnitList ModifyAbscissaUnitList()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'chartAbscissaUnit')]//img[contains(@src,'ModifyObject.png')]"), this);
            return new GTModifyUnitList(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_popup_material\") and not(contains(@style,\"display: none\"))]//div[starts-with(@id,\"chartUnitSelector\") ]"));
        }

        [TxAction("ReadAllAbscissaPointName", "Reads the abscissa name")]
        public ActionColl<string> ReadAllAbscissaPointName()
        {
            IWebElement parent = GetDriver().WaitForElement(By.XPath(".//*[name()=\"svg\" and @class=\"highcharts-root\"]/*[name()=\"g\" and contains(@class,\"highcharts-xaxis-labels\")]"), this).GetElement();
            ICollection<IWebElement> abscissaList = parent.FindElements(By.XPath("./*[name()=\"text\"]"));
            List<string> abscissaName = new List<string>();
            foreach (IWebElement elem in abscissaList)
                abscissaName.Add(elem.Text.Trim());
            return new ActionColl<string>(abscissaName);
        }

        [TxAction("ReadAllOrdinatePointName", "Reads the abscissa name")]
        public ActionColl<string> ReadAllOrdinatePointName()
        {
            IWebElement parent = GetDriver().WaitForElement(By.XPath(".//*[name()=\"svg\" and @class=\"highcharts-root\"]/*[name()=\"g\" and contains(@class,\"highcharts-yaxis-labels\")]"), this).GetElement();
            ICollection<IWebElement> abscissaList = parent.FindElements(By.XPath("./*[name()=\"text\"]"));
            List<string> abscissaName = new List<string>();
            foreach (IWebElement elem in abscissaList)
                abscissaName.Add(elem.Text.Trim());
            return new ActionColl<string>(abscissaName);
        }

        [TxAction("IsChartPreviewPresent", "***.")]
        public bool IsChartPreviewPresent(string chartName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//*[name()=\"svg\" and @class=\"highcharts-root\"]/*[name()=\"g\" and contains(@class,\"highcharts-series-group\")]/*[name()=\"g\" and contains(@class,\"highcharts-"+chartName+"-series\")]"), this.GetElement());
        }

        [TxAction("ReadPieParameters", "********")]
        public ActionColl<string> ReadPieParameters()
        {
            // IWebElement parent = GetDriver().WaitForElement(By.XPath(".//*[name()=\"svg\" and @class=\"highcharts-root\"]/*[name()=\"g\" and contains(@class,\"highcharts-pie-series\")]/*[name()=\"g\"]"), this).GetElement();
            ///ICollection<IWebElement> abscissaList = parent.FindElements(By.XPath("./*[name()=\"text\"]"));
            //ICollection<IWebElement> abscissaList = GetDriver().FindElement(By.XPath(".//*[name()=\"svg\" and @class=\"highcharts-root\"]/*[name()=\"g\" and contains(@class,\"highcharts-pie-series\")]/*[name()=\"g\"]")).FindElements(By.XPath("./*[name()=\"text\"]"));
            //  parent.FindElements(By.XPath("./*[name()=\"text\"]"));
            ICollection<IWebElement> abscissaList = this.GetElement().FindElements(By.XPath(".//*[@class=\"highcharts-text-outline\"]"));
            int i = 0;
            List<string> abscissaName = new List<string>();
            foreach (IWebElement elem in abscissaList)
                abscissaName.Add(elem.Text.Trim());
            return new ActionColl<string>(abscissaName);
        }
    }
}
