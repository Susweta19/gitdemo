using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TXSpecification
{
    public class GTTxSpecificationControlLayout : WERefreshed
    {
        public GTTxSpecificationControlLayout(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("IsDBCardPresent", "***.")]
        public bool IsDBCardPresent(string cardName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"spcDBControlTitle\" and text()=\"" + cardName+"\"]"), this.GetElement());
        }

        [TxAction("ReturnTxSpecificationInNewTab", "Passes Teexma to New Tab")]
        public GTTab<GTTxSpecification> ReturnTxSpecificationInNewTab()
        {
            GetDriver().WaitForAjax();
            GTTxSpecification txSpecification = new GTTxSpecification(GetDriver(), By.TagName("html"));
            return new GTTab<GTTxSpecification>(GetDriver(), txSpecification);
        }
        [TxAction("ClickOnStatistics", "")]
        public void ClickOnStatistics(string statisticsName)
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[text()=\""+ statisticsName + "\"]/.."),this);
        }


        [TxAction("IsStatisticsPresent", "")]
        public bool IsStatisticsPresent(string StatisticsName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[text()=\""+StatisticsName+"\"]/../..//div[contains(@id,\"spcDBCard\")]"));
        }
        [TxAction("GetStatisticsControlTabName", "")]
        public string GetStatisticsControlTabName(string StatisticsName)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[text()=\"" + StatisticsName + "\"]/../../..//div[contains(@class,\"spcDBControlTitle\")]")).GetElement().Text;
        }

    }
}
