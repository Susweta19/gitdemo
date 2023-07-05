using OpenQA.Selenium;

namespace TxSelenium.Utils
{
    public class ElemByPictureName
    {
        //Main ToolBar Button
        public static readonly By TableViewButton = By.XPath(".//img[contains(@src,\"24x24-TableView.png\")]");
        public static readonly By WriteModeButton = By.XPath(".//img[contains(@src,\"24x24-wEditObject.png\")] | .//img[contains(@src,\"24x24-EditObject.png\")]");
        public static readonly By PortalButton = By.XPath(".//img[contains(@src,\"logo.png\")]");
        public static readonly By PortalDisableButton = By.XPath(".//img[contains(@src,\"24x24-HomeDisabled.png\")]");
        public static readonly By MscButton = By.XPath(".//img[contains(@src,\"24x24-wMCS.png\")] | .//img[contains(@src,\"24x24-MCS.png\")]");
        public static readonly By ChoiceGuideButton = By.XPath(".//img[contains(@src,\"24x24-wChoiceGuide.png\")] | .//img[contains(@src,\"24x24-ChoiceGuide.png\")]");
        public static readonly By OpenRequirementSetButton = By.XPath(".//img[contains(@src,\"24x24-wRequirementList.png\")] | .//img[contains(@src,\"24x24-RequirementList.png\")]");
        public static readonly By LaunchModelButton = By.XPath(".//img[contains(@src,\"32x32-Models.png\")]");
        public static readonly By CurveModuleButton = By.XPath(".//img[contains(@src,\"24x24-TxCharts.png\")] | .//img[contains(@src,\"24x24-wTxCharts.png\")]");
        public static readonly By ExportButton = By.XPath(".//img[contains(@src,\"24x24-wExportation.png\")] | .//img[contains(@src,\"24x24-Exportation.png\")]");
        public static readonly By ExtractButton = By.XPath(".//img[contains(@src,\"24x24-wExtraction\")] | .//img[contains(@src,\"24x24-Extraction\")]");
        public static readonly By HelpButton = By.XPath(".//img[contains(@src,\"24x24-Help.png\")]");
        public static readonly By AboutButton = By.XPath(".//img[contains(@src,\"24x24-wAbout.png\")]");
        public static readonly By OpenCollapseComment = By.XPath(".//img[contains(@src,'24x24-wDisplayTxCommunity.png')] | .//img[contains(@src,'24x24-wComment.png')]");
        public static readonly By SearchButton = By.XPath(".//img[contains(@src,\"24x24-wSearch.png\")]");
        public static readonly By UserInformationButton = By.XPath(".//img[contains(@src,\"24x24-User.png\")]|.//img[contains(@src,\"24x24-wUser.png\")]");
        public static readonly By WfPromoteButton = By.XPath(".//img[contains(@src, \"Flow Chart-48.png\")]");
        
        //Tree
        public static readonly By SelectAll = By.XPath(".//img[contains(@src,\"/check.png\")] | .//img[contains(@src,\"20x20-CheckObject.png\")]");
        public static readonly By UnCheckChildObject = By.XPath(".//img[contains(@src,\"20x20-UncheckChildObject.png\")]");
        public static readonly By UnCheckAll = By.XPath(".//img[contains(@src,\"20x20-UncheckObject.png\")]");
        public static readonly By FullScreen = By.XPath(".//img[contains(@src,\"20x20-Fullscreen.png\")]");
        public static readonly By SelectedObjects = By.XPath(".//img[contains(@src,\"20x20-TreeView.png\")]");
        public static readonly By EntireListObjects = By.XPath(".//div[contains(@id,\"idDivToolbarObjects\")]//img[contains(@src,\"20x20-LinearView.png\")]|.//div[@title=\"Display the entire tree\" and not(contains(@style,\"display: none;\"))]//img[contains(@src,\"LinearView.png\")]|.//div[@class=\"treeBottom\"]//img[contains(@src,\"LinearView.png\")]|.//img[contains(@src,\"LinearView.png\")]");
        public static readonly By SelectObjectandchildren = By.XPath(".//img[contains(@src,\"20x20-CheckChildObject.png\")]");
        public static readonly By DeselectObjectandchildren = By.XPath(".//img[contains(@src,\"20x20-UncheckChildObject.png\")]");
        public static readonly By Reset = By.XPath(".//img[contains(@src,\"24x24-Refresh.png\")]");
        public static readonly By Minimize = By.XPath(".//div[contains(@id,\"idDivBtnBar\")]//div[contains(@style,\"display: inline;\")]//img[contains(@src,\"Minimize.png\")]");

