using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxVisualDesign
{
    public class GTVisualization : WERefreshed
    {
        public GTVisualization(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("VisualizationOption", "")]
        public void VisualizationOption(string Button)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + Button + "\"]"), this, By.XPath(".//iframe[contains(@src,\"5_en.asp\")]"));
        }
    }
}
