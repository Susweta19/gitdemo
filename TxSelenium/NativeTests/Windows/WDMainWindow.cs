using System;
using OpenQA.Selenium;
using TxSelenium.NativeTests.Readable;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;
using TxSelenium.NativeTests.WebElements;
using System.Collections.Generic;
using TxSelenium.GenericTests;
using TxSelenium.GenericTests.TxCharts;
using TxSelenium.NativeTests.Windows.Utils;
using TxSelenium.GenericTests.TxWorkFlow;
using TxSelenium.GenericTests.TxMCS;
using XmlExecutor.DataTypes;
using TxSelenium.GenericTests.TxCommunity;
using TxSelenium.GenericDevs;
using System.IO;
using TxSelenium.GenericTests.TxTestResult;

using TxSelenium.GenericTests.TxObject;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.Utils;
using TxSelenium.GenericDevs.TxTestResults;
using TxSelenium.GenericTests.TxVisualDesign;
using TxSelenium.GenericTests.TxHourTracking;
using TxSelenium.GenericDevs.TxMCS;
using TxSelenium.GenericTests.TxAdvancedSearch;
using TxSelenium.GenericTests.TXSpecification;
using TxSelenium.GenericTests.WebDav;
using TxSelenium.GenericTests.TxFormulation;
using TxSelenium.GenericTests.TxPortal;

namespace TxSelenium.NativeTests.Windows
{
    /// <summary>
    /// This class allows interactions with the elements of the main window.
    /// </summary>
    public class WDMainWindow : WERefreshed
    {
        private TxWebDriver _driver { get; set; }
        #region Buttons list
        public static readonly By hideBannerButton = By.XPath(".//div[starts-with(@id, \"idDivCollapseBanner\")]");
        public static readonly By hideTreeButton = By.XPath(".//div[starts-with(@id, \"idDivCollapseNav\")]");
        private static readonly By expandBannerButton = By.XPath(".//div[contains(@class,\"dhxLayoutButton_dhx_skyblue_ver2b\")] | .//div[@class=\"dhx_cell_layout dhx_cell_nested_layout\"]//div[contains(@class,\"dhxlayout_arrow dhxlayout_arrow_ha\")]");
        private static readonly By expandNavigationTreeButton = By.ClassName("dhxlayout_arrow dhxlayout_arrow_va");
        private static readonly By searchInput = By.ClassName("dhxtoolbar_input");
        private static readonly By disconnection = By.XPath(".//button[starts-with(@id, \"userDisconnection\")]");
        private static readonly By TxHourTrackingutton = By.XPath(".//img[contains(@src,\"TxHourTracking.png\")]");
        #endregion

        public WDMainWindow(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// Return to the portal.
        /// </summary>
        [TxAction("ReturnPortal", "Return to the portal.")]
        public void ReturnPortal(bool enable = true)
        {
            if (enable)
                GetDriver().ClickAt(ElemByPictureName.PortalButton, this);
            else
            {
                if (!GetDriver().IsElementPresent(ElemByPictureName.PortalDisableButton, this.GetElement()))
                    throw new Exception("portal button is enable");
            }
        }

        [TxAction("TeexmaLogo", "Return to the portal by clicking on Teexma Logo.")]
        public void TeexmaLogo()
        {
            GetDriver().ClickAt(By.Id("idDivMainToolbarLeftSide"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("ReadBanner", "check the name of the object in the banner....")]
        public string ReadBanner()
        {
            Thread.Sleep(1000);//to solve 26_32
            WERefreshed headerName;
            try
            {
                headerName = GetDriver().WaitForElement(By.Id("idDivBannerMiddle"), this);
            }
            catch (Exception)
            {
                ExpandBanner();
                headerName = GetDriver().WaitForElement(By.Id("idDivBannerMiddle"), this);
            }
            return headerName.GetElement().Text;
        }


        /// <summary>
        /// 
        /// </summary>
        [TxAction("ReadTabId", "check the tab id ....")]
        public string ReadTabId()
        {
            WERefreshed tab = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhx_tab_element dhx_tab_element_active\")]"), this);
            return tab.GetElement().GetAttribute("tab_id");
        }

        /// <summary>
        /// Passes teexma in table view.
        /// </summary>
        [TxAction("TableView", "Passes Teexma in table view.")]
        public WDWindow<WDTableView> TableView()
        {
            GetDriver().ClickAt(ElemByPictureName.TableViewButton, this);

            WDTableView tableViewCreator = new WDTableView(GetDriver(), By.XPath(".//div[@ida=\"dhxMainCont\"]"));
            return new WDWindow<WDTableView>(GetDriver(), tableViewCreator);
        }

        [TxAction("TableViewInNewTab", "Checks availability of a button.")]
        public GTTab<GTTxTableView> TableViewInNewTab(bool ByClickingNewTab = false)
        {
            Thread.Sleep(2000);
           // By iframePath = By.XPath(".//iframe[contains(@url,\"196.aspx\")] | .//table[@class=\"dhtmlxLayoutPolyContainer_dhx_skyblue\"]//table//tr[3]//iframe|.//iframe[@src]"); 
            if (!ByClickingNewTab)
                GetDriver().ClickAt(By.XPath(".//a[text()=\"TxTableView\"]"), this);
            else
                GetDriver().ClickAt(ElemByPictureName.YellowArrow, this);

            GetDriver().WaitForAjax();

            GTTxTableView txTableView = new GTTxTableView(GetDriver(), By.TagName("html"));
            return new GTTab<GTTxTableView>(GetDriver(), txTableView);
        }

        /// <summary>
        /// Passes teexma in write mode.
        /// </summary>
        [TxAction("Edit", "Passes Teexma in write mode.")]
        public WDValidatedWindow<WForm> Edit()
        {
            GetDriver().ClickAt(ElemByPictureName.WriteModeButton, this);
            //TODO Avoid thread.sleep
            Thread.Sleep(3000);
            WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }

        /// <summary>
        /// Gets the main form in read mode.
        /// </summary>
        /// <returns>the main form in read mode</returns>
        [TxAction("ReadForm", "Gets the handle for the form in read mode.")]
        public RForm GetReadForm()
        {
            //TODO Avoid thread.sleep
            Thread.Sleep(1000);
            return new RForm(GetDriver(), By.Id("idDivFormContainer"), this);
        }

        /// <summary>
        /// Use the multicriteria selection.
        /// </summary>
        [TxAction("MultiCriteria", "Use the multicriteria selection.")]
        public WDFramedWindow<WDMultiCriteria> MultiCriteria()
        {
            GetDriver().ClickAt(ElemByPictureName.MscButton, this);

            WDMultiCriteria multiCriteria = new WDMultiCriteria(GetDriver(), By.Id("idBodyParent"));
            return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multiCriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);

        }

