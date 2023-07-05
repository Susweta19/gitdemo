using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.Windows
{
    public class WDChoiceGuide : WERefreshed
    {
        private static readonly By selectButton = By.Id("id_btn_select_cg");
        private static readonly By resetButton = By.Id("id_btn_reset_cg");
        private static readonly By extractButton = By.Id("id_btn_extract_cg");
        private static readonly By printButton = By.Id("id_btn_print_cg");
        private static readonly By exportButton = By.Id("id_btn_export_cg");

        public WDChoiceGuide(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Formulaire", "To manage all the attributes in the specified opend template.")]
        public WFormGDC Form()
        {
            return new WFormGDC(GetDriver(), By.Id("general"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Reset", "Click on Reset button")]
        public void Reset()
        {
            GetDriver().ClickAt(WDChoiceGuide.resetButton, this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Print", "Click on print button")]
        public bool Print()
        {
            WERefreshed printButton = new WERefreshed(GetDriver(), WDChoiceGuide.printButton, this);
            return printButton.GetElement().Displayed && printButton.GetElement().Enabled;
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Export", "Click on export button")]
        public void Export()
        {
            GetDriver().ClickAt(WDChoiceGuide.exportButton, this);
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Extract", "Click on Extract button")]
        public void Extract()
        {
            GetDriver().ClickAt(WDChoiceGuide.extractButton, this);
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TxAction("Select", "Click on select button which open a multicriteria selection")]
        public WDFramedWindow<WDMultiCriteria> Select(bool popUp = false)
        {
            GetDriver().ClickAt(WDChoiceGuide.selectButton, this);

            if (popUp)
            {
                new WESPopUp(GetDriver()).ClosePopup(true);
                return null;
            }
            else
            {
                WDMultiCriteria multicritria = new WDMultiCriteria(GetDriver(), By.Id("idBodyParent"));
                return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multicritria, null, WDFramedWindow<WERefreshed>.frameWindowBy);
            }
        }
        [TxAction("IsButtonEnabled", "Checks weather a button is enable or not")]
        public bool IsButtonEnabled(string buttonName)
        {
            return !GetDriver().IsElementPresent(By.XPath(".//input[@value=\"" + buttonName + "\"and @disabled]"));
        }

        [TxAction("ReadAllButtons", "Reads all button name present")]
        public ActionColl<string> ReadAllButtons()
        {
            List<IWebElement> fields = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'choix_critere_num')]"), this).GetElement().FindElements(By.XPath(".//input[@type=\"button\"]")).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                buttonNames.Add(fields.ElementAt(i).GetAttribute("value"));
            return new ActionColl<string>(buttonNames);
        }

    }
}
