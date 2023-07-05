using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.GenericTests.TxTestResult;
using TxSelenium.GenericTests;
using TxSelenium.GenericTests.TxTableView;

namespace TxSelenium.GenericDevs.TxTestResults
{
    /// <summary>
    ///  TxTestResult is very similar to TxTableView module
    ///  That's why this class is inherited from it.
    /// </summary>
    public class GTTxTestResult : GTTxTableView
    {
        public GTTxTestResult(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("PreFilteredView", "Pre-Filtered View")]
        public WDValidatedWindow<GTTxTestResultView> PreFilteredView()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"table_view.png\")]|.//img[contains(@src,\"TableView.png\")]"), this);
            GTTxTestResultView testResultView = new GTTxTestResultView(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTTxTestResultView>(GetDriver(), testResultView);
        }


        [TxAction("SelectLinkForTestResult", "...")]
        public WDValidatedWindow<GTTxTestResultView> SelectLinkForTestResult(String link)
        {
            WERefreshed table = GetDriver().WaitForElement(By.ClassName("objbox"), this);
            GetDriver().ScrollToBottom(table.GetElement());

            WEGLink selectedLink = new WEGLink(GetDriver(), GetLink(link), this);
            selectedLink.Click();

            GTTxTestResultView testResultView = new GTTxTestResultView(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<GTTxTestResultView>(GetDriver(), testResultView);
        }

    }
}
