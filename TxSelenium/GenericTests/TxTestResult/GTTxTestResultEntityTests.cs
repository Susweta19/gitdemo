using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
   public  class GTTxTestResultEntityTests : WERefreshed
    { 
        public GTTxTestResultEntityTests(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }
        private string GetActiveTabId()
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@class,\"dhxtabbar_tab dhxtabbar_tab_actv\")]"), this).WaitForElement();
            return elem.GetElement().GetAttribute("tab_id");
        }

        [TxAction("Table", "Manage ant Table")]
        public GTTxBaseEntityTests Table(int tableIndex = 1)
        {
            return new GTTxBaseEntityTests(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_tabbar\") and contains(@style,\"visibility: visible;\")]//div[@class=\"trBlockContainer\"][" + tableIndex + "]"), this);
        }

        [TxAction("ChangeTab", " Change the tab using its name")]
        public void ChangeTab(String tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxtabbar_tab_text\") and text()=\""+tabName+"\"]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("ConditionTable", "ConditionTable")]
        public GTTxBaseEntityTests ConditionTable()
        {
            return new GTTxBaseEntityTests(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_tabbar\") and contains(@style,\"visibility: visible;\")]//div[@class=\"trBlockContainer\"][1]"), this);
        }

        [TxAction("ResultTable", "Result Table")]
        public GTTxBaseEntityTests ResultTable()
        {
            return new GTTxBaseEntityTests(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_tabbar\") and contains(@style,\"visibility: visible;\")]//div[@class=\"trBlockContainer\"][2]"), this);
        }

        [TxAction("TractionTable", "Traction Table")]
        public GTTxBaseEntityTests TractionTable()
        {
            return new GTTxBaseEntityTests(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_tabbar\") and contains(@style,\"visibility: visible;\")]//div[@class=\"trBlockContainer\"][1]"), this);
        }

        [TxAction("Ok", "Closes the window by clicking on Ok or Cancel or the equivalent.")]
        public virtual void Validate(Boolean validate = true)
        {
            if(validate)
                GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idBtnValidate\")]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idBtnQuit\")]"), this);
        }
        [TxAction("OkDisable", "Check if ok button is disable.")]
        public void OkDisable()
        {
            WERefreshed okButton = GetDriver().WaitForElement(Translator.GetButtonByValue(GetDriver(), Translator.validateButton), this);
            String disable = okButton.GetElement().GetAttribute("disabled");
            if (disable == null)
                throw new Exception("Ok button enable");
        }

        [TxAction("ClickOnCell", "Check if ok button is disable.")]
        public void ClickOnCell(int row, int col)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"objbox\"]//tr[" + row + "]//td[" + col + "]"), this);
        }

        [TxAction("PressShiftTab", "Check if ok button is disable.")]
        public void PressShiftTab(int row, int col)
        {
            IWebElement cell = GetDriver().WaitForElement(By.XPath(".//div[@class=\"objbox\"]//tr[" + row + "]//td[" + col + "]//input"), this).GetElement();
            new PressKeyFromKeyBoard(GetDriver(), cell).PressShiftTab();
        }

        [TxAction("IscellSelected", "Check if ok button is disable.")]
        public string IscellSelected(int row, int col)
        {
            return GetDriver().FindElement(this.GetElement(), By.XPath(".//div[@class=\"objbox\"]//tr[" + row + "]//td[" + col + "]")).GetAttribute("class").ToString();
        }
    }
}
