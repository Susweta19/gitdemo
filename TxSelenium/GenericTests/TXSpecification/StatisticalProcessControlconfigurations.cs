using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;

namespace TxSelenium.GenericTests.TXSpecification
{
  public  class StatisticalProcessControlconfigurations : WERefreshed
    {
        public StatisticalProcessControlconfigurations(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("AddSPC", "")]
        public WDValidatedWindow< EditConfiguration> AddSPC()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivConfigManagerListToolbar\")]//img[contains(@src,\"AddObject.png\")]"));
             EditConfiguration editconfig=new EditConfiguration(GetDriver(),By.ClassName("dhxwin_active"));
            return new WDValidatedWindow<EditConfiguration>(GetDriver(), editconfig);
        }
        [TxAction("ReadHeader", "")]
        public string ReadHeader()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxwin_text_inside\"]"), this).GetElement().Text;
        }
        [TxAction("ClickOnButtonFromToolbar", "")]
        public void ClickOnButtonFromToolbar(string buttonName)
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[@title=\"" + buttonName + "\"]"), this);
            WaitForAjax();
        }
        [TxAction("ReturnManageDashboard", "")]
        public WDValidatedWindow<ManageDashBoard> ReturnManageDashboard()
        {
            ManageDashBoard dash = new ManageDashBoard(GetDriver(), By.ClassName("dhxwin_active"));
            return new WDValidatedWindow<ManageDashBoard>(GetDriver(), dash);
        }
    }
}
