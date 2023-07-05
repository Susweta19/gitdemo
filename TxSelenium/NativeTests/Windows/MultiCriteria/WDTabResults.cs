using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Windows
{
    public class WDTabResults : WERefreshed
    {
        private static readonly By exportButton = By.XPath(".//input[contains(@onclick, \"Exportation\")]");

        private static readonly By ExportToExcelFormatButton = By.XPath(".//input[contains(@onclick, \"MCSResultsToExcel();\")]");

        public WDTabResults(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>WEMultipleLink</returns>
        [TxAction("TreeTabResults", "return a multiple link")]
        public WMultipleLink Tree()
        {
            return new WMultipleLink(GetDriver(), By.Id("smc_affiche_resultat_detaille"), WESTree.expandByTableViewTree, WSingleLink.entitySelectorTableViewBy, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>WEMultipleLink</returns>
        [TxAction("ClickHeader", "For Sorting the objects.")]
        public void ClickHeader(int indexColumn)
        {
            indexColumn++;
            GetDriver().ClickAt(By.XPath(".//tbody/tr[2]/td[" + indexColumn + "]/div"), this);
        }



        /// <summary>
        /// Export to excel format (results tab).
        /// </summary>
        [TxAction("DiscardedElements", "discarded or questionnable elements")]
        public WEGCheckBox Discard()
        {
            return new WEGCheckBox(GetDriver(), By.Id("inp_mcs_view"), this);
        }

        /// <summary>
        /// Export.
        /// </summary>
        [TxAction("Export", "Export")]
        public WDFramedWindow<WDExport> Export()
        {
            GetDriver().ClickAt(exportButton, this);

            WDExport export = new WDExport(GetDriver(), By.TagName("body"));
            return new WDFramedWindow<WDExport>(GetDriver(), export, null, null);
        }

        /// <summary>
        /// Export.
        /// </summary>
        [TxAction("ExportToExcelFormat", "to Export in Excel Format")]
        public void ExportToExcelFormat()
        {
            GetDriver().ClickAt(ExportToExcelFormatButton, this);
            GetDriver().WaitForAjax();
        }

        [TxAction("Extract", "Extract")]
        public WDWindow<WDExtraction> Extract(bool popUp = false)
        {
            GetDriver().ClickAt(Translator.GetButtonByValue(GetDriver(), Translator.extractValueButton), this);

            if (popUp)
            {
                new WESPopUp(GetDriver()).ClosePopup(true);

                return null;
            }
            //Use this id because there two ida = MainContent in the same time.
            WDExtraction extractCreator = new WDExtraction(GetDriver(), By.XPath(".//div[@id=\"idDivCellTxExtraction\"]/.."));
            return new WDWindow<WDExtraction>(GetDriver(), extractCreator);
        }
        [TxAction("DisplayedResult", "....")]
        public string DisplayedResult()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"xhdr\"]/parent::div[not(contains(@style,\"display: none\"))]//div[@class=\"xhdr\"]//tr[not(@style)]"), this).GetElement().Text.Split('(')[1].Split(')')[0];
        }
    }
}
