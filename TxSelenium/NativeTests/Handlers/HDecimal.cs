using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Handlers
{
    class HDecimal : IReadingHandler<DTDecimalValue>, IWritingHandler<DTDecimalValue>
    {
        public static readonly By minBy = By.XPath(".//label[starts-with(@id,\"minVal\")]");
        public static readonly By maxBy = By.XPath(".//label[starts-with(@id,\"maxVal\")]");
        public static readonly By meanBy = By.XPath(".//label[starts-with(@id,\"meanVal\")]");
        public static readonly By unitWriteReadBy = By.XPath(".//div[contains(@class,\"dhxcombo_material\")]");
        public static readonly By decimalReadFormBy = By.XPath("./div");

        public static readonly string MinType = "min";
        public static readonly string MaxType = "max";
        public static readonly string MeanType = "mean";

        public static By GetFormReadBy(int index = 1)
        {
          //  return By.XPath(".//div[@class=\"cl_div_decimal\"]");
            return By.XPath(".//label[contains(@id , \"minValdiv\")]");

        }

        public static By GetDecWriteAttr(string type)
        {
            return By.XPath(".//input[starts-with(@id, \"" + type + "Valdiv\")]");
        }

        public Func<WERefreshed, DTDecimalValue> GetAssoParser()
        {
            return (elem) => ReadForAsso(elem);
        }

        //For Asso
        public DTDecimalValue ReadForAsso(WERefreshed elem)
        {
            DTDecimalValue value;
            string results = elem.GetElement().Text;
            string[] res = new string[20];
            res = results.Split('\n');
            for(int i=0; i<res.Length;i++)
            {
                res[i] = res[i].Replace("\r", "");
            }
           switch(res.Length)
            {
                case 1:
                    value = new DTDecimalValue(res[0], null, null, null);
                    break;
                case 2:
                    value = new DTDecimalValue(res[0], null, null, new DTDecimalUnit(res[1]));
                    break;
                case 3:
                    value = new DTDecimalValue(res[0], res[2], null, null);
                    break;
                case 4:
                    value = new DTDecimalValue(res[0], res[2], null, new DTDecimalUnit(res[3]));
                    break;
                case 5:
                    value = new DTDecimalValue(res[0], res[2], res[4], null);
                    break;
                case 6:
                    value = new DTDecimalValue(res[0], res[2], res[5],new DTDecimalUnit(res[3]));
                    break;
                case 9:
                    value = new DTDecimalValue(res[1], res[3], res[7], new DTDecimalUnit(res[4]));
                    break;
                case 7:
                    value = new DTDecimalValue(res[0], res[2], res[5], new DTDecimalUnit(res[6]));
                    break;
                default:
                    throw new System.Exception("Can not handel, contact Sir");
            }
                
            return value;
         }



        public DTDecimalValue ReadForm(WERefreshed elem, int attrId)
        {
            elem.Sleep(3);
            DTDecimalValue value;
            string min;
            string max = null;
            string mean = null;
            DTDecimalUnit unit = null;
            TxWebDriver driver = elem.GetDriver();
            WERefreshed elemDec = driver.WaitForElement( decimalReadFormBy, elem);

            //A decimal value should always have a min value, 
            //throwing an exception if there is no min is the normal behaviour
            WERefreshed attrText = driver.WaitForElement(HDecimal.minBy, elemDec);
            min = ExtractDoubleFromString(attrText.GetElement().Text);

            if (driver.IsElementPresent(HDecimal.maxBy, elemDec.GetElement()))
            {
                attrText = driver.WaitForElement(HDecimal.maxBy, elemDec);
                max = ExtractDoubleFromString(attrText.GetElement().Text);
            }

            if (driver.IsElementPresent(HDecimal.meanBy, elemDec.GetElement()))
            {
                attrText = driver.WaitForElement(HDecimal.meanBy, elemDec);
                mean = ExtractDoubleFromString(attrText.GetElement().Text);
            }

            if (elem.GetDriver().IsElementPresent(HDecimal.unitWriteReadBy, elem.GetElement()))
            {
                WEGDHtmlxComboBox comboBox = new WEGDHtmlxComboBox(driver, HDecimal.unitWriteReadBy, elem);     
                string unitName = comboBox.Read();
                unit = new DTDecimalUnit(unitName);
            }
            value = new DTDecimalValue(min, max, mean, unit);
            return value;
        }

        public DTDecimalValue ReadInputContent(WERefreshed elem)
        {
            string min = GetSubValue(elem, HDecimal.MinType);
            string max = GetSubValue(elem, HDecimal.MaxType);
            string mean = GetSubValue(elem, HDecimal.MeanType);

            DTDecimalUnit unit = null;
            if (elem.GetDriver().IsElementPresent(HDecimal.unitWriteReadBy, elem.GetElement()))
            {
                WERefreshed unitElem = elem.GetDriver().WaitForElement(HDecimal.unitWriteReadBy, elem);
                WERefreshed elem2 = elem.GetDriver().WaitForElement(By.XPath(".//input[@class='dhxcombo_input']"), unitElem);
                string unitName = elem2.GetElement().GetAttribute("value");
                // SelectElement select = new SelectElement(unitElem.GetElement());
                //  string name = select.SelectedOption.Text;
                unit = new DTDecimalUnit(unitName);
            }

            return new DTDecimalValue(min, max, mean, unit);
        }

        public void WriteForm(WERefreshed elem, int attrId, DTDecimalValue value)
        {
            DTDecimalUnit unit = value.Unit;
            if (unit != null)
            {
                WEGDHtmlxComboBox combo = new WEGDHtmlxComboBox(elem.GetDriver(), unitWriteReadBy, elem);
                combo.SelectOption(unit.UnitName);
            }

            SetSubValue(elem, HDecimal.MinType, value.Min);
            SetSubValue(elem, HDecimal.MaxType, value.Max);
            SetSubValue(elem, HDecimal.MeanType, value.Mean);
           
            
        }

        private string ExtractDoubleFromString(string text)
        {
            //This is to skip leading characters which are not numbers
            // String number = new String(text.SkipWhile(x => ! Char.IsDigit(x)).ToArray());
            string number = "";
            foreach (char x in text)
                 if (char.IsDigit(x) || x == '-' || x == ',' || x == 'E' || x == '+' || x == '.' || x == 'e' )
                    number += x;

            return number;
        }

        private void SetSubValue(WERefreshed elem, string type, string value)
        {
            if (value != null)
            {
                WERefreshed elemValue = elem.GetDriver().WaitForElement(HDecimal.GetDecWriteAttr(type), elem);
                elem.GetDriver().ClickAt( HDecimal.GetDecWriteAttr(type), elem);
                //clear doesn't work in this case, have to use ctrl + a and delete
                elemValue.GetElement().SendKeys(Keys.Control + "a");
                elemValue.GetElement().SendKeys(Keys.Delete);
                //elemValue.GetElement().Clear();
                elemValue.GetElement().SendKeys(value);
            }
        }

        private string GetSubValue(WERefreshed elem, string type)
        {
            bool isPresent = elem.GetDriver().IsElementPresent(HDecimal.GetDecWriteAttr(type), elem.GetElement());
            string value = null;
            if (isPresent)
            {
                WERefreshed valueElem = elem.GetDriver().WaitForElement( HDecimal.GetDecWriteAttr(type), elem);
                value = valueElem.GetElement().GetAttribute("value");
            }
            return value;
        }

        public void DeleteData(WERefreshed elem)
        {
            elem.GetDriver().ClickAt(ElemByPictureName.FalseTxTr,elem);
        }
    }
}
