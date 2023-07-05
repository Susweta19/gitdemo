using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.Writable;
using TxSelenium.NativeTests.Windows;
using TxSelenium.GenericTests.TxMCS;
using System.Threading;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericDevs.TxMCS
{
    public class GTTxMCS : WERefreshed
    {
        By advancedTabXpath = By.XPath(".//div[starts-with(@class,\"dhxtabbar_tab\")]//div[text()=\"Advanced options\"]/..");
        By criteriaTabXpath = By.XPath(".//div[starts-with(@class,\"dhxtabbar_tab\")]//div[text()=\"Criterion\"]/..");
        By advancedTabArea = By.XPath(".//div[starts-with(@id,\"idMCSAdvanced\")]");
        By advancedOptionButton = By.XPath(".//input[starts-with(@id,'btnMCSAdvanced')]");

        public GTTxMCS(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("SelectionResult", "")]
        public WESTree SelectionResult()
        {
            return new WESTree(GetDriver(), By.XPath(".//div[starts-with(@id,\"idMCSManagerResultGrid\")]"), WESTree.expandByClassicTree);
        }

        [TxAction("SelectObjectType","Select the Object Type")]
        public void SelectObjectType(String objectTypeName)
        {
            GetDriver().ClickAt( By.XPath(".//div[@class = \"accLabelWithIcon\" and text() = \"" + objectTypeName + "\" ] | .//span[@class = \"accLabelWithIcon\" and text() = \"" + objectTypeName + "\" ]"));
        }

        [TxAction("BlankRequirementSet", "Click on the Blank Requirement Set.")]
        public void BlankRequirementSet(string objectType)
        {
           // GetDriver().ClickAt(new ByChained(By.XPath(".//span[text() = \"" + specificationName + "\" ]/../.."), By.ClassName("mcsBoxName")) , this);
            GetDriver().ClickAt(By.XPath(".//div[text() = \"" + objectType + "\" ]/ancestor::div[starts-with(@id,\"accItem\")]//img[contains(@src , \"CreateNewRequirementList.png\")]"));
        }

        [TxAction("Advanced", "Click on the Advanced button.")]
        public GTAdvanced Advanced(bool alreadyOpen = false)
        {
            if (!alreadyOpen)
            {
                GetDriver().ClickAt(advancedOptionButton, this);
                GetDriver().ClickAt(advancedTabXpath, this);
            }

            else
            {
                GetDriver().ClickAt(advancedTabXpath, this);
            }
            
            return new GTAdvanced(GetDriver(), advancedTabArea,this);
        }

        [TxAction("IsAdvancedTabDisplayed", "")]
        public bool IsAdvancedTabDisplayed() 
        {
            return !this.GetElement().FindElement(advancedTabXpath).GetAttribute("class").Contains("dhxtabbar_tab_hidden");
        }

        [TxAction("FilledRequirementSet", "Click on the Blank Requirement Set.")]
        public void FilledRequirementSet(string objectType, string requirmentSet)
        {
            //GetDriver().ClickAt(new ByChained(By.XPath(".//span[text() = \"" + specificationName + "\" ]/../.."), By.ClassName("mcsBoxName")), this);
             GetDriver().ClickAt(By.XPath(".//div[text() = \"" + objectType + "\" ]/../..//div[text() = \""+requirmentSet+"\"]"));
        }


        /// <summary>
        /// Tree to manage settings>
        /// </summary>
        /// <returns></returns>
        [TxAction("Settings", "Manage the tree for setting in the left side.")]
        public WMultipleLink Settings()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with( @id,\"idMCSMiddleCell\")]"), WESTree.expandByClassicTree,WSingleLink.entitySelectorWLinkBy , this);
        }

        [TxAction("Criteria", "Click on the Blank Requirement Set.")]
        public GTCriteria Criteria()
        {
            GetDriver().ClickAt(criteriaTabXpath,this);
            return new GTCriteria(GetDriver(), By.XPath(".//div[starts-with( @id,\"idMCSRightCell\")]"), this);
        }


        [TxAction("ShowAllRequirementSet", "Display All Requirement Sets")]
        public void ShowAllRequirementSet(string objectTypeName)
        {
            GetDriver().ClickAt(By.XPath(".//span[text() = \"" + objectTypeName + "\" ]/../..//div[starts-with (@id,\"mcsManagerLoadRl\")]  |  .//div[text() = \"" + objectTypeName + "\" ]/../..//div[starts-with (@id,\"mcsManagerLoadRl\")]"));
        }

        [TxAction("ViewAllTypeOfEntities", "Display All Requirement Sets")]
        public void ViewAllTypeOfEntities()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id , \"idDivLoadAllObjectsType\")]"));
        }


        /********************************************** BUTTONS ****************************************************/

        [TxAction("NewSelection", "For New Selection")]
        public void NewSelection()
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with( @id,\"btnMCSNew\")]"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disable">if disbale is false we click on the button, if true we check button is disbaled</param>
        [TxAction("Next", "Click on Next Button or check if disabled.")]
        public void NextButton(bool disable = false)
        {
            if (!disable)
                GetDriver().ClickAt(By.XPath(".//input[starts-with( @id,\"btnMCSNext\")]"), this);
            else
            {
                if (GetDriver().WaitForElement(By.XPath(".//input[starts-with( @id,\"btnMCSNext\")]"), this).GetElement().GetAttribute("disabled") != "disabled")
                    throw new Exception("Button is not disabled");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disable">if disbale is false we click on the button, if true we check button is disbaled</param>
        [TxAction("Previous", "Click on Previous Button or check if disabled.")]
        public void PreviousButton(bool disable = false)
        {
            if (!disable)
                GetDriver().ClickAt(By.XPath(".//input[starts-with( @id,\"btnMCSPrevious\")]"), this);
            else
            {
                if (GetDriver().WaitForElement(By.XPath(".//input[starts-with( @id,\"btnMCSPrevious\")]"), this).GetElement().GetAttribute("disabled") != "disabled")
                    throw new Exception("Button is not disabled");
            }
        }


        [TxAction("Close", "Click on Close Button.")]
        public void CloseButton()
        {
            GetDriver().ClickAt(By.XPath(".//input[starts-with( @id,\"idBtnCancel\")]"), this);
        }

        //=======================================Entities Selected=======================================================
        [TxAction("SelectedEntity", "")]
        public WESTree SelectedEntity()
        {
            return new WESTree(GetDriver(), By.XPath(".//div[starts-with(@id,\"idMCSManagerResultGrid\")]"), WESTree.expandByClassicTree,this);
        }

        [TxAction("ClickOnHeader", "")]
        public void ClickHeader()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class = \"hdrcell\"]"));
        }

        //=======================================ACTIONS=======================================================

        [TxAction("ExtractEntities", "...............")]
        public WDExtraction ExtractEntities()
        {
            GetDriver().ClickAt(By.XPath(".//*[contains(@id,\"idMCSManagerResultDev\")]/div[2]/div/div/div[1]"), this);
            return new WDExtraction(GetDriver(), By.XPath(".//div[@class = \"dhtmlx_window_active\"][2]"), this);
        }

        [TxAction("ExportEntities", "............")]
        public WDWindow<WDExport> ExportEntities()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src ,\"48x48-Exportation.png\")]"), this);
            WDExport exportWindow = new WDExport(GetDriver(), By.XPath(".//div[contains(@class,\"Exportation.png)\")]/../..//div[starts-with(@class,\"dhx_cell_wins\")]"));
            return new WDWindow<WDExport>(GetDriver(), exportWindow);
        }


        [TxAction("ExportEntitiesToExcel", "For New Selection")]
        public void ExportEntitiesXL()
        {
            GetDriver().ClickAt(By.XPath(".//div[@data-model-name=\"SimplifiedExportation\"]//img"), this);
        }

        [TxAction("ReadRequirementTitle", "Reads the heading of Criteria tab when an attribute is selected")]
        public string ReadRequirementTitle(string requirementName)
        {

            //var aa= GetDriver().WaitForElement(By.XPath(".//div[text()=\"" + requirementName + "\" and @class=\"mcsBoxName\"]/..//div[@class=\"mcsBoxDescription\"]"), this).GetElement().Text;
            try
            {
                WERefreshed ele = GetDriver().WaitForElement(By.XPath(".//div[text()=\"" + requirementName + "\" and @class=\"mcsBoxName\"]/..//div[@class=\"mcsBoxDescription\"]"), this);
                int i = 0;
                return ele.GetElement().Text;
            }
            catch
            {
                WERefreshed ele = GetDriver().WaitForElement(By.XPath(".//div[text()=\"" + requirementName + "\" and @class=\"mcsBoxName\"]/..//div[@class=\"mcsBoxDescription\"]"));
                int i = 0;
                return ele.GetElement().Text;
            }
        }
        [TxAction("ReadCriteriaHeading", "Reads the heading of Criteria tab when an attribute is selected")]
        public string ReadCriteriaHeading(string attributeName)
        {
            return GetDriver().WaitForElement(By.XPath(".//span[text()='\"" + attributeName + "\"']"), this).GetElement().Text;
        }

        [TxAction("ReadNumberOfEntites", "Read the number of entities for Multicriteria Selection- New Selection")]
        public string ReadNumberOfEntites()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class='hdrcell']"), this).GetElement().Text.Split('(').Last().Split('/')[0];
        }

        [TxAction("IsRequirementSetPresent", "Checks weather a particular requirement set is present or not")]
        public bool IsRequirementSetPresent(string RSname)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class='accItemContentArea']//div[@class=\"mcsBoxName\" and text()=\"" + RSname + "\"]"), this.GetElement());
        }

        [TxAction("ReadAllButtons","Reads all button name present")]
        public ActionColl<string>ReadAllButtons()
        {
            List<IWebElement> fields = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'idDivBtns')]"), this).GetElement().FindElements(By.XPath(".//input[@type=\"button\"]")).ToList();
            List<string> buttonNames = new List<string>();
            for (int i = 0; i < fields.Count; i++)
                buttonNames.Add(fields.ElementAt(i).GetAttribute("title"));
            return new ActionColl<string>(buttonNames);
        }
    }
}
