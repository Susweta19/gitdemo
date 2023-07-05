using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTRegression : WERefreshed
    {
        public GTRegression(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
           : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("SelectCurve", ".....")]
        public WEGDHtmlxComboBox SelectCurve()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartRegressionSeries\")]"), this);
        }

        [TxAction("RegressionType", ".....")]
        public WEGDHtmlxComboBox RegressionType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartRegressionType\")]"), this);
        }

        [TxAction("Equation", "We can only read the input")]
        public WEGInput Equation()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"equation\")]"), this);
        }

        [TxAction("PolynomOrder", "We can only read the input")]
        public WEGInput PolynomOrder()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,\"correlationContainer\")]"), this);
        }

        [TxAction("AbscissasLowerbound", "Set the value of Abscissas - Lower bound")]
        public WEGInput AbscissasLowerbound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@name,'abscissaLB')]"), this);
        }

        [TxAction("AbscissasUpperbound", "Set the value of Abscissas - Upper bound")]
        public WEGInput AbscissasUpperbound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@name,'abscissaUB')]"), this);
        }

        [TxAction("AddRegression", "............")]
        public void AddRegression()
        {
            GetDriver().ClickAt(By.Id("idBtnValidate"), this);
        }

        [TxAction("Calculate", "............")]
        public void Calculate()
        {
            GetDriver().ClickAt(By.Id("idBtnCalculate"), this);
        }

        [TxAction("Cancel", "............")]
        public void Cancel()
        {
            GetDriver().ClickAt(By.Id("idBtnCancel"), this);
        }
        [TxAction("Correlation", "We can only read the input")]
        public WEGInput Correlation()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@name,\"correlation\")]"), this);
                //By.XPath(".//input[@name='correlation']"), this);
        }
    }
}
