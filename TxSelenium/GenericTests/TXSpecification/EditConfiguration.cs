using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.GenericDevs.TxMCS;
using TxSelenium.GenericTests.TxCharts;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TXSpecification
{
    public class EditConfiguration : WERefreshed
    {
        public EditConfiguration(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("ReadNumberOfTab", "")]
        public int ReadNumberOfTab()
        {
            return GetDriver().FindElement(By.XPath(".//ul[@role=\"tablist\"]")).FindElements(By.XPath(".//li")).Count();
        }
        [TxAction("XAxisName", "")]
        public WEGInput XAxisName()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"xAxisName\")]"), this);
        }
        [TxAction("YAxisName", "")]
        public WEGInput YAxisName()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"yAxisName\")]"), this);
        }
        [TxAction("SelectAttributeDate", "")]
        public WMultipleLink SelectAttributeDate()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[contains(@id,\"axisXTree\")]//div[@class=\"containerTableStyle\"]/../.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("SelectProperties", "")]
        public WMultipleLink SelectProperties()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[contains(@id,\"axisYTree\")]//div[@class=\"containerTableStyle\"]/../.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("Selectattributetocontrol", "")]
        public WMultipleLink Selectattributetocontrol()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[@id=\"colList\"]/.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("Next", "")]
        public void Next()
        {
            GetDriver().ClickAt(By.Id("btnNext"), this);
            WaitForAjax();
        }
        [TxAction("Previous", "")]
        public void Previous()
        {
            GetDriver().ClickAt(By.Id("btnPrevious"), this);
            WaitForAjax();
        }
        [TxAction("SelectAttributeControl", "")]
        public WMultipleLink SelectAttributeControl()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[@id=\"colList\"]/ancestor::div[@class=\"GenericField\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("IsAttributeNamePresent", "")]
        public bool IsAttributeNamePresent(string AttributeName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[text()=\""+AttributeName+ "\"]|.//span[text()=\""+AttributeName+"\"]"));
        }
        [TxAction("RefreshContent", "Refresh")]
        public void RefreshContent()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Refresh.png\")]"), this);
            WaitForAjax();
        }
        [TxAction("ReturnTableView", "")]
        public GTTxTableView ReturnTableView()
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//div[@class=\"previewData\"]"));
        }
        [TxAction("DifineMulticriteriaSelection", "Display the multicriteria selection module...")]
        public WDValidatedWindow< GTTxMCS> DifineMulticriteriaSelection()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"MCS.png\")]"), this);
            WaitForAjax();
            GTTxMCS gttxmcs = new GTTxMCS(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<GTTxMCS>(GetDriver(), gttxmcs);
        }
        [TxAction("IsGroupPresent", "")]
        public bool IsGroupPresent(string groupName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//legend[@class=\"GenericLegend\"and text()=\""+groupName+"\"]"));
        }
        [TxAction("TargetValue", "")]
        public WEGInput TargetValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"parametersTargetValue\")]"));
        }

        [TxAction("ReadTargetValue", "")]
        public string ReadTargetValue(string targetFieldName)
        {
         //   string aa = GetDriver().FindElement(By.XPath(".//div[@class=\"GenericRow\"]//div[starts-with(@class,\"GenericLabel \")and text()=\"Low Oversight limits at: \"]/..//div[@class=\"GenericField\"]")).Text;
          
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"GenericRow\"]//div[starts-with(@class,\"GenericLabel \")and text()=\""+targetFieldName+ "\"]/..//span[@class=\"displayTargetValue\"] | .//label[text()=\"" + targetFieldName + "\"]/ancestor::div[starts-with(@class,\"GenericRow\")]//span[@class=\"displayTargetValue\"]"), this).GetElement().Text;
        }

        [TxAction("ClickOnSigma", "")]
        public void ClickOnSigma(string fieldName)
        {
            GetDriver().WaitForElement(By.XPath(".//div[@class=\"GenericRow\"]//div[starts-with(@class,\"GenericLabel \")and text()=\""+fieldName+ "\"]/..//div[starts-with(@id,\"sigmaParameters\") ]| .//label[text()=\""+fieldName+"\"]/ancestor::div[starts-with(@class,\"GenericRow\")]//div[starts-with(@id,\"sigmaParameters\")and @class=\"toggle-btn toggle-btn--on\"]"), this).GetElement().Click();
        }
        [TxAction("WriteOnField", "")]
        public WEGInput WriteOnField(string fieldName)
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"GenericRow\"]//div[starts-with(@class,\"GenericLabel \")and text()=\"" + fieldName + "\"]/..//div[@class=\"GenericField\"]//input | .//label[text()=\""+fieldName+"\"]/ancestor::div[starts-with(@class,\"GenericRow\")]//input"), this);
        }
        [TxAction("IsSigmaEnabled", "")]
        public bool IsSigmaEnabled(string fieldName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"GenericRow\"]//div[starts-with(@class,\"GenericLabel \")and text()=\""+fieldName+ "\"]/..//div[starts-with(@id,\"sigmaParameters\")and @class=\"toggle-btn toggle-btn--on\"] | .//label[text()=\""+fieldName+"\"]/ancestor::div[starts-with(@class,\"GenericRow\")]//div[starts-with(@id,\"sigmaParameters\")and @class=\"toggle-btn toggle-btn--on\"]"));
        }
        [TxAction("SelectColour", "")]
        public GTSelectColor SelectColour()
        {
            GetDriver().ClickAt(By.XPath(".//span[starts-with(@id,\"representationColor\")]"));
            return new GTSelectColor(GetDriver(), By.TagName("body"));
        }
        [TxAction("ControlChartName", "")]
        public WEGInput ControlChartName()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"controlChartName\")]"));
        }
        [TxAction("ControlChartDescription", "")]
        public WEGInput ControlChartDescription()
        {
            return new WEGInput(GetDriver(), By.XPath(".//textarea[starts-with(@id,\"controlChartDescription\")]"));
        }
        [TxAction("ChangeTab", "")]
        public void ChangeTab(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxtabbar_tab_text\"and text()=\""+tabName+"\"]"));
            WaitForAjax();
        }
        [TxAction("IsGraphVisable", "")]
        public bool IsGraphVisable()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_cell_tabbar\"and contains(@style,\"visibility: visible;\")]"));
        }
        [TxAction("ClickOnSlider", "")]
        public void ClickOnSlider(string FieldName)
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Display in dashboard: \"]/..//span[@class=\"displayBannerSlider\"] | .//label[text()=\"Display in dashboard\"]/ancestor::div[starts-with(@class,\"GenericRow\")]//label[@class=\"displayBannerSwitch\"]"));
        }
    }
}