        /// <summary>
        /// Open the choice guide window.
        /// </summary>
        /// <param name="modelName"></param>
        [TxAction("MainToolbar", "Click On MainToolbar.")]
        public void MainToolbar(string modelName)
        {
            GetDriver().ClickAt(By.XPath(".//*[contains(@title , \"" + modelName + "\")]/img"));
        }

        [TxAction("ChoiceGuide", "Open athe choice guide.")]
        public WDFramedWindow<WDChoiceGuideSelection> ChoiceGuide()
        {
            GetDriver().ClickAt(ElemByPictureName.ChoiceGuideButton, this);

            WDChoiceGuideSelection choiceGuideSelection = new WDChoiceGuideSelection(GetDriver(), By.Id("id_div_refresh_cg_list"));
            return new WDFramedWindow<WDChoiceGuideSelection>(GetDriver(), choiceGuideSelection, null, WDFramedWindow<WERefreshed>.frameWindowBy);

        }

        [TxAction("ReadUrlNewTab", "Read the alignement of pictures")]
        public string ReadUrlNewTab()
        {
            WaitForAjax();
            GetDriver().SwitchNewWindow();
            Thread.Sleep(5000);
            GetDriver().GetCurrentFrame();
            Thread.Sleep(3000);
            WebDriverWait wait = new WebDriverWait(GetDriver().Driver, TimeSpan.FromSeconds(10));
            string url = wait.Until<string>((d) =>
            {
                string urlTemp = GetDriver().GetCurrentUrl();
                if (urlTemp == "about:blank")
                    return null;
                else
                    return urlTemp;
            });
            GetDriver().Close();
            GetDriver().SwitchOldWindow();
            GetDriver().SwitchToContent();
            return url;
        }

        /// <summary>
        /// Function created for script 2_93.xml
        /// </summary>
        [TxAction("TableViewInWidget", "Passes Teexma in write mode.")]
        public GTTxTableView TableViewInWidget()
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//*[@id=\"TxTableViewForObjWidget\"]|.//div[@id=\"idDivPortal\"]"),null,true,null,true);
        }

        [TxAction("TableViewInWidget2", "Passes Teexma in write and read mode.")]
        public GTTxTableView TableViewInWidget2(bool writemode)
        {
            if (writemode)
                return new GTTxTableView(GetDriver(), By.XPath(".//*[@id=\"portalTableView1\"]/../.."), null, true);
            else
                return new GTTxTableView(GetDriver(), By.XPath(".//*[@id=\"portalTableView2\"]/../.."), null, true);
        }

