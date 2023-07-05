using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;

namespace TxSelenium.GenericTests.TxHourTracking
{
   public class GTManageNavigationLayout : WERefreshed
    {
        public GTManageNavigationLayout(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        #region TxHourTracking_NavigationLayout_Sections_And_Buttons
        public static By Expand_Collapse_LinkBox_Arrow_Button = By.XPath(".//div[starts-with(@class,\"dhx_cell_hdr_arrow\")]");
        #endregion

        #region Static_Methods
        public static string GetLinkBox(string linkName)
        {
            return ".//span[text()='" + linkName + "']/ancestor::div[starts-with(@class,\"dhx_cell_acc\")]";
        }

        public static string GetLinkBoxName(string linkName)
        {
            return ".//div[contains(@class,\"dhx_cell_hdr_text\")]/span[text()='"+ linkName + "']";
        }
        #endregion

        [TxAction("SelectDate", "")]
        public WDate SelectDate()
        {
            return new WDate(GetDriver(), WDate.Static_Calendar_Interface, this);
        }

        [TxAction("ManageLinkBox", "")]
        public WMultipleLink ManageLinkBox(string linkName,Boolean alreadyOpen=false)
        {
            WERefreshed linkDiv = GetDriver().WaitForElement(By.XPath(GetLinkBox(linkName)), this);
            ClickOnLinkBox(linkName);
            WaitForAjax();
            //if (!alreadyOpen)
            //    GetDriver().ClickAt(GTManageNavigationLayout.Expand_Collapse_LinkBox_Arrow_Button, linkDiv);
            return new WMultipleLink(GetDriver(), By.ClassName("dhx_cell_cont_acc"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, linkDiv);//should be in WMultipleLink
        }

        [TxAction("ClickOnLinkBox", "")]
        public void ClickOnLinkBox(string linkName)
        {
            GetDriver().ClickAt(By.XPath(GetLinkBoxName(linkName)), this);
        }

        [TxAction("IsAccordionCollapsed", "Checks weather the accordion is collapsed or expanded")]
        public bool IsAccordionCollapsed(string accordionName)//accordionName=linkName
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"dhx_cell_closed\")]//parent::span[text()=\""+ accordionName + "\"]"));
        }
    }
}
