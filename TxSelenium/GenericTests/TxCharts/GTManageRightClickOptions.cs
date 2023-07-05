using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTManageRightClickOptions : WERefreshed
    {
        public GTManageRightClickOptions(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("AddLine", "AddLine")]
        public void AddLine()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,'sub_item_text') and contains(text(),'Add line')]"),this);
        }

        [TxAction("InsertLine", "InsertLine")]
        public void InsertLine()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,'sub_item_text') and contains(text(),'Insert line')]"), this);
        }

        [TxAction("DuplicateLine", "DuplicateLine")]
        public void DuplicateLine()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,'sub_item_text') and contains(text(),'Duplicate line')]"), this);
        }

        [TxAction("ClearLine", "ClearLine")]
        public void ClearLine()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,'sub_item_text') and contains(text(),'Clear line')]"), this);
        }

        [TxAction("RemoveLine", "RemoveLine")]
        public void RemoveLine()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,'sub_item_text') and contains(text(),'Remove line')]"), this);
        }
        [TxAction("ReadAllFunctions", "Read all the function name upon right click")]
        public ActionColl<string> ReadAllFunctions()
        {
            WERefreshed parrent = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"dhtmlxMenu_material_SubLevelArea_Polygon \")and not(contains(@style,'display: none;'))]"));
            List<IWebElement> functions = parrent.GetElement().FindElements(By.XPath(".//div[starts-with(@class,'sub_item_text')]")).ToList();
            List<string> functionName = new List<string>();
            for (int i = 0; i < functions.Count; i++)
                functionName.Add(functions.ElementAt(i).Text);
            return new ActionColl<string>(functionName);
        }

    }
}
