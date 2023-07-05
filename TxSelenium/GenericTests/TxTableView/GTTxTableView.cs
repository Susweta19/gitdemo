using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.GenericTests.TxWorkFlow;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.Utils;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.Readable;

//namespace TxSelenium.GenericTests.TxTableView
namespace TxSelenium.GenericTests.TxTableView
{
    public class GTTxTableView : WERefreshed
    {
        public object WRefreshed { get; private set; }
        WERefreshed DataDiv;
        WERefreshed DataDivHeader;
        WERefreshed FullDataDiv;

        private By ExportOptionListPath = By.XPath(".//div[contains(@class,\"dhx_toolbar_poly_material\") and not(contains(@style,\"display: none\"))]");

        private static By GetColumn(int index)
        {
            return By.XPath(".//div[@class = \"xhdr\"]//tbody/tr[3]/td[not(contains(@style,\"display: none\"))][" + index + "]");
        }

        public static By GetLink(string link)
        {
            return By.XPath(".//div[@class = \"objbox\"]//a[text()= \"" + link + "\"]");
        }
        //The some xpath can be different if allowEdition is false
        private bool allowEdition;

        public GTTxTableView(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, bool allowEdition = true, By frameBy = null, bool widget = false)
            : base(driver, elemIdentifier, parent, frameBy)
        {
            Thread.Sleep(2000);
            this.allowEdition = allowEdition;
            driver.WaitForAjax();
        }

