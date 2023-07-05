using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTExportChart : WERefreshed
    {
        public GTExportChart(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("Format", "Select the type of Format")]
        public WEGDHtmlxComboBox Format()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivComboExportType\")]"), this);
        }

        [TxAction("Separator", "Select the Separator")]
        public WEGDHtmlxComboBox Separator()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idSeparator\")]"), this);//idDivComboSeparator
        }

        [TxAction("SetHeaderLinesToSkip", "Number of header lines to skip")]
        public WEGInput SetHeaderLinesToSkip()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"linesToIgnore\")]"), this);
        }

        [TxAction("Tranpose", "Tick Tranpose")]
        public WEGCheckBox Tranpose()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"transpose\")]"), this);
        }

        [TxAction("FileName", "Set File Name")]
        public WEGInput FileName()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"filename\")]"), this);
        }

        [TxAction("ImageResolution", "Set Image Resolution")]
        public WEGDHtmlxComboBox ImageResolution()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartExporterPictureResolution\")]"), this);
        }

        [TxAction("Curves", "****")]
        public WMultipleLink Curves()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"chartExporterSeriesTree\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("Ok", "click on ok button and manage the popup")]
        public void Ok(bool validate=true)
        {
            if(validate)
            GetDriver().ClickAt(By.Id("btnValidate"), this);
            else
                GetDriver().ClickAt(By.Id("btnCancel"), this);
        }


    }
}