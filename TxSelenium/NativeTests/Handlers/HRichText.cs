using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    class HRichText : IWritingHandler<DTRichText>, IReadingHandler<DTRichText>
    {

        public DTRichText ReadForm(WERefreshed elem, int attrId)
        {
            WERefreshed attrText = elem.GetDriver().WaitForElement(By.TagName("body"), null, new ByChained(elem.ElemIdentifier, By.XPath(".//iframe[starts-with(@id,\"idFrameText\")]")));
            return new DTRichText(attrText.GetElement().Text);
        }

        public Func<WERefreshed, DTRichText> GetAssoParser()
        {
            return ReadInputContent;
        }

        public DTRichText ReadInputContent(WERefreshed elem)
        {
            elem.GetElement().Click();
            elem.WaitForAjax();
            return new DTRichText(elem.GetDriver().WaitForElement(By.TagName("body"), null, new ByChained(elem.ElemIdentifier, By.XPath(".//iframe[starts-with(@id,\"textdiv\")]"))).GetElement().Text);
        }

        public void WriteForm(WERefreshed elem, int attrId, DTRichText value)
        {
            //maybe click before 
            elem.GetElement().Click();
            elem.WaitForAjax();
            new WEGInput(elem.GetDriver(),By.TagName("body"), null, new ByChained(elem.ElemIdentifier, By.XPath(".//iframe[starts-with(@id,\"textdiv\")]"))).Fill(value.Value);
        }

        public void DeleteData(WERefreshed elem)
        {
            //maybe click before
            new WEGInput(elem.GetDriver(), By.TagName("body"), null, new ByChained(elem.ElemIdentifier, By.XPath(".//iframe[starts-with(@id,\"textdiv\")]"))).Clear();
        }
    }
}
