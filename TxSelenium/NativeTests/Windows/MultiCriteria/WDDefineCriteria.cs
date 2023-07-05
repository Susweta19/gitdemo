using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDDefineCriteria : WERefreshed
    {
        private static readonly By deleteCrtiterion = By.Id("inp_efface_critere");
        public static readonly By preselectTree = By.XPath(".//div[starts-with(@id,\"tree\") and .//div[@class = \"treeview_liste\"]]");

        public static By GetCheckBox(string boxName)
        {
            return By.XPath(".//td[text()= \"" + boxName + "\" ]/input");
        }

        public static By FieldSet(By idFieldSet, int value)
        {

            return new ByChained(idFieldSet, By.XPath(".//input[" + value + "]"));
        }

        public WDDefineCriteria(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// Select an attribute in the list.
        /// </summary>
        /// <param name="path">The path to the attribute</param>
        [TxAction("ListOfAttributes", "Select an attribute.")]
        public virtual WESTree ListOfAttributes()
        {
            return new WESTree(GetDriver(), By.Id("smc_zone_gauche"), WESTree.expandByDefCriteriaTree, this, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="words"></param>
        [TxAction("TextMustContain", "The text must contain all of the following words.")]
        public virtual void TextContain(string words)
        {
            new WEGInput(GetDriver(), new ByChained(By.Id("interieur_critere"), By.TagName("input")), this).Fill(words);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="words"></param>
        [TxAction("SelectObjectslinked", "Select the Objects linked to option.")]
        public void ObjectsLinked(string objectName)
        {
            new WEGSelect(GetDriver(), By.Id("id_select_mcs_object_linked_to"), this).SelectByText(objectName);
        }

        [TxAction("SelectAttribute", "Only for Demo Standard, there is two attr with same name")]
        public void SelectAttribute(string idMet)
        {
            GetDriver().ClickAt(By.XPath(".//span[@id_met='" + idMet + "']"), this);
        }

        /// <summary>
        /// criterion for formulary, link and ...
        /// </summary>
        /// <param name="open"></param>
        /// <returns></returns>
        [TxAction("PreselectTree", "Preselect all objects(value is false) or some of objects (value is true)")]
        public WMultipleLink PreselectTree(bool open = false)
        {
            if (open != false)
            {
                GetDriver().ClickAt(By.Id("id_inp_select_some"), this);
                return new WMultipleLink(GetDriver(), WDDefineCriteria.preselectTree, WESTree.expandByDefCriteriaTree, WSingleLink.entitySelectorPreselctBy, this, 2);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkAttribute"></param>
        /// <param name="applyRecursivity"></param>
        [TxAction("Recursivity", "Select link attribute to use and apply recursivity")]
        public void Recursivity(string linkAttribute, string applyRecursivity)
        {
            new WEGSelect(GetDriver(), By.Id("id_select_mcs_link_attribute"), this).SelectByText(linkAttribute);
            GetDriver().ClickAt(GetCheckBox(applyRecursivity), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TxAction("DecimalCriterion", "return the object to manage the 4 functions")]
        public WDCriterionFunction DecCrit()
        {
            return new WDCriterionFunction(GetDriver(), By.Id("smc_zone_droite"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("ShowGraph", "Click on show graph button")]
        public void ShowGraph()
        {
            GetDriver().ClickAt(new ByChained(By.Id("representation_valeur"), By.TagName("input")), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">value : </param>
        [TxAction("CriterionOf", "Select CriteriaOf, 0-> Selection , 1-> Comparaison")]
        public void CriteriaOf(int value)
        {
            By buttonBy = By.XPath(".//fieldset[@id=\"type_critere\"]/input[" + (value + 1) + "]");
            GetDriver().ClickAt(buttonBy, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaName"></param>
        [TxAction("NoData", "Select NoData")]
        public void NoData(string noDataName)
        {
            new WEGSelect(GetDriver(), By.Id("id_select_mcs_no_data"), this).SelectByText(noDataName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [TxAction("BooleanCriterion", "Definition of a Criterion by the Attribute of Type Booléen")]
        public void BoolCriterion(bool value)
        {
            if (value)
                GetDriver().ClickAt(By.XPath(".//input[@name = \"cri_booleen\" and @value = \"0\"]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//input[@name = \"cri_booleen\" and @value = \"1\"]"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaName"></param>
        [TxAction("OptimizationCriterion", "Select Optimization Criterion, 1->None, 2->Minimize, 3->Maximize, 4-> Target Value")]
        public void OptCrit(int value, string targetValue = null)
        {
            GetDriver().ClickAt(FieldSet(By.Id("optimisation"), value), this);

            if (value == 4)
                new WEGInput(GetDriver(), By.XPath(".//input[@name = \"inp_val_cible\"]"), this).Fill(targetValue);
        }

        /// <summary>
        /// Delete the criterion.
        /// </summary>
        [TxAction("DeleteTheCriterion", "Delete the criterion.")]
        public void Delete()
        {
            GetDriver().ClickAt(WDDefineCriteria.deleteCrtiterion, this);
        }
    }
}
