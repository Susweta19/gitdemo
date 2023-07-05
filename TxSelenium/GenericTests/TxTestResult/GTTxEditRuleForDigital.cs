using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTTxEditRuleForDigital : GTTxBaseEditRule
    {

        public GTTxEditRuleForDigital(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("Defaultvalue", "Default value")]
        public  WEGInput Defaultvalue()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTACellNumDefaultValue\")]"), this);
        }
        /// <summary>
        /// Do not add the attribute in your xml if you don't want to write on this block
        /// Ex: Do not add min as parameter if you don't want to write min value 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        [TxAction("ValidityPeriod", "Validity Period only int value")]
        public void ValidityPeriod(String min=null, String max = null)
        {
            if(min!=null)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldVRangeMin\")]"), this).Fill(min);

            if (max != null)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldVRangeMax\")]"), this).Fill(max);
        }

        /// <summary>
        /// Do not add the attribute in your xml if you don't want to write on this block
        /// Ex: Do not add min as parameter if you don't want to write min value 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        [TxAction("WarningInterval", "Warning interval only int value")]
        public void WarningInterval(String min = null, String max = null)
        {
            if (min != null)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldARangeMin\")]"), this).Fill(min);

            if (max != null)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldARangeMax\")]"), this).Fill(max);
        }
        /// <summary>
        /// Do not add the attribute in your xml if you don't want to write on this block
        /// Ex: Do not add min as parameter if you don't want to write min value 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        [TxAction("RestrictionInterval", "Restriction intervall only int value")]
        public void RestrictionInterval(String min = null, String max = null)
        {
            if (min != null)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldRRangeMin\")]"), this).Fill(min);

            if (max != null)
                new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"inputTAFieldRRangeMax\")]"), this).Fill(max);
        }

        [TxAction("OkDisable", "Check if ok button is disable.")]
        public void OkDisable()
        {
            WERefreshed okButton = GetDriver().WaitForElement(Translator.GetButtonByValue(GetDriver(), Translator.validateButton), this);
            String disable = okButton.GetElement().GetAttribute("disabled");
            if (disable == null)
                throw new Exception("Ok button enable");
        }
    }
}
