using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XmlExecutor.Attributes;

namespace TxSelenium.Utils
{
    public class PressKeyFromKeyBoard
    {
        TxWebDriver driver;
        IWebElement elem;

        public PressKeyFromKeyBoard(TxWebDriver driver, IWebElement elem = null)
        {
            this.driver = driver;
            this.elem = elem;
        }

        //[TxAction("PressShiftTabV2", "....")]
        //public void PressShiftTabV2()
        //{
        //    elem.SendKeys(Keys.Shift + Keys.Tab);
        //}

        [TxAction("PressShiftArrowUp", "....")]
        public void PressShiftArrowUp()
        {
            driver.PressKey(Keys.Shift + Keys.ArrowUp);
            driver.KeyUp(Keys.Shift);
        }
        [TxAction("PressShiftArrowDown", "....")]
        public void PressShiftArrowDown()
        {
            driver.PressKey(Keys.Shift + Keys.ArrowDown);
            driver.KeyUp(Keys.Shift);
        }

        [TxAction("PressTab", "...")]
        public void PressTab()
        {
            Thread.Sleep(1000);
           driver.PressKey(Keys.Tab);
        }

        [TxAction("PressEnter", "")]
        public void PressEnter()
        {
            Thread.Sleep(1000);
           driver.PressKey(Keys.Enter);
        }

        [TxAction("PressUpKey", "ArrowUp")]
        public void PressKeyUp()
        {
           driver.PressKey(Keys.ArrowUp);
        }

        [TxAction("PressDownKey", "ArrowDown")]
        public void PressKeyDown()
        {
           driver.PressKey(Keys.ArrowDown);
        }

        [TxAction("PressHomeKey", "....")]
        public void PressHomeKey()
        {
            driver.PressKey(Keys.Home);
        }

        [TxAction("PressEndKey", "....")]
        public void PressEndKey()
        {
            driver.PressKey(Keys.End);
        }

        [TxAction("PressRightArrow", "....")]
        public void PressRightArrow(int times)
        {
            for (int i = 0; i < times; i++)
                driver.PressKey(Keys.ArrowRight);
        }

        [TxAction("PressLeftArrow", "....")]
        public void PressLeftArrow(int times)
        {
            for (int i = 0; i < times; i++)
                driver.PressKey(Keys.ArrowLeft);
        }

        [TxAction("PressShiftTab", "Check if ok button is disable.")]
        public void PressShiftTab()
        {
            if (elem != null)
                elem.SendKeys(Keys.Shift + Keys.Tab);
            else
            driver.PressKey(Keys.Shift + Keys.Tab);
           
        }

        [TxAction("PressF2", "Check if ok button is disable.")]
        public void PressF2()
        {
            driver.PressKey(Keys.F2);
        }

        [TxAction("PressEscapeKey", "")]
        public void PressKeyEscape()
        {
            driver.PressKey(Keys.Escape);
        }
    }
}
