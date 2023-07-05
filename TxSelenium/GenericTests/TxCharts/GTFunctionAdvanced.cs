using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTFunctionAdvanced : WERefreshed
    {
        private WERefreshed sideItemBarParent;
        public GTFunctionAdvanced(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
            this.sideItemBarParent = new WERefreshed(GetDriver(), By.XPath(".//fieldset[@class='clFieldsetAbs']"),this);
        }

        [TxAction("AbscissaFormat", "AbscissaFormat")]
        public WEGDHtmlxComboBox AbscissaFormat()
        {
            return new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[starts-with(@id,\"ordinateFormat\")]"), this);
            //(".//fieldset[@class='clFieldsetOrd']//input[@class='dhx_combo_input']")
        }

        [TxAction("OrdinateFormat", "OrdinateFormat")]
        public WEGDHtmlxComboBox OrdinateFormat()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"ordinateFormat\")]"), this);
        }

        [TxAction("AbscissaSignificantFigure", "AbscissaSignificantFigure")]
        public WEGInput AbscissaSignificantFigure()
        {
            return new WEGInput(GetDriver(),By.XPath(".//input[starts-with(@id ,\"abscissaPrecision\")]"),this);
        }

        [TxAction("OrdinateSignificantFigure", "OrdinateSignificantFigure")]
        public WEGInput OrdinateSignificantFigure()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id ,\"ordinatePrecision\")]"), this);
        }
    }
}
