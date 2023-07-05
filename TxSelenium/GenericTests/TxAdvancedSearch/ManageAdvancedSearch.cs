using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.DataTypes;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.GenericTests.TxAdvancedSearch;
using TxSelenium.GenericDevs.TxMCS;

namespace TxSelenium.GenericTests.TxAdvancedSearch
{
    public class ManageAdvancedSearch : WERefreshed
    {
        public ManageAdvancedSearch(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }


        [TxAction("ExpandedCollapsedConfigList", "Expanded andCollapsed ConfigList by click")]
        public void ExpandedCollapsedConfigList(string configName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"dhx_cell_hdr_text dhx_cell_hdr_icon\"]//span[text()=\"" + configName + "\"]/../..//div[starts-with(@class,\"dhx_cell_hdr_arrow\")]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("ReadConfigList", "")]
        public ActionColl<string> ReadConfigList(string objectType)
        {
            By configListByOT = By.XPath(".//span[text()=\""+ objectType + "\"]/ancestor::div[@class=\"dhx_cell_acc\"]//ul[@class=\"advanceSearchOTRequirementsList\"]");
            ICollection<IWebElement> element = GetDriver().WaitForElement(configListByOT, this).GetElement().FindElements(By.XPath(".//li//div[@class='advanceSearchBoxName']"));
            ICollection<string> configNames = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                configNames.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(configNames);
        }
        [TxAction("SelectConfiguration", "")]
        public void SelectConfiguration(string configName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"advanceSearchBoxName\" and text()=\"" + configName + "\"]/../../.."), this);
        }

        [TxAction("ManageEntityTree", "")]
        public void ManageEntityTree(string Name)
        {
            GetDriver().DoubleClickAt(By.XPath(".//span[starts-with(@class,\"clLigneObjectResults\") and @title=\"" + Name + "\"]"), this);
        }
        
        [TxAction("IsConfigurationPresent", "Configuration Present ")]
        public bool IsConfigurationPresent(string configName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"advanceSearchBoxName\" and text()=\"" + configName + "\"]"), this.GetElement()); ;
        }
        [TxAction("SaveAs", "")]
        public WEGInput SaveAs()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,'idInputSaveAs')]"), this);
        }
        [TxAction("DownloadConfiguration", " To download a Configuration")]
        public void DownloadConfiguration(string configName)
        {
            WERefreshed parent = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"advanceSearchBoxName\" and text()=\"" + configName + "\"]/ancestor::li"), this);
            GetDriver().MouseOver(parent);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Download.png\")]"), parent);
            GetDriver().Release();
        }
        [TxAction("RemoveConfiguration", " To remove a Configuration")]
        public void RemoveConfiguration(string configName)
        {
            WERefreshed parent = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"advanceSearchBoxName\" and text()=\"" + configName + "\"]/ancestor::li"), this);
            GetDriver().MouseOver(parent);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"False.png\")]"), parent);
            GetDriver().Release();
        }
        [TxAction("EditConfigurationName", " To rename a Configuration")]
        public void EditConfigurationName(string oldConfigName, string newConfigName)
        {
            WERefreshed parent = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"advanceSearchBoxName\" and text()=\"" + oldConfigName + "\"]/ancestor::li"));
            GetDriver().MouseOver(parent);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"EditObject.png\")]"), parent);
            GetDriver().Release();
            Sleep(1);
            new WEGInput(GetDriver(), By.XPath("//li//input"), this).Fill(newConfigName);
            new WEGInput(GetDriver(), By.XPath("//li//input"), this).Enter();
        }
        [TxAction("ReturnTxAdvancedSearch", "Passes Teexma in table view.")]
        public WDValidatedWindow<TxAdvancedSearch> ReturnTxAdvancedSearchsamewin()
        {
            TxAdvancedSearch txadvancedsearch = new TxAdvancedSearch(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"][2]"));
            return new WDValidatedWindow<TxAdvancedSearch>(GetDriver(), txadvancedsearch);
        }

    }
}