        private void InitializeElements()
        {
            if (FullDataDiv == null)
            {
                try
                {
                    FullDataDiv = GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[contains(@class,\" gridbox gridbox_material isModern\")]/following-sibling::div"), this);
                }
                catch (Exception)
                {
                    FullDataDiv = GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"tvDivGrid\") and contains(@class,\" gridbox gridbox_material isModern\")]"), this);
                }
                DataDiv = GetDriver().WaitForElement(By.XPath(".//div[@class=\"objbox\"]"), FullDataDiv);
                DataDivHeader = GetDriver().WaitForElement(By.XPath(".//div[@class=\"xhdr\"]"), FullDataDiv);
            }
        }

        [TxAction("IsTaskPresent", "Checks weather a task is present or not")]
        public bool IsTaskPresent(string taskName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//span[text()=\"" + taskName + "\"]"));
        }

        [TxAction("Continue", "Checks weather a task is present or not")]
        public void Continue()
        {
            //GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvYesConfirmPanel\")]"));
            
        }

        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string buttonName)
        {
            Sleep(2);
            return GetDriver().IsElementPresent(By.XPath(".//input[@title=\"" + buttonName + "\" and @disabled=\"disabled\"]"));
        }

        [TxAction("SortColumn2", "indexColumn start at 1")]
        public void SortColumn2(int indexColumn)
        {
            Thread.Sleep(1000);
            GetDriver().ClickAt(By.XPath(".//div[@class = \"xhdr\"]//tbody/tr[2]/td[" + (indexColumn + 1) + "]//div[text()]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("SwitchToPageNumber", "")]
        public void SwitchToPageNumber(string pageNumber)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"bigCombo\")]/img"), this);
            Sleep(1);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"bigComboOptions\")]//span[text()='" + pageNumber + "']"));
        }
        [TxAction("MasterCheckBox", ".........")]
        public WEGCheckBox MasterCheckBox()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivMasterCheckBox\")]/img"), this);
        }

        [TxAction("SelectLineByName", "...")]
        public void SelectLineByName(string name)
        {
            GetDriver().ClickAt(By.XPath(".//span[@class = \"cellLinkValue\" and text()=\"" + name + "\"]/ancestor::tr"), this);
        }

        [TxAction("OpenInNewTab", "Open In New Tab")]
        public GTTab<GTTxTableView> OpenInNewTab()
        {
            GetDriver().ClickAt(ElemByPictureName.YellowArrow, this);
            GTTxTableView tableview = new GTTxTableView(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxTableView>(GetDriver(), tableview);
        }

        [TxAction("ManagePreviewData", "")]
        public ManagePreviewData ManagePreviewData(int rowindex, int columnindex)
        {
            InitializeElements();
            WERefreshed entity = GetDriver().WaitForElement(By.XPath(".//tr[" + (rowindex + 1) + "]/td[not(contains(@style,\"display: none\"))][" + columnindex + "]"), DataDiv);
            //  (".//div[contains(@id,\"id_div_TxTableGrid\")]/div[2]/div[2]/table/tbody/tr[" + (rowindex + 1) + "]/td[" + (columnindex + 1) + "]"), this);
            int height = entity.GetElement().Size.Height;
            Thread.Sleep(1000);
            entity.GetElement().Click();
            Thread.Sleep(2000);
            GetDriver().MouseOver(entity);

            Thread.Sleep(2000);
            GetDriver().Release();
            Thread.Sleep(3000);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"16x16-Preview.png\") and not(contains(@style,\"display: none\"))]"));
            //(".//img[contains(@src,\"16x16-Preview.png\") and contains(@style,\"display: block\")]"));
            Thread.Sleep(2000);
            //  WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//iframe[contains(@url,'/34352.aspx')]"));
            return new ManagePreviewData(GetDriver(), By.XPath(" .//div[@class='dhx_popup_area']//table[@class='dhx_popup_table']//div[contains(@id,'mainContainerPopup')]"), null, this.FrameBy);

            //return new ManagePreviewData(GetDriver(),By.XPath(".//div[starts-with(@id,\"popupInterface\") and starts-with(@class,\"popupInterface\")]"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Save", "Save")]
        public void Save()
        {
            GetDriver().ClickAt(ElemByPictureName.SaveButtonTv, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        [TxAction("ActionModel", "Click on actions button")]
        public void Actions(int row, int column)
        {
            row++;
            column++;
            GetDriver().ClickAt(By.XPath("//*[@id=\"tvDivGrid\"]/div[2]/div[2]/table/tbody/tr[" + row + "]/td[" + column + "]//img"), this);
        }

        [TxAction("DiscardChange", "Save")]
        public void DiscardChange()
        {
            GetDriver().ClickAt(ElemByPictureName.Cancel, this);
        }

        [TxAction("CheckallObjects", " Click on the check all objects button")]
        public void CheckallObjects()
        {
            GetDriver().ClickAt(ElemByPictureName.CheckObjectsTv, this);
        }

        [TxAction("UnCheckallObjects", " Click on the uncheck all objects button")]
        public void UnCheckallObjects()
        {
            GetDriver().ClickAt(ElemByPictureName.UncheckObjectsTv, this);
        }

        [TxAction("ReadFilteredResult", "")]
        public string ReadFilteredResult()
        {
            Sleep(2);
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"btnSelectPage\")]/following-sibling::div[starts-with(@class,\"dhx_toolbar_text\")]"), this).GetElement().Text;
        }
        /// <summary>
        ///  Refresh ...
        /// </summary>              
        [TxAction("Refresh", "Refresh")]
        public void Refresh()
        {
            GetDriver().ClickAt(ElemByPictureName.Refresh, this);
            TimeToWaitForAjax(5);
        }

        /// <summary>
        /// Delete Filters...
        /// </summary>
        [TxAction("DeleteFilters", "Delete...")]
        public void DeleteFilters()
        {
            GetDriver().ClickAt(ElemByPictureName.DeleteFilters, this);
        }

        /// <summary>
        /// Export all entities...
        /// </summary>
        [TxAction("StandardExport", "Export...")]
        public WDWindow<WDExport> StandardExport()
        {
                GetDriver().ClickAt(ElemByPictureName.ExportTv, this);
                WERefreshed optionList = GetDriver().WaitForElement(ExportOptionListPath);
                GetDriver().ClickAt(By.XPath(".//div[text()=\"Export filtered Objects...\"]"), optionList);
                WDExport export = new WDExport(GetDriver(), By.XPath(".//div[starts-with(@class,'dhx_cell_cont_wins')]/div[contains(@class,'dhxlayout_base_material')]"), this);
                return new WDWindow<WDExport>(GetDriver(), export, this);
        }

        [TxAction("ExportCurrentView", "Export...")]
        public void ExportCurrentView()
        {
            GetDriver().ClickAt(ElemByPictureName.ExportTv, this);
            WERefreshed optionList = GetDriver().WaitForElement(ExportOptionListPath);
            GetDriver().ClickAt(By.XPath(".//div[text()=\"Export current view\"]"), optionList);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("SwitchOnOff", "switch on/off")]
        public void Swicth()
        {
            GetDriver().ClickAt(By.Id("onoffswitch_3"), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexColumn">start at 1</param>
        /// <param name="field"></param>
        [TxAction("FilterInput", "indexColumn start at 1")]
        public void FilterInput(int indexColumn, string field)
        {
            InitializeElements();
            WEGInput input = new WEGInput(GetDriver(), new ByChained(GetColumn(indexColumn), By.XPath(".//input[contains(@id,\"tvInputFilter\")]|.//input[contains(@id,\"id_text_filter\")]")), FullDataDiv);
            //(".//input[contains(@id,\"id_text_filter\")]")
            input.Fill(field);
            input.Enter();
            Sleep(1);
            WaitForAjax();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexColumn">start at 1</param>
        /// <param name="ascDesc"></param>
        [TxAction("SortColumn", "indexColumn start at 1")]
        public void SortColumn(int indexColumn, bool Ascending = true)
        {
            WaitForAjax();
            Thread.Sleep(1000);
            InitializeElements();
            //WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[starts-with(@id,\"tvDivGrid\")]"));
            //GetDriver().MoveToElement(By.XPath(".//tr[2]//td[not(contains(@style,\"display: none\"))][" + (indexColumn + 1) + "]"), DataDivHeader);
            GetDriver().ClickAt(By.XPath(".//tr[2]//td[not(contains(@style,\"display: none\"))][" + (indexColumn) + "]//*[text()]"), DataDivHeader);
            Sleep(2);
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexColumn">start at 1</param>
        /// <param name="typeFilter"></param>
        /// <param name="valueFilter"></param>
        /// <param name="ValidateOrCancel">Choose between Validate, Cancel</param>
        [TxAction("Filter", "deleteValidCancel can be Validate, Cancel")]
        public GTFilter Filter(int indexColumn)
        {
            Thread.Sleep(2000);
            InitializeElements();
            IWebElement ele = GetDriver().FindElement(By.XPath(".//div[@class = \"xhdr\"]//tbody/tr[3]/td[not(contains(@style,\"display: none\"))][" + indexColumn + "]"));
            GetDriver().ScrollToElement(ele);
            GetDriver().ClickAt(new ByChained(GetColumn(indexColumn), By.XPath(".//input[@title = \"Apply a filter\"]|.//img[contains(@src,\"-MCS.png\")]|.//input[contains(@src,\"-MCS.png\")]")), FullDataDiv);
            return new GTFilter(GetDriver(), By.ClassName("dhxnode"), this);
        }

        [TxAction("DeleteFilter", "indexColumn start at 1")]
        public void DeleteFilter(int indexColumn)
        {
            Thread.Sleep(2000);
            InitializeElements();
            GetDriver().ClickAt(new ByChained(GetColumn(indexColumn), By.XPath(".//input[@title = \"Apply a filter\"]|.//input[starts-with(@id ,\"tvBtnFilter\")]")), this);
            GetDriver().ClickAt(By.XPath(".//*[id=\"id_btn_delete\"]|.//input[starts-with(@id,\"tvPopupFilterBtnDelete\")]"), this);
            Sleep(1);
            WaitForAjax();
        }

        [TxAction("SelectLink", "...")]
        public void SelectLink(string link)
        {
            Thread.Sleep(1000);
            WERefreshed table = GetDriver().WaitForElement(By.ClassName("objbox"), this);
            GetDriver().ScrollToBottom(table.GetElement());

            WEGLink selectedLink = new WEGLink(GetDriver(), GetLink(link), this);
            Thread.Sleep(1000);
            selectedLink.Click();

        }

        [TxAction("SelectLine", "...")]
        public void SelectLine(int rowindex)
        {
            rowindex++;
            GetDriver().ClickAt(By.XPath(".//div[@class = \"objbox\"]//tr[" + rowindex + "]"), this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Row">start at 1</param>
        /// <param name="Column">start at 1</param>      
        [TxAction("Equipments", "Click on an equipment from Equipment column")]
        public void Equipments(int Row, int Column)
        {
            WEGLink selectedLink = new WEGLink(GetDriver(), By.XPath(".//*[contains(@id=\"id_div_TxTableGrid\")]/div[2]//tr[" + (Row + 1) + "]/td[" + Column + "]"), this);
            selectedLink.Click();
        }

        [TxAction("SaveSingleChanges", "...")]
        public void SaveSingle(int row)
        {
            WERefreshed save = GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[contains(@class,\" gridbox gridbox_material isModern\")]//div[@class=\"objbox\"]//tr[" + (row + 1) + "]/td[1]/img[contains(@src,\"Save.png\")]"), this);
            GetDriver().ClickAt(save);
        }

        [TxAction("CancelSingleChanges", "...")]
        public void CancelSingleChanges(int row)
        {
            GetDriver().ClickAt(By.XPath(".//tr[" + (row + 1) + "]/td[3]/a/img[contains(@src,\"16x16-Cancel.png\")]|.//tr[3]/td[2]/img[contains(@src,\"16x16-Cancel.png\")]"), this);
        }

        [TxAction("ClickOnObject", "...")]
        public void ClickOnObject(string ObjectName)
        {
            try
            {
                GetDriver().ClickAt(By.XPath(".//span[text()=\"" + ObjectName + "\"]"), this);
            }
            catch
            {
                GetDriver().ClickAt(By.XPath(".//span[text()=\"" + ObjectName + "\"]"));
            }
        }

        [TxAction("ChangeBooleanValue", "Use Yes No and Undefined in SetValue")]
        public void ChangeBooleanValue(int Row, int Column, string SetValue)
        {
            InitializeElements();
            string value;
            switch (SetValue)
            {
                case "Yes":
                    value = "-True.png";
                    break;
                case "No":
                    value = "-False.png";
                    break;
                case "Undefined":
                    value = "-About.png";
                    break;
                default:
                    throw new Exception("Input Value is not Valid");
            }

            int i = 0;
            while (i < 4 && !GetDriver().IsElementPresent(By.XPath(".//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]//img[contains(@src,\"16x16" + value + "\")]"), DataDiv.GetElement()))
            {
                try
                {
                    GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvDivGrid\")]//div[not(contains(@id,\"cgrid2\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[@class=\"not_m_line\" and not(contains(@style,\"display: none\"))][" + Column + "]//img"));
                }
                catch (Exception)
                {
                    GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvDivGrid\")]//div[not(contains(@id,\"cgrid2\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[@class=\"not_m_line\" and not(contains(@style,\"display: none\"))][" + Column + "]"));
                }
                i++;
            }

        }

        //if (i > 4)
        //    throw new Exception("boolean value doesn't exist.");


        [TxAction("DateAndTime", "Edit date and Time")]
        public WDate DateAndTime(int Row, int Column)
        {
            InitializeElements();
            By dateTimeDiv = By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[not(contains(@class,\" gridbox gridbox_material isModern\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]");
            //  int a = 0;
            Thread.Sleep(1000);
            By calendar = By.XPath(".//div[@class=\"dhtmlxcalendar_wn\"]");
            GetDriver().DoubleClickAt(dateTimeDiv, this);
            //  if (GetDriver().IsElementPresent(calendar, this.GetElement()))
            //  GetDriver().ClickAt(dateTimeDiv, this);
            return new WDate(GetDriver(), calendar, this);
        }

        [TxAction("WriteTodayOrNow", "Today or now only for TxTableviewbis")]
        public void WriteTodayOrNow(int Row, int Column, string value)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]"), this);
            By inputField = By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/input");
            GetDriver().DoubleClickAt(elem.ElemIdentifier, this);
            if (elem.GetElement().Text != null)
                new WEGInput(GetDriver(), inputField, this).Clear();
            new WEGInput(GetDriver(), inputField, this).Fill(value);
            new WEGInput(GetDriver(), inputField, this).Enter();
        }

        [TxAction("EditStringValue", "Edit String, Text, URL and Decimal1")]
        public WEGInput EditStringValue(int Row, int Column, bool byClick = true)
        {
            InitializeElements();
            By Path = By.XPath((".//input[starts-with(@id,\"inputCustom\")]"));
            //(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/ input | .//div[@class=\"dhx_textarea\"]/textarea|.//div[starts-with(@id,\"id_div_TxTableGrid\")]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/input");
            if (byClick)
            {
                Thread.Sleep(2000);

                try
                {
                    GetDriver().ScrollToElement(GetDriver().FindElement(DataDiv.GetElement(), By.XPath(".//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]")));
                    GetDriver().DoubleClickAt(By.XPath(".//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]"), DataDiv);
                    Thread.Sleep(2000);
                }
                catch
                {
                    GetDriver().DoubleClickAt(By.XPath(".//div[starts-with(@id,\"tvDivGrid\")]//div[not(contains(@id,\"cgrid2\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[" + Column + "]"), this);
                }
            }
            return new WEGInput(GetDriver(), Path, this);
        }

        [TxAction("EditTextValue", "Edit String, Text, URL and Decimal1")]
        public WDValidatedWindow<WRichText> EditTextValue(int Row, int Column, bool byClick = true)
        {
            InitializeElements();
            //  By Path = By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/ input | .//div[@class=\"dhx_textarea\"]/textarea");
            GetDriver().ScrollToElement(GetDriver().FindElement(DataDiv.GetElement(), By.XPath(".//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]")));
            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]"), DataDiv);
            WRichText richText = new WRichText(GetDriver(), By.XPath(".//div[@class='dhx_cell_cont_wins']"), this, null, By.XPath(".//iframe[starts-with(@id,'rta')]"));
            return new WDValidatedWindow<WRichText>(GetDriver(), richText);
        }
       


        [TxAction("EditTable", "EditTable")]
        public GTEditTable EditTable(string configName = "")
        {
            if (configName.Length > 0)
                GetDriver().ClickAt(By.XPath(".//div[@class=\"clDivTxToolbarName\" and text()=\"" + configName + "\"]/ancestor::div[@class=\"clDivTxToolbar\"][1]//img[contains(@src,'wEditTable.png')]"), this);
            else
                try
                {
                    GetDriver().ClickAt(By.XPath(".//div[@class=\"clDivTxToolbar\"][1]//img[contains(@src,'wEditTable.png')]"));
                }
                catch (Exception e)
                {
                    GetDriver().ClickAt(By.XPath(".//div[@class=\"clDivTxToolbar\"][2]//img[contains(@src,'wEditTable.png')]|.//span[starts-with(@id,\"portalImgAddTV\")]"));
                }
            return new GTEditTable(GetDriver(), By.XPath(".//div[starts-with(@id,'popupInterface') and starts-with(@class,'popupInterface')]"), this);
        }

        [TxAction("EditTableInWiget", "EditTable")]
        public GTEditTable EditTableInWiget()
        {

            GetDriver().ClickAt(By.XPath(".//img[contains(@src,'24x24-EditTable.png')]"), this);

            return new GTEditTable(GetDriver(), By.XPath(".//div[starts-with(@id,'popupInterface') and starts-with(@class,'popupInterface')]"), this);
        }

        [TxAction("EditDecimalvalue", "For Edit decimal value")]
        public void EditDecimalvalue(int Row, int Column, string value, bool byClick = true)
        {
            By Path = By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/ input | .//div[@class=\"dhx_textarea\"]/textarea");
            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]"), this);
            GetDriver().ClickAt(By.XPath(".//select[@class=\"dhx_combo_select\"]/option[text()=\"" + value + "\"]"));
        }

        [TxAction("EditDecimal2", "Edit Decimal2 in edit form")]
        public WDValidatedWindow<GTEditDecimal> EditDecimal2(int Row, int Column, bool byClick = true)
        {
            By Path = By.XPath(".//div[contains(@id,\"id_div_TxTableGrid\")]/div[2]/div[2]//tr[" + (Column + 1) + "]/td[not(contains(@style,\"display: none\"))][" + (Row + 1) + "]/input | .//div[@class=\"dhx_textarea\"]/textarea");

            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[not(contains(@class,\" gridbox gridbox_material isModern\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]"), this);

            GTEditDecimal editDecimal = new GTEditDecimal(GetDriver(), By.ClassName("dhtmlx_wins_body_outer"), this);
            return new WDValidatedWindow<GTEditDecimal>(GetDriver(), editDecimal);
        }

        [TxAction("EditSingleLink", "Edit SingleLink")]
        public WDValidatedWindow<WSingleLink> EditSingleLink(int Row, int Column, bool byClick = true)
        {
            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]|.//div[contains(@id,\"tvDivGrid\")]//div[not(contains(@class,\" gridbox gridbox_material isModern\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]"), this);

            WSingleLink singleLink = new WSingleLink(GetDriver(), By.XPath(".//div[@class=\"dhtmlx_wins_body_outer\"] |.//div[@class=\"dhx_cell_wins\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WSingleLink>(GetDriver(), singleLink);
        }

        [TxAction("EditMultipleLink", "Edit MultipleLink")]
        public WDValidatedWindow<WMultipleLink> EditMultipleLink(int Row, int Column, bool byClick = true)
        {
            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//div[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]|.//div[contains(@id,\"tvDivGrid\")]//div[not(contains(@class,\" gridbox gridbox_material isModern\"))]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + Column + "]"), this);

            WMultipleLink singleLink = new WMultipleLink(GetDriver(), By.XPath(".//div[@class=\"dhtmlx_wins_body_outer\"] |.//div[@class=\"dhx_cell_wins\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), singleLink);
        }



        [TxAction("FirstPage", "Clicks on FirstPage Button")]
        public void First()
        {
            GetDriver().ClickAt(ElemByPictureName.FirstPng, this);
            GetDriver().WaitForAjax();
        }

        [TxAction("PreviousPage", "Clicks on PreviousPage Button")]
        public void Previous()
        {
            GetDriver().ClickAt(ElemByPictureName.PrevPNG, this);
            GetDriver().WaitForAjax();
        }

        [TxAction("NextPage", "Clicks on NextPage Button")]
        public void Next()
        {
            GetDriver().ClickAt(ElemByPictureName.NextPng, this);
            GetDriver().WaitForAjax();
        }

        [TxAction("LastPage", "Clicks on LastPage Button")]
        public void Last()
        {
            GetDriver().ClickAt(ElemByPictureName.LastPng, this);
            GetDriver().WaitForAjax();
        }

        [TxAction("SelectPage", "Selects Page")]
        public void SelectPage(int pageNumber)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class = \"arwimg\"]"));
            GetDriver().ClickAt(By.XPath(".//div[text() = \"Page n°" + pageNumber + "\"]"), this);
            GetDriver().WaitForAjax();

        }
        /// <summary>
        ///  add a space before a bool value expected result
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        [TxAction("ReadValue", "ReadValue")]
        public string ReadValue(int Row, int Column, bool fixedCell = false)
        {
            //Column = 5;
            //(".//div[@class=\"objbox\"]/parent::div[not(contains(@class,\" gridbox gridbox_material isModern\"))]//tr[contains(@class,\"_material\")][1]//td[not(contains(@style,\"display: none\"))][4]")
            WERefreshed elem;
            try
            {
                if (fixedCell)
                {
                    elem = GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"id_div_TxTableGrid\")]/div[1]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]|.//div[starts-with(@id,\"tvDivGrid\")]//div[@class=\"objbox\"]//tr[" + (Row+1) + " / td["+(Column+1)+"]"), this);
                }
                else
                {
                    elem = GetDriver().WaitForElement(By.XPath(".//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\")) and not(contains(@id,\"Toolbar\"))]//div[@class='objbox']//table//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]"), this);
                    
                    //(".//div[contains(@id,\"id_div_TxTableGrid\")]/div[not(contains(@id,\"cgrid\"))]//div[@class='objbox']//table//tr[" + (Row + 1) + "]/td[not(contains(@style,\"display: none\"))][" + (Column) + "]"), this);
                }
            }
            catch (Exception)
            {
                // (".//div[contains(@id,\"DivArray\") and not(contains(@id,\"Toolbar\"))]//div[@class='objbox']//table//tr[3]/td[2]")
                Console.WriteLine("In ReadValue method for TxTableView. Reading Value...");
                elem = GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"id_div_TxTableGrid\") and not(contains(@id,\"Toolbar\"))]//div[@class='objbox']//table//tr[" + (Row + 1) + "]/td[" + Column + "] | .//div[contains(@id,\"DivArray\") and not(contains(@id,\"Toolbar\"))]//div[@class='objbox']//table//tr[" + (Row + 2) + "]/td[" + Column + "]"), this);
            }
            string a= elem.GetElement().Text;
            //a= "Everything is OK"
            return elem.GetElement().Text;
            //elem.GetElement().Text;
        }

        [TxAction("ModelAction", " Click on the model action button")]
        public void ModelAction(int modelNumber)
        {
            By path = ElemByPictureName.NoModelButton;
            ICollection<IWebElement> modelList = this.GetElement().FindElements(path);
            modelList.ElementAt(modelNumber).Click();
        }

        [TxAction("ReadHeaderName", "")]
        public void ReadHeaderName(string name, bool newTab = false)
        {
            WERefreshed elem;
            if (!newTab)
            {
                elem = GetDriver().WaitForElement(By.XPath(".//div[@id=\"accordion\"]/h1"), null, By.XPath(".//iframe[contains(@url,\"19356.asp\")]"));
                bool aa = elem.GetElement().Text.Contains(name);
                if (!elem.GetElement().Text.Contains(name))
                    throw new Exception("Header name does not match");
            }
            else
            {
                elem = GetDriver().WaitForElement(By.XPath(".//div[@id=\"id_div_tableName\"]"));
                if (!elem.GetElement().Text.Contains(name))
                    throw new Exception("Header name does not match");
            }
        }

        [TxAction("MulticriteriaSelectionOnOrOff", "switch on/off")]
        public void MulticriteriaSelectionOnOrOff()
        {
            GetDriver().ClickAt(By.Id("tvOnOffSwitch"), this);
        }

        [TxAction("ReadPageName", "")]
        public void ReadPageName(string name)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[6]/div"), this);
            string elementName = elem.GetElement().Text;
            if (!elementName.Contains(name))
                throw new Exception("Page not found");
        }

        [TxAction("ReadNumberOfPage", "")]
        public void ReadNumberOfPage(string name)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhx_toolbar_text\"][1]"), this);
            if (!elem.GetElement().Text.Contains(name))
                throw new Exception("Number of page does not match");
        }

        [TxAction("GoTo", "")]
        public ManagePreviewData GoTo(int rowindex, int columnindex)
        {
            WERefreshed entity = GetDriver().WaitForElement(By.XPath(".//*[@id=\"id_div_TxTableGrid\"]/div[2]/div[2]/table/tbody/tr[" + (rowindex + 1) + "]/td[" + (columnindex + 1) + "]"), this);
            int height = entity.GetElement().Size.Height;
            Thread.Sleep(1000);
            entity.GetElement().Click();
            Thread.Sleep(1000);
            GetDriver().MouseOver(entity);
            Thread.Sleep(1000);
            GetDriver().Release();
            Thread.Sleep(1000);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"16x16-Preview.png\") and contains(@style,\"display: block\")]"));
            return new ManagePreviewData(GetDriver(), By.XPath(".//div[starts-with(@id,\"popupInterface\") and starts-with(@class,\"popupInterface\")]"));
        }

        [TxAction("ClickOnBlock", "...")]
        public void ClickOnBlock(int rowindex, int columnindex, bool fixedColumn = true)
        {
            InitializeElements();
            if (fixedColumn)
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvDivGrid\")]//div[starts-with(@id,\"cgrid2\")]//div[@class=\"objbox\"]//tr[" + (rowindex + 1) + "]/td[" + columnindex + "]|.//div[starts-with(@id,\"tvDivGrid\")]//div[@class=\"objbox\"]//tr[" + (rowindex + 1) + "]/td[@class=\"not_m_line\" and not(contains(@style,\"display: none\"))][" + columnindex + "]"), this);
            else
            {
                try
                {
                    GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"tvDivGrid\")]//div[not(contains(@id,\"cgrid2\"))]//div[@class=\"objbox\"]//tr[" + (rowindex + 1) + "]/td[@class=\"not_m_line\" and not(contains(@style,\"display: none\"))][" + columnindex + "]"), this);
                }
                catch (Exception)
                {
                    GetDriver().ClickAt(By.XPath(".//div[@stagtableview]//div[2]/div[@class=\"objbox\"]//table//tr[" + (rowindex + 1) + "]/td[@class=\"not_m_line\" and not(contains(@style,\"display: none\"))][" + columnindex + "]"), this);
                }
            }
        }

        [TxAction("ClickOnLink", "...")]
        public void ClickOnLink(int rowindex, int columnindex)
        {
            InitializeElements();
            GetDriver().ClickAt(By.XPath(".//tr[" + (rowindex + 1) + "]/td[not(contains(@style,\"display: none\"))][" + columnindex + "]//span"), DataDiv);
        }

        [TxAction("ShrinkLink", " ")]
        public void ShrinkLink(int resize)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.ClassName("dhtmlx_wins_resizer_r"), this);
        }

        [TxAction("WorkflowAction", "...")]
        public void WorkflowAction(string wfActionName)
        {
            GetDriver().ClickAt(By.XPath(".//ul[starts-with(@id,\"listActionsWorkFlow\")]//div[@class=\"actionWorkflowName\" and text()=\"" + wfActionName + "\"]"));
        }

        [TxAction("SelectLines", "...")]
        public void SelectLines(int firstRowindex, int links)
        {
            GetDriver().PressKey(Keys.Control);
            for (int i = 0; i < links; i++)
            {
                firstRowindex++;
                GetDriver().ClickAt(By.XPath(".//div[@class = \"objbox\"]//tr[" + firstRowindex + "]"), this);
            }
        }

        [TxAction("ModelActionByTitle", " Click on the model action button")]
        public void ModelActionByTitle(string modelTitle)
        {
            GetDriver().ClickAt(By.XPath(".//div[@title =\"" + modelTitle + "\"]/img"), this);
        }

        [TxAction("NoEntity", "")]
        public void NoEntity()
        {
            bool elem = GetDriver().IsElementPresent(By.XPath(".//table/tr/td"));
            if (elem)
                throw new Exception("Entities should not be present ");
        }

        [TxAction("WorkFlowActions", "Click on actions button")]
        public GTTxWorkFlow WorkFlowActions(int index)
        {
            index++;
            GetDriver().ClickAt(By.XPath(".//tr[" + (index) + "]/td/a/img[contains(@src,\"wf.png\")]"), this); //TODO-PICTURE not found
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_area\"]//div[starts-with(@id,\"mainContainerPopup\")]"), this);
            return WFPromoteCreator;
        }

        [TxAction("WFPromoteActions", "Click on actions button")]
        public GTTxWorkFlow WFPromoteActions()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Promote.png\")]"), this);
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_area\"]//div[starts-with(@id,\"mainContainerPopup\")]"), this);
            return WFPromoteCreator;
        }

        [TxAction("UncheckAllEntities", "click on UncheckAllEntities")]
        public void UncheckAllEntities()
        {
            GetDriver().ClickAt(ElemByPictureName.UncheckObjectsTv, this);
        }

        [TxAction("CheckBox", ".........")]
        public WEGCheckBox CheckBox(int Row, int Column)
        {
            //(".//div[contains(@id,\"tvDivGrid\")]//div[contains(@class,\" gridbox gridbox_material isModern\")]//div[@class=\"objbox\"]//tr["+(Row+1)+"]/td["+Column+"]//img")
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[" + Column + "]//img"), this);
        }

        [TxAction("CheckBox2", "Not for TxTv DB")]
        public WEGCheckBox CheckBox2(int Row, int Column)
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column) + "]/img"), this);
        }

        [TxAction("ReadObjectName", "...")]
        public string ReadObjectName(int rowindex, int columnindex)
        {
            InitializeElements();
            return GetDriver().WaitForElement(By.XPath(".//tr[" + (rowindex + 1) + "]/td[@class=\"not_m_line\" and not(contains(@style,\"display: none\"))][" + columnindex + "]"), DataDiv).GetElement().Text;
        }

        [TxAction("EditFile", "Edit Decimal2 in edit form")]
        public WDValidatedWindow<GTEditFile> EditFile(int Row, int Column, bool byClick = true)
        {
            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[not(contains(@class,\" gridbox gridbox_material isModern\"))]//div[@class=\"objbox\"]//tr["+(Row+1)+"]/td[starts-with(@class,\"not_m_line\") and not(contains(@style,\"display: none\"))]["+Column+"]"), this);
            Thread.Sleep(1000);
            GTEditFile editDecimal = new GTEditFile(GetDriver(), By.XPath(".//div[@class='dhx_cell_cont_wins']"), this, this.FrameBy);
            return new WDValidatedWindow<GTEditFile>(GetDriver(), editDecimal);
        }
        [TxAction("ReadColumName", "Edit String, Text, URL and Decimal1")]
        public ActionColl<string> ReadColumName()
        {
            //ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//table[@class='hdr']/ancestor::div[@id=\"tvDivGrid\"]")).GetElement().FindElements(By.XPath(".//div[@class=\"headerCellTableView\"]|.//div[@class=\"hdrcell\"]//div[@class=\"clTvHeaderCell\"]"));
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//table[@class='hdr']"), this).GetElement().FindElements(By.XPath(".//div[@class=\"headerCellTableView\"]|.//div[@class=\"clTvHeaderCell\"]"));
            ICollection<string> configName = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
            {
                configName.Add(element.ElementAt(i).Text);
            }
            return new ActionColl<string>(configName);
        }
        [TxAction("IsEditwindowPresene", "Edit String, Text, URL and Decimal1")]
        public bool IsEditwindowPresene()
        {
            return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,\"EditTable.png\")]")); ;
        }
        [TxAction("ReturnTxWorkFlow", "Click on actions button")]
        public GTTxWorkFlow ReturnTxWorkFlow()
        {
            GTTxWorkFlow WFPromoteCreator = new GTTxWorkFlow(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_area\"]//div[starts-with(@id,\"mainContainerPopup\")]"), this);
            return WFPromoteCreator;
        }

        [TxAction("ReadCellValueInValidatedWidow", "ReadValue")]
        public string ReadCellValueInValidatedWidow(int Row, int Column)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"spcManagerResultGrid\")]//div[@class='objbox']//table//tr[" + (Row + 1) + "]/td[" + Column + "]|.//div[contains(@id,\"tvDivGrid\")]//div[@class='objbox']//table//tr[" + (Row + 1) + "]/td[" + Column + "]"), this).GetElement().Text;

        }

        [TxAction("ReadrowNumber", "ReadValue")]
        public int ReadrowNumber()
        {
            int row = GetDriver().FindElement(By.XPath(".//div[contains(@id,\"spcManagerResultGrid\")]")).FindElements(By.XPath(".//div[@class=\"objbox\"]//tr")).Count;
            return (row - 1);
        }
        [TxAction("EditStringValue2", "Edit String, Text, URL and Decimal1")]
        public WEGInput EditStringValue2(int Row, int Column, bool byClick = true)
        {
            string elempath = ".//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]";
            GetDriver().ScrollToElement(this.GetElement().FindElement(By.XPath(elempath)));
            Sleep(2);
            By Path = By.XPath(".//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/ input | .//div[@class=\"dhx_textarea\"]/textarea");
            if (byClick)
                GetDriver().DoubleClickAt(By.XPath(".//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]|.//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/ input | .//div[@class=\"dhx_textarea\"]/textarea"), this);
            return new WEGInput(GetDriver(), Path, this);
        }
        [TxAction("EditListing", "*****")]
        public WEGSelect EditListing(int Row, int Column)
        {
            try
            {
                GetDriver().DoubleClickAt(By.XPath(".//*[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]"));
            }
            catch (Exception)
            {
                GetDriver().DoubleClickAt(By.XPath(".//*[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]"));
            }
            return new WEGSelect(GetDriver(), (By.XPath(".//select[@class=\"dhx_combo_select\"]")));
        }
        [TxAction("ClickOnCell", "")]
        public void ClickOnCell(int Row, int Column)
        {
            try
            {
                GetDriver().ClickAt(By.XPath(".//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]|.//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]/div[2]/div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]/ input | .//div[@class=\"dhx_textarea\"]/textarea"), this);
            }
            catch (Exception)
            {
                GetDriver().ClickAt(By.XPath(".//div[(contains(@id,\"id_div_TxTableGrid\") or starts-with(@id,\"tvDivGrid\"))]//div[2]//tr[" + (Row + 1) + "]/td[" + (Column + 1) + "]"), this);
            }
        }

        [TxAction("SwitchToLinearGridView", "Switch between linear and grid view")]
        public void SwitchToLinearGridView(bool linear = true)
        {
            if (linear)
                GetDriver().ClickAt(By.XPath(".//span[@title=\"Switch to a linear view\"]|.//span[@title=\"Passer à une vue linéaire\"]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//span[@title=\"Switch to a grid view\"]|.//span[@title=\"Passer à une vue en grille\"]"), this);
        }

        [TxAction("DeleteTableView", "Delete the table view")]
        public void DeleteTableView()
        {
            GetDriver().ClickAt(By.XPath(".//span[starts-with(@id,\"portalTVImgDelete\")]"), this);
        }

        [TxAction("SelectAnotherTableView", "SelectAnotherTableView")]
        public GTEditTable SelectAnotherTableView()
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"clPortalTVHeader\"]/img[contains(@src,'wEditTable.png')]"));
            return new GTEditTable(GetDriver(), By.XPath(".//div[starts-with(@id,'popupInterface') and starts-with(@class,'popupInterface')]"), this);
        }
        [TxAction("LaunchModel", " Click on the model action button")]
        public void LaunchModel(int Row, int Column)
        {
            GetDriver().ClickAt(By.XPath($".//div[@class=\"objbox\"]//tr[contains(@class,\"ev_material\")][{Row}]//td[{Column}]/img"), this);
        }
    }
}