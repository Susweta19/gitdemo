using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTTxEditRuleForDate : GTTxBaseEditRule
    {
        public GTTxEditRuleForDate(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null) 
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("DefaultValueUsingCalender", "Default Value Using Calender")]
        public WDate DefaultValueUsingCalender()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"clDivIconCCalendar\"]/img"), this);
            return new WDate(GetDriver(), By.Id("ui-datepicker-div"));
        }
        [TxAction("DefaultValueUsingString", "Default Value Using String")]
        public WEGInput DefaultValueUsingString()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTACellDateDefaultValue\")]"), this);
        }


    }
}
