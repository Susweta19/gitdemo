using OpenQA.Selenium;
using System;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    class HText : IReadingHandler<DTText>, IWritingHandler<DTText>
    {
        public static readonly By textFormReadBy = By.TagName("label");


        public static By GetFormWriteBy()
        {
            return By.XPath(".//textarea | .//input");
        }

        public DTText ReadForm(WERefreshed elem, int attrId)
        {
            WERefreshed attrText = elem.GetDriver().WaitForElement(textFormReadBy, elem);
            return new DTText(attrText.GetElement().Text);
        }

        public Func<WERefreshed, DTText> GetAssoParser()
        {
            return ReadInputContent;
        }

        public DTText ReadInputContent(WERefreshed elem)
        {
            string value = elem.GetElement().Text;
            if (value == " ")
                return new DTText("#Space");
            else
                return new DTText(elem.GetElement().Text);
        }

        public void WriteForm(WERefreshed elem, int attrId, DTText value)
        {

			new WEGInput(elem.GetDriver(), GetFormWriteBy(), elem).Fill(value.Value);                       
        }

        public  void DeleteData(WERefreshed elem)
        {
            new WEGInput(elem.GetDriver(), GetFormWriteBy(), elem).Clear();
        }
    }
}
