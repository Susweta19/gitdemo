using OpenQA.Selenium;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements
{
    public class WEGCheckBox : WERefreshed
    {
        public WEGCheckBox(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            :base(driver, elemIdentifier, parent)
        {
            base.WaitForElement();
        }

        [TxAction("Tick", "...")]
        public void Tick()
        {      
            //new WERefreshed(GetDriver(), this.elemIdentifier, parent).GetElement().Click();
           GetDriver().ClickAt(this.ElemIdentifier, Parent);
        }

        public void Toggle()
        {
            GetDriver().ClickAt(this.ElemIdentifier, Parent);
        }

        [TxAction("Read", "Use the drop down list.")]
        public bool ReadValue()
        {
            return this.IsChecked();
        }


        public bool IsChecked()
        {
            bool firstTest  = GetDriver().IsAttributePresent("checked", this);
            bool secondTest = GetElement().GetAttribute("style").Contains("chk1");
            bool thirdTest  = GetElement().GetAttribute("style").Contains("CheckAll");
            bool fourthTest = GetElement().GetAttribute("style").Contains("iconCheckDis.gif");
            bool fifthTest = GetElement().GetAttribute("style").Contains("radio_on.gif");
            bool sixthTest = false;
            if ( GetDriver().IsAttributePresent("src",this) == true)
            {
                sixthTest = GetElement().GetAttribute("src").Contains("chk1.gif")| GetElement().GetAttribute("src").Contains("chk1_dis.gif");
            }               
            return firstTest | secondTest | thirdTest | fourthTest | fifthTest| sixthTest;            
        }

        [TxAction("IsDisabled", "Checks weather the objects is ticked or not")]
        public bool IsDisabled()
        {
            return GetElement().GetAttribute("src").Contains("_chk1.gif") || GetElement().GetAttribute("src").Contains("_dis.gif");
        }
    }
}
