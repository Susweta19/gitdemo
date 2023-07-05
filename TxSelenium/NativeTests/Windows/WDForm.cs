using OpenQA.Selenium;
using System;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.Writable;
using TxSelenium.NativeTests.DataTypes;

namespace TxSelenium.NativeTests.Windows
{
    public class WDForm : WERefreshed
    {
        private static By GetTab(int tabIndex)
        {
            return By.XPath(".//div[@class=\"dhxtabbar_tabs_cont_left\"]/div["+ (tabIndex + 2) + "]");
        }

        public int CurrentTab { get; protected set; }

        public WDForm(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null,bool formInsiddeWindow = false)
            : base(driver, elemIdentifier, parent)
        {
            this.CurrentTab = GetFirstTab(formInsiddeWindow);
        }

        protected  static By GetAttrDiv(int characId, int currentTab, int groupId = 0)
        {
            if (groupId == 0)
                return By.XPath(".//div[starts-with(@slabelhtmlid, \"Label" + characId + "_" + currentTab + "_\")]");
            else
                return By.XPath(".//div[starts-with(@id, \"divGroup"+groupId+"\")]//div[starts-with(@slabelhtmlid, \"Label" + characId + "_" + currentTab + "_\")]");

        }

        /// <summary>
        /// Gets the werefreshed corresponding to the attribute a specified id.
        /// </summary>
        /// <param name="id">the attribute's id</param>
        /// <returns>the element</returns>
        protected virtual WERefreshed GetAttributeElem(int id)
        {
            WERefreshed attributeElem = GetDriver().WaitForElement(GetAttrDiv(id, CurrentTab), this);
            return attributeElem;
        }

        /// <summary>
        /// Changes the current tab for this form.
        /// </summary>
        /// <param name="tabIndex">the tab id</param>
        [TxAction("ChangeTab", "Changes this from current tab.")]
        public void ChangeTab(int tabIndex)
     {
            CurrentTab = tabIndex;
            GetDriver().ClickAt(GetTab(tabIndex), this);
            WaitForAjax();
        }

        /// <summary>
        /// Read the current tab for testing translation
        /// </summary>
        /// <param name="tabId">the tab id</param>
        [TxAction("ReadTab", "Changes this from current tab.")]
        public string ReadTab(int tabId)
        {
            WERefreshed tabName =  GetDriver().WaitForElement(new ByChained(GetTab(tabId), By.TagName("div")), this);
            return tabName.GetElement().Text;
        }

        /// <summary>
        /// get the first tab because we need itq
        /// for locating the attribute id_tab
        /// </summary>
        /// <returns></returns>
        protected virtual int GetFirstTab(bool formInsiddeWindow)
        {
            int indexNo;
            By parent;
            if (this.GetType() == typeof(WForm) || formInsiddeWindow)
                parent = WDWindow<WERefreshed>.windowBy;
            else
                parent = By.Id("idDivFormContainer");
            IWebElement elem = GetDriver().WaitForElement(new ByChained(parent, By.XPath(".//div[@class=\"dhxtabbar_tabs_cont_left\"]"))).GetElement();
            int count = elem.FindElements(By.XPath("./div")).Count;            
            for (indexNo = 1 ; indexNo <= count ; indexNo++)
            {
                //string val = elem.FindElement(By.XPath("./div[" + indexNo + "]")).GetAttribute("class").ToString();
                if (elem.FindElement(By.XPath("./div["+ indexNo + "]")).GetAttribute("class").Equals("dhxtabbar_tab dhxtabbar_tab_actv"))
                {
                    break;
                }
            }
            return indexNo-2;
        }

        [TxAction("ExtendContractGroup","Extend contract group.")]
        public void ExtendContractGroup(string groupName)
        {
            GetDriver().ClickAt(By.XPath(".//legend[text() = \"" + groupName + "\"]"), this);
        }

        [TxAction("ScrollRight", "click on the button to scroll right")]
        public void ScrollRight(int Point = 1)
        {
            GetDriver().WaitForAjax();
            for (int i = 1; i <= Point; i++)
            {
                GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhxtabbar_tabs_ar_right\")]"), this);
            }
        }





        [TxAction("ScrollLeft", "click on the button to scroll right")]
        public void ScrollLeft(int Point = 1)
        {
            for (int i = 1; i <= Point; i++)
            {
                GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxtabbar_tabs_ar_left\"]"), this);
            }
            // Thread.Sleep(1000);
        }

        [TxAction("GetRequiredDate", "To get previous or future date.")]
        public string GetRequiredDate(int dayAfterOrBefore)
        {
            return DateTime.Now.Date.AddDays(dayAfterOrBefore).ToShortDateString().ToString();
        }

        [TxAction("ConvertStringToDTDate", "")]
        public DTDate ConvertStringToDTDate(string date)
        {
            return new DTDate(date);
        }
        [TxAction("IsGroupExtend", " ")]
        public bool IsGroupExtend(string groupName)
        {
            return !GetDriver().IsElementPresent(By.XPath(".//legend[text()=\"" + groupName + "\"]/following-sibling::div[contains(@style,\"display: none\")]"), this.GetElement());
        }
        [TxAction("IsGroupDisplayed", " ")]
        public bool IsGroupDisplayed(string groupId)
        {
            string attr = GetDriver().FindElement(this.GetElement(), By.XPath(".//fieldset[starts-with(@id,\"group" + groupId + "_\")]")).GetAttribute("style");
            return !attr.Contains("display: none");
        }
        [TxAction("IsTabPresent", "")]
        public bool IsTabPresent(string tabName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhxtabbar_tab_text\"and text()=\"" + tabName + "\"]"), this.GetElement());
        }
        [TxAction("IsManadatorAttributeFieldPoperValue", "check value for Webfrom")]
        public bool IsManadatorAttributeFieldPoperValue(int attributeId)
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[contains(@id,\"minValdiv" + attributeId + "\")and contains(@style,\"rgb(51, 182, 121)\")]"));
        }
        [TxAction("IsToolBarPresent", "CHECK TOOLBAR PRESENT OR NOT IN RICHTEXT")]
        public bool IsToolBarPresent(int attrID)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"mce-toolbar-grp\")]"), GetAttributeElem(attrID).GetElement());
        }
        [TxAction("IsFieldColourGreen", "check colour for Webfrom")]
        public bool IsFieldColourGreen(int attributeId)
        {
            string colour = GetDriver().FindElement(By.XPath(".//input[starts-with(@id,\"minValdiv" + attributeId + "\")]")).GetAttribute("style");
            if (colour.Contains("51, 182, 121"))
            {
                return true;
            }
            else
                return false;
        }
    }
}
