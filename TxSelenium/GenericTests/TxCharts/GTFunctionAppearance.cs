using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTFunctionAppearance : WERefreshed
    {
        private WERefreshed sideItemBarParent;
        public GTFunctionAppearance(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            this.sideItemBarParent = new WERefreshed(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]"), this);
        }

        [TxAction("SelectPointsStyle", "1  for points ,  2 for lines 3 for ponits and lines")]
        public void SelectPointsStyle(int style)
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//span[@data-icon-id= \"" + style + "\"]/img"), this);
        }

        [TxAction("SelectCurveColor", "Selects a curve color")]
        public GTSelectColor SelectCurveColor()
        {
            WaitForAjax();
            return new GTSelectColor(GetDriver(), By.XPath(".//*[starts-with(@id, \"colorPickerContainer\")]"), this);
        }

        [TxAction("EditSeriesName", "EditSeriesName")]
        public WEGInput EditSeriesName()
        {
            return new WEGInput(GetDriver(),By.XPath(".//input[@name = \"name\"]"),this);
        }
    }
}
