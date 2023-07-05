using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.GenericDevs.TxMCS;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxAdvancedSearch
{
    public class TxAdvancedSearch : WERefreshed
    {
        public TxAdvancedSearch(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("IfPopUpAppears", "")]
        public void IfPopUpAppears(bool validate = false)
        {
            try
            {
                new WESPopUp(GetDriver()).CloseConfirmationPopup(validate);
            }
            catch (Exception)
            {
                Console.WriteLine("No Confimation PopUp appears..");
            }
        }

        [TxAction("ReturnTableViewResult", "")]
        public GTTxTableView ReturnTableViewResult(int index)
        {
            return new GTTxTableView(GetDriver(), By.XPath("(.//div[@class=\"tvSubwrapperWidgetTableView\"])[" + index + "]"), this);
        }

        [TxAction("ReadActiveTab", "")]
        public string ReadActiveTab()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhxtabbar_tab_actv\")]"), this).GetElement().Text;
        }

        [TxAction("SelectAdvancedSearch", "Gets a handle on the window's content.")]
        public WDValidatedWindow<ManageAdvancedSearch> SelectAdvancedSearch(bool click = false)
        {
            if (click)
                GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"wNew.png\")]"), this);
            ManageAdvancedSearch window = new ManageAdvancedSearch(GetDriver(), By.XPath(".//div[starts-with(@id,'idadvanceSearchManagerMainLayout')]/.."));
            return new WDValidatedWindow<ManageAdvancedSearch>(GetDriver(), window);
        }

        //[TxAction("LoadAdvanceSearch", "Gets a handle on the window's content.")]
        //public WDValidatedWindow<ManageAdvancedSearch> LoadAdvanceSearch()
        //{
        //    ManageAdvancedSearch window = new ManageAdvancedSearch(GetDriver(), By.XPath(".//div[starts-with(@class,'dhxwin_active')]"));
        //    return new WDValidatedWindow<ManageAdvancedSearch>(GetDriver(), window);
        //}

        [TxAction("ReadWindowTitle", "Reads the title of the window opened")]
        public string ReadWindowTitle()
        {
            //if (currentwin)

            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@class,\"dhxwin_text_inside\")]|.//div[contains(@class,\"clDivTxToolbarName\")]"), this);
            return elem.GetElement().Text;

            //else
            //{
            //    WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"][2]//div[@class=\"dhxwin_text_inside\"]"), this);
            //    return elem.GetElement().Text;
            //}
        }

        [TxAction("ReadConfigName", "")]
        public ActionColl<string> ReadConfigName()
        {
            {
                ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//ul[@class=\"advanceSearchOTRequirementsList\"and not(contains(@id,\"idDivAdvancedUserSearchList\"))]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"advanceSearchBoxName\"]"));
                ICollection<string> CriteriaName = new List<string>(element.Count);
                for (int i = 0; i < element.Count; i++)

                    CriteriaName.Add(element.ElementAt(i).Text);
                return new ActionColl<string>(CriteriaName);

            }
        }

        [TxAction("ReadCriteriaList", "")]
        public ActionColl<string> ReadCriteriaList()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivItemTools\")]"), this).GetElement().FindElements(By.XPath(".//label[starts-with(@for,\"idItem\")]"));
            ICollection<string> CriteriaName = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)

                CriteriaName.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(CriteriaName);

        }

        [TxAction("ShowMoreFilters", "Open the TxMCS")]
        public WDValidatedWindow<GTTxMCS> ShowMoreFilters()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivBtnFooterOptions\")]//img[contains(@src,\"AddObject.png\")]"), this);
            //  GetDriver().ClickAt(TxMCSButton, this);
            GetDriver().WaitForAjax();
            GTTxMCS TxMCS = new GTTxMCS(GetDriver(), By.XPath(".//div[@class='dhxwin_active']"));
            return new WDValidatedWindow<GTTxMCS>(GetDriver(), TxMCS);
        }


        [TxAction("CloseAllGroup", "Collapse all expanded group")]
        public void CloseAllGroup()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivBtnFooterOptions\")]//img[contains(@src,\"CloseAccordions.png\")]"), this);
        }
        [TxAction("IsShowMoreFiltersPresent", "Open the TxMCS")]
        public bool IsShowMoreFiltersPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idDivBtnFooterOptions\")]//div[contains(@title,\"Show more filter\") and not(contains(@style,\"display: none;\"))]//img[contains(@src,\"AddObject.png\")]"), this.GetElement());
        }

        [TxAction("ClearAll", "Open the TxMCS")]
        public void ClearAll()
        {
            GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivBtnClearAll\")]//button[starts-with(@id,\"idBtnClearAll\")]"), this).GetElement().Click();
        }
        [TxAction("RemoveCriterion", "Open the TxMCS")]
        public void RemoveCriterion(int criteriaIndex = 1)
        {
            Sleep(2);
            WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[starts-with(@id,\"idItem\")][" + criteriaIndex + "]"), this);
            GetDriver().MouseOver(ele);
            Sleep(1);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idItem\")][" + criteriaIndex + "]//div[@class=\"clRemoveCriterion\"]"), this);
            GetDriver().Release();
        }

        [TxAction("ReadCurrentConfiguration", "....")]
        public string ReadCurrentConfiguration()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivTxToolbarName\")]"), this).GetElement().Text;
        }

        [TxAction("ChangeTab", "")]
        public void ChangeTab(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhxtabbar_tab_text\") and text()=\"" + tabName + "\"]"), this);
        }

        [TxAction("ReturnTableView", "")]
        public GTTxTableView ReturnTableView()
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//div[@class=\"sousWrapperWidgetTableView\"]"), this);

        }

        [TxAction("ManageQuestionList", "")]
        public ManageQuestionList ManageQuestionList()
        {

            return new ManageQuestionList(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivRequirementsList\")and @class=\"clDivRequirementsList\"]"));
        }
        [TxAction("Export", "")]
        public WDWindow<WDExport> Export()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"24x24-wExportation.png\")]"), this);
            WDExport export = new WDExport(GetDriver(), By.ClassName("dhtmlx_window_active"));
            return new WDWindow<WDExport>(GetDriver(), export);
        }

        [TxAction("IsCriteriaDisabled", "")]
        public bool IsCriteriaDisabled(string questionname)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@title,\"" + questionname + " :\")]"), this).GetElement().GetAttribute("class").Contains("clItemsTools clItemsToolsDelete");
        }

        [TxAction("RefreshResult", "")]
        public void RefreshResult()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"dhx_cell_tabbar\" and not(contains(@style,\"visibility: hidden\"))]//div[@id=\"idRefreshContent\"]//img[contains(@src,\"Refresh.png\")]"), this);
            Sleep(3);
        }

        [TxAction("GetObjectId", "")]
        public string GetObjectId(string objName)
        {
            WERefreshed list = GetDriver().WaitForElement(By.XPath(".//div[@class=\"divColumnResults\"]"), this);
            return GetDriver().WaitForElement(By.XPath(".//span[text()=\"" + objName + "\" and @class=\"clLigneObjectResults\"]"), list).GetElement().GetAttribute("objectid");
        }

        [TxAction("ReadObjectList", "")]
        public ActionColl<string> ReadObjectList()
        {
            WaitForAjax();
            List<string> objectsName = new List<string>();
            WERefreshed list = GetDriver().WaitForElement(By.XPath(".//div[@class=\"divColumnResults\"]"), this);
            ICollection<IWebElement> objects = list.GetElement().FindElements(By.XPath(".//span[text() and contains(@class,\"clLigneObjectResults\")]"));
            foreach (IWebElement elem in objects)
                objectsName.Add(elem.Text);
            return new ActionColl<string>(objectsName);
        }

        [TxAction("CheckBoxInResultView", "")]
        public WEGCheckBox CheckBoxInResultView(string objId)
        {
            WERefreshed list = GetDriver().WaitForElement(By.XPath(".//div[@class=\"divColumnResults\"]"), this);
            return new WEGCheckBox(GetDriver(), By.XPath(".//img[@objectid=\"" + objId + "\"]"), list);
        }

        [TxAction("ManageQuickView", "")]
        public WMultipleLink ManageQuickView()
        {
            WMultipleLink link = new WMultipleLink(GetDriver(), By.XPath(".//div[contains(@class,\"resultsQuickView\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return link;
        }
        [TxAction("DisplayedResult", "")]
        public string DisplayedResult()
        {
            WaitForAjax();
            Sleep(3);
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_cell_tabbar\" and contains(@style,\"visibility: visible\")]//div[starts-with(@id,\"view\")]//div[starts-with(@id,\"idTxAdvancedSearchToolbar\") or starts-with(@id,\"tvDivGridToolbar\")]//div[starts-with(@id,\"btnSelectPage\")]/following-sibling::div[@class=\"dhx_toolbar_text\"]"), this).GetElement().Text;
        }

        [TxAction("DisplayOtherObjectTypes", "Clicks on Display other object type button")]
        public void DisplayOtherObjectTypes()
        {
            GetDriver().ClickAt(By.Id("idBtnAddOtherObjectTypes"));
        }

        [TxAction("DoubleClickOnEntity", "")]
        public void DoubleClickOnEntity(string objName)
        {
            WERefreshed list = GetDriver().WaitForElement(By.XPath(".//div[@class=\"divColumnResults\"]"), this);
            GetDriver().DoubleClickAt(By.XPath(".//span[text()=\"" + objName + "\" and contains(@class,\"clLigneObjectResults\")]"), list);
        }
        [TxAction("CheckAllObject", "")]
        public void CheckAllObject()
        {
            WaitForAjax();
            GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idTxAdvancedSearchToolbar\")]//img[contains(@src,\"CheckObject.png\")]"), this).GetElement().Click();
            //GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn dhxtoolbar_btn_def\") and not(contains(@style,\"display: none;\"))]//img[contains(@src,\"CheckObject.png\")]"), this).GetElement().Click();
        }
        [TxAction("UnCheckAllObject", "")]
        public void UnCheckAllObject()
        {
            WaitForAjax();
            GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idTxAdvancedSearchToolbar\")]//img[contains(@src,\"UncheckObject.png\")]"), this).GetElement().Click();
            //GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn dhxtoolbar_btn_def\") and not(contains(@style,\"display: none;\"))]//img[contains(@src,\"UncheckObject.png\")]"), this).GetElement().Click();
        }
        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            IWebElement elm = GetDriver().FindElement(By.XPath(".//div[@id=\"idTxAdvancedSearchToolbar\"]"));

            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_dis\"and @title=\"" + buttonName + "\"]"), elm);
        }

        [TxAction("ReadRefreshButton", "Reads the name of refresh button")]
        public string ReadRefreshButton()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"dataLoadingsInfo\")]")).GetElement().Text;
        }


        [TxAction("SaveConfiguration", "Manage save window")]
        public WDValidatedWindow<ManageAdvancedSearch> SaveConfiguration()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,'wSave.png')]"), this);
            ManageAdvancedSearch advancesearch = new ManageAdvancedSearch(GetDriver(), By.XPath(".//div[@class='dhxwin_active']"));
            return new WDValidatedWindow<ManageAdvancedSearch>(GetDriver(), advancesearch);
        }
        [TxAction("DownloadCurrentAdvancedSearch", " To download current advanced search")]
        public void DownloadCurrentAdvancedSearch()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"wAdvancedOptions.png\")]"), this);
            Sleep(1);
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Download the current Advanced Search\"]"), this);
        }
        [TxAction("ExtractAnswersWord", " To extract answer in word")]
        public void ExtractAnswersWord()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"wAdvancedOptions.png\")]"), this);
            Sleep(1);
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Extract answers to Word\"]"), this);
        }
        [TxAction("DisableEnableCriteria", "")]
        public void DisableEnableCriteria(int criteriaIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idItem\")][" + criteriaIndex + "]"), this);
        }
        [TxAction("AddResultView", "")]
        public void AddResultView()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivAddTab\")]//img[contains(@src,\"Add.png\")]"), this);
        }
        [TxAction("EditParameteredView", "")]
        public WDValidatedWindow<GTAddConfiguration> EditParameteredView()
        {
            GTAddConfiguration addConfiguration = new GTAddConfiguration(GetDriver(), By.XPath(".//div[@class='dhxwin_active']"));
            return new WDValidatedWindow<GTAddConfiguration>(GetDriver(), addConfiguration);
        }
        [TxAction("IsTabPresent", "")]
        public bool IsTabPresent(string tabName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"dhxtabbar_tab_text\") and text()=\"" + tabName + "\"]"), this.GetElement());
        }

        [TxAction("CloseTab", "")]
        public void CloseTab(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"dhxtabbar\") and text()=\"" + tabName + "\"]/../div[@class=\"dhxtabbar_tab_close\"]"), this);
            Sleep(1);
        }

        [TxAction("ClickOnToolBarButtons", "Click On button in toolbar.")]
        public void ClickOnToolBarButtons(string btnName)
        {
            GetDriver().ClickAt(By.XPath(".//*[contains(@title , \"" + btnName + "\")]/img"));
        }

        [TxAction("Extraction", "Open an extraction window.")]
        public WDWindow<WDExtraction> Extraction()
        {
            WDExtraction extractCreator = new WDExtraction(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"), this);
            return new WDWindow<WDExtraction>(GetDriver(), extractCreator);
        }

        [TxAction("ReadPageNumber", "*********************")]
        public string ReadPageNumber()
        {
            return GetDriver().WaitForElement(By.XPath(".//span[contains(@id,\"bigComboSelectText\")]")).GetElement().Text;
        }

        [TxAction("SelectPageNumber", "*******************")]
        public void SelectPageNumber(int pageNumber)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"bigCombo\")]//img"), this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"bigComboOptions\")]//span[@data-value][" + pageNumber + "]"));
        }

        [TxAction("SelectPageNumberBySearchBox", "*******************")]
        public WEGInput SelectPageNumberBySearchBox()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"bigCombo\")]//img"), this);
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"bigComboInputSearch\")]"));
        }

        [TxAction("SelectPageNumberAfterGivingInputInSearchBox", "*******************")]
        public void SelectPageNumberAfterGivingInputInSearchBox()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"bigComboOptions\")]//span[@data-value]"));
        }
    }
}