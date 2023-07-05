using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTConfiguration : WERefreshed
    {
        public GTConfiguration(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("ChartTitle", "Enter Chart Title")]
        public WEGInput ChartTitle()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='chartName']"), this);
        }

        [TxAction("ImageResolution", "Select Image Resolution")]
        public WEGDHtmlxComboBox ImageResolution()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("idChartSettingsPictureResolution"), this);
        }

        [TxAction("AbscissaFormat", "Select Abscissa Format")]
        public WEGDHtmlxComboBox AbscissaFormat()
        {
            try
            {
                return new WEGDHtmlxComboBox(GetDriver(), By.Id("idChartSettingsAbscissaFormat"), this);
            }
            catch
            {
                return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idChartSettingsAbscissaFormat\")]"), this);
            }
        }

        [TxAction("SpecifyAbscissa", "Specify Abscissa")]
        public WEGInput SpecifyAbscissa()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='abscissaPrecision']"), this);
        }

        [TxAction("OrdinateFormat", "Select Ordinate Format")]
        public WEGDHtmlxComboBox OrdinateFormat()
        {
            try
            {
                return new WEGDHtmlxComboBox(GetDriver(), By.Id("idChartSettingsOrdinateFormat"), this);
            }
            catch
            {
                return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idChartSettingsOrdinateFormat\")]"), this);
            }
        }

        [TxAction("OrdinateSpecification", "Enter Ordinate Specification")]
        public WEGInput OrdinateSpecification()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='ordinatePrecision']"), this);
        }

        [TxAction("AbscissaAxis", "Select Abscissa Axis")]
        public WEGDHtmlxComboBox AbscissaAxis()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idChartSettingsAbscissaAxis\")]"), this);
        }

        [TxAction("OrdinateAxis", "Select Ordinate Axis")]
        public WEGDHtmlxComboBox OrdinateAxis()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idChartSettingsOrdinateAxis\")]"), this);
        }

        [TxAction("AbscissaLabels", "Enter Abscissa Labels")]
        public WEGInput AbscissaLabels()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='abscissaLabel']"), this);
        }

        [TxAction("OrdinatesLabel", "Enter Ordinates Label")]
        public WEGInput OrdinatesLabel()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='ordinateLabel']"), this);
        }

        [TxAction("AbcissaeLowerBound", "Enter Abcissae - lower bound")]
        public WEGInput AbcissaeLowerBound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='abscissaLowerBound']"), this);
        }

        [TxAction("AbcissaeUpperBound", "Enter Abcissae - upper bound")]
        public WEGInput AbcissaeUpperBound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='abscissaUpperBound']"), this);
        }

        [TxAction("OrdiantesLowerBound", "Enter Ordiantes - lower bound")]
        public WEGInput OrdiantesLowerBound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='ordinateLowerBound']"), this);
        }

        [TxAction("OrdinateUpperBound", "Enter Ordinate - Upper bound")]
        public WEGInput OrdinateUpperBound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name='ordinateUpperBound']"), this);
        }

        //[TxAction("DefaultBoundsValues", "Default bounds values")]
        //public void DefaultBoundsValues()
        //{
        //    GetDriver().ClickAt(By.Id("idBtnDefault"), this);
        //}
    }
}
