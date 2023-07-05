using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.GenericTests.TxIndicator;
using TxSelenium.NativeTests.Writable;
using TxSelenium.Utils;
using System.Collections.Generic;
using XmlExecutor.DataTypes;
using System.Linq;

namespace TxSelenium.GenericDevs
{
    public class GTTxIndicator : WERefreshed
    {
        public GTTxIndicator(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("Export", " Click Export button")]
        public void Export()
        {
            GetDriver().ClickAt(ElemByPictureName.ExportBisButton, this);
        }

        [TxAction("Save", " Click save button")]
        public GTSave save()
        {
            GetDriver().ClickAt(ElemByPictureName.SaveButton, this);
            return new GTSave(GetDriver(), By.XPath(".//div[@class=\"dhtmlx_wins_body_inner\"]"), this);
        }

        //This function use div XPath for check all property of an entity boxes
        [TxAction("FilterProperty", "Filter graph")]
        public WMultipleLink FilterProperty(int index = 0)
        {
            if (index == 0)
                return new WMultipleLink(GetDriver(), By.XPath(".//div[@id=\"idDivFilter0_0\"] and .//div[@id=\"IdFilterToolbar0_0\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            else
                return new WMultipleLink(GetDriver(), By.XPath(".//div[@id=\"idDivFilter0_" + index.ToString() + "\"] and .//div[@id=\"IdFilterToolbar0_" + index.ToString() + "\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("IsRepresentationPresent", "")]
        public bool IsRepresentationPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,'idDivIndicator0')]"), this.GetElement());
        }

        [TxAction("Filter", "")]
        public WMultipleLink Filter(int filterId)
        {
           
            return new WMultipleLink(GetDriver(), By.XPath(".//div[@id=' idDivFilter0']"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
         //   return new WMultipleLink(GetDriver(), By.XPath(".//div[@id='idDivFilter0_" + filterId + "']|.//div[@id='IdFilterToolbar0_" + filterId + "']"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);

            //return new WMultipleLink(GetDriver(), By.XPath(".//div[@id='idDivFilter0_" + filterId + "']|.//div[@id='IdFilterToolbar0_"+filterId+"']"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("ReadNumberofLegend", "")]
        public int ReadNumberofLegend()
        {
            return GetDriver().WaitForElement(By.XPath(".//*[@class=\"highcharts-legend\"]")).GetElement().FindElements(By.XPath(".//*[starts-with(@class,\"highcharts-legend-item\")]")).Count;
        }
        [TxAction("ReadNumberofPiler", "")]
        public int ReadNumberofPiler(int graphindex=1)
        {
            return GetDriver().WaitForElement(By.XPath(".//*[starts-with(@class,\"highcharts-series highcharts-series-10\")and not(contains(@visibility,\"hidden\"))][1]")).GetElement().FindElements(By.XPath(".//*[@class]")).Count;
        }
        [TxAction("ReadGraphValue", "")]
        public ActionColl<string> ReadGraphValue()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//*[starts-with(@id,\"idDivIndicator0\")]")).GetElement().FindElements(By.XPath(".//*[@class=\"highcharts-text-outline\"][1]"));
            ICollection<string> value = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                value.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(value);
        }
    }
}
