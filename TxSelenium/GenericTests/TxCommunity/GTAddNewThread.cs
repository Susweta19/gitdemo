using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;

namespace TxSelenium.GenericTests.TxCommunity
{
    public class GTAddNewThread : WERefreshed
    {
        public GTAddNewThread(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        //Park Up button or Minimize Button
        [TxAction("ParkUp", "To maximize and Minimize the Window")]
        public void ParkUp()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Park Up\"]"), this);
        }

        //Close The window
        [TxAction("Close", "To Close the Window")]
        public void Close()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Close\"]"), this);
        }

        //Post something
        [TxAction("Post", "Post a comment")]
        public void Post()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id=\"idBtnValidate\"]"), this);
        }

        //What's on your mind textarea box
        [TxAction("WhatsOnYourMind", "Write values in the specified RichText attribute.")]
        public WRichText RichText()
        {
            return new WRichText(GetDriver(), By.ClassName("mce-container-body mce-stack-layout"), this, null, By.XPath(".//iframe[starts-with(@id, \"idTextTiny\")] | .//*[@id = \"Comment_text_ifr\"]"));
        }

        //People to Notify Entity tree
        [TxAction("PeopleToNotify", "Select People")]
        public WMultipleLink PeopleToNotify()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//*[starts-with(@id,\"idDivTreePeopleConcerned\")]/parent::td"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
        }

        //Group to Notify Entity tree
        [TxAction("GroupsToNotify", "Toggles the selection box of  entities identified by the collection leading to it.")]
        public WMultipleLink GroupsToNotify()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//*[starts-with(@id,\"idDivTreeGroupsConcerned\")]/parent::td"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
        }

        //Privacy 
        [TxAction("Privacy", "Setting Privacy ,true for public and false for private")]
        public void Privacy(bool property)
        {
            if (!property)
                GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idRadioPublicActive\")]"), this);
            else
                GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"idRadioPrivacyActive\")]"), this);
        }
        [TxAction("IsAttributePresent", "")]
        public bool IsAttributePresent(string attributeName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//span[contains(text(),\"" + attributeName + "\")] |.//h4[contains(text(),\""+attributeName+"\")]"), this.GetElement());
        }

        [TxAction("ReadHeader", "")]
        public string ReadHeader()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class = \"dhxwin_text_inside\"]"), this).GetElement().Text;
        }
    }
}
