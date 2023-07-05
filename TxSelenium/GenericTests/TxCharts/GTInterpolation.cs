using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTInterpolation : WERefreshed
    {
        public GTInterpolation(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("SelectCurve", ".....")]
        public WEGDHtmlxComboBox SelectCurve()
        {
            try
            {
                return new WEGDHtmlxComboBox(GetDriver(), By.Id("chartInterpolationSeries"), this);
            }
            catch
            {
                return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartInterpolationSeries\")]"));
            }
        }

        [TxAction("Abscissa", "............")]
        public WEGInput Abscissa()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@name,'abscissa')]"), this);
        }

        [TxAction("Ordinate", "............")]
        public WEGInput Ordinate()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@name,'ordinate')]"), this);
        }

        [TxAction("Clear", "............")]
        public void Clear()
        {
            GetDriver().ClickAt(By.Id("idBtnClear"), this);
        }

        [TxAction("Calcutate", "............")]
        public void Calcutate()
        {
            GetDriver().ClickAt(By.Id("idBtnValidate"), this);
        }

        [TxAction("Cancel", "............")]
        public void Cancel()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class='dhxwin_active']//input[@id='idBtnCancel']"));
        }
    }
}
