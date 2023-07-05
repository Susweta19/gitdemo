using OpenQA.Selenium;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    /// <summary>
    /// This class manages the four function for decimal criterion in define criteria window
    /// </summary>
    public class WDCriterionFunction : WERefreshed
    {

        public WDCriterionFunction(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }


        [TxAction("Date", "Manage the calendar for less than, greater than, equal than , range (frst calendar).")]
        public WDate Date()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class =\"valeur_critere_num\" and contains(@style, \"display: block;\")]//img[contains(@src,\"calendar.png\")]"), this);
            return new WDate(GetDriver(), By.TagName("body"), null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("TodayOrNowSingleValue", "")]
        public void TodayOrNowSingleValue()
        {
            By removeButton = By.XPath(".//div[not(contains(@style,\"display: none;\")) and starts-with(@class,\"clDivIconRemoveCCalendar\")]/img[contains(@src,\"16x16-False.png\")]");
            if (GetDriver().IsElementPresent(removeButton, this.GetElement()))
                GetDriver().ClickAt(removeButton, this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@style,\"display: block;\")]//input[starts-with(@codage,\"date\")]"), this);
        }

        [TxAction("TodayOrNowUpperValue", "")]
        public void TodayOrNowUpperValue()
        {
            By removeButton = By.XPath(".//div[contains(@id,\"sup_a\")]//img[contains(@src,\"16x16-False.png\")]");
            if (GetDriver().IsElementPresent(removeButton, this.GetElement()))
                GetDriver().ClickAt(removeButton, this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"sup_a\")]//input[starts-with(@codage,\"date\")]"), this);
        }

        [TxAction("TodayOrNowLowerValue", "")]
        public void TodayOrNowLowerValue()
        {
            By removeButton = By.XPath(".//div[contains(@id,\"inf_a\")]//img[contains(@src,\"16x16-False.png\")]");
            if (GetDriver().IsElementPresent(removeButton, this.GetElement()))
                GetDriver().ClickAt(removeButton, this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"inf_a\")]//input[starts-with(@codage,\"date\")]"), this);
        }

        [TxAction("DateBis", "Used only for managing the second calendar when we set a range for the atribute.")]
        public WDate DateInf()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class =\"valeur_critere_num\" and not(contains(@style, \"display: none;\"))][2]//img[contains(@src, \"17x17-Calendar.png\")]"), this);
            return new WDate(GetDriver(), By.Id("ui-datepicker-div"), null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("GreaterThan", "Fill the field for the greater criterion")]
        public void GreaterThan()
        {
            GetDriver().ClickAt(By.Id("inp_sup"), this);
        }

        [TxAction("LessThan", "Fill the field for the greater criterion")]
        public void LessThan()
        {
            GetDriver().ClickAt(By.Id("inp_inf"), this);
        }

        [TxAction("EqualTo", "Fill the field for the greater criterion")]
        public void EqualTo()
        {
            GetDriver().ClickAt(By.Id("inp_egal"), this);
        }

        [TxAction("Range", "Fill the field for the greater criterion")]
        public void Range()
        {
            GetDriver().ClickAt(By.Id("inp_intervalle"), this);
        }

        //Input Vallue inf sup equal
        [TxAction("InpValInf", "Fill the field for the greater criterion")]
        public WEGInput InpValInf()
        {
            return new WEGInput(GetDriver(), By.Id("inp_val_inf"), this);
        }

        [TxAction("InpValSup", "Fill the field for the greater criterion")]
        public WEGInput InpValSup()
        {
            return new WEGInput(GetDriver(), By.Id("inp_val_sup"), this);
        }

        [TxAction("InpValEgal", "Fill the field for the greater criterion")]
        public WEGInput InpValEgal()
        {
            return new WEGInput(GetDriver(), By.Id("inp_val_egal"), this);
        }

        //Unit
        [TxAction("UnitInf", "Fill the field for the greater criterion")]
        public WEGSelect UnitInf()
        {
            return new WEGSelect(GetDriver(), By.Id("select_unit_val_inf"), this);
        }


        [TxAction("UnitSup", "Fill the field for the greater criterion")]
        public WEGSelect UnitSup()
        {
            return new WEGSelect(GetDriver(), By.Id("select_unit_val_sup"), this);
        }

        [TxAction("UnitEgal", "Fill the field for the greater criterion")]
        public WEGSelect UnitEgal()
        {
            return new WEGSelect(GetDriver(), By.Id("select_unit_val_egal"), this);
        }

        //Checkbox Strictly greater less than
        [TxAction("StrictInf", "Fill the field for the greater criterion")]
        public WEGCheckBox StrictInf()
        {
            return new WEGCheckBox(GetDriver(), By.Id("inp_strict_inf"), this);
        }

        [TxAction("StrictSup", "Fill the field for the greater criterion")]
        public WEGCheckBox StrictSup()
        {
            return new WEGCheckBox(GetDriver(), By.Id("inp_strict_sup"), this);
        }

        [TxAction("ClickToTolInf", "Fill the field for the greater criterion")]
        public void ClickToInf()
        {
            GetDriver().ClickAt(By.XPath(".//div[@id = \"sup_a\"]//input[@value = \"Tolerance\"]"), this);
        }

        [TxAction("ClickToTolSup", "Fill the field for the greater criterion")]
        public void ClickToSup()
        {
            GetDriver().ClickAt(By.XPath(".//div[@id = \"inf_a\"]//input[@value = \"Tolerance\"]"), this);
        }

        [TxAction("ClickToTolEqual", "Fill the field for the greater criterion")]
        public void ClickToTolEqual()
        {
            GetDriver().ClickAt(By.XPath(".//div[@id =\"egal_a\"]//input[@value = \"Tolerance\"]"), this);
        }

        [TxAction("ToleranceInf", "Fill the field for the greater criterion")]
        public WEGInput ToleranceInf()
        {
            return new WEGInput(GetDriver(), By.Id("inp_tol_inf"), this);
        }

        [TxAction("ToleranceSup", "Fill the field for the greater criterion")]
        public WEGInput ToleranceSup()
        {
            return new WEGInput(GetDriver(), By.Id("inp_tol_sup"), this);
        }

        [TxAction("ToleranceEqual", "Fill the field for the greater criterion")]
        public WEGInput ToleranceEqual()
        {
            return new WEGInput(GetDriver(), By.Id("inp_tol_egal"), this);
        }
    }
}
