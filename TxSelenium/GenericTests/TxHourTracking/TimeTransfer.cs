using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class TimeTransfer : WERefreshed
    {
        private static By SourceTask_Link = By.XPath(".//div[starts-with(@id,\"idDivTimeTransferCellA\")]");

        public TimeTransfer(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        
        [TxAction("SourceTask", "")]
        public WMultipleLink SourceTask()
        {
            return new WMultipleLink(GetDriver(), SourceTask_Link, WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("DestinationTask", "")]
        public WMultipleLink DestinationTask()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivTimeTransferCellB\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("IsSelectDestinationTaskDisabled", "")]
        public bool IsSelectDestinationTaskDisabled(string TaskName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@style,\"radio_offDis\")]/../..//span[text()=\"" + TaskName + "\"]"), this.GetElement());
        }

        [TxAction("IsSelectSourceTaskDisabled", "")]
        public bool IsSelectSourceTaskDisabled(string TaskName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@style,\"iconUncheckDis\")]/../..//span[text()=\"" + TaskName + "\"]"), this.GetElement());
        }

        [TxAction("IsDestenationCheckAllButtonPresent", "")]
        public bool IsDestenationCheckAllButtonPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idDivDestinationTaskToolBar\")]//img[contains(@src,\"CheckObject.png\")and contains(style,\"visablity:visable\")]"), this.GetElement());
        }

        [TxAction("ExpandCollapseWindow", "")]
        public void ExpandCollapseWindow(string windowName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + windowName + "\"]/../..//div[@title=\"Expand / Collapse\"]"), this);
        }

        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[@value=\"" + buttonName + "\"and @disabled=\"disabled\"]"));
        }

        [TxAction("Transfer", "")]
        public void Transfer()
        {
            GetDriver().ClickAt(By.XPath(".//input[@value=\"Transfer\"]"));
        }
    }
}