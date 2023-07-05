using OpenQA.Selenium;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using System.Threading;
using XmlExecutor.DataTypes;
using System.Collections.Generic;
using System;

namespace TxSelenium.NativeTests.Windows
{
    public class WDWindow<T> : WERefreshed where T : WERefreshed
    {
        private static readonly By closeButtonBy = By.XPath(".//div[contains(@class, \"dhtmlx_button_close_default\")] | .//button[contains(@class, \"close\")] | .//input[@id = \"close\"] |"+
            ".//div[@class=\"dhxwin_button dhxwin_button_close\"]");
        public static readonly By windowBy = By.XPath(".//*[@class = \"dhtmlx_window_active\"] | .//*[@class = \"dhxwin_active\"]");
        
        /// <summary>
        /// The interactive content of this window.
        /// </summary>
        public T Content
        {
            [TxAction("Content", "Gets a handle on the window's content.")]
            get;
            private set;
        }

        public WDWindow(TxWebDriver driver, T content, WERefreshed parent=null, By frameby=null)
            : base(driver, WDWindow<T>.windowBy,parent,frameby)
        {
            //TODO better way to fix window active and inactive
            Thread.Sleep(500);
            content.Parent = this;
            WaitForAjax();
            Content = content;
        }
        
        [TxAction("Close", "Close the window.")]
        public void Close()
        {
            GetDriver().ClickAt(WDWindow<T>.closeButtonBy, this);
        }
        //[TxAction("MinMaxedWindow", "Minimize and Maximize the window")]
        //public void MinMaxedWindow()
        //{
        //    GetDriver().ClickAt(By.XPath(".//div[@title=\"Min/Max\"]"), this);
        //}

        [TxAction("Cancel", "")]
        public void Cancel()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id=\"idBtnCancel\"]|.//input[@id=\"cancel\"]|.//input[@value=\"Cancel\"]"), this);
        }

        [TxAction("ReadWindowTitle", "")]
        public string ReadWindowTitle()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxwin_text\"]//div[@class=\"dhxwin_text_inside\"]"), this).GetElement().Text;
        }

        [TxAction("MinMaxedWindow", "Minimize and Maximize the window")]
        public void MinMaxedWindow(bool currentwin=true)
        {
            if (currentwin)
                GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxwin_active\"][2]//div[@title=\"Min/Max\"]|.//div[@title=\"Min/Max\"]"));
            else
             GetDriver().ClickAt(By.XPath(".//div[@title=\"Min/Max\" and not(contains(@style,\"display: none\"))]"), this);
        }

        [TxAction("ReadHeader", "")]
        public string ReadHeader()
        {
            WaitForAjax();
            return GetDriver().WaitForElement(By.XPath(".//div[@class = \"dhxwin_text_inside\"]"), this).GetElement().Text;
        }

        [TxAction("IsAttributePresent", "")]
        public bool IsAttributePresent(string attrbuteName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//label[text()=\"" + attrbuteName + "\"] | .//legend[text()=\"" + attrbuteName + "\"]"), this.GetElement());
        }

        [TxAction("IsButtonPresent", "")]
        public bool IsButtonPresent(string buttonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"smc_btn_action_vertical\"]//input[@value=\"" + buttonName + "\"] |.//div[@class=\"dhx_cell_cont_layout\"]//input[@value=\"" + buttonName + "\"]|.//div[@title=\"" + buttonName + "\"]|.//input[@value=\"" + buttonName + "\"]"), this.GetElement());
        }
        [TxAction("Minimize", "")]
        public void Minimize()
        {
            GetDriver().ClickAt(By.XPath(".//button[@id = \"idBtnMinimize\"]"), this);
        }
        [TxAction("ReadValidationMessages", "To read data validation messages")]
        public ActionColl<string> ReadValidationMessages(string attrId = "")
        {
            List<string> msgs = new List<string>();
            ICollection<IWebElement> messages;
            if (attrId!="")
                messages = this.GetElement().FindElements(By.XPath(".//div[starts-with(@id,\"invalid_" + attrId + "\")]//span[starts-with(@class,\"message\")]|.//div[starts-with(@id,\"mandatory_" + attrId + "\")]//span[starts-with(@class,\"message\")]"));
            else
                messages = this.GetElement().FindElements(By.XPath(".//span[starts-with(@class,\"message\")]"));
            foreach (IWebElement message in messages)
                msgs.Add(message.Text);
            return new ActionColl<string>(msgs);
        }

        [TxAction("Export", "Export...")]
        public void Export()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id=\"idBtnValidate\"]|.//input[@value=\"Export\"]"), this);
        }

        [TxAction("IsWindowMaximized", "Read Window is Maximized or Not")]
        public bool IsWindowMaximized()
        {
            int top = 100;
            IWebElement window = this.GetElement();
            string aa = window.GetAttribute("style");
            string[] data = window.GetAttribute("style").Split(';');

            foreach (string d in data)
                if (d.Contains("top"))
                {
                    top = Convert.ToInt32(d.Split(':')[1].Trim().Replace("px", ""));
                    break;
                }
            return !(top > 0);
        }

        [TxAction("Cross", "Click on the Cross of the window.")]
        public void Cross()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,'dhxwin_button_close')]"), this);
        }

        [TxAction("Refresh", "")]
        public void Refresh()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id = \"idBtnRefreshGrid\"]"), this);
        }
    }
}
