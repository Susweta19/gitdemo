using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTModifyUnitList : WERefreshed
    {
        public GTModifyUnitList(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("ValidateCancel", "ValidateCancel")]
        public void ValidateCancel(bool validate)
        {
            By button = validate ? By.Id("btnValidate") : By.Id("btnCancel");
            GetDriver().ClickAt(button,this);
        }

        [TxAction("SelectUnit", "SelectUnit")]
        public WMultipleLink SelectUnit()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,'idChartUnitsTree')]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }
    }
}
