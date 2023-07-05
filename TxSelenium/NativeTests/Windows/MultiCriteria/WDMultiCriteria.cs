using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows
{
    public class WDMultiCriteria : WERefreshed
    {
        private static readonly By criteriaTab = By.Id("onglet_cdc");
        private static readonly By resultsTab = By.Id("onglet_resultat_detaille");

        public WDMultiCriteria(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("CriteriaTab", "open Objet managing the Criteria Tab functions")]
        public WDFramedWindow<WDTabCriteria> CriteriaTab()
        {
            GetDriver().ClickAt(WDMultiCriteria.criteriaTab, this);
            WDTabCriteria tabCriteria = new WDTabCriteria(GetDriver(), By.Id("bar"));
            return new WDFramedWindow<WDTabCriteria>(GetDriver(), tabCriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);

        }

        [TxAction("ResultsTab", "open Objet managing the Result Tab functions")]
        public WDFramedWindow<WDTabResults> ResultsTab()
        {
            GetDriver().ClickAt(WDMultiCriteria.resultsTab, this);
            WDTabResults tabResults = new WDTabResults(GetDriver(), By.Id("bar"));
            return new WDFramedWindow<WDTabResults>(GetDriver(), tabResults, null, WDFramedWindow<WERefreshed>.frameWindowBy);

        }

        public bool IsCurrentTab(By byTab)
        {
            WERefreshed elem = GetDriver().WaitForElement(byTab, this);
            if (GetDriver().IsAttributePresent("class", elem))
                return true;
            else
                return false;
        }
       
        
    }
}
