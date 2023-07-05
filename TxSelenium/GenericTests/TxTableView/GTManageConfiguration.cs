using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.DataTypes;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTableView
{
    public class GTManageConfiguration : WERefreshed
    {
        public GTManageConfiguration(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ExpandCollapseOT", "ExpandCollapseOT")]
        public void ExpandCollapseOT(int idItem)
        {
            GetDriver().ClickAt(By.XPath(".//div[@iditem='"+ idItem + "']//div[starts-with(@class,'accItemLabel')]"),this);
        }

        [TxAction("ReadOTName", "ReadOTName")]
        public string ReadOTName(int idItem)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@iditem='" + idItem + "']//div[starts-with(@class,'accLabelWithIcon')]"),this).GetElement().Text;
        }

        [TxAction("ReadConfigList", "ReadConfigList")]
        public ActionColl<string> ReadConfigList(int idItem)
        {
            ICollection<IWebElement> elems= GetDriver().FindElement(By.XPath(".//div[@iditem='" + idItem + "']//ul[starts-with(@class,'TxTableListConfigs')]")).FindElements(By.XPath(".//div[starts-with(@class,'TxTableListConfigName')]"));
            ICollection<string> configNames = new List<string>(elems.Count);
            for (int i = 0; i < elems.Count; i++)
            {
                configNames.Add(GetDriver().WaitForElement(By.XPath(".//div[@iditem='"+idItem+ "']//li[contains(@id,'config')]["+(i+1)+"]//div[starts-with(@class,'TxTableListConfigName')]"), this).GetElement().Text);
            }
            return new ActionColl<string>(configNames);
        }

        [TxAction("IsOTExpanded", "IsOTExpanded")]
        public bool IsOTExpanded(int idItem)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,'tvPopupSelectCritere') and starts-with(@class,'clPopupConfig')]"), this.GetElement());
        }

        [TxAction("NumberOfConfig", "NumberOfConfig")]
        public int NumberOfConfig(int idItem)
        {
            return GetDriver().FindElement(this.GetElement(),By.XPath(".//div[@iditem='" + idItem + "']//ul[starts-with(@class,'TxTableListConfigs')]")).FindElements(By.XPath(".//div[starts-with(@class,'TxTableListConfigName')]")).Count;
        }

        [TxAction("SelectConfiguration", "SelectConfiguration")]
        public void SelectConfiguration(string configId)
        {
            GetDriver().ClickAt(By.XPath(".//li[starts-with(@id,'config"+configId+"')]"), this);
        }
        
        [TxAction("RemoveConfiguration", "RemoveConfiguration")]
        public void RemoveConfiguration(string configId)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//li[starts-with(@id,'config" + configId + "')]"), this);
            GetDriver().MouseOver(elem);
            GetDriver().ClickAt(By.XPath(".//span[starts-with(@id,'btnRemoveConfig" + configId + "')]"),this);
        }

        [TxAction("GetConfigId", "GetConfigId")]
        public string GetConfigId(string configName)
        {
            string totalConfigId = GetDriver().WaitForElement(By.XPath(".//div[text()=\"" + configName + "\"]/../../.."), this).GetElement().GetAttribute("id");
            return totalConfigId.Substring(6);
        }
    }
}
