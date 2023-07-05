using OpenQA.Selenium;
using System;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    class HBool : IReadingHandler<DTBool>, IWritingHandler<DTBool>
    {

        public static readonly string boolSrcTrue = "16x16-True.png";
        public static readonly string boolSrcFalse = "16x16-False.png";
        private static readonly By boolReadFormBy = By.XPath(".//img[contains(@src,\"16x16\")]");


        public DTBool ReadForm(WERefreshed elem, int attrId)
        {
            return Parse(elem);
        }

        public Func<WERefreshed, DTBool> GetAssoParser()
        {
            return (elem) => HBool.GetTxBoolFromDiv(elem.GetElement().FindElement(boolReadFormBy));
        }

        private DTBool Parse(WERefreshed elem)
        {
            IWebElement attrImg = elem.GetElement().FindElement(boolReadFormBy);
            return HBool.GetTxBoolFromDiv(attrImg);
        }

        public static DTBool GetTxBoolFromDiv(IWebElement attrImg)
        {
            string src = attrImg.GetAttribute("src");
            if (src.Contains(HBool.boolSrcTrue))
            {
                return DTBool.True;
            }
            else if (src.Contains(HBool.boolSrcFalse))
            {
                return DTBool.False;
            }
            else
            {
                throw new Exception("Problem with src picture png");

            }
        }

        public DTBool ReadInputContent(WERefreshed elem)
        {   

            return DTBool.GetTxBool("yes");
        }

        public void WriteForm(WERefreshed elem, int attrId, DTBool value)
        {
            if (value != null)
            {
                if (value.ToString() == DTBool.True.ToString())
                {
                    elem.GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"boolYes\")]"), elem);
                }
                else
                    elem.GetDriver().ClickAt(By.XPath(".//input[contains(@id,  \"boolNo\") ]"), elem);
            }
        }

        public void DeleteData(WERefreshed elem)
        {

        }

    }
}
