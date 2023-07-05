using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using System.Threading;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.Windows;

namespace TxSelenium.GenericTests.TXSpecification
{
    public class GTTxSpecification : WERefreshed
    {
        public GTTxSpecification(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("MouseHoverInChart", "")]
        public void MouseHoverInChart(int chartIndex, int pointIndex)
        {
            WERefreshed point = new WERefreshed(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivCharts\") and @class=\"chartContainer\"][" + chartIndex + "]//*[contains(@class,\"highcharts-tracker\")]//*[@class=\"highcharts-point\"][" + pointIndex + "]"), this);
            Thread.Sleep(1000);
            GetDriver().MouseOver(point);
        }
        [TxAction("ReadPointTitle", "")]
        public string ReadPointTitle(string text)
        {
            return GetDriver().WaitForElement(By.XPath("(.//*[contains(text(),\"" + text + "\")]/..)[2]")).GetElement().Text.ToString();

        }
        [TxAction("ReadAllToolbarButtons", "Reads all button name present in toolbar")]
        public ActionColl<string> ReadAllToolbarButtons()
        {
            List<IWebElement> fields = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"clDivTxToolbar\")]"), this).GetElement().FindElements(By.XPath(".//div[starts-with(@class,\"dhx_toolbar_btn dhxtoolbar_btn_def\")]")).ToList();
            List<string> toolbarButtonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                toolbarButtonNames.Add(fields.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(toolbarButtonNames);
        }
        [TxAction("ClickOnButtonFromToolbar", "")]
        public void ClickOnButtonFromToolbar(string buttonName)
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[@title=\"" + buttonName + "\"]"), this);
            WaitForAjax();
        }
        [TxAction("ReturnManageDashboard", "")]
        public WDValidatedWindow<ManageDashBoard> ReturnManageDashboard()
        {
            ManageDashBoard dash = new ManageDashBoard(GetDriver(), By.ClassName("dhxwin_active"));
            return new WDValidatedWindow<ManageDashBoard>(GetDriver(), dash);
        }
        [TxAction("ClickOnSpecificIndex", "")]
        public void ClickOnSpecificIndex(string specificIndexName)
        {
            GetDriver().ClickAt(By.XPath(".//*[text()=\"" + specificIndexName + "\"]/.."));
        }

        [TxAction("IsSpecificIndexDisabled", "")]
        public bool IsSpecificIndexDisabled(string specificIndexName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[contains(@class,\"highcharts-legend-item-hidden\")]//*[text()=\"" + specificIndexName + "\"]"));
        }

        [TxAction("ReturnEditConfiguration", "")]
        public WDValidatedWindow<EditConfiguration> ReturnEditConfiguration()
        {
            EditConfiguration edit = new EditConfiguration(GetDriver(), By.ClassName("dhxwin_active"));
            return new WDValidatedWindow<EditConfiguration>(GetDriver(), edit);
        }

    }

}
