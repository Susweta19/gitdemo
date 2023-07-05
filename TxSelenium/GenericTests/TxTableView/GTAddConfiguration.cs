using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTableView
{
    public class GTAddConfiguration : WERefreshed
    {
        public GTAddConfiguration(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ConfigName", "ConfigName")]
        public WEGInput ConfigName()
            
        {
            //WERefreshed ele=new WERefreshed(GetDriver(),By.XPath)
           
            //if (iframe == true)
            //{
            //    By ele = By.XPath(".//iframe[contains(@url,'34352.aspx')]");
            //    return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,'configName')]"), null,ele);
            //}


            return new WEGInput(GetDriver(),By.XPath(".//input[starts-with(@id,'configName')]"),this);

        }

        [TxAction("SelectObjectType", "SelectObjectType")]
        public WEGDHtmlxComboBox SelectObjectType()
        {
            return new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[starts-with(@id,'editConfigOTChoice')]"), this);
            
                //return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,'editConfigOTChoice')]"), this);
        }

        [TxAction("AttributesList", "AttributesList")]
        public WESTree AttributesList()
        {
            return new WESTree(GetDriver(), By.XPath(".//div[starts-with(@id,'editConfigTreeAttribute')]"), WESTree.expandByDefCriteriaTree, this, 1);
        }

        [TxAction("AddAttribute", "AddAttribute")]
        public void AddAttribute()
        {
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id,'editConfigButtonSelectCols')]"),this);
        }

        [TxAction("DeleteAttribute", "DeleteAttribute")]
        public void DeleteAttribute()
        {
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id,'editConfigButtonReturnCols')]"),this);
        }

        //[TxAction("ReadNotification", "ReadNotification")]
        //public string ReadNotification()
        //{
        //    return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'notifSave')]//div[contains(@class,'alert')]//span[starts-with(@class,'message')]"), this).GetElement().Text;
        //}

        [TxAction("ColumnGrid", "ColumnGrid")]
        public GTColumnGrid ColumnGrid()
        {
            return new GTColumnGrid(GetDriver(),By.XPath(".//div[starts-with(@id,'editConfigCGrid')]"),this);
        }

        [TxAction("IsConfigNameDisabled", "IsConfigNameDisabled")]
        public bool IsConfigNameDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//input[starts-with(@id,'configName') and @disabled]"), this.GetElement());
        }
        [TxAction("IsOTDropDownListDisplayed", " IsOTDropDownListDisplayed")]
        public bool IsOTDropDownListDisplayed()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,'otComboContainer') and contains(@style,'display:block;')]"), this.GetElement());
        }
        //[TxAction("OKCancel", "ValidateCancel")]
        //public void ValidateCancel(bool validate)
        //{
        //    By button = validate ? By.Id("idBtnValidate") : By.XPath(".//button[@id=\"idBtnCancel\"]|.//button[contains(@id,\"btnCancelPopup\")]"); //btnCancelPopup15621353076757865772503715851
        //    GetDriver().ClickAt(button,this);
        //}
        [TxAction("ClickOnCheckbox", "ValidateCancel")]
        public WEGCheckBox ClickOnCheckbox(string id)
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[@class='dhx_toolbar_btn dhxtoolbar_btn_def']//img[contains(@src,'/dhxgrid_material/item_chk" + id + ".gif')]"), this);
        }
        [TxAction("ReadCheckboxName", "ValidateCancel")]
        public string ReadCheckboxName(string id)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhxtoolbar\")]/div/input[contains(@id,\"checkboxSpeCol" + id + "\")]/.."), this).GetElement().GetAttribute("title");
        }
    }
}