        [TxAction("TableViewInNewTab2", "Passes Teexma in write and read mode.")]
        public GTTab<GTTxTableView> TableViewInNewTab2(bool writemode)
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"Tableview (Read view)\"]"),this);
            if (writemode) {
                GTTxTableView txTableView1 = new GTTxTableView(GetDriver(), By.XPath(".//*[@id=\"portalTableView1\"]/../.."), null, true);
                return new GTTab<GTTxTableView>(GetDriver(), txTableView1);
            }
            else {
                GTTxTableView txTableView2 = new GTTxTableView(GetDriver(), By.TagName("body"));
                return new GTTab<GTTxTableView>(GetDriver(), txTableView2);
            }
        }
        /// <summary>
        /// Open a  multicriteria window with a requirement selected from an object type.
        /// If ReqSet is null, we open a new multicriteria window.
        /// </summary>
        /// <returns></returns>
        [TxAction("OpenRequirementSet", "Open an open requirement set window.")]
        public WDFramedWindow<WDRequirementSet> OpenReqSet()
        {
            GetDriver().ClickAt(ElemByPictureName.OpenRequirementSetButton, this);

            WDRequirementSet requirementSet = new WDRequirementSet(GetDriver(), By.TagName("body"));
            return new WDFramedWindow<WDRequirementSet>(GetDriver(), requirementSet, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("Export", "Export data")]
        public WDFramedWindow<WDExport> Export()
        {
            GetDriver().ClickAt(ElemByPictureName.ExportButton, this);

            WDExport export = new WDExport(GetDriver(), By.ClassName("dhtmlx_window_active"));
            return new WDFramedWindow<WDExport>(GetDriver(), export, null, null);

        }
     
        [TxAction("CheckButtonAvailability", "Checks availability of a button deactivated or activated.")]
        public Boolean CheckButtonAvailability(string pictureName)
        {
            //TODO try to remove this sleep. Without sleep now code is not working . Bcz MainToolbar is loading slow
            Thread.Sleep(5000);
            bool  clicka= GetDriver().IsClickable(By.XPath(".//img[contains(@src,\"" + pictureName + "\")]"), this);
            return GetDriver().IsClickable(By.XPath(".//img[contains(@src,\""+pictureName+"\")]"), this);
        }

        [TxAction("IsEditButtonEnable", "Checks availability of Edit button deactivated or activated.")]
        public Boolean IsEditButtonEnable()
        {
            bool btnDisable = GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_dis\"]/img[contains(@src,\"24x24-wEditObject\")]"), this.GetElement());
            return !btnDisable;
        }


        /// <summary>
        /// 
        /// </summary>
        [TxAction("Extraction", "Open an extraction window.")]
        public WDWindow<WDExtraction> Extraction()
        {
            GetDriver().ClickAt(ElemByPictureName.ExtractButton, this);

            WDExtraction extractCreator = new WDExtraction(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"));
            return new WDWindow<WDExtraction>(GetDriver(), extractCreator);
        }

        [TxAction("ReturnEditFileWindow", "")]
        public WDWindow<EditFileForm> ReturnEditFileWindow()
        {
            WaitForAjax();
            EditFileForm editFile = new EditFileForm(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"));
            return new WDWindow<EditFileForm>(GetDriver(), editFile);
        }

        [TxAction("TitlePage", "Return the title of the page")]
        public string Titlepage()
        {
            return GetDriver().GetTitle();
        }

        [TxAction("ReturnExtraction", "Open an extraction window.")]
        public WDWindow<WDExtraction> ReturnExtraction()
        {
            GetDriver().WaitForAjax();
            WDExtraction extractCreator = new WDExtraction(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"));
            return new WDWindow<WDExtraction>(GetDriver(), extractCreator);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("LaunchModel", "Click on launch model icon.")]
        public void LaunchModel(string modelName)
        {
            //".//*[contains(@class, \"dhx_toolbar_arw\")]" 
            //GetDriver().ClickAt(launchModelButton, this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@class, \"dhx_toolbar_arw dhxtoolbar_btn_def\")]/div[@class =\"arwimg\"]"), this);
            Thread.Sleep(1000);
            GetDriver().ClickAt(new ByChained(By.XPath(".//div[contains(@class, \"dhx_toolbar_poly_material\")]"), By.XPath(".//*[@class = \"btn_sel_text\" and text() = \"" + modelName + "\"]")));
        }
        /// <summary>
        /// Return to the portal.
        /// </summary>
        [TxAction("Help", "Click on Help icon.")]
        public virtual void Help()
        {
            GetDriver().ClickAt(ElemByPictureName.HelpButton, this);
            GetDriver().SwitchNewWindow();
            GetDriver().Close();
            GetDriver().SwitchOldWindow();
            GetDriver().SwitchToContent();
        }

        /// <summary>
        /// Return to the portal.
        /// </summary>
        [TxAction("About", "Click on about icon.")]
        public void About()
        {
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"aboutTeexma\")]"));
            GetDriver().ClickAt(WDFramedWindow<WERefreshed>.closeButton);
        }

        /// <summary>
        /// Use the search box in teexma.
        /// </summary>
        /// <param name="research"></param>
        [TxAction("SearchBox", "Use the searchbox")]
        public WDWindow<WDSearchBox> SearchBox(string research, bool byClick = false)
        {
            WEGInput input = new WEGInput(GetDriver(), WDMainWindow.searchInput, this);

            input.Fill(research);
            if (byClick)
            {
                GetDriver().ClickAt(ElemByPictureName.SearchButton, this);
            }
            else
                input.Enter();

            GetDriver().WaitForAjax();
            if (research.Length == 1)
                new WESPopUp(GetDriver()).ClosePopup(true);

            WDSearchBox searchBoxCreator = new WDSearchBox(GetDriver(), By.XPath(".//div[@ida=\"dhxMainCont\"]"));
            return new WDWindow<WDSearchBox>(GetDriver(), searchBoxCreator);
        }

        /// <summary>
        /// Logs out the current user, confirms loging out and waits for the login page.
        /// </summary>
        [TxAction("Logout", "Clicks the logout button and confirms the popup.")]
        public virtual WESPopUp Logout()
        {
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
            GetDriver().ClickAt(WDMainWindow.disconnection);

            return new WESPopUp(GetDriver());
        }

        /// <summary>
        /// Logs out the current user, confirms loging out and waits for the login page.
        /// </summary>
        [TxAction("UserInformation", "Clicks the logout button and confirms the popup.")]
        public ActionColl<string> UserInformation()
        {
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
            ICollection<string> infoList = new List<string>();
            infoList.Add(GetDriver().WaitForElement(By.Id("userName")).GetElement().Text);
            infoList.Add(GetDriver().WaitForElement(By.Id("lastConnection")).GetElement().Text);

            return new ActionColl<string>(infoList);
        }

        
        /// <summary>
        /// Change the password.
        /// </summary>
        [TxAction("ChangePassword", "Clicks the useInfobutton and change the password.")]
        public void ChangePassword(string newPassword, string oldPassword, bool ok = true)
        {
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id,\"userChangePassword\")]"));

            new WEGInput(GetDriver(), By.XPath(".//*[starts-with(@id,\"idInputOldPswd\")]")).Fill(oldPassword);
            new WEGInput(GetDriver(), By.XPath(".//*[starts-with(@id,\"idInputNewPswd\")]")).Fill(newPassword);
            new WEGInput(GetDriver(), By.XPath(".//*[starts-with(@id,\"idInputNewPswdConfirm\")]")).Fill(newPassword);
            if (ok == true)
            {
                GetDriver().ClickAt(By.Id("idBtnValidate"));
                new WESPopUp(GetDriver()).ClosePopup(true);
            }
            else
                GetDriver().ClickAt(By.Id("idBtnCancel"));
        }
    
        /// <summary>
        /// 
        /// </summary>
        [TxAction("HideNavigationTree", "Hide the navigation tree on the left of MainWindow ")]
        public void HideNavigationTree()
        {
            //GetDriver().ClickAt(hideTreeButton, this);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"-wMainOptions.png\")]"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("ExpandNavigationTree", "Expand the navigation tree on the left of MainWindow ")]
        public void ExpandNavigationTree()
        {
            //GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxlayout_arrow dhxlayout_arrow_va\"]"), this);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"-wMainOptions.png\")]"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("ResizeObjectPanel", "Resize Object Panel To Move The Mouse ")]
        public void ResizeObjectPanel(int moveX)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"dhxlayout_sep dhxlayout_sep_resize_v\")]"), this);
            GetDriver().ClickAndHold(elem.GetElement());
            GetDriver().MouseMove(moveX, 0);
            GetDriver().Release();
        }

        /// <summary>
        /// </summary>
        /// 
        [TxAction("HideBanner", "Hide the Banner just below the main toolbar")]
        public void HideBanner()
        {
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
            GetDriver().ClickAt(By.Id("checkboxDisplayBanner"));
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("ExpandBanner", "Expand the Banner just below the main toolbar")]
        public void ExpandBanner()
        {
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
            GetDriver().ClickAt(By.Id("checkboxDisplayBanner"));
            GetDriver().ClickAt(ElemByPictureName.UserInformationButton, this);
        }

        [TxAction("ExpandeCollapseGroup", "Expand the Group just below the main toolbar")]
        public void ExpandGroup()
        {
            GetDriver().ClickAt(By.ClassName("cl_div_group_title"), this);
        }
        [TxAction("NavigateToIndicator", "Navigate To TxIndicator in different tab by button name.")]
        public GTTab<GTTxIndicator> NavigateToIndicator(int index)
        {
            try
            {
                GetDriver().ClickAt(By.XPath(".//div[contains(@class,'active')]//button[starts-with(@class,'buttondisplay')][" + index + "]"));
                GTTxIndicator indicator = new GTTxIndicator(GetDriver(), By.TagName("body"));
                return new GTTab<GTTxIndicator>(GetDriver(), indicator);
            }
            catch
            {
                GetDriver().ClickAt(By.XPath(".//div[contains(@class,'active')]//button[starts-with(@class,'buttondisplay')][" + index + "]"));
                GTTxIndicator indicator = new GTTxIndicator(GetDriver(), By.TagName("body"));
                return new GTTab<GTTxIndicator>(GetDriver(), indicator);
            }
        }
        [TxAction("NavigateToTableView", "Navigate To any table view in same tab by button name.")]
        public GTTxTableView NavigateToTableView(int index)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,'active')]//button[starts-with(@class,'buttondisplay')][" + index + "]"), this);
            //, By.XPath(".//iframe[contains(@url,\"/5.aspx\")]|.//iframe[contains(@url,\"/5_fr.aspx\")]|.//iframe[contains(@url,\"/3287.aspx\")]"));
            GetDriver().WaitForAjax();
            return new GTTxTableView(GetDriver(), By.XPath(".//div[starts-with(@class,\"widgetbody\")]"), this);
            //, true, By.XPath(".//iframe[contains(@url,\"/5.aspx\")]|.//iframe[contains(@url,\"/5_fr.aspx\")]|.//iframe[contains(@url,\"/3287.aspx\")]"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("CopyToClipBoard", "Copy to clipboard")]
        public void CopyToClipBoard()
        {
            GetDriver().ClickAt(By.Id("idImgCopyToClipboard"), this);
            new WEGInput(GetDriver(), By.Id("idTextQueryString")).Enter();
        }
        [TxAction("ChangePortalTab", "Change Portal Tab by index")]
        public void ChangePortalTab(int index)
        {
            WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//ul[starts-with(@class,'nav navbar-nav')]/li[starts-with(@class,'navli')][" + index + "]|.//div[@class=\"tab\"]/button[" + (index +1)+ "]"));
        }

        /// <summary>
        /// Switch to the business view.
        /// </summary>       
        [TxAction("CurveModule", "Click on curve module - TxCharts")]
        public GTTab<GTTxCharts> CurveModule()
        {
            GetDriver().ClickAt(ElemByPictureName.CurveModuleButton, this);
            GTTxCharts txCharts = new GTTxCharts(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxCharts>(GetDriver(), txCharts);
        }

        /// <summary>
        /// Open the navigation tree and use it.
        /// </summary>
        [TxAction("NavigationTree", "Open the navigation tree if it is closed and return a naviagation tree ")]
        public WESNavigationTree NavigationTree()
        {
            GetDriver().WaitForAjax();
            Console.WriteLine("Sleep added inside navigation for 430");
            Thread.Sleep(5000);
            bool selecOTComboBoxClickable = GetDriver().IsClickable(new ByChained(By.Id("idDivNavigationCombo")), this.GetElement());

            if (!selecOTComboBoxClickable)
            {
                GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"-wMainOptions.png\")]"), this);
                GetDriver().WaitForAjax();
            }
            return new WESNavigationTree(GetDriver(), By.Id("idDivNavigation"), this);

        }

        /// <summary>
        /// Return to the portal.
        /// </summary>    
        [TxAction("LinkPortal", "Click on a selected link inside the main window.")]
        public void LinkPortal(string link)
        {
            GetDriver().ClickAt(By.XPath(".//a[text() = \"" + link + "\"]"), this);
        }

        /// <summary>
        /// Return to the portal.
        /// </summary>    
        [TxAction("ButtonPortal", "Return null.")]
        public void ButtonPortal(string link,bool hasSpan=false)
        {
            if(hasSpan)
            GetDriver().ClickAt(By.XPath(".//span[text() = \"" + link + "\"]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//button[text() = \"" + link + "\"]|.//button[contains(text() ,\"" + link + "\")]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("WFPromote", "Open WF_Promote Windows")]
        public GTTxWorkFlow WFPromote()
        {
            GetDriver().ClickAt(ElemByPictureName.WfPromoteButton, this);
            GetDriver().WaitForAjax();
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[not(contains(@style,\"display: none\"))]/div[@class=\"dhx_popup_area\"]"));
            return WFPromoteCreator;
        }

        /// <summary>
        /// Return to the portal.
        /// </summary>    
        [TxAction("LinkWindowPortal", "For linking msc, export or choiceguide window.")]
        public WDLinkWindowFromPortal LinkWindowFromPortal(string link)
        {
            GetDriver().ClickAt(By.XPath(".//a[text() = \"" + link + "\"] | .//button[text() = \"" + link + "\"]"), this);
            return new WDLinkWindowFromPortal(GetDriver());
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("PopUp", "Manage PopUp in main window when happens.")]
        public WESPopUp PopUp()
        {
            return new WESPopUp(GetDriver());
        }

        [TxAction("TxMCS", "Open the TxMCS")]
        public WDWindow<GTTxMCS> TxMCS()
        {
            //  GetDriver().ClickAt(TxMCSButton, this);
            GTTxMCS TxMCS = new GTTxMCS(GetDriver(), WForm.WriteFormDiv);
            return new WDWindow<GTTxMCS>(GetDriver(), TxMCS);
        }

        //Click on Comment button
        [TxAction("TxCommunity", "Click on the next object icon.")]
        public GTTxCommunity TxCommunity()
        {
            if (!IsCommunityExpanded())
                GetDriver().ClickAt(By.XPath(".//*[@id =\"idDivMainToolbar\"]//img[contains(@src,\"24x24-wDisplayTxCommunity.png\")] | .//img[contains(@src,'24x24-wComment.png')]"));
            return new GTTxCommunity(GetDriver(), By.XPath(".//div[@id=\"IdDivTreeComment\"]"), this);
        }

        [TxAction("ExpandCollapseTxCommunity", "")]
        public void ExpandCollapseTxCommunity()
        {
            //if (GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhxlayout_arrow dhxlayout_arrow_vb\"]/../..//div[@class=\"dhx_cell_layout dhxlayout_collapsed_v\"]")))
            
                GetDriver().ClickAt(ElemByPictureName.OpenCollapseComment, this);
            
        }
       
        [TxAction("IsCommunityExpanded", "")]
        public bool IsCommunityExpanded()
        {
            return !GetDriver().IsElementPresent(By.XPath(".//*[@id=\"IdDivTreeComment\"]/ancestor:: div[contains(@class,\"dhx_cell_layout dhxlayout_collapsed_v\")]"),this.GetElement());
        }

        [TxAction("TxObject", "Click on the next object icon.")]
        public WDValidatedWindow<GTTxObject> TxObject()
        {
            GTTxObject txobject = new GTTxObject(GetDriver(), By.Id("mainLayoutModifyObj"), this);
            return new WDValidatedWindow<GTTxObject>(GetDriver(), txobject);
        }

        [TxAction("TxIndicator", " Click all Indicator link without Departments button")]
        public GTTab<GTTxIndicator> ClickIndicator(string indicatorname)
        {
            GetDriver().ClickAt(By.XPath(".//button/span[text()=\"" + indicatorname + "\"]|.//button[text()=\"" + indicatorname + "\"]"),this);
            GTTxIndicator txindicator = new GTTxIndicator(GetDriver(), By.TagName("html"));
            return new GTTab<GTTxIndicator>(GetDriver(), txindicator);
        }

        [TxAction("Departments", " Click on departments which will return to main window")]
        public void Departments()
        {
            GetDriver().ClickAt(By.XPath(".//button/span[text()=\"Departments\"]"),this);
        }

        

        [TxAction("TxWorkFlow", "flag can be Demote,Promote,Cancel")]
        public virtual GTTxWorkFlow TxWorkFlow(string flag)
        {
            //No need Thread
            Thread.Sleep(500);
            GetDriver().ClickAt(By.XPath(".//div[@title=\"" + flag + "\"]/img"), this);

            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_area\"]"));
            return WFPromoteCreator;
        }

        /// <summary>
        /// Function created for script 2_93.xml
        /// </summary>
        [TxAction("TxTableView", "Passes Teexma in write mode.")]
        public GTTxTableView TxTableView( string sTagTableview)
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//div[@stagtableview = \"" + sTagTableview + "\"]"), this, true);
        }

        [TxAction("Consult", "")]
        public GTTxTableView Consult()
        {
            GetDriver().ClickAt(By.XPath(".//span[contains(@multilingualism,\"SPAN_Consult\")]"), this);
            return new GTTxTableView(GetDriver(), By.Id("page"), this, true);
        }

        [TxAction("MyDashboard", "")]
        public GTTab<GTTxTableView> MyDashboard()
        {
            GetDriver().ClickAt(By.XPath(".//button/span[text()=\"My Dashboard\"]"), this);
            GTTxTableView tableView = new GTTxTableView(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxTableView>(GetDriver(), tableView);
        }

        [TxAction("Continue", "")]
        public void Continue()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvYesConfirmPanel\")]"), this);
        }

        [TxAction("TxHourTracking", "Open the TxHourTracking")]
        public WDValidatedWindow<GTTxHourTracking> TxHourTracking()
        {
            GetDriver().ClickAt(TxHourTrackingutton, this);
            GTTxHourTracking txHourTracking = new GTTxHourTracking(GetDriver(), By.ClassName("dhx_cell_wins"), this);
            return new WDValidatedWindow<GTTxHourTracking>(GetDriver(), txHourTracking);
        }

        [TxAction("ReturnTxHourTrackingInNewTab", "Passes Teexma to New Tab")]
        public GTTab<GTTxHourTracking> ReturnTxHourTrackingInNewTab()
        {
            GTTxHourTracking txHourTracking = new GTTxHourTracking(GetDriver(), By.TagName("html"));
            return new GTTab<GTTxHourTracking>(GetDriver(), txHourTracking);
        }

        [TxAction("ReturnTxHourTrackingInWindow", "Passes Teexma to New Window")]
        public WDValidatedWindow<GTTxHourTracking> ReturnTxHourTrackingInWindow()
        {
            WaitForAjax();
            GTTxHourTracking txHourTracking = new GTTxHourTracking(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_wins\")]"));
            return new WDValidatedWindow<GTTxHourTracking>(GetDriver(), txHourTracking);
        }
        [TxAction("ReturnChartInNewTab", "")]
        public GTTab<GTTxCharts> ReturnChartInNewTab()
        {
            GTTxCharts newcurve = new GTTxCharts(GetDriver(), By.Id("idDivManagerContainer"));
            return new GTTab<GTTxCharts>(GetDriver(), newcurve);
        }

        [TxAction("TimeExportationInWindow", "Passes Teexma to New Window")]
        public WDWindow<GTTxHourTracking> TimeExportationInWindow()
        {
            GTTxHourTracking txHourTracking = new GTTxHourTracking(GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_cell_wins\")]"));
            return new WDWindow<GTTxHourTracking>(GetDriver(), txHourTracking);
        }

        [TxAction("TimeExportationInNewTab", "Passes Teexma to New Tab")]
        public GTTab<GTTxHourTracking> TimeExportationInNewTab()
        {
            GTTxHourTracking txHourTracking = new GTTxHourTracking(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxHourTracking>(GetDriver(), txHourTracking);
        }

        [TxAction("AllA", "")]
        public GTTab<GTTxTableView> AllA()
        {
            GetDriver().ClickAt(By.XPath(".//span[text()=\"All A\"]"),this);
            GTTxTableView tableView = new GTTxTableView(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxTableView>(GetDriver(), tableView);
        }

        [TxAction("CloseBrowserTab", "Switch to the object type view.")]
        public void CloseBrowserTab()
        {
            //???
            GetDriver().SwitchNewWindow();
            GetDriver().Close();
            GetDriver().SwitchOldWindow();
            GetDriver().SwitchToContent();
        }

        [TxAction("ReturnTxTestResult", "Checks availability of a button.")]
        public WDValidatedWindow<GTTxTestResult> ReturnTxTestResult()
        {
            WaitForAjax();
            GTTxTestResult txTestResult = new GTTxTestResult(GetDriver(), By.TagName("html"));
            return new WDValidatedWindow<GTTxTestResult>(GetDriver(),txTestResult);
        }

        [TxAction("ReturnTxTestResultInNewTab", "Checks availability of a button.")]
        public GTTab<GTTxTestResult> ReturnTxTestResultInNewTab()
        {
            WaitForAjax();
            GTTxTestResult txTestResult = new GTTxTestResult(GetDriver(), By.XPath(".//html"));
            return new GTTab<GTTxTestResult>(GetDriver(), txTestResult);
        }

        [TxAction("ReturnTestResultEntityTest", "Entity test window")]
        public GTTxTestResultEntityTests ReturnTestResultEntityTest()
        {
            WaitForAjax();
            Sleep(2);
            GTTxTestResultEntityTests entityTest = new GTTxTestResultEntityTests(GetDriver(), By.XPath(".//div[contains(@id,\"idDivTRContainer\")]"));
            return entityTest;
        }

        [TxAction("ReturnEditForm", "Passes Teexma in write mode.")]
        public WDValidatedWindow<WForm> ReturnEditForm()
        {
            Thread.Sleep(2000);
            WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv, null);
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }
        [TxAction("ReturnAdministrationModelTables", "Administration of model tables")]
        public WDValidatedWindow<GTAdministrationModelTables> ReturnAdministrationModelTables()
        {
            GTAdministrationModelTables adminModelTable = new GTAdministrationModelTables(GetDriver(), By.XPath(".//div[contains(@id,\"divTrAdminTables\")]"), this);
            return new WDValidatedWindow<GTAdministrationModelTables>(GetDriver(), adminModelTable);
        }

        [TxAction("ReturnTxVisualDesign", "")]
        public GTTab<GTVisualDesign> TxVisualDesign()
        {
            GTVisualDesign txVisualDesign = new GTVisualDesign(GetDriver(), By.TagName("html"));
            return new GTTab<GTVisualDesign>(GetDriver(), txVisualDesign);
        }

        [TxAction("Edition", "Click on launch model icon.")]
        public void Edition(String modelName)
        {
            GetDriver().ClickAt(By.XPath(".//div[@title='Edition']"), this);
            GetDriver().ClickAt(By.XPath(".//div[text()='" + modelName + "']"));
        }

        [TxAction("IsViewObjectsDisabled", "View the checked Objects")]
        public bool IsViewObjectsDisabled()
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//img[contains(@src,'20x20-TreeView.png')]/.."), this);
            string className = elem.GetElement().GetAttribute("class");
            if (className.Contains("dhxtoolbar_btn_dis"))
                return false;
            else
                return true;
            //return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,'20x20-TreeViewDisabled.png')]"), this.GetElement());
        }

        [TxAction("ReadHeader", "Read The Headername of portal")]
        public string ReadHeader()
        {
            Thread.Sleep(3000);
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"clPortalTitle\"]//h1|.//div[@id=\"idDivPortal\"]//h1|.//div[@class=\"dhxwin_text_inside\"]"));
            return elem.GetElement().Text;
        }


        [TxAction("ReadPortalName", "")]
        public string ReadPortalName()
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class='clPortalTitle']"), this);
            return elem.GetElement().Text;
           
        }

        //[TxAction("ReadPortalScript", "")]
        //public string ReadPortalScript()
        //{
        //    WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//h3[@id='portalScriptTest']"), this);
        //    return elem.GetElement().Text;

        //}
        [TxAction("IsCssColourBlue", "")]
        public bool IsCssColourBlue()
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//td[@class='clPortalRowForTest clPotalLink']"), this);
            string aa= elem.GetElement().GetCssValue("color");
            if (aa == "rgba(0, 0, 255, 1)")
            {
                return true;
            }
            else
                return false;
 
            }

        [TxAction("ReturnTxTableView", "")]
        public GTTab<GTTxTableView> ReturnTxTableView()
        {
            GTTxTableView txtableview = new GTTxTableView(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxTableView>(GetDriver(), txtableview);
        }

        [TxAction("ReturnMultiCriteria", "")]
        public WDFramedWindow<WDMultiCriteria> ReturnMultiCriteria()
        {
            WDMultiCriteria multiCriteria = new WDMultiCriteria(GetDriver(), By.Id("idBodyParent"));
            return new WDFramedWindow<WDMultiCriteria>(GetDriver(), multiCriteria, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("ReturnChoiceGuide", "")]
        public WDFramedWindow<WDChoiceGuideSelection> ReturnChoiceGuide()
        {
            WDChoiceGuideSelection choiceGuideSelection = new WDChoiceGuideSelection(GetDriver(), By.Id("id_div_refresh_cg_list"));
            return new WDFramedWindow<WDChoiceGuideSelection>(GetDriver(), choiceGuideSelection, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }

        [TxAction("ReturnExport", "Export data")]
        public WDFramedWindow<WDExport> ReturnExport()
        {

            WDExport export = new WDExport(GetDriver(), By.ClassName("dhtmlx_window_active"));
            return new WDFramedWindow<WDExport>(GetDriver(), export, null, null);

        }

        [TxAction("ReturnTxAdvancedSearch", "Passes Teexma in table view.")]
        public GTTab<TxAdvancedSearch> ReturnTxAdvancedSearch()
        {
            TxAdvancedSearch txadvancedsearch = new TxAdvancedSearch(GetDriver(), By.TagName("body"));
            return new GTTab<TxAdvancedSearch>(GetDriver(), txadvancedsearch);
        }
      

        [TxAction("returnsTxAdvanceSearchInSameWindow", "")]
        public WDWindow<ManageAdvancedSearch> returnsTxAdvanceSearchInSameWindow()
        {
            ManageAdvancedSearch advencesearch = new ManageAdvancedSearch(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"][2]"));
            return new WDWindow<ManageAdvancedSearch>(GetDriver(), advencesearch);
        }
        [TxAction("IsButtonNamePresent", "")]
        public bool IsButtonNamePresent(string buttonName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_def\" and @title=\"" + buttonName + "\"] |.//div[@title=\"" + buttonName + "\"] |.//input[@title=\"" + buttonName + "\"]"));
        }
        [TxAction("ClickOnPortalTab", "")]
        public void ClickOnPortalTab(string tabName, string frame = "11296")
        {
            WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div [@class=\"collapse navbar-collapse\"]//li//a[text()=\"" + tabName + "\"]|.//div [@class=\"collapse navbar-collapse\"]//li//a[starts-with(text(),\"" + tabName + "\")]|.//div [@class=\"collapse navbar-collapse\"]//button[starts-with(text(),\"" + tabName + "\")]"), this);
           // Sleep(5);
            WaitForAjax();
        }
        [TxAction("ReadButtonList", "")]
        public ActionColl<string> ReadButtonList(string iframe = "5")
        {

            IWebElement parent = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"buttoncontainer\") and contains(@class,\"active\")]"), this).GetElement();
                //, By.XPath(".//iframe[contains(@url,\"/" + iframe + ".aspx\")]|.//iframe[contains(@url,\"/3287.aspx\")]")).GetElement();
            ICollection<IWebElement> buttonList = parent.FindElements(By.XPath(".//button[@design]"));
            List<string> buttonNames = new List<string>();
            foreach (IWebElement elem in buttonList)
                buttonNames.Add(elem.Text.Trim());
            return new ActionColl<string>(buttonNames);
        }
        [TxAction("NavigateToOT", "Navigate To any OT by button name.")]
        public void NavigateToOT(int index, string iframe = "5")
        {

            GetDriver().ClickAt(By.XPath(".//div[contains(@class,'active')]//button[starts-with(@class,'buttondisplay')][" + index + "]"), this);
            //, By.XPath(".//iframe[contains(@url,\"/" + iframe + ".aspx\")]|.//iframe[contains(@url,\"/3287.aspx\")]"));
        }
        [TxAction("ReturnTxCalendar", "")]
        public GTTab<GTTxCalendar> ReturnTxCalendar()
        {
            GTTxCalendar gttxcalender = new GTTxCalendar(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxCalendar>(GetDriver(), gttxcalender);

        }
        [TxAction("ReturnTxCalendarInWindow", "")]
        public WDValidatedWindow< GTTxCalendar> ReturnTxCalendarInWindow()
        {
            GTTxCalendar gttxcalender = new GTTxCalendar(GetDriver(), By.TagName("body"));
            return new WDValidatedWindow<GTTxCalendar>(GetDriver(),gttxcalender);

        }
        [TxAction("ClickOnButton", "")]
        public void ClickOnButton(string buttonName)
        {
            WaitForAjax();
            Sleep(5);
            GetDriver().ClickAt(By.XPath(".//button[text()=\"" + buttonName + "\"]"), this);
        }

        [TxAction("ReadNumberOfObjectInRL", "")]
        public string ReadNumberOfObjectInRL()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"clPortalCountObjectResultNum\"]"), this).GetElement().Text;
        }
        [TxAction("ReadSearchItem", "")]
        public ActionColl<String> ReadSearchItem()
        {
            ICollection<IWebElement> elems = GetDriver().WaitForElement(By.XPath(".//div[@class=\"clportalDivSearchResults\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"clPortalSearchResultRow\"]/div|.//div[@class=\"clPortalSearchNoResult\"]"));
            if(elems.Count==0)
                elems = GetDriver().WaitForElement(By.XPath(".//div[@class=\"clportalDivSearchResults\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"clPortalSearchNoResult\"]"));
            List<string> result = new List<string>();
            for (int i=0;i<elems.Count;i++)
            {
                result.Add(GetDriver().WaitForElement(By.XPath(".//div[@class=\"clPortalSearchResultRow\"]["+(i+1)+ "]|.//div[@class=\"clPortalSearchNoResult\"][" + (i + 1) + "]"), this).GetElement().Text);
            }
            return new ActionColl<string>(result) ;
        }
        [TxAction("Rechercher", "")]
        public void Rechercher(string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[@data-search-objects-ot-tag]"), this).Fill(value);
        }

        [TxAction("ReturnGanttInNewTab", "")]
        public GTTab<GTTxGantt> ReturnGanttInNewTab()
        {
            GTTxGantt gantt = new GTTxGantt(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxGantt>(GetDriver(), gantt);
        }

        [TxAction("IsPlaceHolderNameInSearchPresent", "")]
        public bool IsPlaceHolderNameInSearchPresent(string placeHolderName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_def\" and @title=\"" + placeHolderName + "\"]//input[@placeholder=\"" + placeHolderName + "\"]"));
        }

        [TxAction("ReadFormInNewTab", "Gets the handle for the form in read mode.")]
        public GTTab<RForm> ReadFormInNewTab()
        {
            //TODO Avoid thread.sleep
            Thread.Sleep(1000);
            RForm rfrom = new RForm(GetDriver(), By.Id("divObjectForm"), this);
            return new GTTab<RForm>(GetDriver(), rfrom);
        }
        [TxAction("TxTableViewValidatedWindow", "Passes Teexma in table view.")]
        public WDValidatedWindow<GTTxTableView> TxTableViewValidatedWindow()
        {

            GTTxTableView tableViewCreator = new GTTxTableView(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<GTTxTableView>(GetDriver(), tableViewCreator);
        }

        [TxAction("IsGraphOverlayPanelDisplayed", "")]
        public bool IsGraphOverlayPanelDisplayed()
        {
            GetDriver().WaitForAjax();
            IWebElement rightPanel = GetDriver().FindElement(By.XPath(".//div[@id=\"sIdDivPanelRightSide\"]/ancestor::div[starts-with(@class,\"dhx_cell_cont_layout\")][1]/.."));
            string value = rightPanel.GetAttribute("class");

            return !value.Contains("collapsed_v");
        }

        [TxAction("ShowHideOverlayCurves", "")]
        public void ShowHideOverlayCurves()
        {
            WaitForAjax();
            GetDriver().ClickAt(By.XPath("(.//img[contains(@src,\"-wTxCharts.png\")])[2]"), this);
        }

        [TxAction("ReturnGraphOverlayPanel", "")]
        public GTOverlayCurves ReturnGraphOverlayPanel()
        {
            WaitForAjax();
            return new GTOverlayCurves(GetDriver(), By.XPath(".//div[starts-with(@id,\"IdDivCharts\")]"));
        }

        [TxAction("ReturnSPCDashboardWindow", "")]
        public WDValidatedWindow<GTTxSpecificationControlLayout> ReturnSPCDashboardWindow()
        {
            GTTxSpecificationControlLayout txSpecification = new GTTxSpecificationControlLayout(GetDriver(), By.XPath(".//div[contains(@class,\"dhxwin_active\")]"));
            return new WDValidatedWindow<GTTxSpecificationControlLayout>(GetDriver(), txSpecification);
        }

        [TxAction("ReturnSPCInNewTab", "")]
        public GTTab<StatisticalProcessControlconfigurations> ReturnSPCInNewTab()
        {
            StatisticalProcessControlconfigurations config = new StatisticalProcessControlconfigurations(GetDriver(), By.ClassName("dhxwin_active"));
            return new GTTab<StatisticalProcessControlconfigurations>(GetDriver(), config);
        }

        [TxAction("ReturnWFPromote", "ReturnWFPromote")]
        public GTTxWorkFlow ReturnWFPromote()
        {
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_material\" and not(contains(@style,\"display: none\"))]//div[@class=\"dhx_popup_area\"]"));
            return WFPromoteCreator;
        }

        [TxAction("ReturnManageSample", "Return TxTableView")]
        public WDValidatedWindow<GTManageSample> ReturnManageSample()
        {
            WaitForAjax();
            GTManageSample manageSample = new GTManageSample(GetDriver(), By.XPath(".//div[contains(@class,\"dhx_cell_wins\")]"));
            return new WDValidatedWindow<GTManageSample>(GetDriver(), manageSample);
        }

        [TxAction("ReturnTestResultEntityTestNewTab", "Entity test window")]
        public GTTab<GTTxTestResultEntityTests> ReturnTestResultEntityTestNewTab()
        {
            WaitForAjax();
            GTTxTestResultEntityTests entityTest = new GTTxTestResultEntityTests(GetDriver(), By.XPath(".//div[contains(@id,\"idDivTRContainer\")]"));
            return new GTTab<GTTxTestResultEntityTests>(GetDriver(), entityTest);
        }

        [TxAction("ReturnTimeTransfer", "")]
        public WDValidatedWindow<TimeTransfer> ReturnTimeTransfer()
        {
            TimeTransfer ele = new TimeTransfer(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<TimeTransfer>(GetDriver(), ele);
        }

        [TxAction("ReturnTimeExportationInWindow", "TO Export Time Details")]
        public WDWindow<GTTimeExportation> ReturnTimeExportationInWindow()
        {
            //GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"24x24-wExportTime.png\")] | //img[contains(@src,\"24x24-ExportTime.png\")]"), this);
            GTTimeExportation timeExportation = new GTTimeExportation(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivTimeExportationLayout\")]"));
            return new WDWindow<GTTimeExportation>(GetDriver(), timeExportation);
        }

        [TxAction("ReturnLinkedObject", "")]
        public WDWindow< GTTxWFLinkedObject> ReturnLinkedObject()
        {
            GTTxWFLinkedObject linkedObject = new GTTxWFLinkedObject(GetDriver(), By.ClassName("dhtmlx_window_active"));
            return new WDWindow<GTTxWFLinkedObject>(GetDriver(), linkedObject);
        }
        [TxAction("ReturnIngredientWindow", "")]
        public WDValidatedWindow<IngredientWindow> ReturnIngredientWindow()
        {
            IngredientWindow win=  new IngredientWindow(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<IngredientWindow>(GetDriver(),win);
        }

        [TxAction("ReturnFormulationWindow", "Returns Formulation window")]
        public WDValidatedWindow<FormulationWindow> ReturnFormulationWindow()
        {
            FormulationWindow win = new FormulationWindow(GetDriver(), By.XPath(".//div[@class=\"dhx_cell_wins\"]"));
            return new WDValidatedWindow<FormulationWindow>(GetDriver(), win);
        }

        [TxAction("TableViewInCurrentTab", "Passes Teexma in write mode.")]
        public GTTxTableView TableViewInCurrentTab()
        {
            return new GTTxTableView(GetDriver(), By.XPath(".//body"), null, true, null, true);
        }

        [TxAction("FormulationActions", "****")]
        public GTTxWorkFlow FormulationActions(string modelName)
        {
            GetDriver().ClickAt(By.XPath(".//*[contains(@title , \"" + modelName + "\")]/img"));
            GetDriver().WaitForAjax();
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[not(contains(@style,\"display: none\"))]/div[@class=\"dhx_popup_area\"]"));
            return WFPromoteCreator;
        }
        [TxAction("ReturnTxPortal", "Returning TxPortal")]
        public Portal ReturnTxPortal()
        {
            return new Portal(GetDriver(),By.TagName("body"));
        }
        [TxAction("ReturnReadForminWindow", "Passes Teexma in Read mode in Window")]
        public WDValidatedWindow<RForm> ReturnReadForminWindow()
        {
            Thread.Sleep(2000);
            RForm formCreator = new RForm(GetDriver(), WForm.WriteFormDiv, null, true);
            return new WDValidatedWindow<RForm>(GetDriver(), formCreator);
        }
        [TxAction("ReturnTxIndicatorNewTab", " Click all Indicator link without Departments button")]
        public GTTab<GTTxIndicator> ReturnTxIndicatorNewTab()
        {
            GTTxIndicator txindicator = new GTTxIndicator(GetDriver(), By.TagName("html"));
            return new GTTab<GTTxIndicator>(GetDriver(), txindicator);
        }
    }
}