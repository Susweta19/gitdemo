using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTManageSample : WERefreshed
    {
        public GTManageSample(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        By AttributeList = By.XPath(".//div[starts-with(@id,\"idDivSamplesObjectSelectionCell\")]");
        By SampleTxTableViewGrid = By.XPath(".//div[starts-with(@id,\"idDivSamplesGrid\")]");
        By SampleRoutingGrid = By.XPath(".//div[starts-with(@id,\"idDivGridRouting\") and not(contains(@id,\"Toolbar\"))]");
        By ContextPath = By.XPath(".//div[starts-with(@id,\"idDivSamplesBreadCrumb\")]//span");
        By CurrentTab = By.XPath(".//div[contains(@class,\"dhxtabbar_tab_actv\")]//div[@class=\"dhxtabbar_tab_text\"]");
        By AttributeListParent = By.XPath(".//div[starts-with(@id,\"idDivSamplesObjectSelectionCell\")]/parent::div[starts-with(@class,dhx_cell_cont_layout)]/..");
        By OptionListParent = By.XPath(".//div[not(contains(@style,\"display: none\"))]/table[@class=\"buttons_cont\"]");



        By AdvancedButton = By.XPath(".//input[starts-with(@id,\"idBtnShowTree\")]");
        By AddSampleButton = By.XPath(".//img[contains(@src,\"-AddObject.png\")]");
        By DeleteSampleButton = By.XPath(".//div[contains(@class,\"dhx_toolbar_btn\")]/img[contains(@src,\"DeleteObject\")]");
        By OptionButton = By.XPath(".//img[contains(@src,\"-Structure.png\")]");
        private By GetTabXPath(string tabName)
        {
            return By.XPath(".//div[text()='" + tabName + "' and @class=\"dhxtabbar_tab_text\"]");
        }

        private By GetOptionXpath(string name)
        {
            return By.XPath(".//div[@class=\"btn_sel_text\" and text()='" + name + "']");
        }

        private By GetContextXPath(string name)
        {
            return By.XPath(".//div[starts-with(@id,\"idDivSamplesBreadCrumb\")]//span[text()='" + name + "']");
        }
        [TxAction("AddSample", "Adding New Sample")]
        public WDValidatedWindow<WTable> AddSample()
        {

            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn\")]/img[contains(@src,\"AddObject\")]"),this);
            WTable manageSample = new WTable(GetDriver(), By.XPath(".//div[contains(@class,\"dhx_cell_wins\")]"));
            return new WDValidatedWindow<WTable>(GetDriver(), manageSample, null);
        }
        [TxAction("RemoveSample", "Remove Selected Sample")]
        public void RemoveSample()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn\")]/img[contains(@src,\"DeleteObject\")]"), this);
        }
        [TxAction("ImportSample", "Import A Sample")]
        public void ImportSample()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn\")]/img[contains(@src,\"AddChildObject\")]"), this);
        }
        [TxAction("PrintLable", "")]
        public void PrintLable()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn\")]/img[contains(@src,\"gbarcode-alt\")]"), this);
        }
        [TxAction("ManageTable", "Manage Table Cell")]
        public WTable ManageNewSampleTable()
        {
            return new WTable(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivSamplesGrid\")]"),this);
        }

        [TxAction("ManageSampleRoutingTable", "Manage Table Cell")]
        public WTable ManageSampleRoutingTable()
        {
            return new WTable(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivSamplesRoutingGrid\")]"), this);            
        }
        [TxAction("ReadContext", "")]
        public string ReadContext()
        {
            GetDriver().WaitForAjax();
            List<IWebElement> elem = this.GetElement().FindElements(ContextPath).ToList();
            string res = "";
            if (elem.Count > 1)
            {
                foreach (var e in elem)
                    res += e.Text + "/";
                res = res.Substring(0, res.Length - 1);
            }
            else
                foreach (var e in elem)
                    res += e.Text;

            return res;
        }
        [TxAction("ReadCurrentTab", "")]
        public string ReadCurrentTab()
        {
            return GetDriver().WaitForElement(CurrentTab, this).GetElement().Text;
        }

        [TxAction("ListOfAttributes", "Select an attribute.")]
        public WESTree ListOfAttributes()
        {
            return new WESTree(GetDriver(), AttributeList, WESTree.expandByDefCriteriaTree, this);
        }
        [TxAction("SampleGrid", "")]
        public GTTxTableView SampleGrid()
        {
            return new GTTxTableView(GetDriver(), SampleTxTableViewGrid, this);
        }
        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string imageName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn dhxtoolbar_btn_dis\")]/img[contains(@src,\"" + imageName + "\")]"), this.GetElement());
        }
        [TxAction("ChangeTab", "")]
        public void ChangeTab(string tabName)
        {
            GetDriver().ClickAt(GetTabXPath(tabName), this);
        }
        [TxAction("SampleRoutingTable", "Manage Table Cell")]
        public GTRoutingTable SampleRoutingTable()
        {
            return new GTRoutingTable(GetDriver(), SampleRoutingGrid, this);
        }
        [TxAction("IsTreeVisible", "")]
        public bool IsTreeVisible()
        {
            return !this.GetElement().FindElement(AttributeListParent).GetAttribute("class").Contains("dhxlayout_collapsed_v");
        }
        [TxAction("Advanced", "")]
        public void Advanced()
        {
            GetDriver().ClickAt(AdvancedButton, this);
        }
        [TxAction("ClickOnContext", "")]
        public void ClickOnContext(string name)
        {
            GetDriver().ClickAt(GetContextXPath(name), this);
        }

        [TxAction("SelectOption", "")]
        public void SelectOption(string option)
        {
            GetDriver().ClickAt(OptionButton, this);
            GetDriver().ClickAt(new ByChained(OptionListParent, GetOptionXpath(option)));
        }
        [TxAction("ReturnSampleHistory", "ReturnObjectHistory")]
        public WDWindow<WTable> ReturnSampleHistory()
        {
            WTable manageSample = new WTable(GetDriver(), By.XPath(".//div[contains(@class,\"dhx_cell_wins\")]"));
            return new WDWindow<WTable>(GetDriver(), manageSample);
        }
    }
}
