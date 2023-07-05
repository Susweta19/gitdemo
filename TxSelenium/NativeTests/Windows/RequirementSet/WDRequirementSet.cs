using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDRequirementSet : WERefreshed
    {

        private static readonly By openButton = By.XPath(".//input[contains(@onclick, \"Open_CDC\")]");
        private static readonly By newButton = By.XPath(".//input[contains(@onclick, \"Open_New_CDC\")]");

        private static By GetReqSetLine(string reqSet)
        {
            return new ByChained(By.Id("liste_cdc"), By.XPath(".//div[text()=\"" + reqSet + "\"]"));
        }

        public WDRequirementSet(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        [TxAction("Browse", "Click on browse button.")]
        public WDFramedWindow<WDMultiCriteria> Browse(string pathFile)
        {
            WERefreshed button = GetDriver().WaitForElement(By.Id("idBtnUploadRequirementSet"), this);
            GetDriver().UploadFile(button, pathFile);
            WDMultiCriteria multicriteria = new WDMultiCriteria(GetDriver(), By.Id("idBodyParent"));
            return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multicriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("ObjectType", "Requirement Set for the Object Type.")]
        public void ObjectType(string objectType)
        {
            new WEGDHtmlxComboBox(GetDriver(), By.Id("idDivComboOTRequirementSetChoice"), this).SelectV2(objectType);
          //  new WEGSelect(GetDriver(), By.Id("cmb_principal"), this).SelectCMB(objectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementSet"></param>
        [TxAction("RequirementSet", "Requirement Set selection.")]
        public void RequirementSet(string requirementSet)
        {
            GetDriver().ClickAt(GetReqSetLine(requirementSet), this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("New", "Click on New button to open MultiCriteria window.")]
        public WDFramedWindow<WDMultiCriteria> New()
        {
            GetDriver().ClickAt(newButton, this);

            WDMultiCriteria multicriteria = new WDMultiCriteria(GetDriver(), By.Id("idBodyParent"));
            return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multicriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);

        }

        /// <summary>
        /// Open an open requirement set from an object type.
        /// </summary>
        /// <param name="objectType">The name of object type</param>
        /// <param name="reqSet">The name of requirement set, if null we click on New</param>
        /// <returns></returns>
        [TxAction("Open", "Click on Open button to open MultiCriteria window.")]
        public WDFramedWindow<WDMultiCriteria> Open(bool popUp = false)
        {
            GetDriver().ClickAt(openButton, this);

            if (popUp)
                new WESPopUp(GetDriver()).ClosePopup(true);

            WDMultiCriteria multicriteria = new WDMultiCriteria(GetDriver(), By.Id("idBodyParent"));
            return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multicriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }
        [TxAction("IsRequirementSetPresent", "Requirement Set selection.")]
        public bool IsRequirementSetPresent(string requirementSet)
        {
            return GetDriver().IsElementPresent(GetReqSetLine(requirementSet), this.GetElement());
        }
    }
}