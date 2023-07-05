using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxMCS
{
    public class GTManageGroup : WERefreshed
    {
        public GTManageGroup(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("GroupName", "")]
        public WEGInput GroupName()
        {
            return new WEGInput(GetDriver(),By.XPath(".//input[contains(@id,'idMCSWdowGroupName')]"), this);
        }

        [TxAction("Aggregation", "")]
        public WEGDHtmlxComboBox Aggregation()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@id,'idMCSWdowGroupAggregation')]"), this);
        }

        [TxAction("OKValidate", "")]
        public void Ok(bool validate=true)
        {
            By okButton = By.XPath(".//input[contains(@id,'idBtnValidate')]");
            By cancelButton = By.XPath(".//input[contains(@id,'idBtnCancel') and @value=\"Cancel\"]");
            By button = validate ? okButton : cancelButton;
            GetDriver().ClickAt(button);
        }
    }
}
