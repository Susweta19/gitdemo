using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Windows.Utils
{
    public class WDModificationsHistory : WERefreshed
    {
        public WDModificationsHistory(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ModificationDate", "Date of modification")]
        public WDModificationDate ModificationDate()
        {
            GetDriver().ClickAt(By.Id("textFilter_0"), this);
            return new WDModificationDate(GetDriver(), By.ClassName("divBoundFilter"), this);
        }


        [TxAction("UserType", "Type of User")]
        public WEGDHtmlxComboBox UserType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("comboFilter_1"), this);
        }

        [TxAction("Attribute", "Select Attribute")]
        public WEGDHtmlxComboBox Attribute()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.Id("comboFilter_2"), this);
        }

        [TxAction("FormerValue", "Previous Value(Need To Press Enter To Get Result)")]
        public WEGInput DeletedLink()
        {
            return new WEGInput(GetDriver(), By.XPath(".//table[@class=\"hdr\"]//td[4]//div[@class=\"divGridFilters\"]/input"));
        }

        [TxAction("NewValue", "New Value(Need To Press Enter To Get Result)")]
        public WEGInput AddedLink()
        {
            return new WEGInput(GetDriver(), By.XPath(".//table[@class=\"hdr\"]//td[5]//div[@class=\"divGridFilters\"]/input"));
        }

        [TxAction("ResultsList", "New Value(Need To Press Enter To Get Result)")]
        public WESTree ResultsList()
        {
            return new WESTree(GetDriver(), By.ClassName("objbox"), WESTree.expandByModifHistTree, this);
        }

    }
}