        //TxTableView
        public static readonly By YellowArrow = By.XPath(".//img[contains(@src,\"18x18-OpenInNewTab.png\")]");
        public static readonly By Cancel = By.XPath(".//img[contains(@src,\"Cancel.png\")]");
        public static readonly By CheckObjectsTv  = By.XPath(".//img[contains(@src,\"24x24-CheckObjects.png\")]|.//img[contains(@src,\"24x24-CheckObject.png\")]|.//img[contains(@src,\"24x24-wCheckObject.png\")]");
        public static readonly By UncheckObjectsTv = By.XPath(".//img[contains(@src,\"24x24-UncheckObjects.png\")]|.//img[contains(@src,\"24x24-UncheckObject.png\")]|.//img[contains(@src,'wUncheckObject.png')]");
        public static readonly By Refresh = By.XPath(".//img[contains(@src,\"24x24-wRefresh.png\")]|.//img[contains(@src,\"24x24-Refresh.png\")]|.//img[@class and contains(@src,\"56x56-bRefresh.png\")]");
        public static readonly By DeleteFilters = By.XPath(".//img[contains(@src,\"24x24-RemoveFilters.png\")]");
        public static readonly By ExportTv = By.XPath(".//img[contains(@src,\"24x24-Exportation.png\")]|.//img[contains(@src,\"24x24-wExportation.png\")]");
        public static readonly By NoModelButton = By.XPath(".//img[contains(@src,\"24x24_No_Model\")]");
        public static readonly By SaveButtonTv = By.XPath(".//img[contains(@src,\"24x24-wSave.png\")]|.//img[contains(@src,\"24x24-Save.png\")]")
            ;
        public static readonly By FirstPng = By.XPath(".//img[contains(@src , \"First.png\")]");
        public static readonly By PrevPNG = By.XPath(".//img[contains(@src , \"Previous.png\")]");
        public static readonly By NextPng = By.XPath(".//img[contains(@src , \"Next.png\")]");
        public static readonly By LastPng = By.XPath(".//img[contains(@src , \"Last.png\")]");

        //Table
        public static readonly By ImportButton =  By.XPath(".//img[contains(@src, \"20x20-Importation.png\")]");
        public static readonly By DeleteData = By.XPath(".//img[contains(@src , \"20x20-False.png\")]");
        
        //AssociativeClass
        public static readonly By AddButton = By.XPath(".//img[contains(@src , \"20x20-AddObject.png\")]|.//img[contains(@src , \"20x20-addObject.png\")]");
        public static readonly By EditButton = By.XPath(".//img[contains(@src , \"20x20-Structure.png\")]");

        //TxCharts
        public static readonly By AddObject = By.XPath(".//img[contains(@src, \"-AddObject.png\")]");
        public static readonly By NewButton = By.XPath(".//img[contains(@src,\"wNew.png\")]");
        public static readonly By ExportBisButton = By.XPath(".//img[contains(@src,\"Exportation.png\")]");
        public static readonly By ModifyObject = By.XPath(".//img[contains(@src,\"ModifyObject.png\")]");
        public static readonly By Structure =   By.XPath(".//img[contains(@src , \"24x24-wStructure.png\")]");
        public static readonly By DeleteObject = By.XPath(".//img[contains(@src , \"DeleteObject.png\")]");
        public static readonly By Unit = By.XPath(".//img[contains(@src , \"Units.png\")]");
        public static readonly By DataMining = By.XPath(".//img[contains(@src , \"wDataMining.png\")]");

        //TxMCS
        public static readonly By Calenddar = By.XPath(".//img[contains(@src ,\"Calendar.png\" )]");
        public static readonly By ExportTxMcs = By.XPath(".//img[contains(@src ,\"48x48-Exportation.png\")]");

        //TxCommunity
        public static readonly By PictureComment = By.XPath(".//img[contains(@src, \"32x32-Message0.png\")] | .//img[contains(@src, \"32x32-CommentExpanded.png\")] | .//div[contains(@style, \"32x32-Message0.png\")]");
       
        //TxIndicator
        public static readonly By SaveButton = By.XPath(".//img[contains(@src,\"Save.png\")]");

        //TxTestResult
        public static readonly By FalseTxTr = By.XPath(".//img[contains(@src,\"16x16-False.png\")]");

        //WForm
        public static readonly By SendMail =  By.XPath(".//img[contains(@src,\"16x16-SendEmail.png\")]");

        //RForm
        public static readonly By ExportRf = By.XPath(".//img[contains(@src , \"20x20-Exportation.png\")]");

        //WEGLink
        public static readonly By SendMailLink = By.XPath("./../..//img[contains(@src , \"16x16-SendEmail.png\")]");
        
        //NavigationTree
        public static readonly By SwitchReadOrWrite = By.XPath(".//img[contains(@src , \"20x20-EditObject.png\")]");
        public static readonly By AddObjectButton = By.XPath(".//img[contains(@src , \"20x20-AddObject.png\")]");
        public static readonly By AddChildObjectButton = By.XPath(".//img[contains(@src , \"20x20-AddChildObject.png\")]");
        public static readonly By DeleteObjectButton = By.XPath(".//img[contains(@src , \"20x20-DeleteObject.png\")]");
        public static readonly By DownButton = By.XPath(".//img[contains(@src , \"20x20-Down.png\")]");
        public static readonly By UpButton = By.XPath(".//img[contains(@src , \"20x20-Up.png\")]");
        public static readonly By AddLinkObjectButton = By.XPath(".//img[contains(@src , \"20x20-AddLink.png\")]");
        public static readonly By DeleteLinkObjectButton = By.XPath(".//img[contains(@src , \"20x20-DeleteLink.png\")]");

        //TODO search "contains(@src" pattern in the entire solution to get last elements to add in this class...


    }
}
