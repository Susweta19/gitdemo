using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.Readable;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxTableView
{
    public class ManagePreviewData : WERefreshed
    {
        public ManagePreviewData(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("PreviewLink", "")]
        public ActionColl<WEGLink> PreviewLink()
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"contentPopup\")]"), this);
            ICollection<IWebElement> listLink = elem.GetElement().FindElements(By.XPath(".//div[@id= \"cellDataString\"]//a[@href]|.//div[@class=\"accLabelWithIcon\"]|.//div[@id= \"cellDataString\"]//li/a[@id=\"linkItem\"]"));
                //(".//a[not(contains(@style, \"display: none;\"))]"));
            ICollection <WEGLink> values = new List<WEGLink>(listLink.Count);
            for (int i = 0; i < listLink.Count; i++)
                values.Add(new WEGLink(GetDriver(), By.XPath(".//div[@id= \"cellDataString\"]//a[@href][" + (i+1)+ "]|.//div[@class=\"accLabelWithIcon\"]["+(i+1)+ "]|.//div[@id= \"cellDataString\"]//li[" + (i + 1) + "]/a[@id=\"linkItem\"]")));
                    //(".//a[starts-with(@id,\"linkItem" + i + "\")]"), elem));

            return new ActionColl<WEGLink>(values);
        }

        [TxAction("Close", "")]
        public void Close( bool validate = true)
        {
            By button1 = validate ? (By.XPath(".//button[starts-with(@id,\"btnCancelPopup\")]")) : (By.XPath(".//button[starts-with(@id,\"btnCancelPopup\")]"));
            GetDriver().ClickAt(button1);
        }

        [TxAction("ReadRichText", "")]
        public ActionColl<string> ReadRichText()
        {
            ICollection<IWebElement> element = GetDriver().WaitForElement(By.XPath(".//body[@id=\"tinymce\"]"), this, By.XPath(".//iframe[contains(@id,\"rtDisplayCellData\")]")).GetElement().FindElements(By.XPath(".//p"));
            ICollection<string> ReadRichText = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                ReadRichText.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(ReadRichText);
        }

        [TxAction("ReadRchTextStyle", "Readble")]
        public RRichText ReadRichTextStyle()
        {
            //  RRichText richtext = new RRichText(GetDriver(), By.XPath(".//div[starts-with(@id,\"divPreviewText\")]");
            return new RRichText(GetDriver(), By.XPath(".//div[starts-with(@id,\"divPreviewText\")]"),this, By.XPath(".//iframe[contains(@id,\"rtDisplayCellData\")]"));
        }

        [TxAction("ReadPictureNames", "")]
        public ActionColl<string> ReadPictureNames()
        {
            List<string> pictures = new List<string>();
            WERefreshed parent = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"contentPopup\")]"));
            ICollection<IWebElement> elements = parent.GetElement().FindElements(By.XPath(".//div[starts-with(@id,\"accItem\")]")).ToList();

            foreach (var elem in elements)
            {
                WERefreshed div= GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"accItem\")][1]"),parent);
                pictures.Add(GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,\"accLabelWithIcon\")]"), div).GetElement().Text);
            }

            return new ActionColl<string>(pictures);
        }

        [TxAction("FullScreen", "")]
        public void FullScreen()
        {
            GetDriver().ClickAt(By.XPath(".//button[contains(@id,'fullScreen16')]"),this);
        }
        [TxAction("Goto", "Url open in new tab")]
        public void Goto(int index)
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,'OpenInNewTab')]["+(index+1)+"]"), this);
        }
        [TxAction("ClickOutSideofWindow", "Click OutSide of the Preview Window")]
        public void ClickOutSideofWindow()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'popupInterfaceModalScreen')]"));
        }
    }
}
