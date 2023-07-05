using OpenQA.Selenium;
using System.Threading;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTableView
{
    public class GTEditDecimal : WERefreshed
    {
        public GTEditDecimal(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("EditMin", "......")]
        public WEGInput EditMin()
        {
            By path = By.Id("minInput");
            return new WEGInput(GetDriver(), path, this);
        }

        [TxAction("EditMax", "......")]
        public WEGInput EditMax()
        {
            By path = By.Id("maxInput");
            return new WEGInput(GetDriver(), path, this);
        }

        [TxAction("EditMean", "......")]
        public WEGInput EditMean()
        {
            By path = By.Id("meanInput");
            return new WEGInput(GetDriver(), path, this);
        }

        [TxAction("Units", "......")]
        public void Units(string Unit)
        {
            GetDriver().ClickAt(By.Id("unitSelect"), this);
            Thread.Sleep(2000);
            if(Unit== "°F")
                GetDriver().PressKey(Keys.ArrowDown);
            else
                GetDriver().PressKey(Keys.ArrowUp);
            GetDriver().PressKey(Keys.Enter);
        }
    }
}
