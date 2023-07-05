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
   public class GTTxBaseEditRule : WERefreshed
    {

        public GTTxBaseEditRule(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("ChangeTab", " Change the tab using id")]
        public void ChangeTab(String tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\""+ tabName + "\" and contains(@class,\"dhxtabbar_tab_text\")]"), this);
        }

        [TxAction("RequiredField", "Required Field checkBox")]
        public WEGCheckBox RequiredField()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[contains(@id,\"inputTACellMandValue\")]"), this);
        }

        [TxAction("DuplicationOfValue", "Duplication of the value checkBox")]
        public WEGCheckBox DuplicationOfValue()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[contains(@id,\"inputTACellDuplValue\")]"), this);
        }

        [TxAction("VisibleColumn", "Visible column checkBox")]
        public WEGCheckBox VisibleColumn()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[contains(@id,\"inputTACellVisibleCol\")]"), this);
        }
    }
}
