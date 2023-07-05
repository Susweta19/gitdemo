using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.DataTypes;
using System.Linq;
using XmlExecutor.Attributes;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.Writable;
using TxSelenium.NativeTests.Windows;

namespace TxSelenium.GenericTests.TxWorkFlow
{
    public class GTTxWFLinkedObject : WERefreshed
    {
        public GTTxWFLinkedObject(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("LinkedObjects", "")]
        public ActionColl<String> LinkedObjects()
        {
            List<String> objectNames = new List<string>();
            WERefreshed objectDivision = new WERefreshed(GetDriver(),By.XPath(".//div[starts-with(@id,\"wfLkdObjTableview\")]"),this);
            List<IWebElement> linkedObjects = objectDivision.GetElement().FindElements(By.XPath(".//li[@class='wfLkdObjLi']")).ToList();
            foreach (IWebElement elem in linkedObjects)
                objectNames.Add(elem.Text);
            return new ActionColl<string>(objectNames);
        }

        [TxAction("ReadActionList", "")]
        public ActionColl<String> ReadActionList()
        {
            List<String> actionNames = new List<string>();
            WERefreshed actionDivision = new WERefreshed(GetDriver(), By.XPath(".//div[starts-with(@id,\"divWFListActions\")]"), this);
            List<IWebElement> actions = actionDivision.GetElement().FindElements(By.XPath(".//div[starts-with(@class,'actionWorkflowName')]" +
                "|.//div[@class='informationTexteWorkflow']")).ToList();
            foreach (IWebElement elem in actions)
                actionNames.Add(elem.Text);
            return new ActionColl<string>(actionNames);
        }

        [TxAction("ReadSelectedList", "")]
        public ActionColl<String> ReadSelectedList()
        {
            List<String> objNames = new List<string>();
            List<IWebElement> objs = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'wfLkdObjTableview')]"),this)
                .GetElement().FindElements(By.XPath(".//li[contains(@class,'wfLkdObjLiSelected')]")).ToList();
            foreach (IWebElement elem in objs)
                objNames.Add(elem.Text);
            return new ActionColl<string>(objNames);
        }

        [TxAction("SelectObject", "")]
        public void SelectObject(string objectName)
        {
            GetDriver().ClickAt(By.XPath(".//li[starts-with(@class,'wfLkdObjLi') and text()='" + objectName+"']"),this);
            Sleep(1);
        }

        [TxAction("HideIrreliventActions", "")]
        public WEGCheckBox HideIrreliventActions()
        {
            return new WEGCheckBox(GetDriver(),By.XPath(".//input[starts-with(@id,'wfLkdObjInputShowActions')]"),this);
        }

        [TxAction("PerformActions", "")]
        public GTTxWorkFlow PerformActions()
        {
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_material\" and not(contains(@style,\"display: none\"))]//div[@class=\"dhx_popup_area\"]"));
            return WFPromoteCreator;
        }

        [TxAction("LinkedObjectInTableView", "")]
        public GTTxTableView LinkedObjectInTableView()
        {
            return new GTTxTableView(GetDriver(),By.XPath(".//div[starts-with(@id,'wfLkdObjTableview')]"),this);
        }

        [TxAction("ChangeObjectSelected", "")]
        public void ChangeObjectSelected()
        {
            GetDriver().ClickAt(By.Id("idWfooBtnChoose"),this);
            GetDriver().WaitForAjax();
        }

        [TxAction("ObjectSelection", "")]
        public WMultipleLink ObjectSelection(int windowIndex = 1)
        {
            WMultipleLink multiplelink = new WMultipleLink(GetDriver(), By.XPath("(.//div[@class='dhx_cell_wins'])[" + windowIndex + "]")
                , WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
            return multiplelink;
        }
    }

}
