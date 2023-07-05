using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTSelectColor : WERefreshed
    {
        public GTSelectColor(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
                : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("SetCurveTint", "Sets the curve Color")]
        public WEGInput SetColor()
        {
            return new WEGInput(GetDriver(),By.XPath(".//tr[1]//input[@class = \"dhxcp_input_hsl\"]"), this);
        }

        [TxAction("SetHexColor", "Sets the HexCode of curve Color")]
        public WEGInput SetHexColor()
        {
            WaitForAjax();
            return new WEGInput(GetDriver(), By.XPath(".//div[@class = \"dhxcp_value_cont\"]/input"), this);
        }

        [TxAction("SelectColor", "Selects a curve color form the given colors below the Add Color button")]
        public void SelectColor(int index, string chartType = "Scatter")
        {
            GetDriver().WaitForAjax();
            //  string aa = ".//div[starts-with(@class,\"" + chartType + "\")]//div[starts-with(@id,\"leftPallet\")]//div[@class=\"colorsContainer\"]";
            GetDriver().ClickAt(By.XPath(".//div[@class = \"dhxcp_memory_els_cont\"]/a[" + index + "]|.//div[starts-with(@id,\"leftPallet\")]//div[@class=\"colorsContainer\"]//div[starts-with(@class,\"colorBlock\")][" + index + "]"), this);
        }

        [TxAction("SetCurveSaturation", "Selects a curve color")]
        public WEGInput SetSaturation()
        {
            return new WEGInput(GetDriver(), By.XPath(".//tr[2]//input[@class = \"dhxcp_input_hsl\"]"), this);
        }

        [TxAction("SetCurveBrightness", "Selects a curve color")]
        public WEGInput SetBrightness()
        {
            return new WEGInput(GetDriver(), By.XPath(".//tr[3]//input[@class = \"dhxcp_input_hsl\"]"), this);
        }

        [TxAction("SetRed", "Selects a curve color")]
        public WEGInput SetRed()
        {
            return new WEGInput(GetDriver(), By.XPath(".//tr[1]//input[@class = \"dhxcp_input_rgb\"]"), this);
        }

        [TxAction("SetGreen", "Selects a curve color")]
        public WEGInput SetGreen()
        {
            return new WEGInput(GetDriver(), By.XPath(".//tr[2]//input[@class = \"dhxcp_input_rgb\"]"), this);
        }
        
        [TxAction("SetBlue", "Selects a curve color")]
        public WEGInput SetBlue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//tr[3]//input[@class = \"dhxcp_input_rgb\"]"), this);
        }

        [TxAction("AddColor", "Selects a curve color")]
        public void AddColor()
        {
            GetDriver().ClickAt(By.XPath(".//button[@class = \"dhxcp_save_to_memory\"]"), this);
        }

        [TxAction("AdvancedColour", "Selects a curve color")]
        public void AdvancedColour()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"pallet\")and not(contains(@style,\"display: none;\"))]//img[starts-with(@id,\"iconAdvanced\")]"));
        }

        [TxAction("ValidatedColour", "Selects a curve color")]
        public void ValidatedColour(bool validated = true)
        {
            if (validated)
            {
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"rightPallet\")and not(contains(@style,\"display: none;\"))]//img[starts-with(@id,\"iconValid\")]"));
            }
            else
            {
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"rightPallet\")and not(contains(@style,\"display: none;\"))]//img[starts-with(@id,\"iconCancel\")]"));
            }

        }
    }
}
