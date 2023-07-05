using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;
using TxSelenium.Utils;
using TxSelenium.NativeTests.Writable;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTOverlayCurves : WERefreshed
    {
        public GTOverlayCurves(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("AddSeries", "")]
        public WDValidatedWindow<WMultipleLink> AddSeries(bool returnWindow = true)
        {
            GetDriver().ClickAt(ElemByPictureName.AddObject, this);
            GetDriver().WaitForAjax();
            WMultipleLink linkBox = new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"seriesTree\")]/.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return returnWindow ? new WDValidatedWindow<WMultipleLink>(GetDriver(), linkBox) : null;
        }

        [TxAction("IsSuperpositionPreviewDisplayed", "")]
        public bool IsSuperpositionPreviewDisplayed()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"chartSuperpositionPreview\")]//div"), this.GetElement());
        }

        [TxAction("ChartsTree", "")]
        public WMultipleLink ChartsTree()
        {
            WMultipleLink linkBox = new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartsTree\") and starts-with(@class,\"chartsTree\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return linkBox;
        }

        [TxAction("ManageSuperpositionPreview", "")]
        public GTTxCharts ManageSuperpositionPreview()
        {
            return new GTTxCharts(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartSuperpositionPreview\")]"), this);
        }

        [TxAction("CheckAllObjects", "")]
        public void CheckAllObjects()
        {
            GetDriver().ClickAt(ElemByPictureName.SelectAll, this);
        }

        [TxAction("UncheckAllObjects", "")]
        public void UncheckAllObjects()
        {
            GetDriver().ClickAt(ElemByPictureName.UnCheckAll, this);
        }

        [TxAction("Reset", "")]
        public void Reset()
        {
            GetDriver().ClickAt(ElemByPictureName.Reset, this);
        }
    }
}
