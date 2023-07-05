using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.DataTypes;
using System.Linq;
using System;

namespace TxSelenium.GenericTests
{
    public class GTFilter : WERefreshed
    {
        private bool _inWidget;
        public GTFilter(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, bool inWidget = false)
            : base(driver, elemIdentifier, parent)
        {
            _inWidget = inWidget;
        }

        [TxAction("DeleteFilter", "*****")]
        public void DeleteFilter()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"tvPopupFilterBtnDelete\")]"), this);
            Sleep(1);
            WaitForAjax();
        }

        [TxAction("ValidateOrCancel", "*****")]
        public void ValidateOrCancel(bool ok = true)
        {
            if (ok)
            {

                try
                {

                    GetDriver().WaitForElement(By.XPath(".//input[starts-with(@id,\"tvPopupFilterBtnValid\")]"), this).GetElement().Click();
                }

                catch (Exception e)
                {
                    WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[starts-with(@style,\"visibility: visible;\")]//div[@class=\"dhx_popup_area\"]/.."));
                    //  GetDriver().FindElement(By.XPath(".//input[starts-with(@id,\"tvPopupFilterBtnValid\")]"),).Click();
                    GetDriver().WaitForElement(By.XPath(".//input[starts-with(@id,\"tvPopupFilterBtnValid\")]"), ele).GetElement().Click();
                }
                }


            else
                GetDriver().ClickAt(By.XPath(".//input[@value=\"Cancel\"]|.//input[starts-with(@id,\"tvPopupFilterBtnCancel\")]"), this);

            Sleep(1);
            WaitForAjax();
        }

        [TxAction("FilterType", "Select a filtertype")]
        public WEGDHtmlxComboBox FilterType()
        {
            if (_inWidget)
                return new WEGDHtmlxComboBox(GetDriver(), By.ClassName("tvPopupFilterLine"), this, this.Parent.FrameBy);
            else
                return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"tvPopupComboFilterTypes\")]"), this);
        }

        [TxAction("LowerBound", "Select Start Date")]
        public WDate LowerBound()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@id , \"id_bound_inf_value\")]"), this);
            return new WDate(GetDriver(), By.Id("ui-datepicker-div"));
        }

        [TxAction("UpperBound", "Select end Date")]
        public WDate UpperBound()
        {
            GetDriver().ClickAt(By.XPath("//div[@id = \"id_div_bounds_values\"]//div[@class= \"clDivLine\"][2]//img[@title = \"Calendar\"]"), this);
            return new WDate(GetDriver(), By.Id("ui-datepicker-div"));
        }

        [TxAction("ListOfChoices", "Select end Date")]
        public WEGDHtmlxComboBox ListOfCoices()
        {
            if (_inWidget)
                return new WEGDHtmlxComboBox(GetDriver(), By.Id("id_div_combo_values"), this, this.Parent.FrameBy);
            else
                return new WEGDHtmlxComboBox(GetDriver(), By.Id("id_div_combo_values"), this);
        }

        [TxAction("FilterValue", "Put the value of filter")]
        public void FilterValue(string value)
        {
            try
            {
                WEGInput input = new WEGInput(GetDriver(), By.ClassName("clTvPopupInputFilter"), this);
                input.Fill(value);
            }
            catch
            {
                WEGInput input = new WEGInput(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_material\" and contains (@style,\"visibility: visible;\")]//*[@class=\"clTvPopupInputFilter\" and contains(@style,\"display:\")]"));
                input.Fill(value);
            }
        }

        [TxAction("FilterValueForDate", "Input Date or Date and time in the Filter Value field.")]
        public WDate FilterValueForDate()
        {
            GetDriver().ClickAt(By.XPath(".//img[@title=\"Calendar\"]|.//div[starts-with(@id,\"tvPopupFilterValue\")]//div[@class=\"clDivIconCCalendar\" ]"));
            return new WDate(GetDriver(), By.XPath((".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\" ]|.//*[@id=\"ui-datepicker-div\"]")));
        }

        [TxAction("FilterDropdownValue", "Put the value of filter")]
        public WEGDHtmlxComboBox FilterDropdownValue()
        {
            if (_inWidget)
                return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"tvPopupFilterValue\")]//div[contains(@id,\"tvpopupFilterComboBoolean\")]"), this, this.Parent.FrameBy);
            else
                return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"tvPopupFilterValue\")]//div[contains(@id,\"tvpopupFilterComboBoolean\")]"), this);
        }

        [TxAction("LowerBoundDate", "Select Start Date")]
        public WDate LowerBoundDate()
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id , \"tvPopupFilterUpperValue\")]/../..//img[contains(@src,\"calendar.png\")]"), this); 
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));
        }

        [TxAction("UpperBoundDate", "Select end Date")]
        public WDate UpperBoundDate()
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id , \"tvPopupFilterLowerValue\")]/../..//img[contains(@src,\"calendar.png\")]"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));
        }
        [TxAction("IsDeleteFilterDisabled", "To Check DeleteFilterButton is disabled or not")]
        public bool IsDeleteFilterDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[starts-with(@id,\"tvPopupFilterBtnDelete\")and @disabled=\"disabled\"]"), this.GetElement());
        }

        [TxAction("FilterValueForBooleanListOfChoices", "Select end Date")]
        public WEGDHtmlxComboBox FilterValueForBooleanListOfChoices()
        {
            WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"tvPopupFilterComboBoolean\")]//div[contains(@class,\"dhxcombo_select_button\")]"),this);
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@class,\"dhxcombolist_material\")]"));
        }
        [TxAction("Listingvalue", "Select List for ListOfChoice")]
        public void Listingvalue(ActionColl<string> collection)
        {
            WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"bigCombo\")]//img[contains(@src,\"dhxcombo_arrow_down\")]"), this);
            WaitForAjax();
            for (int i = 1; i < collection.Count(); i++)
            {
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"bigComboOptions\")]//span[text()=\"" + collection.ElementAt(i) + "\"]"));
            }            
            WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"bigComboBackgroundPanel\")]"));
        }

        [TxAction("FilterValueForListOfChoices", "")]
        public WEGDHtmlxComboBox FilterValueForListOfChoices()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvPopupFilterValue\")]//div[contains(@id,\"bigCombo\")]"),this);
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,\"bigComboPanel\")]"));
        }

        [TxAction("LowerBoundValue", "Sets the lower bound value")]
        public void LowerBoundValue(string value)
        {
            WEGInput input = new WEGInput(GetDriver(), By.XPath(".//input[contains(@id , \"tvPopupFilterLowerValue\")]"), this);
            input.Fill(value);
        }

        [TxAction("UpperBoundValue", "Sets the upper bound value")]
        public void UpperBoundValue(string value)
        {
            WEGInput input = new WEGInput(GetDriver(), By.XPath(".//input[contains(@id , \"tvPopupFilterUpperValue\")]"), this);
            input.Fill(value);
        }
    }
}