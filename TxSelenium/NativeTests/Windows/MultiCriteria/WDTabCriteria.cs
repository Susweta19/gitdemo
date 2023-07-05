using OpenQA.Selenium;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.DataTypes;
using System.Collections.Generic;

namespace TxSelenium.NativeTests.Windows
{
    public class WDTabCriteria : WERefreshed
    {
        private static readonly By DefineCriteriaButton = By.XPath(".//input[@onclick=\"Display_Popup_Modify_Criteria(); return false;\"]");
        private static readonly By LaunchCalculationButton = By.XPath(".//input[@onclick=\"LancerCalcul(false);\"]");
        private static readonly By AddNewGroupButton = By.XPath(".//input[@onclick=\"Display_Popup_Add_Group();\"]");
        private static readonly By EditButton = By.XPath(".//input[@onclick=\"Display_Popup_Modify_RS();\"]");

        private static readonly By modifyGroupButton = By.Id("btn_modifGC");
        private static readonly By deleteGroupButton = By.Id("btn_supprGC");
        private static readonly By saveButton = By.Id("btn_enregCDC");
        private static readonly By exportXmlButton = By.Id("btn_exportCDC");
        private static readonly By deleteReqSetButton = By.Id("btn_supprCDC");
        private static readonly By writeButton = By.Id("btn_expressionCDC");

        //private WETree tree;

        public WDTabCriteria(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            //this.tree = new WETree(GetDriver(), By.Id("smc_text_cdc"), WETree.expandByClassicTree, this, 2);        
        }

