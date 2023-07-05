using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium;
using TxSelenium.GenericDevs;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;
using XmlExecutor.Interfaces;
using TxSelenium.NativeTests.Writable;
using TxSelenium.GenericTests;
using TxSelenium.GenericTests.TxWorkFlow;
using System.Threading;
using TxSelenium.NativeTests.DataTypes;

namespace Lafarge
{
    public class AHMainWindow : WDMainWindow
    {
        By iframe = By.XPath(".//iframe[contains(@src,\"41037.aspx\")] | .//iframe[contains(@src,\"41038.aspx\")]");

        public AHMainWindow(WDMainWindow baseWindow)
            : base(baseWindow.GetDriver(), baseWindow.ElemIdentifier, baseWindow.Parent) {}

        [TxOverload(typeof(WDMainWindow), typeof(AHMainWindow))]
        public static object Overload(object baseInstance)
        {
            return new AHMainWindow(baseInstance as WDMainWindow);
        }

        [TxAction("ValidationProcess", "Open ValidationProcess Windows")]
        public GTTxWorkFlow ValidationProcess()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title='Validation process']"), this);
            return new GTTxWorkFlow(GetDriver(),By.XPath(".//td[@class='dhx_popup_td']"),this);
        }

        [TxAction("AHReadBanner", "check the name of the object in the banner....")]
        public object AHReadBanner(String convertHastagAs)
        {
            Thread.Sleep(1000);//to solve 26_32
            WERefreshed headerName = GetDriver().WaitForElement(By.Id("idDivBannerMiddle"), this);


            switch (convertHastagAs)
            {
                case "DTEntityNode":
                    ICollection<string> hh = new List<string>();
                    hh.Add(headerName.GetElement().Text);
                    DTEntityNode node = new DTEntityNode(hh);
                    return node;
                default:
                    return headerName.GetElement().Text;
            }


            //return headerName.GetElement().Text;
        }
    }
}
