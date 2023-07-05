using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    /// <summary>
    /// Represents a window with an interactive content.
    /// </summary>
    /// <typeparam name="T"> the type of the interactive content </typeparam>
    /// 
    public class WDFramedWindow<T> : WERefreshed where T : WERefreshed 
    {
        public static readonly By closeButton = By.XPath(".//div[@class=\"dhxwin_active\"]//div[@title=\"Close\"]|.//div[@class=\"dhxwin_active\"]//div[contains(@class,\"button_close\")]");
        public static readonly By frameWindowBy = By.XPath(".//div[contains(@class, \"dhtmlx_window_active\")]//iframe | .//div[@class=\"dhxwin_active\"]//div[@class=\"dhx_cell_wins\"]//iframe | .//div[contains(@id, \"SMC_Form\")]");

        protected TxWebDriver Driver { get; set; }

        /// <summary>
        /// The interactive content of this window.
        /// </summary>
        public T Content
        {
            [TxAction("Content", "Gets a handle on the window's content.")]
            get;
            private set;
        }

        /// <summary>
        /// The window constructor.
        /// The content object's By identifier should be relative to the iframe.
        /// The constructor must be called once the window is open and as long as
        /// the window is not closed elements from other frames will not be accessible.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="driver"></param>
        public WDFramedWindow(TxWebDriver driver, T content, WERefreshed parent, By frameby)
            :base(driver, null, parent, frameby)
        {
            this.Driver = driver;
            content.Parent = this;
            Content = content;
        }
      
        /// <summary>
        /// Closes the window and switch the driver's focus back to the main frame.
        /// </summary>
        [TxAction("Close", "Closes the frame window.")]
        public void Close()
        {
            Driver.ClickAt(WDFramedWindow<T>.closeButton);
        }
        [TxAction("ReadHeader", "")]
        public string ReadHeader()
        {
            WaitForAjax();
            return GetDriver().WaitForElement(By.XPath(".//div[@class = \"dhxwin_text_inside\"]")).GetElement().Text;
        }
        [TxAction("IsButtonPresent", "")]
        public bool IsButtonPresent(string buttonName)
        {
            WaitForAjax();
            string path = ".//div[@class=\"smc_btn_action_vertical\"]//input[@value=\"" + buttonName + "\"]|.//input[@value=\"" + buttonName + "\"]";
            int a = 0;
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"smc_btn_action_vertical\"]//input[@value=\"" + buttonName + "\"]|.//input[@value=\"" + buttonName + "\"]"), this.GetElement());
        }
        [TxAction("IsAttributePresent", "")]
        public bool IsAttributePresent(string attrbuteName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idDivExportCellA\")]//label[text()=\"" + attrbuteName + "\"] |.//div[starts-with(@class,\"clDivZipExport\")]//label[text()=\"" + attrbuteName + "\"] |.//legend[text()=\"" + attrbuteName + "\"]"), this.GetElement());
        }

        //No longer used
        /// <summary>
        /// Creates a decorated object wich switches to the active 
        /// window every time an interaction is tried.
        /// </summary>
        /// <param name="content">the original content object</param>
        /// <returns>the decorated object</returns>
        //private T CreateProxy(T content)
        //{
        //    var decorator = new Decorator<T>(content);
        //    decorator.BeforeExecute += (s, e) => Driver.SwitchToActiveFramedWindow();
        //    AttributeUtils utils = new AttributeUtils(typeof(TxActionAttribute));
        //    decorator.Filter = m => utils.IsActionMethod(m);
        //    return decorator.GetTransparentProxy() as T;
        //}
    }
}
