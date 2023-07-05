using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxHourTracking
{
    public class GTTaskContextMenu : WERefreshed
    {
        public GTTaskContextMenu(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("AddTask", "")]
        public WDValidatedWindow<GTAddTask> AddTask()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()='Add Task']"), this);
            GetDriver().WaitForAjax();
            GTAddTask addtask = new GTAddTask(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"), this);
            return new WDValidatedWindow<GTAddTask>(GetDriver(), addtask);
        }

        [TxAction("AddComment", "")]
        public WEGInput AddComment()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()='Add comment']"), this);
            return new WEGInput(GetDriver(), By.XPath(".//textarea[@id='idTextHourTrackingComment']"));
        }

        [TxAction("CompleteTask", "Clicks on complete task")]
        public void CompleteTask()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Complete Task\"]"), this);
        }

        [TxAction("RemoveTask", "Clicks on complete task")]
        public void RemoveTask()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Remove Task\"]"), this);
        }

        [TxAction("DuplicateTIme", "Clicks on Duplicate Time")]
        public void DuplicateTIme()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Duplicate time\"]"), this);

        }

        [TxAction("DeleteTime", "Clicks on Duplicate Time")]
        public void DeleteTime()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Delete time\"]"), this);
        }

        [TxAction("EditComment", "")]
        public WEGInput EditComment()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()='Edit comment']"), this);
            return new WEGInput(GetDriver(), By.XPath(".//textarea[@id='idTextHourTrackingComment']"));
        }

        [TxAction("EditCommentButton", "")]
        public void EditCommentButton()
        {
            GetDriver().ClickAt(By.XPath(".//input[@value='Edit the comment']"),this);
        }

        [TxAction("AddCommentButton", "")]
        public void AddCommentButton()
        {
            GetDriver().ClickAt(By.XPath(".//input[@value='Add a comment']"),this);
        }

        [TxAction("RemoveComment", "")]
        public void RemoveComment()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()='Remove comment']"), this);
        }

        [TxAction("ViewDetails", "Clicks on View Details upon right click")]
        public void ViewDetails()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"View details\"]"), this);
        }
        [TxAction("IsFunctionPresent", "Clicks on View Details upon right click")]
        public bool IsFunctionPresent(string functionName)
        {
           return GetDriver().IsElementPresent(By.XPath(".//div[text()=\""+functionName+"\"]"));
        }


        [TxAction("ReadAllFunctions", "Read all the function name upon right click")]
        public ActionColl<string> ReadAllFunctions()
        {
            List<IWebElement> functions = this.GetElement().FindElements(By.XPath(".//tr[starts-with(@class,\"sub_item\") and not(contains(@style,\"display: none;\"))]//td[starts-with(@class,\"sub_item_text\")]")).ToList();
            List<string> functionName = new List<string>();
            for (int i = 0; i < functions.Count; i++)
                functionName.Add(functions.ElementAt(i).Text);
            return new ActionColl<string>(functionName);
        }

        [TxAction("ValidateResourcesTime", "Clicks on Validate Resources Time")]
        public void ValidateResourcesTime()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Validate resource times\"]"), this);
        }

        [TxAction("CancelResourceTimeValidation", "Clicks on Validate Resources Time")]
        public void CancelResourceTimeValidation()
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Cancel resource times validation\"]"), this);
        }

        [TxAction("IsCancelResourceTimeValidationDisabled", "Checks weather the Cancel Resource Time Validation is disabled or not")]
        public bool IsCancelResourceTimeValidationDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[text()=\"Cancel resource times validation\"]/ancestor::tr[starts-with(@class,\"sub_item_dis\")]"),this.GetElement());
        }

        [TxAction("IsValidateResourcesTimeDisabled", "Checks weather the Validate Resources Time is disabled or not")]
        public bool IsValidateResourcesTimeDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[text()=\"Validate resource times\"]/ancestor::tr[starts-with(@class,\"sub_item_dis\")]"),this.GetElement());
        }

        [TxAction("IsFunctionDisabled", "Checks weather right click function is disabled or not")]
        public bool IsFunctionDisabled(string functionName)
        {
            Sleep(1);
            return GetDriver().IsElementPresent(By.XPath(".//tr[@class=\"sub_item_dis\"]//div[text()=\"" + functionName + "\"]"), this.GetElement());
        }
    }
}
