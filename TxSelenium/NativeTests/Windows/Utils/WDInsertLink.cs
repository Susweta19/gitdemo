using OpenQA.Selenium;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDInsertLink : WERefreshed
    {
        public static readonly By deleteimg1 = By.XPath(".//div[contains(@tab_id,\"tab\") and not(contains(@style,\"visibility: hidden\"))]//*[contains(@id,\"entityText\")]//following-sibling::*");

        public static readonly By frameInsertLinkBy = By.XPath(".//iframe[@id =\"richTextLinkFrame\"]");

        public WDInsertLink(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("ChangeTab", "Changes this form current tab.")]
        public void ChangeTab(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\"" + tabName + "\"]"), this);
        }

        [TxAction("ModifyLabel", "Modifies a lable.")]
        public void ModifyLabel(string Label, bool delete = false)
        {
            if (GetDriver().IsClickable(By.Id("entityText"), this.GetElement()))
                new WEGInput(GetDriver(), By.Id("entityText"), this).FillWithoutClear(Label);
            else
                new WEGInput(GetDriver(), By.Id("entityText2"), this).FillWithoutClear(Label);

            if (delete)
                GetDriver().ClickAt(deleteimg1);
        }

        [TxAction("ModifyExternalLink", "Modifies an external link.")]
        public void ModifExternalLink(string link, bool delete = false)
        {
            new WEGInput(GetDriver(), By.XPath(".//*[@id = \"URL\"]")).FillWithoutClear(link);
            if (delete)
                GetDriver().ClickAt(By.XPath(".//*[@id = \"URL\"]//following-sibling::*"), this);

        }
        [TxAction("LinkNameGT", "Manage link in general tab")]
        public WEGInput LinkNameGT()
        {
            return new WEGInput(GetDriver(), By.Id("entityText"), this);
        }

        [TxAction("DeleteLinkNameGT", "Manage link in general tab")]
        public void DeleteLinkNameGT()
        {
            GetDriver().ClickAt(By.XPath(".//*[@id = \"entityText\"]//following-sibling::*"), this);
        }

        [TxAction("LinkNameA", "Manage Link in appearance tab")]
        public WEGInput LinkNameA()
        {
            return new WEGInput(GetDriver(), By.Id("entityText2"), this);
        }

        [TxAction("DeleteLinkNameA", "Manage Link in appearance tab")]
        public void DeleteLinkNameA()
        {
            GetDriver().ClickAt(By.XPath(".//*[@id = \"entityText2\"]//following-sibling::*"), this);
        }

        [TxAction("SelectOT", "Selects the specified entity type")]
        public WEGDHtmlxComboBox SelectEntityType(string objectTypeName)
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.ClassName("dhx_combo_box"), this, frameInsertLinkBy);
        }
        [TxAction("WebSite", "Manage Link in appearance tab")]
        public void WebSite(string websiteName, bool delete = false)
        {
            new WEGInput(GetDriver(), By.Id("URL"), this).FillWithoutClear(websiteName);
            if (delete)
                GetDriver().ClickAt(By.XPath(".//*[@id = \"URL\"]//following-sibling::*"), this);
        }

        [TxAction("ObjectType", "Selects the specified entity type")]
        public WEGDHtmlxComboBox ObjectType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("id_div_combo"), this, frameInsertLinkBy);
        }

        [TxAction("Objects", "Unchecks all checked entities.")]
        public WSingleLink Objects()
        {
            return new WSingleLink(GetDriver(), By.Id("id_div_treebox"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("Ok", "validated the window")]
        public void Ok(bool vatidated = true)
        {
            if (vatidated)
            {
                GetDriver().ClickAt(By.Id("insert"), null, By.XPath(".//iframe[@id=\"richTextLinkFrame\"]"));
            }
            else
            {
                GetDriver().ClickAt(By.Id("cancel"), null, By.XPath(".//iframe[@id=\"richTextLinkFrame\"]"));
            }
        }
    }
}
