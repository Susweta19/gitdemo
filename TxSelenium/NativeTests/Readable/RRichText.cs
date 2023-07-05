using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using System.Drawing;
using TxSelenium.NativeTests.DataTypes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.Readable
{
    public class RRichText : WERefreshed
    {
        public RRichText(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
          : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ReadListText", "Use to read List of texts that are ordered by bullets or numbers")]
        public ActionColl<DTText> ReadListText()
        {
            ICollection<DTText> values;
            ICollection<IWebElement> lists = this.GetElement().FindElements(By.XPath(".//ul/li|.//p"));
            if (lists.Count==0)
            {
                lists = this.GetElement().FindElements(By.XPath(".//ol/li"));  //In most cases this part will not be used

                values = new List<DTText>(lists.Count);

                for (int i = 1; i <= lists.Count; i++)
                    values.Add(new DTText(GetDriver().WaitForElement(By.XPath(".//ol/li[" + i + "]"), this).GetElement().Text));
            }
            else
            {
                values = new List<DTText>(lists.Count);

                for (int i = 1; i <= lists.Count; i++)
                    values.Add(new DTText(GetDriver().WaitForElement(By.XPath(".//ul/li[" + i + "]|.//p[" + i + "]"), this).GetElement().Text));
            }

            return new ActionColl<DTText>(values);
        } 

        [TxAction("ReadBold", "Read the expected bold text")]
        public void ReadBold(string Expectedboldtext)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//strong[contains(text(), \"" + Expectedboldtext + "\")]|.//strong//span[contains(text(), \"" + Expectedboldtext + "\")]"), GetElement()))
                throw new Exception("No Bold Text Found");
        }

        [TxAction("ReadItalic", "Read the expected italic text")]
        public void ReadItalic(string Expecteditalictext)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//em[contains(text(), \"" + Expecteditalictext + "\")]|.//em//span[contains(text(), \"" + Expecteditalictext + "\")]"), GetElement()))
                throw new Exception("No Italic Text Found");
        }

        [TxAction("ReadUnderline", "Read the expected underline text")]
        public void ReadUnderline(string ExpectedUnderlineText)
        {

            if (!GetDriver().IsElementPresent(By.XPath(".//span[@style=\"text-decoration: underline;\"and contains(text(),\"" + ExpectedUnderlineText + "\")]|.//span[@style=\"text-decoration: underline;\"]//span[ contains(text(),\""+ ExpectedUnderlineText + "\")]"), GetElement()))
                throw new Exception("No Underlined Text Present");
        }

        [TxAction("ReadLeftAlign", "Read the expected left aligned text")]
        public void ReadLeftAlign(string ExpectedLeftAlignText)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//p[@style=\"text-align: left;\"and contains(text(),\""+ExpectedLeftAlignText+"\")]"),this.GetElement()))
                throw new Exception("No Left Aligned Text Present");
        }

        [TxAction("ReadRightAlign", "Read the expected right aligned text")]
        public void ReadRightAlign(string ExpectedRightAlignText)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//p[@style=\"text-align: right;\"and contains(text(),\"" + ExpectedRightAlignText + "\")]"), this.GetElement()))
                throw new Exception("No Right Aligned Text Present");
        }

        [TxAction("ReadCenterAlign", "Read the expected center aligned text")]
        public void ReadenterAlign(string ExpectedCenterAlignText)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//p[@style=\"text-align: center;\"and contains(text(),\"" + ExpectedCenterAlignText + "\")]"), this.GetElement()))
                throw new Exception("No Center Aligned Text Present");
        }

        [TxAction("ReadJustifyAlignment", "Read the expected justified text")]
        public void ReadJustify(string ExpectedJustifyAlignText)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//p[@style=\"text-align: justify;\" and contains(text(),\"" + ExpectedJustifyAlignText + "\")]"), this.GetElement()))
                throw new Exception("No Justified Text Present");
        }

        [TxAction("ReadFontType", "Read the expected font type")]
        public void ReadFontType(string ExpectedFontText)
        {
            
            if (!GetDriver().IsElementPresent(By.XPath(".//span[contains(@style , \"font-family:\")and text()=\""+ExpectedFontText+"\"]"), this.GetElement()))
                throw new Exception("Expected Font Type Not Found");
        }

        [TxAction("ReadFontSize", "Read the expected font size")]
        public void ReadFontSize(string ExpectedFontSize)
        {
            if (!GetDriver().IsElementPresent(By.XPath(".//span[contains(@style , \"" + ExpectedFontSize + "\")]"), this.GetElement()))
                throw new Exception("Expected Font Size Not Found");
        }

        [TxAction("ReadTextColor", "Read the expected text color")]
        public void ReadTextColor(string ExpectedTextColor)
        {
            string ColourHex = ReturnColorName(ExpectedTextColor);
            if (!GetDriver().IsElementPresent(By.XPath(".//span[contains(@style, \"color: " + ColourHex + "\")]"), this.GetElement()))
                throw new Exception("Expected Font Color Not Found");
        }

        [TxAction("ReadBackGroundColor", "Read the expected BackGround color")]
        public void ReadBackGroundColor(string ExpectedTextColor)
        {
            string ColourHex = ReturnColorName(ExpectedTextColor);
            if (!GetDriver().IsElementPresent(By.XPath(".//span[contains(@style , \"background-color: " + ColourHex + "\")]"), this.GetElement()))
                throw new Exception("Expected Background Color Not Found");
        }

        [TxAction("ReadPictureTitle", "Read the expected justified text")]
        public DTText ReadPictureTitle()
        {
            string title = GetDriver().WaitForElement(By.XPath(".//body//img"), this).GetElement().GetAttribute("title").Split('_')[0];
            return new DTText(title);
        }

        [TxAction("ReadPictureDescription", "Read the expected justified text")]
        public DTText ReadPictureDescription()
        {
            string description = GetDriver().WaitForElement(By.XPath(".//body//img"),this).GetElement().GetAttribute("alt");
            return new DTText(description);
        }

        [TxAction("ReadAlignment", "Read the alignement of pictures")]
        public void ReadAlignment(string expectedAlignment)
        {
            string align = GetDriver().WaitForElement(By.TagName("img"), this).GetElement().GetAttribute("align");
            if (expectedAlignment != align)
                throw new Exception("No Right Aligned Text Present");
        }

        [TxAction("RichTextLink", "Read the alignement of pictures")]
        public WEGLink RichTextLink(string linkName)
        {
            return new WEGLink(GetDriver(), By.XPath(".//a[contains(text(), \""+linkName+"\")]"), this);
        }

        private string ReturnColorName(string colorCode)
        {
            int ColorValue = Color.FromName(colorCode).ToArgb();
            string colorhex = string.Format("{0:x6}", ColorValue);

            StringBuilder sb = new StringBuilder(colorhex);
            sb.Remove(0, 2);
            sb.Insert(0, "#");

            return sb.ToString();
        }
        [TxAction("IsPictureNamePresent", "Read the expected justified text")]
        public bool IsPictureNamePresent(string pictureName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,\"" + pictureName + "\")]"), this.GetElement());
        }
    }
}

