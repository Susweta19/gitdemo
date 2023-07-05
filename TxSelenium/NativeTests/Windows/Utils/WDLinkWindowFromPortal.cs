using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows.Utils
{
    /// <summary>
    /// this class retunr some windows without clicking on the button
    /// It uses when clicking on a link in the portal
    /// </summary>
    public class WDLinkWindowFromPortal
    {
        private TxWebDriver Driver { get; set; }

        public WDLinkWindowFromPortal(TxWebDriver driver)
        {
            this.Driver = driver;
        }
        
        [TxAction("ChoiceGuide", "Open athe choice guide and selcet a template.")]
        public WDFramedWindow<WDChoiceGuide> ChoiceGuide()
        {
            WDChoiceGuide choiceGuide = new WDChoiceGuide(Driver, By.TagName("body"));
            return new WDFramedWindow<WDChoiceGuide>(Driver, choiceGuide, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("MultiCriteriaSelection", "Use the multicriteria selection.")]
        public WDFramedWindow<WDMultiCriteria> MultiCriteria()
        {
            WDMultiCriteria multiCriteria = new WDMultiCriteria(Driver, By.Id("idBodyParent"));
            return new WDFramedWindow<WDMultiCriteria>(Driver, multiCriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("Export", "Export data")]
        public WDFramedWindow<WDExport> Export()
        {
            WDExport export = new WDExport(Driver, By.TagName("body"));
            return new WDFramedWindow<WDExport>(Driver, export, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }
    }
}
