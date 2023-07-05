using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using System.Threading;
using System.Collections.Generic;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.Windows
{
    public class WDExport : WERefreshed
    {

        private static readonly By currentFrameBy = By.XPath(".//div[contains(@class, \"dhtmlx_window_active\")]//iframe | .//iframe[contains(@url,\"TxWebExportation.asp\")]");


        public WDExport(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("Filtre", "Select a filter")]
        public WEGDHtmlxComboBox Filtre()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboOT\")]"), this);
        }

        [TxAction("Exports", "Select a standard export")]
        public WEGDHtmlxComboBox Exportations()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboExports\")]"), this);
        }

        [TxAction("ObjectsTree", "Select entity(ies)")]
        public WMultipleLink EntityTree()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivExportCellB\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("DataTree", "Select data(s)")]
        public WMultipleLink EntityData()
        {
            WaitForAjax();
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivExportCellC\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }


        [TxAction("CompressFile", " true to compress file, false otherwise")]
        public WEGCheckBox Compress()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"idCheckCompressFile\")]"), this);
        }

        [TxAction("Export", "click on export button")]
        public void Export()
        {
            WaitForAjax();
            GetDriver().ClickAt(By.Id("idBtnValidate"), this);
            TimeToWaitForAjax(5);
            Thread.Sleep(1000);
        }

        [TxAction("Close", "click on Close button")]
        public void Close()
        {
            GetDriver().ClickAt(By.Id("idBtnCancel"), this);

            /*Be carefull : SwitchToMainFrame is necessary when you quit a frameWindow to back On the mainWindow 
              without click on WEFramedWindow<T>.closeButton */
            // GetDriver().SwitchToContent();
        }

        [TxAction("ReadAllFields", "")]
        public ActionColl<string> ReadAllFields()
        {
            WERefreshed parent = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivExportCell\") ]/ancestor::div[@class=\"dhx_cell_wins\"]"));
            ICollection<IWebElement> fields = parent.GetElement().FindElements(By.XPath(".//label"));
            List<string> fieldNames = new List<string>();
            for (int i = 1; i <= fields.Count; i++)
                fieldNames.Add(GetDriver().WaitForElement(By.XPath("(.//div[starts-with(@id,\"idDivExportCell\") ]/ancestor::div[@class=\"dhx_cell_wins\"]//label)["+i+"]")).GetElement().Text);
            return new ActionColl<string>(fieldNames);
        }
        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//*[@value=\"" + buttonName + "\" and @disabled]"), this.GetElement());
        }
    }
}
