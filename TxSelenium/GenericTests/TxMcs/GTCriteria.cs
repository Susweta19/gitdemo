using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using OpenQA.Selenium.Support.PageObjects;

namespace TxSelenium.GenericDevs.TxMCS
{
    /// <summary>
    /// This class is used to set all kind of criteria we want for TxMCS.
    /// </summary>
    public class GTCriteria : WERefreshed
    {
        public GTCriteria(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

       /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("BooleanCriterion", "Definition of a Criterion by the Attribute of Type Booléen")]
        public void BoolCriterion(bool value)
        {
            if (value)
                GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idMcsBooleanYesCriterion\")]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idMcsBooleanNoCriterion\")]"), this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("TheValueMustBe", "Data Setting")]
        public void TheValueMustBe(String value)
        {
            new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"comboBoxQuestionNumerical\")]"), this).SelectOption(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("TextMustContain", "The text must contain all of the following words.")]
        public WEGInput TextContain()
        {
           return new WEGInput(GetDriver(), new ByChained(By.XPath(".//div[starts-with(@id,\"idMCSCriteriaHeader\")]"), By.TagName("input")), this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("TargetDate", "Select date using calendar")]
        public WDate TargetDate()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src ,\"calendar.png\" )]"));
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("MinDate", "Select date using calendar")]
        public WDate MinDate()
        {
            GetDriver().ClickAt(By.XPath(".//label[text()=\"Lower bound:\"]/following::div[@class=\"clDivContainerCCalendar\"]//img[contains(@src,\"calendar.png\")]"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("MaxDate", "Select date using calendar")]
        public WDate MaxDate()
        {
            GetDriver().ClickAt(By.XPath(".//label[text()=\"Upper bound:\"]/following::div[@class=\"clDivContainerCCalendar\"]//img[contains(@src,\"calendar.png\")]"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("TargetValue", "Data Setting")]
        public void TargetValue(String value)
        {
             new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalTargetValue\")]|.//input[starts-with(@id,\"idAdvancedSearchNumericalMinValueInRange\")]"),this).FillWithoutClear(value);
        }

        [TxAction("Tolerancevalue", "Data Setting")]
        public void Tolerancevalue(string value)
        {
            //(".//input[starts-with(@id,\"idAdvancedSearchNumericalMinTolerance\")]")
            new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalEqualTolerance\")]|.//input[starts-with(@id,\"idAdvancedSearchNumericalMinTolerance\")]"), this).FillWithoutClear(value);
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("MinValue", "Data Setting")]
        public void MinValue(String value)
        {
             new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalMinValueInRange\")]"), this).FillWithoutClear(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("MaxValue", "Data Setting")]
        public void MaxValue(String value)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalMaxValueInRange\")]"), this).FillWithoutClear(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        [TxAction("IsMultipleClendarIconForMinValue", "Check if MultipleCalendar present")]
        public bool IsMultipleClendarIconForMinValue()
        {
            IWebElement elem = new WERefreshed(GetDriver(), By.XPath(".//label[starts-with(@for,\"idMCSNumericalMinValue\")]/.."), this).GetElement();
            return !(elem.FindElements(By.XPath(".//img[contains(@src,\"calendar.png\")]")).Count==1);

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("LinkCritType", "Data Setting")]
        public void linkCritType(String optionName)
        {
            new WEGSelect(GetDriver(), By.XPath(".//select[@name = \"sSubCritType\"]|.//select[@name = \"sLinkCritType\"]"), this).SelectByText(optionName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("PreselectionType", "Preselect all objects(value is false) or some of objects (value is true)")]
        public WMultipleLink preselectionType(String optionName)
        {
            //GetDriver().ClickAt(By.XPath(".//div[@class = \"formGroup\"]//select[@name = \"sPreselectionType\"]"), this);
            new WEGSelect(GetDriver(), By.XPath(".//div[@class = \"formGroup\"]//select[@name = \"sPreselectionType\"]"), this).SelectByText(optionName);
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with( @id,\"idMCSCriteriaHeader\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("Table", "Data Setting")]
        public GTTableCriteria Table()
        {
            return new GTTableCriteria(GetDriver(), By.XPath(".//div[starts-with(@id,\"idMCSCriteriaHeader\")]"), this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name=""></param>
        [TxAction("NumericalEqualitySlider", "Data Setting")]
        public void Slider(double left, double right = 0, bool withInRange = false)
        {
            if (withInRange)
            {
                IWebElement button1 = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id , \"idMCSNumericalRangeSlider\")]//span[contains(@class,\"ui-slider-handle\")][1]")).GetElement();
                IWebElement button2 = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id , \"idMCSNumericalRangeSlider\")]//span[contains(@class,\"ui-slider-handle\")][2]")).GetElement();
                int Positionleft = Convert.ToInt32(6.03 * left);
                int Positionright = Convert.ToInt32(6.03 * (100-right));
                // Moving Cursor to extreme left and right positions
                GetDriver().ClickAndHold(button1);
                GetDriver().MouseMove(-305, 0);
                GetDriver().Release();

                GetDriver().ClickAndHold(button2);
                GetDriver().MouseMove(305, 0);
                GetDriver().Release();
                // Moving Cursor to exact positions....
                GetDriver().ClickAndHold(button1);
                GetDriver().MouseMove(Positionleft, 0);
                GetDriver().Release();
                GetDriver().ClickAndHold(button2);
                GetDriver().MouseMove(-Positionright, 0);
                GetDriver().Release();
            }
            else
            {
                IWebElement button = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id , \"idMCSNumericalEqualitySlider\")]//span[contains(@class,\"ui-slider-handle\")]")).GetElement();
                int Position = Convert.ToInt32(6.03 * left);
                // Moving Cursor to extreme left positions
                GetDriver().ClickAndHold(button);
                GetDriver().MouseMove(-305,0);
                GetDriver().Release();
                // Moving Cursor to exact position....
                GetDriver().ClickAndHold(button);
                GetDriver().MouseMove(Position,0);
                GetDriver().Release();
            }
        }
        [TxAction("IsAttributePresent", "")]
        public bool IsAttributePresent(string attributeName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//span[text()=\"" + attributeName + "\"]"), this.GetElement());
        }

        [TxAction("EqualTargetValueType", "")]
        public WEGDHtmlxComboBox EqualTargetValueType()
        {
            return new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[@class=\"nctEqual_To\"]//div[starts-with(@id,\"idDivComboDateCritType\")]|.//div[starts-with(@id,\"idDivComboDateCritTypeLB\")]"),this); ;
        }
        [TxAction("EqualMoreOrLesstValueType", "")]
        public WEGDHtmlxComboBox EqualMoreOrLesstValueType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[@class=\"nctEqual_To\"]//div[starts-with(@id,\"idDivComboLB\")]|.//div[starts-with(@id,\"idDivComboLBNcurrentDateLB\")]"), this); ;
        }
        [TxAction("EqualMoreOrLessValue", "")]
        public WEGInput EqualMoreOrLessValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[@class=\"nctEqual_To\"]//input[starts-with(@id,\"idAdvancedSearchNumericalNbDays\")]|.//input[starts-with(@id,\"idAdvancedSearchNumericalNbDaysLB\")]"), this); ;
        }
        [TxAction("EqualToleranceValue", "")]
        public WEGInput EqualToleranceValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalEqualTolerance\")]|.//input[starts-with(@id,\"idAdvancedSearchNumericalMinTolerance\")]"), this); ;
        }
        [TxAction("MinTargetValueType", "")]
        public WEGDHtmlxComboBox MinTargetValueType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboDateCritTypeLB\")]"), this); ;
        }
        [TxAction("MinMoreOrLesstValueType", "")]
        public WEGDHtmlxComboBox MinMoreOrLesstValueType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboLBNcurrentDateLB\")]"), this); ;
        }
        [TxAction("MinMoreOrLessValue", "")]
        public WEGInput MinMoreOrLessValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalNbDaysLB\")]"), this); ;
        }
        [TxAction("MinToleranceValue", "")]
        public WEGInput MinToleranceValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalMinTolerance\")]"), this); ;
        }
        [TxAction("MaxTargetValueType", "")]
        public WEGDHtmlxComboBox MaxTargetValueType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboDateCritTypeUB\")]"), this); ;
        }
        [TxAction("MaxMoreOrLesstValueType", "")]
        public WEGDHtmlxComboBox MaxMoreOrLesstValueType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboLBNcurrentDateUB\")]"), this); ;
        }
        [TxAction("MaxMoreOrLessValue", "")]
        public WEGInput MaxMoreOrLessValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalNbDaysUB\")]"), this); ;
        }
        [TxAction("MaxToleranceValue", "")]
        public WEGInput MaxToleranceValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"idAdvancedSearchNumericalMaxTolerance\")]"), this); ;
        }
     
    }
}
