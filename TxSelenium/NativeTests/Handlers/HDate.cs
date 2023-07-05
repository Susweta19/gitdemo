using System;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    class HDate : IWritingHandler<DTDate>, IReadingHandler<DTDate>
    {
        public DTDate ReadInputContent(WERefreshed elem)
        {
            return new DTDate(elem.GetElement().Text);
        }

        public DTDate ReadForm(WERefreshed elem, int attrId)
        {
            WERefreshed attrText = elem.GetDriver().WaitForElement(HText.textFormReadBy, elem);

            string tt = attrText.GetElement().Text;
            return new DTDate(attrText.GetElement().Text);
        }


        public Func<WERefreshed, DTDate> GetAssoParser()
        {
            return ReadInputContent;
        }

        public void WriteForm(WERefreshed elem, int attrId, DTDate value)
        {
            WEGInput input = new WEGInput(elem.GetDriver(), HText.GetFormWriteBy(), elem); 
            input.Fill(value.Value);
            input.Enter();

        }

        public void DeleteData(WERefreshed elem)
        {
            new HDecimal().DeleteData(elem);
        }
    }
}
