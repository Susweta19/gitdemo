using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxAdvancedSearch
{
    public class ManageQuestionList : WERefreshed
    {
        public ManageQuestionList(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("ManageBooleanList", "")]
        public void ManageBooleanList(string BooleanFieldname,int index =1, bool yes = true)
        {
            if (yes)
                GetDriver().ClickAt(By.XPath(".//label[contains(@id,\"idLabelEmptyGroup\") and text()=\""+BooleanFieldname+"\"]/following-sibling::img"), this);
            //(".//span[text()=\"" + BooleanFieldname + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_acc\")]//input[starts-with(@value ,\"bctTrue\")]"), this);
            //GetDriver().WaitForElement(By.XPath(".//div[@id=\"QuestionHeader"+id+"\"]//input[@id=\"idAdvancedSearchBooleanNo"+id+"\"]"), this).GetElement().Click();
            else
                GetDriver().ClickAt(By.XPath(".//span[text()=\"" + BooleanFieldname + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_acc\")]//input[starts-with(@value ,\"bctFalse\")]"), this);

        }
        [TxAction("ManageTextField", "")]
        public void ManageTextField(string questionName, string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/..//div[starts-with(@id,\"questionTextual\")]//input"), this).Fill(value);
        }
        [TxAction("Search", "")]
        public WEGInput Search()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idTextSearchRequirementsList\")]"), this);
        }
        [TxAction("DateLowerBound", "")]
        public WDate DateLowerBound(string questionName)
        {
            WERefreshed questionDiv = new WERefreshed(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[@class=\"dhx_cell_acc\"]"), this);
            WERefreshed lowerBoundDiv = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"formGroup\"]//div[@class=\"clDivContainerCCalendar\"][1]"), questionDiv);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"calendar.png\")]"), lowerBoundDiv);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));

            //(".//span[text()=\"Please chose one !\"]/ancestor::div[@class=\"dhx_cell_cont_acc\"]")
            //(".//span[text()=\"Please chose one !\"]/ancestor::div[@class=\"dhx_cell_cont_acc\"]//div[starts-with(@id,\"idDivChoice\")]")
        }

        [TxAction("DateUpperbound", "")]
        public WDate DateUpperbound(string questionName)
        {
            WERefreshed questionDiv = new WERefreshed(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[@class=\"dhx_cell_acc\"]"), this);
            WERefreshed UpperBoundDiv = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"formGroup\"]//div[@class=\"clDivContainerCCalendar\"][2]"), questionDiv);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"calendar.png\")]"), UpperBoundDiv);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));
        }

        //[TxAction("ManageUnitQuestion", "")]
        //public void ManageUnitQuestion(string name,string value)
        //{
        //    new WEGInput(GetDriver(), By.XPath(".//span[text()=\""+name+"\"]/ancestor::div[@class=\"dhx_cell_acc\"][1]//input"), this).Fill(value);
        //}

        [TxAction("ValueMustBe", "")]
        public void ValueMustBe(string questionName, string value)
        {
            new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_acc\")]//div[starts-with(@class,\"clDivComboNatureNumericBounds\")]"), this).SelectOption(value);
        }

        [TxAction("LowerBound", ".....")]
        public void LowerBound(string questionName, string min)
        {
            //(".//span[text()=\"Advanced Question\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]//input[@name=\"fLBValue\" and not(contains(@style,\"display:none\"))]")
            WERefreshed decimalDiv = GetDriver().WaitForElement(By.XPath(".//span[text()=\""+questionName+ "\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))] | .//span[text()=\""+questionName+"\"]/ancestor::div[starts-with(@class,\"dhx_cell_acc\")]//div[contains(@class,\"clDivQuestionTableLBCritType\")]"),this);
            new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"NumericalMinValueInRange\") and not(contains(@style,\"display:none\"))] | //input[contains(@id,\"idNumberLBValueSub\") and not(contains(@style,\"display:none\"))]"), decimalDiv).Fill(min);
        }

        [TxAction("LowerTolerance", ".....")]
        public void LowerTolerance(string questionName, string min)
        {
            //(".//span[text()=\"Advanced Question\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]//input[@name=\"fLBValue\" and not(contains(@style,\"display:none\"))]")
            WERefreshed decimalDiv = GetDriver().WaitForElement(By.XPath(".//span[text()=\""+questionName+"\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]"), this);
            new WEGInput(GetDriver(), By.XPath(".//input[@name=\"fLBFzness\" and not(contains(@style,\"display:none\"))]"), decimalDiv).Fill(min);
        }

        [TxAction("UpperBound", ".....")]
        public void UpperBound(string questionName, string min)
        {
            WERefreshed decimalDiv = GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + questionName + "\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@class,\"nctEqual_To\"))] | .//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_acc\")]//div[contains(@class,\"clDivQuestionTableUBCritType\")]"), this);
            new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"NumericalMaxValueInRange\") and not(contains(@style,\"display:none\"))] | //input[contains(@id,\"idNumberUBValueSub\") and not(contains(@style,\"display:none\"))]"), decimalDiv).Fill(min);
        }

        [TxAction("UpperTolerance", ".....")]
        public void UpperTolerance(string questionName, string min)
        {
            //(".//span[text()=\"Advanced Question\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]//input[@name=\"fLBValue\" and not(contains(@style,\"display:none\"))]")
            WERefreshed decimalDiv = GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + questionName + "\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@class,\"nctEqual_To\"))]"), this);
            new WEGInput(GetDriver(), By.XPath(".//input[@name=\"fUBFzness\" and not(contains(@style,\"display:none\"))]"), decimalDiv).Fill(min);
        }

        [TxAction("StrictlyLowerThan", ".....")]
        public void StrictlyLowerThan(string questionName)
        { 
            //(".//span[text()=\"Advanced Question\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]//input[@name=\"fLBValue\" and not(contains(@style,\"display:none\"))]")
            WERefreshed decimalDiv = GetDriver().WaitForElement(By.XPath(".//span[text()=\""+questionName+"\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]"), this);
            GetDriver().ClickAt(By.XPath(".//input[@name=\"sLBCritType\" and not(contains(@style,\"display:none\"))]"), decimalDiv);
        }

        [TxAction("StrictlyUpperThan", ".....")]
        public void StrictlyUpperThan(string questionName)
        {
            //(".//span[text()=\"Advanced Question\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]//input[@name=\"fLBValue\" and not(contains(@style,\"display:none\"))]")
            WERefreshed decimalDiv = GetDriver().WaitForElement(By.XPath("//span[text()=\"" + questionName + "\"]/ancestor::div[contains(@class,\"dhx_cell_acc\")] | .//span[text()=\"" + questionName + "\"]/following::div[starts-with(@id,\"idCriterionNumerical\")]/div[not(contains(@style,\"display: none\"))]"), this);
            GetDriver().WaitForElement(By.XPath(".//input[@name=\"sUBCritType\" and not(contains(@style,\"display:none\"))]"), decimalDiv).GetElement().Click();
        }

        [TxAction("ExpandCollaspedGroup", "")]
        public void ExpandCollaspedGroup(string groupName)
        {
            WaitForAjax();
            Sleep(1);
            GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + groupName + "\"]/../following-sibling::div[@class=\"dhx_cell_hdr_arrow\"]"), this).GetElement().Click();
        }

        //[TxAction("SelectSingleAnswer", "Select the Object Type")]
        //public void SelectSingleAnswer(int otAndQuestionId)
        //{

        //    //GetDriver().ClickAt(By.XPath(".//div[@id=\"idDivChoiceSingleTree" + otAndQuestionId + "\"]"), this);
        //    //GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxcombolist_material\"]//div[@class=\"dhxcombo_option_text\"and text()=\""+value+"\"]")).GetElement().Click();
        //    ////new WEGSelect(GetDriver(), By.XPath(".//div[@id=\"idDivChoiceSingleTree" + id+"\"]"), this).SelectByText(value);
        //}

        [TxAction("SelectMultipleAnswer", "Select the Object Type")]
        public void SelectMultipleAnswer(string questionName, string answer)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/..//div[starts-with(@id,\"idDivChoice\")]"), this);
            GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxcombo_option_text dhxcombo_option_text_chbx\"and text()=\"" + answer + "\"]/..//div[starts-with(@class,\"dhxcombo_checkbox dhxcombo_chbx\")]"));
        }
        [TxAction("IsGroupExpand", "Select the Object Type")]
        public bool IsGroupExpand(string name)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_cell_acc\"]//div[@class=\"dhx_cell_hdr_text\"]//span[text()=\"" + name + "\"]"), this.GetElement());
        }

        [TxAction("DropdownlistForComplexAnswer", ".....")]
        public WEGDHtmlxComboBox DropdownlistForComplexAnswer(string questionName)
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/..//div[starts-with(@id,\"idDivChoice\")]"), this);
        }

        [TxAction("ManageDecimalSingleValue", ".....")]
        public void ManageDecimalSingleValue(string questionName, string min)
        {
            new WEGInput(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/..//div[@class=\"AdvancedSearchNumericalSingle\"]//input"), this).Fill(min);
        }

        

        [TxAction("DeleteselectionForComplexAnswer", ".....")]
        public void DeleteselectionForComplexAnswer(string questionName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/..//img[contains(@src,\"-False.png\")]"), this);
        }

        [TxAction("ManageDecimalIntervalValue", "fills in for interval value")]
        public void ManageDecimalIntervalValue(string questionName, string lowerBound = null, string upperBound = null)
        {
            if (lowerBound != null && upperBound == null)
                new WEGInput(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/parent::div[starts-with(@class,\"dhx_cell_acc\")]//input[contains(@id,\"AdvancedSearchNumericalMinValue\")]"), this).Fill(lowerBound);

            if (upperBound != null && lowerBound == null)
                new WEGInput(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/parent::div[starts-with(@class,\"dhx_cell_acc\")]//input[contains(@id,\"AdvancedSearchNumericalMaxValue\")]"), this).Fill(upperBound);

            if (lowerBound != null && upperBound != null)
            {
                new WEGInput(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/parent::div[starts-with(@class,\"dhx_cell_acc\")]//input[contains(@id,\"AdvancedSearchNumericalMinValue\")]"), this).Fill(lowerBound);
                new WEGInput(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[starts-with(@class,\"dhx_cell_hdr\")]/parent::div[starts-with(@class,\"dhx_cell_acc\")]//input[contains(@id,\"AdvancedSearchNumericalMaxValue\")]"), this).Fill(upperBound);

            }
        }
        [TxAction("ReadComment", "Read Comment With Question Name")]
        public string ReadComment(string questionName)
        {
            WERefreshed questionDiv = new WERefreshed(GetDriver(), By.XPath(".//span[text()=\"" + questionName + "\"]/ancestor::div[@class=\"dhx_cell_acc\"]"), this);
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"CommentContent\")]"), questionDiv).GetElement().Text;
        }
        [TxAction("ReadCommentWithoutQuestionname", "Read Comment With Question Name")]
        public string ReadCommentWithoutQuestionname()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"CommentContent\")]"), this).GetElement().Text;
        }
        [TxAction("IsGroupPresent", "Is Group Present By Name")]
        public bool IsGroupPresent(string groupName)
        {
            WaitForAjax();
            Sleep(1);
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_cell_hdr_text\"]//span[text()=\""+groupName+"\"]"));
        }
        [TxAction("QuestionSearchBox", ".....")]
        public void QuestionSearchBox( string value)
        {
             new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"idTextSearchRequirementsList\")]"), this).Fill(value);
        }

        [TxAction("IsSearchBoxPresent", "****")]
        public bool IsSearchBoxPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[contains(@id,\"idTextSearchRequirementsList\")]"), this.GetElement());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("MinDate", "Select date using calendar")]
        public WDate MinDate(string groupName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\""+groupName+"\"]/ancestor::div[@class=\"dhx_cell_acc\"]//div[contains(@class,\"DivCriterionContainer\")]//input[contains(@id,\"AdvancedSearchNumericalMinValue\")]/ancestor::div[contains(@class,\"clDivContainerCCalendar\")]//div[contains(@class,\"clDivIconCCalendar\")]//img"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("MaxDate", "Select date using calendar")]
        public WDate MaxDate(string groupName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + groupName + "\"]/ancestor::div[@class=\"dhx_cell_acc\"]//div[contains(@class,\"DivCriterionContainer\")]//input[contains(@id,\"AdvancedSearchNumericalMaxValue\")]/ancestor::div[contains(@class,\"clDivContainerCCalendar\")]//div[contains(@class,\"clDivIconCCalendar\")]//img"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null);
        }

        [TxAction("RemoveEndDate", "******")]
        public void RemoveEndDate(string groupName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\""+groupName+"\"]/ancestor::div[@class=\"dhx_cell_acc\"]//div[contains(@class,\"DivCriterionContainer\")]//input[contains(@id,\"AdvancedSearchNumericalMaxValue\")]/ancestor::div[contains(@class,\"clDivContainerCCalendar\")]//div[contains(@class,\"clDivIconRemoveCCalendar\")]//img"), this);
        }

        [TxAction("RemoveStartDate", "******")]
        public void RemoveStartDate(string groupName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"" + groupName + "\"]/ancestor::div[@class=\"dhx_cell_acc\"]//div[contains(@class,\"DivCriterionContainer\")]//input[contains(@id,\"AdvancedSearchNumericalMinValue\")]/ancestor::div[contains(@class,\"clDivContainerCCalendar\")]//div[contains(@class,\"clDivIconRemoveCCalendar\")]//img"), this);
        }

        [TxAction("IsGroupCollasped", "Select the Object Type")]
        public bool IsGroupCollasped(string name)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"dhx_cell_closed\")]//div[@class=\"dhx_cell_hdr_text\"]//span[text()=\""+name+"\"]"), this.GetElement());
        }

        [TxAction("SelectMaterialType", "*******")]
        public void SelectMaterialType(String groupName, string optionName)
        {
            new WEGSelect(GetDriver(), By.XPath(".//span[text()=\""+groupName+"\"]/ancestor::div[contains(@class,\"clItemLabelQuestion\")]/..//div[contains(@id,\"idQuestionAccordion\")]"), this).SelectByText(optionName);
        }

        [TxAction("TargetValue", "")]
        public void TargetValue(String value)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalTargetValue\")]|//input[starts-with(@id,\"idAdvancedSearchNumericalMinValueInRange\")]"), this).FillWithoutClear(value);
        }

        [TxAction("DropdownlistForTableCriteria", ".....")]
        public void DropdownlistForTableCriteria(string criteriaName, string value, string subTable = null)
        {
            if(subTable!=null)
                new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//legend[text()=\"" + subTable + "\"]/ancestor::div[@data-name]//label[text()=\"" + criteriaName + "\"]/..//input[@class=\"dhxcombo_input\"]/.."), this).SelectOption(value);

            else
                new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//label[text()=\""+criteriaName+"\"]/..//input[@class=\"dhxcombo_input\"]/.."),this).SelectV2(value);
        }

        [TxAction("CheckAlternateQuestion", "******")]
        public void CheckAlternateQuestion(string groupName)
        {
            WaitForAjax();
            IWebElement element = GetDriver().WaitForElement(By.XPath(".//label[text()=\"" + groupName + "\"]/ancestor::div[contains(@class,\"clDivEmptyGroup\")]//img[contains(@id,\"ImgCheck\")]")).GetElement();
            Sleep(2);
            element.Click();
        }

        [TxAction("ManageLinkBox", "")]
        public WMultipleLink ManageLinkBox(string groupName)
        {
            WaitForAjax();
            return new WMultipleLink(GetDriver(), By.XPath(".//span[text()='" + groupName + "']/ancestor::div[@class=\"dhx_cell_acc\"]//div[@class=\"dhx_cell_cont_acc\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
    }
}
