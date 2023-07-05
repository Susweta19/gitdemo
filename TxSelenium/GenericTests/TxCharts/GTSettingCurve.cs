using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTSettingCurve : WERefreshed
    {
        public GTSettingCurve(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("CurveName", "Set Curvename")]
        public WEGInput CurveName()
        {
            return new WEGInput(GetDriver(), By.Id("idTextCurveName"), this);
        }

        [TxAction("ColumnLineSeries", "Set ColumnLineSeries")]
        public WEGInput ColumnLineSeries()
        {
            return new WEGInput(GetDriver(), By.Id("idTextColumnLine"), this);
        }

        [TxAction("Abscissa", "Select Abscissa")]
        public WEGDHtmlxComboBox Abscissa()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("idDivComboAbscissa"), this);
        }

        [TxAction("Ordinate", "Select Ordinate")]
        public WEGDHtmlxComboBox Ordinate()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("idDivComboOrdinate"), this);
        }

        [TxAction("Labels", "Select Labels")]
        public WEGDHtmlxComboBox Labels()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("idDivComboLabels"), this);
        }

        [TxAction("ClearLabels", "Clear Labels")]
        public WEGDHtmlxComboBox ClearLabels()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[@id = \"idDivClearLabelOption\"]//img"), this);
        }

        [TxAction("TreeCurve", "Manage TreeCurve")]
        public WESTree Tree()
        {
            return new WMultipleLink(GetDriver(), By.Id("idDivTreeCurve"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("DeleteCurve", "Delete Curve")]
        public void DeleteCurve()
        {
            GetDriver().ClickAt(By.XPath(".//*[@id =\"idDivToolbarCurve\"]//img[ @contains(@src , \"DeleteObject.png\") ]"), this);
        }

        [TxAction("AddCurve", "Add Curve")]
        public void AddCurve()
        {
            GetDriver().ClickAt(By.XPath(".//*[@id =\"idDivToolbarCurve\"]//img[ contains(@src , \"AddObject.png\") ]"), this);
        }

        [TxAction("ExcelTab", "select excel file")]
        public WEGDHtmlxComboBox ExcelTab()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("idDivComboExcelTabs"), this);
        }
    }
}
