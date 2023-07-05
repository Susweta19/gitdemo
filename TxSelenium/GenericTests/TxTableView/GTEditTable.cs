using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using System.Threading;

namespace TxSelenium.GenericTests.TxTableView
{
    public class GTEditTable : WERefreshed
    {
        public GTEditTable(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ManageConfiguration", "ManageContainer")]
        public GTManageConfiguration ManageConfiguration()
        {
            return new GTManageConfiguration(GetDriver(),By.XPath(".//div[starts-with(@id,'tvPopupSelectCritere') and starts-with(@class,'clPopupConfig')]"),this);
        }

        [TxAction("AddConfiguration", "AddConfiguration")]
        public WDValidatedWindow<GTAddConfiguration> AddConfiguration()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'btnsContainerPopup')]//button[starts-with(@id,'add')]"),this);
            GTAddConfiguration addConfiguration = new GTAddConfiguration(GetDriver(),By.XPath(".//div[starts-with(@id,'tvWindowEditConfig') and not(starts-with(@id,'tvWindowEditConfigButtons'))]"));
            return new WDValidatedWindow<GTAddConfiguration>(GetDriver(),addConfiguration,null,this.Parent.FrameBy);
        }
        [TxAction("DuplicateConfiguration", "DuplicateConfiguration")]
        public WDValidatedWindow<GTAddConfiguration> DuplicateConfiguration()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'btnsContainerPopup')]//button[starts-with(@id,'add')]"), this);
            GTAddConfiguration addConfiguration = new GTAddConfiguration(GetDriver(), By.XPath(".//div[starts-with(@id,'tvWindowEditConfig') and not(starts-with(@id,'tvWindowEditConfigButtons'))]"));
            return new WDValidatedWindow<GTAddConfiguration>(GetDriver(), addConfiguration, null, this.Parent.FrameBy);
        }

        [TxAction("EditConfiguration", "EditConfiguration")]
        public WDValidatedWindow<GTAddConfiguration> EditConfiguration(string configId)
        {
           // Thread.Sleep(1000);
            GetDriver().ClickAt(By.XPath(".//li[starts-with(@id,'config"+configId+"')]"),this);
            Thread.Sleep(1000);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'btnsContainerPopup')]//button[starts-with(@id,'edit')]"), this);
            
            GTAddConfiguration addConfiguration = new GTAddConfiguration(GetDriver(), By.XPath(".//div[starts-with(@id,'tvWindowEditConfig') and not(starts-with(@id,'tvWindowEditConfigButtons'))]"));
            return new WDValidatedWindow<GTAddConfiguration>(GetDriver(), addConfiguration, null, this.Parent.FrameBy);
        }

        [TxAction("ValidatePopup", "ValidatePopup")]
        public void ValidatePopup(bool validate)
        {
            By button=validate? By.XPath(".//button[starts-with(@id,'ok')]"):By.XPath(".//button[starts-with(@id,'btnCancelPopup')]");
            GetDriver().ClickAt(button,this);
        }

        [TxAction("IsEditDisabled", "IsEditDisabled")]
        public bool IsEditDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//button[starts-with(@id,'edit') and @disabled]"), this.GetElement());
        }

        [TxAction("IsOkDisabled", "IsOkDisabled")]
        public bool IsOkDisabled()
        {
            return GetDriver().IsElementPresent(By.XPath(".//button[starts-with(@id,'ok') and @disabled]"), this.GetElement());
        }
        //idBtnCancel
        [TxAction("Cancel", "IsOkDisabled")]
        public void Cancel()
        {
             GetDriver().ClickAt(By.Id("idBtnCancel"), this);
        }
    }
}
