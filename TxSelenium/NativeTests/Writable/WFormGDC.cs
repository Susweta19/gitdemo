using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows.ChoiceGuide;

namespace TxSelenium.NativeTests.Writable
{
     public class WFormGDC : WERefreshed
    {

        public static By GetDiv(int characId)
        {
            return By.Id("cg_q" + characId );
        }

        public WFormGDC(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// Fill Single value, interval, textfield
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valueInf"></param>
        /// <param name="secondValue"></param>
        [TxAction("Write", "Writes in the input attribute, text, single value, interval...")]
        public void Write(int id, string firstValue, string secondValue = null)
        {
            new WEGInput(GetDriver(), new ByChained(GetDiv(id), By.XPath(".//input | .//textarea")), this).Fill(firstValue);
            if (secondValue != null)
                new WEGInput(GetDriver(), new ByChained(GetDiv(id), By.XPath(".//input[2]")), this).Fill(secondValue);
        }

        [TxAction("SingleAnswer", "Writes in the specified attribute.")]
        public void SingleAnswer(int id, string userChoice)
        {
            new WEGSelect(GetDriver(),  By.Name("inpCG_"+ id), this).SelectByText(userChoice);
        }

        [TxAction("CheckBox", "Tick/Untick the attribute checkboxes.")]
        public WEGCheckBox CheckBox(int id)
        {
           return new WEGCheckBox(GetDriver(),new ByChained(GetDiv(id), By.XPath(".//input[@type =\"checkbox\"]")),this);
        }

        [TxAction("TreeListe", "Gets the handle for the main entity tree.")]
        public WMultipleLink TreeListe(int id)
        {
            return new WMultipleLink(GetDriver(), GetDiv(id), WESTree.expandByDefCriteriaTree, WSingleLink.entitySelectorTreeGDC, this);
        }
        [TxAction("DateCriteria", "")]
        public WDChoiceGuideDateCriteria DateCriteria(int id)
        {
            return new WDChoiceGuideDateCriteria(GetDriver(), By.XPath(".//div[@id='cg_q" + id + "']//div[@class='gdc_choix']"), this);
        }
        [TxAction("CheckBoxForMmultipleEntity", "Tick/Untick the attribute checkboxes.")]
        public WEGCheckBox CheckBoxForMmultipleEntity(int index)
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@name,\"inpCG_3_detail\")][" + index + "]"), this);
        }
        [TxAction("ReadQuestion", "Read Choice Guide Question by id")]
        public string ReadQuestion(int id)
        {
            return GetDriver().WaitForElement(new ByChained(GetDiv(id), By.XPath(".//div[@class=\"gdc_enonce\"]")), this).GetElement().Text;
        }

    }
}
