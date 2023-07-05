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
   public class GTTxEditRuleListChoicesWindow : GTTxBaseEditRule
    {

        public GTTxEditRuleListChoicesWindow(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("Defaultvalue", "Default value")]
        public  WEGDHtmlxComboBox Defaultvalue()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"inputTACellComboDefaultValue\")]"), this);
        }

        /// <summary>
        /// if delete=false then it will create one by one new item as given choices values
        /// if delete=true then it will delte one by one item as given choices values
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="delete"></param>
        [TxAction("ListOfChoices", "Write  List of choices")]
        public void ListOfChoices(ICollection<String> choices, Boolean delete = false)
        {
            if (delete)
            {
                foreach (String c in choices)
                    GetDriver().ClickAt(By.XPath(".//ul[@class=\"tagit ui-widget ui-widget-content ui-corner-all\"]//li//span[text()=\"" + c + "\"]/../a"), this);
            }
            else
            {
                WEGInput input = new WEGInput(GetDriver(), By.XPath(".//input[@class=\"ui-widget-content ui-autocomplete-input\"]"), this);
                foreach (String c in choices)
                {
                    input.FillWithoutClear(c);
                    input.Enter();
                }
            }

        }
    }
}
