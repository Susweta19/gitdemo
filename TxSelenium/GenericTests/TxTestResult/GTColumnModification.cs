using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.GenericTests.TxTestResult
{
   public  class GTColumnModification : WERefreshed
    {

        public GTColumnModification(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("MetadataValue", " Select a Value")]
        public WEGDHtmlxComboBox MetadataValue()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"inputTAFieldMetadata\")]"), this);
        }

        [TxAction("ReadOnly", " ")]
        public WEGCheckBox ReadOnly()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"inputTAFieldReadOnly\")]"), this);
        }

        [TxAction("AddGroup", " Click on AddGroup button")]
        public void AddGroup()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"AddObject.png\") and starts-with(@id,\"inputTAColGroupImg\")]"), this);
        }

        [TxAction("EditGroup", " Click on EditGroup button")]
        public void EditGroup()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"EditObject.png\") and starts-with(@id,\"inputTAColGroupRenameImg\")]"), this);
        }

        [TxAction("Name", "Write a Name")]
        public WEGInput Name()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAColName\")]"), this);
        }

        [TxAction("Type", " Select a Type")]
        public WEGDHtmlxComboBox Type()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"inputTAColType\")]"), this);
        }

        [TxAction("ChangeTab", " Change the tab using id")]
        public void ChangeTab(String tabId)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxtabbar_tab_text\" and text()=\""+ tabId + "\"]"), this);
        }

        [TxAction("Group", " Select a Group")]
        public WEGDHtmlxComboBox Group()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"inputTAColGroup\")]"), this);
        }

        [TxAction("Width", "Write  Width")]
        public WEGInput Width()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAColWidth\")]"), this);
        }

        [TxAction("Alignment", " Select a Alignment")]
        public WEGDHtmlxComboBox Alignment()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"inputTAColAlign\")]"), this);
        }

        [TxAction("Description", "Write  Description")]
        public WEGInput Description()
        {
            return new WEGInput(GetDriver(), By.XPath(".//textarea[contains(@id,\"inputTAColDescr\")]"), this);
        }

        [TxAction("SettingsForDigital", "Write  Description")]
        public void SettingsForDigital(ICollection<String> Unit)
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"inputTAColUnitImg\")]"), this);
            foreach(String u in Unit)
                GetDriver().ClickAt(By.XPath(".//ul[contains(@id,\"taPopupUnitsList\")]//li[@title=\""+ u + "\"]/input"));
            //if (closePopup)
            //    new WESPopUp(GetDriver()).ClosePopup(true);
           // , bool closePopup = false
           //  GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"taPopupUnitsBtnClose\")]"));

        }
        /// <summary>
        /// This function is not so good, It can only select a column
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="clearPreviousData"></param>
        [TxAction("SelectColumnForCalculatedformulaSettings", "Write  Description")]
        public void SelectColumnForCalculatedformulaSettings(ICollection<String> columnName=null,Boolean clearPreviousData=false)
        {
            if (clearPreviousData)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldFormula\")]"), this).Clear();

            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"inputTAFieldFormulaImg\")]"), this);
            foreach (String c in columnName)
                GetDriver().ClickAt(By.XPath(".//table[@class=\"taPopupTable\"]//tr/td[text()=\""+c+"\"]"));

            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"inputTAFieldFormulaImg\")]"), this);

        }
         /// <summary>
        /// You can write formula for CalculatedformulaSettings using this
        /// </summary>
        /// <returns></returns>
        [TxAction("WriteFormulaForCalculatedformulaSettings", "Write  Description")]
        public WEGInput WriteFormulaForCalculatedformulaSettings()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"inputTAFieldFormula\")]"),this);
        }
        /// <summary>
        /// if delete=false then it will create one by one new item as given choice values
        /// if delete=true then it will delte one by one item as given choice values
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="delete"></param>
        [TxAction("SettingsForChoices", "Write  Choise")]
        public void SettingsForChoices(ICollection<String> choice, Boolean delete = false)
        {
            if (delete)
            {
                foreach (String c in choice)
                    GetDriver().ClickAt(By.XPath(".//ul[@class=\"tagit ui-widget ui-widget-content ui-corner-all\"]//li//span[text()=\""+c+"\"]/../a"), this);
            }
            else
            {
                WEGInput input = new WEGInput(GetDriver(), By.XPath(".//input[@class=\"ui-widget-content ui-autocomplete-input\"]"), this);
                foreach (String c in choice)
                {
                    input.FillWithoutClear(c);
                    input.Enter();
                }
            }

        }

        [TxAction("HeightForSettingsFile", "Select Link")]
        public WEGInput HeightForSettingsFile()
        {
            return new  WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"inputTAFieldFileMaxHeight\")]"), this);
        }

        [TxAction("WidthForSettingsFile", "Select Link")]
        public WEGInput WidthForSettingsFile()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"inputTAFieldFileMaxWidth\")]"), this);
        }

        [TxAction("SettingsForLink", "Select Link")]
        public WEGDHtmlxComboBox SettingsForLink()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"inputTAFieldLink\")]"), this);
        }

        [TxAction("SettingsForLinkData", "Select LinkData")]
        public WSingleLink SettingsForLinkData()
        {
            return new WSingleLink(GetDriver(), By.XPath(".//fieldset[contains(@id,\"inputTAColTypeFS\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
        [TxAction("AddNewGroup", "Select LinkData")]
        public WEGInput AddNewGroup()
        {

            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idTextQueryString\")]"));
        }
        [TxAction("ClickOnAddNewGroupButton", "Select LinkData")]
        public void ClickOnAddNewGroupButton()
        {
            GetDriver().ClickAt(By.XPath(".//img[@class=\"tableAdminImg\"and contains(@src,\"AddObject.png\")]"));
        }
        [TxAction("ReadWindowTitle", "Select LinkData")]
        public string ReadWindowTitle()
        {
          return  GetDriver().FindElement(By.XPath(".//div[@class=\"dhxwin_active\"]//div[@class=\"dhxwin_text_inside\"]")).Text;
        }
        [TxAction("SelectGroup", "Select LinkData")]
        public WEGDHtmlxComboBox SelectGroup()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"inputTAColGroup\")]"));
        }
        [TxAction("IsEditButtonDisabled", "Select LinkData")]
        public bool IsEditButtonDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//img[@class=\"tableAdminImg clTxIconDisabled\"]"));
        }

        [TxAction("RenameTheGroupname", "Select LinkData")]
        public void ClickOnRenameGroup()
        {
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id,\"inputTAColGroupRenameImg\")]"));
        }
    }
}
