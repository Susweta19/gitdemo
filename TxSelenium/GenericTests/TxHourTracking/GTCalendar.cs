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
   public class GTCalendar : WERefreshed
    {
        public GTCalendar(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        //[TxAction("ManageDate", "")]
        //public WDate ManageDate()
        //{
        //    return new WDate(GetDriver(),By.ClassName("dhtmlxcalendar_material"),this);
        //}

        //[TxAction("ManageLinkBox", "")]
        //public WMultipleLink ManageLinkBox(string linkid,string linkName,Boolean alreadyOpen=false)
        //{
        //    if (!alreadyOpen)
        //        GetDriver().ClickAt(By.XPath(".//span[text()='" + linkName + "']/../..//div[starts-with(@class,\"dhx_cell_hdr_arrow\")]"), this);
        //    return new WMultipleLink(GetDriver(), By.Id("idDivTreeObjectpaneRT"+ linkid + ""), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        //}

        //[TxAction("CloseLink", "")]
        //public void CloseLink(int linkid)
        //{
        //    GetDriver().ClickAt(By.XPath(".//div[@class='dhx_acc_item'][" + (linkid+1)+"]//div[@class='dhx_acc_item_arrow item_closed']"), this);
        //}

        //[TxAction("ClickOnLinkBox", "")]
        //public void ClickOnLinkBox(string linkName)
        //{
        //    GetDriver().ClickAt(By.XPath(".//span[text()='"+ linkName + "']/.."), this);///../..//div[starts-with(@class,\"dhx_cell_hdr_arrow\")]
        //}
    }
}
