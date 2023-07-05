using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
   public class GTTxEditRuleForTextLinkDataFormulaLinkBoolean : GTTxBaseEditRule
    {

        public GTTxEditRuleForTextLinkDataFormulaLinkBoolean(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("Defaultvalue", "Default value")]
        public  WEGInput Defaultvalue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTACellDefaultValue\")]"), this);
        }
    }
}
