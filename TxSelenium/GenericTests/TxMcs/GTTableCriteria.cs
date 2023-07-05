using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericDevs.TxMCS
{
    public class GTTableCriteria : WERefreshed
    {
        #region elements
        static By flbValue1 = By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//div[@class = \"mcsTableRange\"]//input[@name = \"fLBValue\"]");
        static By fubValue1 = By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//div[@class = \"mcsTableRange\"]//input[@name = \"fUBValue\"]");
        static By flbValue2 = By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//div[@class = \"mcsTableRange\"]//input[@name = \"fLBValue\"]");
        static By fubValue2 = By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//div[@class = \"mcsTableRange\"]//input[@name = \"fUBValue\"]");

        #region Checkerboard_elements
        By tableSubCriterion1 = By.XPath(".//div[@data-name=\"TableSubCriterion1\"]");
        By tableSubCriterion2 = By.XPath(".//div[@data-name=\"TableSubCriterion2\"]");

        By criteriaTypeComboBox = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableTypeCombo\")]/div[@class]");

        By seriesTypeComboBox = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableSeriesType\")]/div[@class]");
        //By seriesTypeComboBox2 = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableSeriesType2Combo\")]/div[@class]");

        By criterionNatureComboBox = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableCriterionNature\")]/div[@class]");
        //By criterionNatureComboBox2 = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableCriterionNature2Combo\")]/div[@class]");

        By forComboBox = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableCritType\")]/div[@class]");
        //By forComboBox2 = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableCritType2Combo\")]/div[@class]");

        By equalTargetValue = By.XPath(".//input[starts-with(@id,\"idNumberLBValueEqual\")]");
        By equalTolerance = By.XPath(".//input[starts-with(@id,\"idNumberLBFuzinessEqual\")]");

        By excludeLowerBound = By.XPath(".//input[starts-with(@id,\"idCheckboxLBCritType\")]");
        By lowerBoundTargetValue = By.XPath(".//input[starts-with(@id,\"idNumberLBValue\")]");
        By lowerBoundTolerance = By.XPath(".//input[starts-with(@id,\"idNumberLBFuziness\")]");

        By excludeUpperBound = By.XPath(".//input[starts-with(@id,\"idCheckboxUBCritType\")]");
        By upperBoundTargetValue = By.XPath(".//input[starts-with(@id,\"idNumberUBValue\")]");
        By upperBoundTolerance = By.XPath(".//input[starts-with(@id,\"idNumberUBFuziness\")]");

        By valueFor = By.XPath(".//input[starts-with(@id,\"idTextSearchedValueSub\")]");
        By columnIndex = By.XPath(".//input[starts-with(@id,\"idNumberColumnIndexSub\")]");
        #endregion

        #region Linear_Interpolation_elements
        By tableInterpolationCriterion = By.XPath(".//div[@data-name=\"TableInterpolationCriterion\"]");

        By equalTargetValue_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberLBValueEqualInter\")]");
        By equalTolerance_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberLBFuzinessEqualInter\")]");
        By equalToValue_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberXValueInter\")]");

        By excludeLowerBound_Interpolation = By.XPath(".//input[starts-with(@id,\"idCheckboxLBCritTypeInter\")]");
        By lowerBoundTargetValue_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberLBValueInter\")]");
        By lowerBoundTolerance_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberLBFuzinessInter\")]");

        By excludeUpperBound_Interpolation = By.XPath(".//input[starts-with(@id,\"idCheckboxUBCritTypeInter\")]");
        By upperBoundTargetValue_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberUBValueInter\")]");
        By upperBoundTolerance_Interpolation = By.XPath(".//input[starts-with(@id,\"idNumberUBFuzinessInter\")]");

        By seriesTypeComboBox1_Interpolation = By.XPath("(.//div[starts-with(@id,\"idDivQuestionTableSeriesType1Combo\")]/div[@class])[1]");
        By valueOfComboBox = By.XPath("(.//div[starts-with(@id,\"idDivQuestionTableSeriesType\")]/div[@class])[2]");

        By criterionNatureComboBox_Interpolation = By.XPath(".//div[starts-with(@id,\"idDivQuestionTableCriterionNatureComboInter\")]/div[@class]");
        #endregion

        #endregion

        public GTTableCriteria(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("CriteriaType", "")]
        public WEGDHtmlxComboBox CriteriaType()
        {
            //Click two times then dropdown list will come
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivQuestionTableTypeCombo\")]"), this);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivQuestionTableTypeCombo\")]"), this);
            return new WEGDHtmlxComboBox(GetDriver(), criteriaTypeComboBox, this);
        }

        [TxAction("ReadComment", "")]
        public string ReadComment()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"entryHelper\"]"), this).GetElement().Text;
        }

        #region Checkerboard_methods
        [TxAction("SubCriterion1", "")]
        public GTTableCriteria SubCriterion1()
        {
            return new GTTableCriteria(GetDriver(), tableSubCriterion1, this);
        }

        [TxAction("SubCriterion2", "")]
        public GTTableCriteria SubCriterion2()
        {
            return new GTTableCriteria(GetDriver(), tableSubCriterion2, this);
        }

        [TxAction("SeriesTypeComboBox", "")]
        public WEGDHtmlxComboBox SeriesTypeComboBox()
        {
            return new WEGDHtmlxComboBox(GetDriver(), seriesTypeComboBox, this);
        }

        [TxAction("CriterionNatureComboBox", "")]
        public WEGDHtmlxComboBox CriterionNatureComboBox()
        {
            return new WEGDHtmlxComboBox(GetDriver(), criterionNatureComboBox, this);
        }

        [TxAction("EqualTargetValue", "")]
        public WEGInput EqualTargetValue()
        {
            return new WEGInput(GetDriver(), equalTargetValue, this);
        }

        [TxAction("EqualTolerance", "")]
        public WEGInput EqualTolerance()
        {
            return new WEGInput(GetDriver(), equalTolerance, this);
        }

        //forComboBox1,2
        [TxAction("ForComboBox", "")]
        public WEGDHtmlxComboBox ForComboBox()
        {
            return new WEGDHtmlxComboBox(GetDriver(), forComboBox, this);
        }

        //excludeLowerBound
        [TxAction("ExcludeLowerBound", "")]
        public WEGCheckBox ExcludeLowerBound()
        {
            return new WEGCheckBox(GetDriver(), excludeLowerBound, this);
        }

        [TxAction("LowerBoundTargetValue", "")]
        public WEGInput LowerBoundTargetValue()
        {
            return new WEGInput(GetDriver(), lowerBoundTargetValue, this);
        }

        [TxAction("LowerBoundTolerance", "")]
        public WEGInput LowerBoundTolerance()
        {
            return new WEGInput(GetDriver(), lowerBoundTolerance, this);
        }

        [TxAction("ExcludeUpperBound", "")]
        public WEGCheckBox ExcludeUpperBound()
        {
            return new WEGCheckBox(GetDriver(), excludeUpperBound, this);
        }

        [TxAction("UpperBoundTargetValue", "")]
        public WEGInput UpperBoundTargetValue()
        {
            return new WEGInput(GetDriver(), upperBoundTargetValue, this);
        }

        [TxAction("UpperBoundTolerance", "")]
        public WEGInput UpperBoundTolerance()
        {
            return new WEGInput(GetDriver(), upperBoundTolerance, this);
        }


        [TxAction("ValueFor", "")]
        public WEGInput ValueFor()
        {
            return new WEGInput(GetDriver(), valueFor, this);
        }

        [TxAction("ColumnIndex", "")]
        public WEGInput ColumnIndex()
        {
            return new WEGInput(GetDriver(), columnIndex, this);
        }

        #endregion

        #region Linear_Interpolation_methods

        [TxAction("Interpolation", "")]
        public GTTableCriteria Interpolation()
        {
            return new GTTableCriteria(GetDriver(), tableInterpolationCriterion, this);
        }

        [TxAction("ValueOf", "")]
        public WEGDHtmlxComboBox ValueOf()
        {
            //Click two times then dropdown list will come
            //GetDriver().ClickAt(By.XPath("(.//div[starts-with(@id,\"idDivQuestionTableSeriesType\")])[2]"), this);
            //GetDriver().ClickAt(By.XPath("(.//div[starts-with(@id,\"idDivQuestionTableSeriesType\")])[2]"), this);

            return new WEGDHtmlxComboBox(GetDriver(), valueOfComboBox, this);
        }

        [TxAction("EqualToleranceInterpolation", "")]
        public WEGInput EqualToleranceInterpolation()//not needed
        {
            return new WEGInput(GetDriver(), equalTolerance_Interpolation, this);
        }

        [TxAction("EqualToValueInterpolation", "")]
        public WEGInput EqualToValueInterpolation()
        {
            return new WEGInput(GetDriver(), equalToValue_Interpolation, this);
        }

        ////equalTargetValue_Interpolation
        //[TxAction("EqualTargetValueInterpolation", "")]
        //public WEGInput EqualTargetValueInterpolation()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGInput(GetDriver(), equalTargetValue_Interpolation, interpolationCriterion);
        //}
        ////equalTolerance_Interpolation
        //[TxAction("EqualToleranceInterpolation", "")]
        //public WEGInput EqualToleranceInterpolation()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGInput(GetDriver(), equalTolerance_Interpolation, interpolationCriterion);
        //}
        ////equalToValue_Interpolation
        //[TxAction("EqualToValueInterpolation", "")]
        //public WEGInput EqualToValueInterpolation()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGInput(GetDriver(), equalToValue_Interpolation, interpolationCriterion);
        //}
        ////excludeUpperBound_Interpolation
        ////upperBoundTargetValue_Interpolation
        //[TxAction("UpperBoundTargetValueInterpolation", "")]
        //public WEGInput UpperBoundTargetValueInterpolation()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGInput(GetDriver(), upperBoundTargetValue_Interpolation, interpolationCriterion);
        //}
        ////upperBoundTolerance_Interpolation
        //[TxAction("UpperBoundToleranceInterpolation", "")]
        //public WEGInput UpperBoundToleranceInterpolation()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGInput(GetDriver(), upperBoundTolerance_Interpolation, interpolationCriterion);
        //}
        ////seriesTypeComboBox1_Interpolation
        //[TxAction("SeriesTypeComboBoxInterpolation1", "")]
        //public WEGDHtmlxComboBox SeriesTypeComboBoxInterpolation1()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGDHtmlxComboBox(GetDriver(), seriesTypeComboBox1_Interpolation, interpolationCriterion);
        //}
        ////seriesTypeComboBox2_Interpolation
        //[TxAction("SeriesTypeComboBoxInterpolation2", "")]
        //public WEGDHtmlxComboBox SeriesTypeComboBoxInterpolation2()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGDHtmlxComboBox(GetDriver(), seriesTypeComboBox2_Interpolation, interpolationCriterion);
        //}
        ////criterionNatureComboBox_Interpolation
        //[TxAction("CriterionNatureComboBoxInterpolation", "")]
        //public WEGDHtmlxComboBox CriterionNatureComboBoxInterpolation()
        //{
        //    WERefreshed interpolationCriterion = GetDriver().WaitForElement(tableInterpolationCriterion, this);
        //    return new WEGDHtmlxComboBox(GetDriver(), criterionNatureComboBox_Interpolation, interpolationCriterion);
        //}


        #endregion


        //[TxAction("LineNumbers", "")]
        //public void LineNumbers(string optionName,string number=null)
        //{
        //    new WEGSelect(GetDriver(),By.XPath(".//select[@name=\"sSubCritType\"]"),this).SelectByText(optionName);            

        //}

        //[TxAction("Column1", "Select the Object Type")]
        //public void Column1(string value)
        //{
        //    new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//select[@name=\"idSeriesType\"]"), this).SelectByText(value);
        //}
        //[TxAction("AValue", "Select the Object Type")]
        //public void AValue(string value)
        //{
        //    //(".//input[(@name = \"sSearchedValue\")]")
        //    new WEGInput(GetDriver(), By.XPath(".//input[(@name = \"sSearchedValue\")]"), this).Fill(value);
        //}

        //[TxAction("Column2", "Select the Object Type")]
        //public void Column2(string value)
        //{
        //    new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//select[@name=\"idSeriesType\"]"), this).SelectByText(value);
        //}

        //[TxAction("ValueEqualTo1", "Select the Object Type")]
        //public void ValueEqualTo1(string Value)
        //{
        //     new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).Fill(Value);
        //}

        //[TxAction("ValueEqualTo2", "Select the Object Type")]
        //public void ValueEqualTo2(string Value)
        //{
        //    new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).Fill(Value);
        //}

        //[TxAction("Value_Within_A_Range1", "Select the Object Type")]
        //public void mcsTableRange1(string minValue, string maxValue)
        //{
        //    new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText("within a range");
        //    new WEGInput(GetDriver(), flbValue1, this).FillWithoutClear(minValue);
        //    new WEGInput(GetDriver(), fubValue1, this).FillWithoutClear(maxValue);
        //}

        //[TxAction("Value_Within_A_Range2", "Select the Object Type")]
        //public void mcsTableRange2(string minValue, string maxValue)
        //{
        //    new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText("within a range");

        //    new WEGInput(GetDriver(), flbValue2, this).FillWithoutClear(minValue);
        //    new WEGInput(GetDriver(), fubValue2, this).FillWithoutClear(maxValue);
        //}


        //[TxAction("Within_A_Range1", "Select the Object Type")]
        //public ActionColl<WEGInput> mcsTableRange1()
        //{           

        //    ICollection<WEGInput> values = new List<WEGInput>(2);
        //    values.Add(new WEGInput(GetDriver(), flbValue1, this));
        //    values.Add(new WEGInput(GetDriver(), fubValue1, this));

        //    return new ActionColl<WEGInput>(values);

        //}



        [TxAction("AlsoFor", "Select the Object Type")]
        public void AlsoFor()
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id , \"mcsTableCriterion\")]"),this);
        }


        [TxAction("AValue2", "Select the Object Type")]
        public void AValue2(string value)
        {
            //(".//input[(@name = \"sSearchedValue\")]")
            new WEGInput(GetDriver(), By.XPath(".//div[@data-name=\"TableSubCriterion2\"]//input[(@name = \"sSearchedValue\")]"), this).Fill(value);
        }
        [TxAction("ValueTypeDropdown1", "")]
        public void ValueTypeDropdown1(string option)
        {
            new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText(option);
        }
        [TxAction("ValueTypeDropdown2", "")]
        public void ValueTypeDropdown2(string option)
        {
            new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText(option);
        }
        [TxAction("ValueMin1", "")]
        public void ValueMin1(string minValue)
        {           
            new WEGInput(GetDriver(), flbValue1, this).FillWithoutClear(minValue);
        }
        [TxAction("ValueMax1", "")]
        public void ValueMax1(string maxValue)
        {
            new WEGInput(GetDriver(), fubValue1, this).FillWithoutClear(maxValue);
        }
        [TxAction("ValueMin2", "")]
        public void ValueMin2(string minValue)
        {
            new WEGInput(GetDriver(), flbValue2, this).FillWithoutClear(minValue);
        }
        [TxAction("ValueMax2", "")]
        public void ValueMax2(string maxValue)
        {
            new WEGInput(GetDriver(), fubValue2, this).FillWithoutClear(maxValue);
        }
        [TxAction("LineNumberValue", "")]
        public void LineNumberValue(string number)
        {          
                new WEGInput(GetDriver(), By.XPath(".//input[@name=\"iValueIndex\"]"), this).FillWithoutClear(number);
        }
    }
}



