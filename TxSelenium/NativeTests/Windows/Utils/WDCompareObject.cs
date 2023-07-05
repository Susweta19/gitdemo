using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Windows
{
    public class WDCompareObject : WERefreshed
    {
        public WDCompareObject(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TxAction("ObjectTree", "Select the objects to comapre")]
        public WMultipleLink ObjectTree()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivMain\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("CurrentObject", "Select the objects to comapre")]
        public WEGInput CurrentObject()
        {
            return new WEGInput(GetDriver(), By.Id("idTextObjectNameTxCompare"),  this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="max_min"></param>
        /// <param name="entire_selected"></param>
        [TxAction("ContextMenuOption", "value : 0-> expand, 1-> collapse, 2-> select, 3-> deselect")]
        public void ContextMenuBranch(DTEntityNode nodes, int value)
        {
            string[] choice = new string[] {Translator.GetTranslation(GetDriver().Language, Translator.expandBranchCM),
                                            Translator.GetTranslation(GetDriver().Language, Translator.collapseBranchCM),
                                            Translator.GetTranslation(GetDriver().Language, Translator.selectBranchCM),
                                            Translator.GetTranslation(GetDriver().Language, Translator.deselectBranchCM) };

            ObjectTree().OpenMenu(nodes).SelectEntry(new string[] {choice[value]});
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Ok", "Ok the ccomparaison")]
        public WDFramedWindow<WDMultiCriteria> Validate()
        {
            GetDriver().ClickAt(Translator.GetButtonByValue(GetDriver(), Translator.validateButton), this);

            WDMultiCriteria multicriteria = new WDMultiCriteria(GetDriver(), By.TagName("body"));
            return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multicriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }
    }
}
