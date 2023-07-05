using OpenQA.Selenium;
using System.Threading;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests
{
    public class GTTab<T> where T : WERefreshed
    {
        private TxWebDriver _driver { get; set; }

        /// <summary>
        /// The interactive content of the new tab.
        /// </summary>
        public T Content
        {
            [TxAction("Content", "Gets a handle on the tab's content.")]
            get; 
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="content"></param>
        public GTTab(TxWebDriver driver, T content)
        {
            Thread.Sleep(2000);
            _driver = driver;
            _driver.SwitchNewWindow();
            _driver.GetCurrentFrame();
            _driver.WaitForAjax();
            Content = content;
        }

        /// <summary>
        /// Closes the tab and switch the driver's focus back to the main frame.
        /// </summary>
        [TxAction("Close", "Closes currentFrame and switch to the one you choose with index.")]
        public void Close(int index = 0)
        {
            _driver.Close();
            _driver.SwitchToWindow(index);
            _driver.SwitchToContent(); 
        }

        //TODO create function to switch between different tab not without closing using ctrl + a ......
        /// <summary>
        /// Closes the tab and switch the driver's focus back to the main frame.
        /// </summary>
        [TxAction("Changetab", "Closes currentFrame and switch to the one you choose with index.")]
        public void Changetab(int indexTab)
        {
            _driver.SwitchToWindow(indexTab);
            _driver.SwitchToContent();
        }

        [TxAction("ReadBrowserTitle", "****")]
        public string ReadBrowserTitle()
        {
            Thread.Sleep(2000);
            return _driver.GetTitle();
        }

        [TxAction("CompareURL", "To read URL and to check url contains a given part or not")]
        public bool CompareURL(string url, bool contains = false)
        {
            Thread.Sleep(3000);
            return contains ? _driver.Driver.Url.Contains(url) : _driver.Driver.Url.Equals(url);
        }
        [TxAction("ReadURL", "To read URL and to check url contains a given part or not")]
        public bool ReadURL(string url, bool contains = false)
        {
            Thread.Sleep(3000);
            return contains ? _driver.Driver.Url.Contains(url) : _driver.Driver.Url.Equals(url);
        }
        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            
            return _driver.IsElementPresent(By.XPath(".//*[@value=\"" + buttonName + "\" and @disabled]"));
        }
    }
}
