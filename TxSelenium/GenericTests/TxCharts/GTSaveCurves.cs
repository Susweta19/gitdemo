using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTSaveCurves : WERefreshed
    {
        public GTSaveCurves(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("ObjectType", "Select the type of object")]
        public WEGDHtmlxComboBox ObjectType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[@id='idChartSaverObjectType']|.//div[@id='idDivComboOTSave']|.//div[starts-with(@id,\"idChartSaverObjectType\")]"), this);
        }

        [TxAction("Attributes", "Select the type of Attributes")]
        public WEGDHtmlxComboBox Attributes()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[@id='idChartSaverAttribute']|.//div[@id='idDivComboAttSave']|.//div[starts-with(@id,\"idChartSaverAttribute\")]"), this);
        }

        [TxAction("Objects", "Select the Object")]
        public WMultipleLink ObjectName()
        {
            //probleme with selectall !!!!
            return new WMultipleLink(GetDriver(), By.XPath(".//div[@id='idChartSaverObjects']/..|.//div[@id='idDivSaveObjects']|.//div[starts-with(@id,\"idChartSaverObjects\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("Ok", "click on ok button and manage the popup")]
        public WESPopUp Ok()
        {
            GetDriver().ClickAt(By.Id("btnValidate"), this);
            return new WESPopUp(GetDriver());
        }

        [TxAction("AddObject", "click on ok button and manage the popup")]
        public WDValidatedWindow<WForm> AddObject()
        {
            //  GetDriver().ClickAt(new ByChained(By.XPath(".//div[@id='idDivSaveObjects']/..|.//div[@id='idChartSaverObjectsToolbar']"), ElemByPictureName.AddObject), this);
            // GetDriver().ClickAt(ElemByPictureName.AddObject);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idChartSaverObjectsToolbar\")]//div[@title=\"Add Object\"]"), this);
            WForm formCreator = new WForm(GetDriver(),By.XPath(".//div[@class='dhx_cell_wins']//div[@class=' dhxlayout_base_material']"));
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);//this is given because "dhtmlx_wins_body_inner" is same for this 
        }
        //[TxAction("ManageRightClickOptions", "ManageRightClickOptions")]
        //public GTManageRightClickOptions ManageRightClickOptions(string Inode_number)
        //{
        //    WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//tr[@idnode='"+Inode_number+"']//span[@class='standartTreeRow']"), this);
        //    GetDriver().RightClick(elem.GetElement(), 10, 10);
        //    return new GTManageRightClickOptions(GetDriver(), By.XPath(".//div[contains(@id,'polygon_dhxId_ZPPxYHRsHCu1_dhxWebMenuTopId')]"));
        //}

    }
}