//[TxAction("value2", "Select the Object Type")]
//public void value2(string minValue,string maxValue = null, string optionName = null)
//{
//    By path = By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//select[@name=\"idSeriesType\"]");

//    if (GetDriver().FindElement(path).Text == "Libellés")
//    {
//        new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).Fill(minValue);
//    }
//    else
//    {
//        new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText(optionName);

//        if (optionName == "égale à")
//            new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion2\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).Fill(minValue);
//        else { 
//        //{
//        //    if (maxValue != null)
//        //        mcsTableRange1(minValue, maxValue);
//        }
//    }
//}

//if (optionName == "Valeurs")
//{
//    new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText(optionName);
//    if()
//    new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).Fill(minValue);
//}
//else
//{
//    if (maxValue != null)
//        mcsTableRange1(minValue, maxValue);
//}

////for values 
//if (optionName != null)
//{
//    new WEGSelect(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//select[@class=\"tableCriterionType\"]"), this).SelectByText(optionName);
//    new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).FillWithoutClear(minValue);
//}
//else
//    //for Search
//    new WEGInput(GetDriver(), By.XPath(".//div[@data-name = \"TableSubCriterion1\"]//div[@class = \"mcsTableEquality\"]//input[@name = \"fLBValue\"]"), this).FillWithoutClear(minValue);