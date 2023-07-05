using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Windows.Utils;
using System.Threading;

namespace TxSelenium.NativeTests.Writable
{
    public class WRichText : WERefreshed
    {

        private By FrameWriteField;
        public WRichText(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null, By frameWriteField = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
            if (frameWriteField == null)
                FrameWriteField = new ByChained(elemIdentifier, By.XPath(".//iframe[starts-with(@id, \"textdiv\")]"));
            else
                FrameWriteField = frameWriteField;
        }

        [TxAction("Clear", "Clear")]
        public void Clear()
        {
            new WEGInput(GetDriver(), By.XPath(".//body[@id=\"tinymce\"]"), this, FrameWriteField).Clear();
        }
        [TxAction("IsFullScreenPresent", "")]
        public bool IsFullScreenPresent()
        {
            return !GetDriver().IsElementPresent(By.XPath(".//div[contains(@style,'display: none') and @aria-label='Fullscreen']" +
                "//i[@class=\"mce-ico mce-i-fullscreen\"]"), this.GetElement());
        }
        [TxAction("SelectAll", "............. ")]
        public void SelectAll()
        {    
            //new WEGInput(GetDriver(), By.XPath(".//body[@id=\"tinymce\"]"), this, FrameWriteField).SelectAll();
            GetDriver().WaitForElement( By.XPath(".//body[@id=\"tinymce\"]"), this, FrameWriteField).GetElement().SendKeys(Keys.Control+"a");
            int q = 0;
        }
        [TxAction("Minimize", "")]//need to delete
        public void Minimize()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"idDivBtnBar\")]//div[contains(@style,\"display: inline;\")]//img[contains(@src,\"Minimize.png\")]"));
        }

        [TxAction("Write", "Write")]
        public void Write(string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//body[@id=\"tinymce\"]"), this, FrameWriteField).FillWithoutClear(value);
            GetDriver().SwitchToContent();
        }

        [TxAction("ManageTextArea", "ManageTextArea")]
        public WEGInput ManageTextArea()
        {
            return new WEGInput(GetDriver(), By.XPath(".//body[@id=\"tinymce\"]"), this, FrameWriteField);
        }

        [TxAction("Bold", "Makes the text bold ")]
        public void Bold()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-bold\"]"), this);
        }

        [TxAction("Italic", "Makes the text italic")]
        public void Italic()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-italic\"]"), this);
        }

        [TxAction("Underline", "Unlerline the text")]
        public void Underline()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-underline\"]"), this);
        }

        [TxAction("BulletList", ".....")]
        public void BulletList(string type = null)
        {

            if (type != null)
            {
                GetDriver().ClickAt(By.XPath(".//div[@id=\"mceu_11\"]//i[@class=\"mce-caret\"]"), this);
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu_\")] //span[(text()=\"" + type + "\")]"));
            }
            else
                GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-bullist\"]"), this);
        }

        [TxAction("NumberedList", "....")]
        public void NumberedList(string type = null)
        {
            if (type != null)
            {
                GetDriver().ClickAt(By.XPath(".//div[@id=\"mceu_12\"]//i[@class=\"mce-caret\"]"),this);
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu_\")] //span[(text()=\"" + type + "\")]"));
            }
            else
                GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-numlist\"]"), this);
        }

        

        [TxAction("FullScreen", "***Full Screen***")]
        public void FullScreen()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-fullscreen\"]"), this);
        }


        [TxAction("Alignleft", "***Align left***")]
        public void Alignleft()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-alignleft\"]"), this);
        }

        [TxAction("AlignRight", "***Align Right***")]
        public void AlignRight()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-alignright\"]"), this);
        }

        [TxAction("AlignCenter", "***Align Centre***")]
        public void AlignCenter()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-aligncenter\"]"), this);
        }

        [TxAction("Justify", "***Justify***")]
        public void Justify()
        {
            GetDriver().ClickAt(By.XPath(".//i[@class=\"mce-ico mce-i-alignjustify\"]"), this);
        }

        /// <summary>
        /// Use only Integer value for parameter Size
        /// </summary>
        /// <param name="size"></param>

        [TxAction("SetFontSize", "***Set fontsize***")]
        public void SetFontSize(string size)
      {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'mceu_') and @aria-label=\"Font Sizes\"]"), this);
            GetDriver().ClickAt(By.XPath(".//div[@role=\"application\" and not(contains(@style,\"display: none\"))]//span[@class=\"mce-text\"and (text()=\"" + size + "px\")]"));
        }

        [TxAction("SetFontType", "***Set font type***")]
        public void SetFontType(string FontNname)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'mceu_') and @aria-label=\"Font Family\"]"), this);
            GetDriver().ClickAt(By.XPath(".//div[@role=\"application\" and not(contains(@style,\"display: none;\"))]//span[starts-with(@id,'mceu_') and text()=\""+FontNname+"\"]"));
        }

        [TxAction("ClickOnFontColor", "***font color will be selected previously and it Will just click on font color button only ***")]
        public void ClickOnFontColor()
        {
            GetDriver().ClickAt(By.XPath(".//button/i[starts-with(@class,\"mce-ico mce-i-forecolor\")]"), this);
        }

        [TxAction("SetFontcolor", "***Set font color***")]
        public WDCustomColor SetFontcolor(string ColorTitle=null,bool custom=false)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"mceu_\")]//button[@class=\"mce-open\"]"), this);
            if (custom)
            {
                GetDriver().ClickAt(By.XPath(".//div[not(contains(@style,\"display: none\"))]//*[starts-with(@class,\"mce-custom-color-btn\")]//button"));
                return new WDCustomColor(GetDriver(), By.XPath(".//div[contains(@id,\"mceu_\")and @class=\"mce-container mce-panel mce-floatpanel mce-window mce-in\"]"));
            }
            else
            {
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu\")and not(contains(@style ,\"display: none\"))]//div[@title=\"" + ColorTitle + "\"]"));
                return null;
            }
        }

        /// <summary>
        /// Index starts from 0
        /// </summary>
        /// <param name="index"></param>
        [TxAction("SetFontcolorFromCustomList", "***Set font color Index starts from 0***")]
        public void SetFontcolorFromCustomList(string index)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"mceu_\")]//button[@class=\"mce-open\"]"), this);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu\")and not(contains(@style ,\"display: none\"))]//div[starts-with(@id,\"mcearia\") and contains(@id,\"-4"+index+"\")]"));
        }

        [TxAction("ClickOnBackgroundcolor", "***Backgroud color will be selected previously , it will just click on the button***")]
        public void ClickOnBackgroundcolor()
        {
            GetDriver().ClickAt(By.XPath(".//button/i[starts-with(@class,\"mce-ico mce-i-backcolor\")]"),this);
        }


        [TxAction("SetBackgroundcolor", "***Set Backgroud color***")]
        public WDCustomColor SetBackgroundcolor(string ColorTitle=null,bool Custom = false)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"mceu_\") and @aria-label=\"Background color\"]//button[2]"),this);
            if (!Custom)
            {
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu\") and not(contains(@style ,\"display: none\"))]//div[@title=\"" + ColorTitle + "\"]"));
                return null;
            }
            else
            {
                GetDriver().ClickAt(By.XPath(".//div[not(contains(@style,\"display: none\"))]//div[@class=\"mce-widget mce-btn mce-btn-small mce-btn-flat\"]/button"));
                return new WDCustomColor(GetDriver(), By.XPath(".//div[contains(@id,\"mceu_\")and @class=\"mce-container mce-panel mce-floatpanel mce-window mce-in\"]"));
            }
        }

        [TxAction("InsertLink", "***.........***")]
        public WDValidatedFrameWindow<WDInsertLink> InsertLink()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu_\")]//i[@class=\"mce-ico mce-i-link\"]"), this);

            WDInsertLink insertLink = new WDInsertLink(GetDriver(), By.TagName("body"));
            return new WDValidatedFrameWindow<WDInsertLink>(GetDriver(), insertLink, null, WDInsertLink.frameInsertLinkBy);
        }

        [TxAction("InsertImage", "***.........***")]
        public WDValidatedFrameWindow<WDInsertImage> InsertImage()
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"mceu_\")]//i[@class=\"mce-ico mce-i-image\"]"),this);
            WDInsertImage insertimage = new WDInsertImage(GetDriver(), By.TagName("body"));
            return new WDValidatedFrameWindow<WDInsertImage>(GetDriver(), insertimage, null, WDInsertImage.frameInsertImageBy);
        }

        [TxAction("OkCancel", "ValidateCancel")]
        public void ValidateCancel(bool validate)
        {
            if (validate)
                GetDriver().ClickAt(By.XPath(".//input[@id='idBtnValidate']"), null, this.FrameBy);
            else
                GetDriver().ClickAt(By.XPath(".//input[@id='idBtnCancel']"), null, this.FrameBy);
        }
        [TxAction("IsToolBarPresent", "")]
        public bool IsToolBarPresent(int attrID)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"div" + attrID + "\")]//div[starts-with(@id,\"mceu_\")]"));
        }
    }
}