        /// <summary>
        /// Select requirement set for the object type.
        /// </summary>
        /// <param name="objetcTypeName"> A string referring the selected object type name.</param>
        [TxAction("ReqSetObjectType", "Select requirement set for the object type.")]
        public WEGDHtmlxComboBox ReqSetObjectType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("idDivComboOTMCS"), this);
        }

        /// <summary>
        /// Define a criteria.
        /// </summary>
        [TxAction("DefineCriteria", "Define a criteria.")]
        public WDValidatedFrameWindow<WDDefineCriteria> DefCriteria()
        {
            GetDriver().ClickAt(DefineCriteriaButton, this);

            WDDefineCriteria defineCriteriaCreator = new WDDefineCriteria(GetDriver(), By.TagName("body"));
            return new WDValidatedFrameWindow<WDDefineCriteria>(GetDriver(), defineCriteriaCreator, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        /// <summary>
        /// Launch the calculation of the selection
        /// </summary>
        [TxAction("LaunchCalculation", "Launch the calculation of the selection")]
        public WDFramedWindow<WDTabResults> ResultsTab()
        {
            //LancerCalcul(false);
            // GetDriver().ClickAt(Translator.GetButtonByValue(GetDriver(), Translator.launchCalculationButton));
            GetDriver().ClickAt(LaunchCalculationButton, this);
            WDTabResults tabResults = new WDTabResults(GetDriver(), By.Id("bar"));
            return new WDFramedWindow<WDTabResults>(GetDriver(), tabResults, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        /// <summary>
        /// Launch the calculation of the selection
        /// </summary>
        [TxAction("CriteriaTree", "return the tree.")]
        public WESTree CriteriaTree()
        {
            return new WESTree(GetDriver(), By.Id("smc_text_cdc"), WESTree.expandByClassicTree, this, 2);
        }

        /// <summary>
        /// Add a new group of criteria.
        /// </summary>
        /// <param name="groupPlace">The name of the goup which will be the father of the group created</param>
        /// <param name="groupName"> The name of the group you are adding.</param>
        /// <param name="aggrFuncName">The name of the aggregation function of the group you are adding.</param>
        /// <param name="validateOrCancel">Validate or Cancel your changes</param>
        [TxAction("AddNewGroup", "Add a new group of criteria. If you don't use path the new group added is son of current node")]
        public WDValidatedFrameWindow<WDFrameSMC> AddNewGroup(/*ICollection<String> path = null*/)
        {
            GetDriver().ClickAt(AddNewGroupButton, this);

            WDFrameSMC frameSMC = new WDFrameSMC(GetDriver(), By.TagName("body"));
            return new WDValidatedFrameWindow<WDFrameSMC>(GetDriver(), frameSMC, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        /// <summary>
        /// Modify the Group.
        /// </summary>
        /// <param name="groupSelected">the group modified</param>
        /// <param name="newGroupName">the group name modified</param>
        /// /// <param name="aggrFuncName">The name of the aggregation function of the group you are adding.</param>
        /// <param name="validateOrCancel">Validate or Cancel your changes</param>
        [TxAction("ModifyTheGroup", "Modify the group.")]
        public WDValidatedFrameWindow<WDFrameSMC> ModifyTheGroup(/*ICollection<String> path*/)
        {
            GetDriver().ClickAt(WDTabCriteria.modifyGroupButton, this);

            WDFrameSMC frameSMC = new WDFrameSMC(GetDriver(), By.TagName("body"));
            return new WDValidatedFrameWindow<WDFrameSMC>(GetDriver(), frameSMC, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string ButtonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[@value='"+ButtonName+"'and @disabled='disabled']"), this.GetElement());
        }

        /// <summary>
        /// Delete the Group.
        /// </summary>
        /// <param name="groupName">The name of the groupe that you delelte</param>
        /// <param name="okOrCancel">Validate or Cancel your changes</param>
        [TxAction("DeleteTheGroup", "Delete the group. Ok => true, Cancel => false")]
        public WESPopUp DeleteTheGroup(/*ICollection<String> path*/)
        {
            GetDriver().ClickAt(WDTabCriteria.deleteGroupButton, this);
            return new WESPopUp(GetDriver());
        }

        /// <summary>
        /// Modify the current requirement set.
        /// </summary>
        /// <param name="aggrFuncName"> The aggregation function for the New RS</param>
        /// <param name="newRSName">New requirement set name</param>
        /// /// <param name="aggrFuncName">The name of the aggregation function of the group you are adding.</param>
        /// <param name="validateOrCancel">Validate or Cancel your changes</param>
        [TxAction("Edit", "Modify the current requirement set.")]
        public WDValidatedFrameWindow<WDFrameSMC> Edit()
        {
            GetDriver().ClickAt(EditButton, this);

            WDFrameSMC frameSMC = new WDFrameSMC(GetDriver(), By.TagName("body"));
            return new WDValidatedFrameWindow<WDFrameSMC>(GetDriver(), frameSMC, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        /// <summary>
        /// Save the current requirement set.
        /// </summary>
        [TxAction("Save", "Save the current requirement set")]
        public WESPopUp Save()
        {
            GetDriver().ClickAt(WDTabCriteria.saveButton, this);
            return new WESPopUp(GetDriver());
        }

        /// <summary>
        /// Export the current requirement set in xml format.
        /// </summary>
        [TxAction("ExportXml", "Export the current requirement set in xml format.")]
        public void ExportXml()
        {
            GetDriver().ClickAt(WDTabCriteria.exportXmlButton, this);
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// Delete the requirement set.
        /// </summary>
        [TxAction("DeleteTheRequirement", "Delete the requirement set.")]
        public WESPopUp DeleteReqSet()
        {
            GetDriver().ClickAt(WDTabCriteria.deleteReqSetButton, this);
            return new WESPopUp(GetDriver());
        }


        /// <summary>
        /// Written expression.
        /// </summary>
        [TxAction("WriteExpression", "Written expression.")]
        public void Write()
        {
            GetDriver().ClickAt(WDTabCriteria.writeButton, this);
        }
        [TxAction("ReadSpecification", "Written expression.")]
        public string ReadSpecification()
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@id='smc_text_cdc']//div[contains(@id,'gc_')]"), this);
            return elem.GetElement().Text;
        }

        [TxAction("ReadCriteriaList", "....")]
        public ActionColl<string> ReadCriteriaList()
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath(".//div[@id=\"smc_text_cdc\"]"), this).GetElement().FindElements(By.XPath(".//ul//li"));
            ICollection<string> criteria = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
                criteria.Add(elem.Text);
            return new ActionColl<string>(criteria);
        }
    }
}