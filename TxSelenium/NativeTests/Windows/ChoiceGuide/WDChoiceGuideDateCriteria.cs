using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows.ChoiceGuide
{
    public class WDChoiceGuideDateCriteria : WERefreshed
    {
        public WDChoiceGuideDateCriteria(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("GreaterThan", "")]
        public void GreaterThan()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@value,'Greater than')]"),this);
        }

        [TxAction("LessThan", "")]
        public void LessThan()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@value,'Less than')]"), this);
        }

        [TxAction("EqualTo", "")]
        public void EqualTo()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@value,'Equal to')]"), this);
        }

        [TxAction("Range", "")]
        public void Range()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@value,'Range')]"), this);
        }

        [TxAction("UpperDate", "")]
        public WDate UpperDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"inf_a_\")]//img[contains(@src,'calendar.png')]"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("LowerDate", "")]
        public WDate LowerDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,'sup_a')]//img[contains(@src,'calendar.png')]"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("EqualToDate", "")]
        public WDate EqualToDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,'egal_a')]//img[contains(@src,'calendar.png')]"), this);
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"), null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("StrictlyGreaterThan", "")]
        public void StrictlyGreaterThan()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@id,'inp_strict_inf_')]"),this);
        }

        [TxAction("StrictlyLessThan", "")]
        public void StrictlyLessThan()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@id,'inp_strict_sup_')]"), this);
        }

        [TxAction("UpperTolerance", "")]
        public void UpperTolerance()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,'inf_')]//input[contains(@value,'Tolerance')]"),this);
        }

        [TxAction("LowerTolerance", "")]
        public void LowerTolerance()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,'sup_')]//input[contains(@value,'Tolerance')]"), this);
        }

        [TxAction("EqualTolerance", "")]
        public void EqualTolerance()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,'egal_')]//input[contains(@value,'Tolerance')]"), this);
        }

        [TxAction("UpperToleranceValue", "")]
        public WEGInput UpperToleranceValue()
        {
            return new WEGInput(GetDriver(),By.XPath(".//input[contains(@id,\"inp_tol_sup\")]"), this);
        }

        [TxAction("LowerToleranceValue", "")]
        public WEGInput LowerToleranceValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,'inp_tol_inf')]"), this);
            //(".//div[contains(@id,'inf_')]//input[contains(@id,'inp_tol_sup')]"), this);inp_tol_egal_47
        }

        [TxAction("EqualToleranceValue", "")]
        public WEGInput EqualToleranceValue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,'inp_tol_egal')]"), this);
        }
        [TxAction("LowerValue", "")]
        public void LowerValue(string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"inp_val_inf\")]"), this).Fill(value);
        }
        [TxAction("UpperValue", "")]
        public void UpperValue(string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"inp_val_sup\")]"), this).Fill(value);
        }
        [TxAction("EqualToValue", "")]
        public void EqualToValue(string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"inp_val_egal\")]"), this).Fill(value);
        }

    }
}
