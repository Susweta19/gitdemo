using OpenQA.Selenium;
using System;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;
using System.Threading;

namespace TxSelenium.NativeTests.Windows
{
    public class WDInsertImage : WERefreshed
    {
        public static readonly By frameInsertImageBy = By.XPath(".//iframe[@id =\"richTextImageFrame\"]");

        private static readonly By selectimgFrameBy = By.XPath(".//div[@id=\"rightfield\"]//iframe");

        public WDInsertImage(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ChangeTab", "Changes this form current tab.")]
        public void ChangeTab(string tabName)
        {
            GetDriver().ClickAt(By.XPath(".//div[text()=\""+ tabName +"\"]"), this);
        }

        [TxAction("SelectFile", "Click on choose picture.")]
        public void SelectFile(string path)
        {
            WaitForAjax();
            Sleep(2);
            IWebElement button = GetDriver().FindElement(this.GetElement(),By.Id("fileupload"));
            Thread.Sleep(1000);
            //button = GetDriver().FindElement(this.GetElement(), By.Id("fileupload"));
            //int u = 0;
            button.SendKeys(path);
            WaitForAjax();
            Thread.Sleep(500);
        }

        [TxAction("WritePictureDetails", "Writes picture details.")]
        public void WritePictureDetails(string Description, string Title)
        {
            new WEGInput(GetDriver(), By.XPath(".//input[@class=\"clInput\" and @id=\"alt\"]"), this).FillWithoutClear(Description);
            new WEGInput(GetDriver(), By.XPath(".//input[@class=\"clInput\" and @id=\"title\"]"), this).FillWithoutClear(Title);
        }

        [TxAction("Insert", "Clicks on the Submit Button.")]
        public void Sumbit()
        {
            GetDriver().ClickAt(By.Id("insert"), this);
        }

        [TxAction("Cancel", "Clicks on the Cancel Button.")]
        public void Cancel()
        {
            GetDriver().ClickAt(By.Id("cancel"), this);
        }

        [TxAction("Preview", "Select the specified Alignment")]
        public void Preview(string pictureDisplayed)
        {
            WERefreshed prev = GetDriver().WaitForElement(By.Id("prev"), this);
            if (!GetDriver().IsElementPresent(By.XPath("./img[contains(@src , \"" + pictureDisplayed + "\")]"), prev.GetElement()))
                throw new Exception("picutre is not displayed");
        }

        [TxAction("DimensionsWidth", "Select the specified width")]
        public WEGInput DimensionsWidth()
        {
            return new WEGInput(GetDriver(), By.XPath((".//input[@name=\"width\"]")), this);
        }

        [TxAction("DimensionsHeight", "Select the specified height")]
        public WEGInput DimensionsHeight()
        {
            return new WEGInput(GetDriver(), By.XPath((".//input[@name=\"height\"]")), this);
        }

        [TxAction("ForceProportions", "Select the specified Alignment")]
        public WEGCheckBox ForceProportions()
        {
            return new WEGCheckBox(GetDriver(), By.Id("constrain"), this);
        }

        [TxAction("Alignment", "Select the specified Alignment")]
        public WEGSelect Alignment()
        {
            return new WEGSelect(GetDriver(), By.Id("align"), this);
        }

        [TxAction("Appearance", "Selects the specified Alignment")]
        public void Appearance(string Alignment, string DWidth, string DHeight, bool Forcetheproportions = true)
        {
            if (!Forcetheproportions)
            {
                GetDriver().ClickAt(By.Id("constrain"), this);
            }
            new WEGSelect(GetDriver(), By.Id("align"), this).SelectByText(Alignment);
            new WEGInput(GetDriver(), By.XPath((".//input[@name=\"width\"]")), this).FillWithoutClear(DWidth);
            new WEGInput(GetDriver(), By.XPath((".//input[@name=\"height\"]")), this).FillWithoutClear(DHeight);
        }
    }
}
