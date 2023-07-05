using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using System.Threading;
using XmlExecutor.DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace TxSelenium.GenericTests.TxWorkFlow
{
    public class GTTxWorkFlow : WERefreshed
    {
        public GTTxWorkFlow(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("WriteFormTask", "Open Writeform Task window")]
        public WDValidatedWindow<WForm> WriteFormTask(string actionName)
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"" + actionName + "\"]"), this);

            WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }

        [TxAction("WFAction", "Any action like DataWritenTask, MessageTask, etc except WriteFormTask")]
        public void WFAction(string actionName)
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"" + actionName + "\"]"), this);
            GetDriver().WaitForAjax();
        }
        [TxAction("ReturnForm", "Passes Teexma in write mode.")]
        public WDValidatedWindow<WForm> ReturnForm(int windowIndex)
        {
            Thread.Sleep(2000);
            WForm formCreator = new WForm(GetDriver(), By.XPath("(.//*[@class = \"dhxwin_active\"]//div[@class='dhx_cell_wins'])["+windowIndex+"]"));
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }

        [TxAction("Close", "Any action like DataWritenTask, MessageTask, etc except WriteFormTask")]
        public void Close()
        {
            GetDriver().ClickAt(By.XPath(".//button[starts-with(@id ,\"btnCancelPopup\")]"), this);
        }

        [TxAction("ReadActionList", "")]
        public ActionColl<string> ReadActionList()
        {
            this.WaitForAjax();
            WERefreshed s = GetDriver().WaitForElement(this.ElemIdentifier);
            List<IWebElement> actions = s.GetElement().FindElements(By.XPath(".//div[@class='actionWorkflowName']")).ToList();
            List<string> actionNames = new List<string>();
            foreach (IWebElement elem in actions)
                actionNames.Add(elem.Text);
            return new ActionColl<string>(actionNames);
        }

        [TxAction("ReadAction", "")]
        public string ReadAction(string index = null, bool action = true)
        {
            string element;
            WERefreshed elem;
            if (action)
            {
                if (index != null)
                {
                    elem = GetDriver().WaitForElement(By.XPath(".//*[@id=\"listActionsWorkFlow\"]/li[" + index + "]//*[@class=\"actionWorkflowName\"]"), this);
                    element = elem.GetElement().Text;
                    return element;
                }
                else
                {
                    elem = GetDriver().WaitForElement(By.ClassName("actionWorkflowName"), this);
                    element = elem.GetElement().Text;
                    return element;
                }
            }
            else
            {
                elem = GetDriver().WaitForElement(By.ClassName("informationTexteWorkflow"), this);
                element = elem.GetElement().Text;
                return element;
            }
        }

        [TxAction("IsWorkFlowActionPresent", "Check the action are present or not")]
        public bool IsWorkFlowActionPresent(string actionName)
        {
            Thread.Sleep(2000);
            return GetDriver().IsElementPresent(By.XPath(".//div[text()=\"" + actionName + "\"]"), this.GetElement());
        }

    }
}
