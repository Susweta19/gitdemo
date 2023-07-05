using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements
{
    public class WEGInput : WERefreshed
    {
        public WEGInput(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
            base.WaitForElement();
        }

        [TxAction("Click", "")]
        public void Click()
        {
            GetElement().Click();
        }

        [TxAction("Write", "Gets the displayed text value.")]
        public void Fill(string value)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver().Driver, TimeSpan.FromSeconds(5));
            bool elem = wait.Until<bool>((d) =>
            {
                try
                {
                    GetElement().Clear();
                    GetElement().SendKeys(value);
                    return true;
                }
                catch (InvalidElementStateException)
                {
                    return false;
                }
            });
            
        }

        [TxAction("Read", "Gets the displayed text value.")]
        public string Read()
        {
            return GetElement().GetAttribute("value");
        }

        [TxAction("PressEnter", "Gets the displayed text value.")]
        public void Enter()
        {
            GetElement().SendKeys(Keys.Enter);
        }

        [TxAction("Tab", "")]
        public void Tab()
        {
            this.GetElement().SendKeys(Keys.Tab);
        }

        [TxAction("ShiftTab", "")]
        public void ShiftTab()
        {
            this.GetElement().SendKeys(Keys.Shift + Keys.Tab);
        }

        [TxAction("F2", "")]
        public void F2()
        {
            this.GetElement().SendKeys(Keys.F2);
        }

        [TxAction("Escape", "")]
        public void Escape()
        {
            this.GetElement().SendKeys(Keys.Escape);
        }

        [TxAction("End", "")]
        public void End()
        {
            this.GetElement().SendKeys(Keys.End);
        }

        [TxAction("Clear", "Gets the displayed text value.")]
        public void Clear()
        {
            GetElement().Clear();          
        }

        [TxAction("WriteWithoutClear", "Gets the displayed text value.")]
        public void FillWithoutClear(string value)
        {
            //TODO Temporary changes to face InvalidElementStateExceptionwhen filling New url 402
            WebDriverWait wait = new WebDriverWait(GetDriver().Driver, TimeSpan.FromSeconds(5));
            bool elem =    wait.Until<bool>((d) =>
            {
               try
                {
                    GetElement().SendKeys(value);
                    return true;
                }
                catch(InvalidElementStateException)
                {
                    return false;
                }
            });         
        }

        [TxAction("SelectAll", "Gets the displayed text value.")]
        public void SelectAll()
        {
            GetDriver().ClickAt(ElemIdentifier, Parent, FrameBy);
            GetDriver().PressKey(Keys.Control + "a");
            GetDriver().KeyUp(Keys.Control);
        }

        [TxAction("ValidateCancel", "ValidateCancel")]
        public void ValidateCancel(bool validate)
        {
            if (validate == true)
            {
                GetDriver().ClickAt(By.XPath(".//div[contains(@id,'idDivBtns')]//input[@id='idBtnValidate']"));
            }
            else
                GetDriver().ClickAt(By.XPath(".//div[contains(@id,'idDivBtns')]//input[@id='idBtnCancel']"));
            //By button = validate ? By.Id("idBtnValidate") : By.Id("idBtnCancel");
            //GetDriver().ClickAt(button, this);
        }
    }
}
