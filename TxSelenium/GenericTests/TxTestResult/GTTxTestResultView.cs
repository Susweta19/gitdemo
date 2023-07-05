using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTTxTestResultView : WERefreshed
    {
        public GTTxTestResultView(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("Table", "Manage ant Table")]
        public GTTxBaseEntityTests Table(int tableIndex = 1, string objectName = "")
        {
            if (objectName.Length > 0)
                return new GTTxBaseEntityTests(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_tabbar\") and contains(@style,\"visibility: visible;\")]//div[@class=\"trBlockContainer\"][" + tableIndex + "]//div[starts-with(@id,\"accItemStd\")]//div[text()=\"" + objectName + "\"]/ancestor::div[starts-with(@id,\"accItemStd\")]"), this);
            else
                return new GTTxBaseEntityTests(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_tabbar\") and contains(@style,\"visibility: visible;\")]//div[@class=\"trBlockContainer\"][" + tableIndex + "]"), this);
        }

        [TxAction("TabStandardEdition", "select this tab.")]
        public void TabStandardEdition()
        {
            GetDriver().ClickAt(By.XPath(".//div[@tab_id = \"tabStdDisplay\"]"), this);
        }

        [TxAction("TabAnalysisEdition", "select this tab.")]
        public void TabAnalysisEdition()
        {
            GetDriver().ClickAt(By.XPath(".//div[@tab_id = \"tabAnalysisDisplay\"]"), this);
        }

        [TxAction("TabSampleEdition", "select this tab.")]
        public void TabSampleEdition()
        {
            GetDriver().ClickAt(By.XPath(".//div[@tab_id = \"tabSamplesDisplay\"]"), this);
        }

        [TxAction("StandardTables", "select this tab.")]
        public GTTxTestResultTree StandardTables()
        {
            return new GTTxTestResultTree(GetDriver(), By.XPath(".//*[starts-with(@id, \"standardTables\")]"), WESTree.expandByClassicTree, this);
        }

        [TxAction("AnalysisTables", "select this tab.")]
        public GTTxTestResultTree AnalysisTables()
        {
            return new GTTxTestResultTree(GetDriver(), By.XPath(".//*[starts-with(@id, \"analysisTables\")]"), WESTree.expandByClassicTree, this);
        }
    }
}
