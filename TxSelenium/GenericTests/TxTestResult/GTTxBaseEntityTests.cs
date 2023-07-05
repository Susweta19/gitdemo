using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
   public class GTTxBaseEntityTests : WERefreshed
    {
        public GTTxBaseEntityTests(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {            
        }

        [TxAction("ReadErrorMessage", "Read String Value")]
        public string ReadErrorMessage()
        {
            return GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,'trDivButtonsCommentError')]")).GetElement().Text;
        }

        [TxAction("ModifyDateValue", "Modify Date Value")]
        public WDate ModifyDateValue(int rowIndex, int columnIndex)
        {
            String path = ".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]";
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(path), this).WaitForElement();
            elem.GetElement().Click();
            return new WDate(GetDriver(), By.XPath(".//div[contains(@class,\"dhtmlxcalendar_material dhtmlxcalendar_in_input\")]"));
        }

        [TxAction("DeleteTable", " Delete all the data from table")]
        public void DeleteTable()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@src,\"16x16-False.png\")]|//img[contains(@src,\"16x16-False.png\")]"), this);
        }
        /// <summary>
        /// First columnIndex=1;
        /// First rowIndex=1;
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// 
        [TxAction("ScrollColumn", "Modify Text Value")]
        public void ScrollColumn(int row,int column)
        {
            IWebElement ele = GetDriver().WaitForElement(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (row + 1) + "]/td[" + column + "]"), this).GetElement();
            GetDriver().ScrollHorizontal(ele);
        }
        [TxAction("ModifyStringValue", "Modify Text Value")]
        public WEGInput ModifyStringValue(int rowIndex,int columnIndex)
        {
            String path = ".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr["+(rowIndex+1) +"]/td["+ columnIndex + "]";
            GetDriver().ScrollToElement(GetDriver().FindElement(this.GetElement(),By.XPath(path)));
            GetDriver().ClickAt(By.XPath(path), this);
            return new WEGInput(GetDriver(), By.XPath(path + "//input"),this);
        }
        /// <summary>
        /// First columnIndex=1;
        /// First rowIndex=1;
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("ModifyEntitySelection", "Modify Link")]
        public WDValidatedWindow<WSingleLink> ModifyEntitySelection(int rowIndex, int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            WSingleLink singleLink = new WSingleLink(GetDriver(),By.XPath(".//div[@class=\"dhtmlx_wins_body_outer\"]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WSingleLink>(GetDriver(),singleLink);
        }
        /// <summary>
        /// First columnIndex=1;
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("ReadColumnInformation", "Modify Text Value")]
        public string ReadColumnInformation(int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[1]/table/tbody/tr[2]/td["+ columnIndex + "]//img"), this);
            Sleep(2);
            WERefreshed infoElem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"dhxpopup_node\")]/div[@class=\"contentPopupDescription\"]"));
            string result = infoElem.GetElement().Text.Replace("\r", "").Replace("\n", "");
            return result;
        }
        /// <summary>
        /// If there is total 10 Dropdown in a table. Leftmost will have index 1 and rightmost will have index 10.Column count will not work here.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        [TxAction("ChangeUnit", "Modify Unit")]
        public WEGDHtmlxComboBox ChangeUnit(int Index)
        {
            WERefreshed table = GetDriver().WaitForElement(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[@class=\"xhdr\"]"), this);
            GetDriver().ScrollHorizontal(table.GetElement(), By.XPath(".//table[@class='hdr']//tr[3]/td["+Index+"]"), this,false);
            if (!GetDriver().IsElementPresent(By.XPath(".//table[@class='hdr']//tr[3]/td[" + Index + "]//div[@id]//div[@class=\"dhxcombo_material\"]"), this.GetElement()))
            {
                GetDriver().ClickAt(By.XPath(".//table[@class='hdr']//tr[3]/td[" + Index + "]//div[@id]"), this);
                if(GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,'dhxcombo_option dhxcombo_option_selected')]/parent::div[not(contains(@style,'display: none'))]")))
                    GetDriver().ClickAt(By.XPath(".//table[@class='hdr']//tr[3]/td[" + Index + "]//div[@id]"), this);
            }
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//table[@class='hdr']//tr[3]/td[" + Index + "]//div[@id]"), this);
                //(".//div[@class=\"trAccItemContentArea\"]/div[1]//table[@class='hdr']//tr[3]/td["+ Index + "]"), this);

        }
        /// <summary>
        /// First columnIndex=1;
        /// First rowIndex=1;
        /// write nothing in value attribute in xml if you don't want to select anything
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        [TxAction("ModifyListingValue", "Mofdify Listing Value")]
        public void ModifyListingValue(int rowIndex,int columnIndex,String value)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            GetDriver().ClickAt(By.XPath(".//td[starts-with(@class,\"trChoiceListOption\") and @value=\"" + value+"\"]"));
        }

        [TxAction("ExpandCollapseTable", "Mofdify Listing Value")]
        public void ExpandCollapseTable()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"table_view.png\")]|.//img[contains(@src,\"TableView.png\")]"), this);
        }
        /// <summary>
        /// First columnIndex=1;
        /// First rowIndex=1;
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("ModifyCheckBoxValue", "Mofdify CheckBox Value")]
        public WEGCheckBox ModifyCheckBoxValue(int rowIndex, int columnIndex)
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr["+(rowIndex+1) +"]/td["+ columnIndex + "]/img"), this);
        }
        /// <summary>
        /// rowIndex=1 for first row
        /// </summary>
        /// <param name="rowIndex"></param>
        [TxAction("DuplicateLine", "Duplicate a line")]
        public void Duplicateline(int rowIndex)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[1]"), this);
            GetDriver().RightClick(elem);
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"SubLevelArea_Polygon\") and not(contains(@style,\"display: none\")) ]//td[@class=\"sub_item_text\"]//div[text()=\"Duplicate one row\"]"));
        }

        [TxAction("Deleteline","Delete the line")]
        public void Deleteline(int rowIndex)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[1]"), this);
            GetDriver().RightClick(elem);
            GetDriver().ClickAt(By.XPath(".//div[contains(@class,\"SubLevelArea_Polygon\") and not(contains(@style,\"display: none\")) ]//td[@class=\"sub_item_text\"]//div[text()=\"Delete the line\"]"));
        }

        /// <summary>
        /// First columnIndex=1;
        /// First rowIndex=1;
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("ReadStringValue", "Read String Value")]
        public string ReadStringValue(int rowIndex, int columnIndex)
        {
            
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this).WaitForElement();
            return elem.GetElement().Text;
        }

        /// <summary>
        /// First columnIndex=1;
        /// First rowIndex=1;
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("IsFieldEditable", "Read String Value")]
        public bool IsFieldEditable(int rowIndex, int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            return GetDriver().IsElementPresent(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]/input"), this.GetElement());
        }

        [TxAction("SpecificationList", "")]
        public WEGDHtmlxComboBox SpecificationList()
        {
            return new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[starts-with(@class,\"trBlockComboSpecification\")]"),this);
        }


        [TxAction("ReturnSpecification", "")]
        public WTable ReturnSpecification()
        {
            return new WTable(GetDriver(),By.XPath(".//div[starts-with(@class,\"trBlockSpecification\")]"),this);
        }

        [TxAction("IsRowColourRed", "Modify Text Value")]
        public bool IsRowColourRed(int rowIndex, int columnIndex)
        {
            string data = GetDriver().FindElement(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]")).GetAttribute("style");
            var dataList = data.Split(':');
            string rgbValue = dataList[1];
            dataList = rgbValue.Split('(')[1].Split(')');
            return dataList[0] == "246, 200, 200";
            //tr[not(contains(@style,\"displayd:none\"))]/td[not(contains(@style,\"displayd:none\"))]
        }
        [TxAction("Selectlink", "Edit Rule For Text")]
        public WDValidatedWindow<WMultipleLink> Selectlink(int rowIndex, int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            WMultipleLink singleLink = new WMultipleLink(GetDriver(), By.XPath(".//div[@class='dhxwin_active']//div[@class=' dhxlayout_base_material']"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), singleLink);

        }
        [TxAction("ModifyStringValueInDrowpdownList", "Modify Text Value")]
        public void ModifyStringValueInDrowpdownList(int rowIndex, int columnIndex, string option)
        {
            String path = ".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]";
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(path), this).WaitForElement();
            elem.GetElement().Click();
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"select_\")]//td[text()=\"" + option + "\"]"));
            ///return new WEGInput(GetDriver(), By.XPath(path + "/input"), this);
        }
        [TxAction("ReadSpecificationTableValue", "Read String Value")]
        public string ReadSpecificationTableValue(int rowIndex, int columnIndex)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@class,\"trBlockSpecification\")]//div[contains(@class,\"objbox\")]//tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this).WaitForElement();
            return elem.GetElement().Text;
        }
        [TxAction("ReadSpecificationTableTitle", "Read String Value")]
        public string ReadSpecificationTitle()
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@class,\"trBlockSpecification\")]//span[contains(@class,\"trBlockNameSpecification\")]"), this).WaitForElement();
            return elem.GetElement().Text;
        }
        [TxAction("ChangeSpecification", "Change Specification From Dropdown")]
        public WEGDHtmlxComboBox ChangeSpecification()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@class,\"trBlockSpecification\")]//div[contains(@class,\"dhxcombo_material\")]"), this);
        }
        [TxAction("ExpandColumn", "Modify Text Value")]
        public void ExpandColumn(int rowIndex, int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]//img[contains(@src,\"plus\")]"), this);
            // No need to scroll now since the button is fixed
            //String cell = ".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]";
            //GetDriver().ScrollToElement(GetDriver().FindElement(this.GetElement(), By.XPath(cell)));
            ////WERefreshed parrent = GetDriver().WaitForElement(By.XPath(cell), this);
            //GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"plus\")]"), GetDriver().WaitForElement(By.XPath(cell), this));
        }
        [TxAction("CollapseColumn", "Modify Text Value")]
        public void CollapseColumn(int rowIndex, int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]//img[contains(@src,\"minus\")]"), this);
            // No need to scroll now since the button is fixed
            //String cell = ".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]";
            //GetDriver().ScrollToElement(GetDriver().FindElement(this.GetElement(), By.XPath(cell)));
            ////WERefreshed parrent = GetDriver().WaitForElement(By.XPath(cell), this);
            //GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"minus\")]"), GetDriver().WaitForElement(By.XPath(cell), this));
        }
        [TxAction("ReadCellColor", "Read Cell Color")]
        public string ReadCellColor(int rowIndex, int columnIndex)
        {
            string data = GetDriver().FindElement(this.GetElement(),By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[not(contains(@style,\"displayd:none\"))][" + (rowIndex + 1) + "]/td[not(contains(@style,\"displayd:none\"))][" + columnIndex + "]")).GetAttribute("style");
            var dataList = data.Split(':');
            return (dataList[1].Split('(')[1].Split(')'))[0];            
        }
        [TxAction("ModifyFileValue", "")]
        public WDValidatedWindow<GTEditFile> ModifyFileValue(int rowIndex, int columnIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[not(contains(@style,\"displayd:none\"))][" + (rowIndex + 1) + "]/td[not(contains(@style,\"displayd:none\"))][" + columnIndex + "]"), this);
            GTEditFile editfile = new GTEditFile(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<GTEditFile>(GetDriver(), editfile);
        }
        [TxAction("isDuplicateLinePresent", "Duplicate a line")]
        public bool isDuplicateLinePresent(int rowIndex)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"trAccItemContentArea\"]//div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[1]"), this);
            GetDriver().RightClick(elem);
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"SubLevelArea_Polygon\") and not(contains(@style,\"display: none\")) ]//td[@class=\"sub_item_text\"]//div[text()=\"Duplicate one row\"]"));
        }
    }
}