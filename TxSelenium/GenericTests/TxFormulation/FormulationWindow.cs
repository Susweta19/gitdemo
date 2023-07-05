using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxFormulation
{
    public class FormulationWindow : WERefreshed
    {
        public FormulationWindow(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("AddObject", "Clicks on the + button in Formulation window")]
        public WDValidatedWindow<IngredientWindow> AddObject()
        {
            GetDriver().ClickAt(ElemByPictureName.AddButton, this);
            IngredientWindow win = new IngredientWindow(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<IngredientWindow>(GetDriver(), win);
        }

        [TxAction("ReadCellValue", "Read the cell value for a particualr row")]
        public string ReadCellValue(int Row, int Column, bool footerTable=false)
        {
            if(!footerTable)
                return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"), this).GetElement().Text;
            else
                return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"idDivMain\")]//div[@class=\"ftr\"]//table//tr[" + (Row + 1) + "]//td[" + Column + "]"), this).GetElement().Text;

        }

        [TxAction("ReadCellHeader", "Read the cell header")]
        public string ReadCellHeader(int Column)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"hdr\")]//tbody//tr//td[" + Column + "]"), this).GetElement().Text;

        }
        /// <summary>
        /// Checks weather a cell is editable or not
        /// Returns true if editable
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        [TxAction("IsCellEditable", "Checks weather a cell is editable or not")]
        public bool IsCellEditable(int Row, int Column)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"), this);
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]")).GetElement().GetAttribute("class").Contains("editable");
        }

        [TxAction("DoubleClickOnCell", "Performs Double click operation on a particular cell")]
        public void DoubleClickOnCell(int Row, int Column)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"), this);
        }

        //[TxAction("SelectRawMaterials", "Select raw material from drop down list")]
        //public void SelectRawMaterials(string materialName, bool byClick=false)
        //{
        //    if(!byClick)
        //    {
        //        GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxcombolist_material\")]//div[text()=\""+ materialName + "\"]"), this);
        //    }
        //    else
        //    {
        //        GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxcombo_select_img\")]"), this);
        //        WERefreshed parrent = GetDriver().WaitForElement(By.XPath(".//div[(@class=\"dhxcombolist_material\") and not(contains(@style,\"display\"))]"));
        //        GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxcombo_option\")]//div[text()=\"" + materialName + "\"]"), parrent);
        //    }
        //}

        [TxAction("SelectRawMaterials", "Select raw material from drop down list")]
        public WEGDHtmlxComboBox SelectRawMaterials(int Row, int Column=1 , bool byClick = false, bool byDoubleClick = true)
        {
            WERefreshed Parrent = GetDriver().WaitForElement(By.XPath(".//table//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this);
            if(byDoubleClick)
                GetDriver().DoubleClickAt(By.XPath(".//table//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this);
            if (!byClick)
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxcombo_select_img\")]"), Parrent);
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//table//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this);
        }

         [TxAction("EditCellValue", "EditCellValue")]
         public WEGInput EditCellValue(int row, int col)
         {
             GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + row + "]//td[" + col + "]"), this);
             return new WEGInput(GetDriver(), By.XPath(".//tr[@class][" + row + "]/td[" + col + "]//input[contains(@class,'edit')]"), this);
         }


        [TxAction("ReturnTableview", "Select raw material from drop down list")]
        public GTTxTableView ReturnTableview() {

            return new GTTxTableView(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
        }

        [TxAction("AddRawMaterial", "Performs Double click operation on a particular cell")]
        public void AddRawMaterial(int Row=1, int Column=1)
        {
            WERefreshed ele=new WERefreshed(GetDriver(),By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"),this);
            GetDriver().RightClick(ele);
            GetDriver().ClickAt(By.XPath(".//tr[@title=\"Add Raw materials…\"]"));
        }

        [TxAction("ReturnIngredientWindow", "Performs Double click operation on a particular cell")]
        public WDValidatedWindow<IngredientWindow> ReturnIngredientWindow()
        {
            IngredientWindow win = new IngredientWindow(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<IngredientWindow>(GetDriver(), win);
        }
        [TxAction("RawMaterialsInput", "Input of raw material")]
        public WEGInput RawMaterialsInput(int Row, int Column = 1, bool byClick = false, bool byDoubleClick = true)
        {
            WERefreshed Parrent = GetDriver().WaitForElement(By.XPath(".//table//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this);
            if (byDoubleClick)
                GetDriver().DoubleClickAt(By.XPath(".//table//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this);
            if (!byClick)
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhxcombo_select_img\")]"), Parrent);
            return new WEGInput(GetDriver(), By.XPath(".//input[@class=\"dhxcombo_input\"]"), Parrent);
        }        

        [TxAction("Deletetheline", "Performs Double click operation on a particular cell")]
        public void Deletetheline(int Row = 1, int Column = 1)
        {
            WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"), this);
            GetDriver().RightClick(ele);
            GetDriver().ClickAt(By.XPath(".//tr[@title=\"Delete the line…\"]"));
        }

        [TxAction("Moveup", "Performs Double click operation on a particular cell")]
        public void Moveup(int Row = 1, int Column = 1)
        {
            WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"), this);
            GetDriver().RightClick(ele);
            GetDriver().ClickAt(By.XPath(".//tr[@title=\"Move up…\"]"));
        }

        [TxAction("Movedown", "Performs Double click operation on a particular cell")]
        public void Movedown(int Row = 1, int Column = 1)
        {
            WERefreshed ele = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"idDivFormulationGrid\") and contains(@class,\"gridbox\")]//div[contains(@class,\"objbox\")]//tbody//tr[contains(@class,\"_material\")][" + Row + "]//td[" + Column + "]"), this);
            GetDriver().RightClick(ele);
            GetDriver().ClickAt(By.XPath(".//tr[@title=\"Move down…\"]"));
        }

        [TxAction("ClickOnDeletethelineButton", "Performs Double click operation on a particular cell")]
        public void ClickOnDeletethelineButton()
        {
            
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivFormulationGridToolbar\")]//img[contains(@src,\"deleteObject.png\")]"));
        }

        [TxAction("ClickOnMoveupButton", "Performs Double click operation on a particular cell")]
        public void ClickOnMoveupButton()
        {
            
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivFormulationGridToolbar\")]//img[contains(@src,\"Up.png\")]"));
        }

        [TxAction("ClickOnMovedownButton", "Performs Double click operation on a particular cell")]
        public void ClickOnMovedownButton()
        {
            
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivFormulationGridToolbar\")]//div[@title=\"Move down\"]"));
        }
        [TxAction("ClickOnSortindescendingorderButton", "Performs Double click operation on a particular cell")]
        public void ClickOnSortindescendingorderButton()
        {

            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivFormulationGridToolbar\")]//div[@title=\"Sort in descending order\"]"));
        }
        [TxAction("IsCellTextBold", "Cell Test Is Bold")]
        public bool IsCellTextBold(int Row, int Column)
        {
            string data = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"clMainDivFormulation\")]//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this).GetElement().GetAttribute("style");
            string bold = data.Split("font-weight:")[1].Split(";")[0].Trim();
            return bold == "bold";
        }
        [TxAction("IsCellTextRed", "Cell Text Is Red")]
        public bool IsCellTextRed(int Row, int Column)
        {
            string data = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"clMainDivFormulation\")]//tr[contains(@class,\"material\")][" + Row + "]/td[" + Column + "]"), this).GetElement().GetAttribute("style");
            string rgbValue = data.Split("color:")[1];
            string value = rgbValue.Split('(')[1].Split(')')[0].Trim();
            return value == "163, 39, 39";
        }
        [TxAction("IsButtonDisable", "Check Button Disable or Not")]
        public bool IsButtonDisable(string title)
        {
            string Elementclass = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhx_toolbar_btn\") and @title=\"" + title + "\"]"), this).GetElement().GetAttribute("class");
            return Elementclass.Contains("btn_dis");
        }
    }
}
