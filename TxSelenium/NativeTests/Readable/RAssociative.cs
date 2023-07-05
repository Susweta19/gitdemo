using OpenQA.Selenium;
using System;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.Handlers;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;
using System.Linq;

namespace TxSelenium.NativeTests.Readable
{
    public class RAssociative : WERefreshed
    {
        public RAssociative(TxWebDriver driver, By by, WERefreshed parent)
            : base(driver, by, parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="expectedHeaderName"></param>
        [TxAction("ReadHeader", "Check the header name according the colIndex Value.")]
        public string Headers(int colIndex)
        {
            WERefreshed rowElem = new WERefreshed(GetDriver(), By.XPath(".//tr[2]"), this);
            GetDriver().ScrollHorizontal(GetDriver().Driver.FindElement(By.XPath(".//div[starts-with(@id,'idDivAssodiv') or starts-with(@id,'idDivGriddiv')]/div[@class='xhdr']")), By.XPath(".//tr[2]/td[" + colIndex + "]"), this, false,true);
            WERefreshed cell = GetDriver().WaitForElement(By.XPath("./td[" + colIndex + "]"), rowElem);
            return cell.GetElement().Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="expectedHeaderName"></param>
        [TxAction("ClickOnHeader", "Click on header to sort data")]
        public void ClickOnHeader(int colIndex)
        {
            WERefreshed rowElem = new WERefreshed(GetDriver(), By.XPath(".//tr[2]"), this);
            GetDriver().ClickAt(By.XPath("./td[" + colIndex + "]"), rowElem);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        [TxAction("Value", "Gets the value of a specified cell and type...")]
        public T GetValue<T>(int colIndex, int rowIndex) where T : IReadableData
        {
            //rowIndex++;
            WERefreshed cell = new WERefreshed(GetDriver(), By.XPath(".//div[@class = \"objbox\"]//tr[" + rowIndex + "]//td[" + colIndex + "]"), this);
            //WERefreshed cell = GetDriver().WaitForElement(By.XPath(".//td[" + colIndex + "]"), rowElem);

            Func<WERefreshed, object> parser = HandlersFactory.GetAssoParser(typeof(WEGLink));
            object returnValue;

            if (cell.GetElement().Text == string.Empty && !GetDriver().HasChildren(cell.GetElement()))
            {
                returnValue = default(T);
            }
            else
            {
                parser = HandlersFactory.GetAssoParser(typeof(T));
                returnValue = parser.Invoke(cell);
            }
            return (T)returnValue;
        }

        [TxAction("ReadUnit", "ReadUnit only for multi unit decimal value")]
        public string ReadUnit(int colIndex, int rowIndex)
        {
            WERefreshed rowElem = new WERefreshed(GetDriver(), By.XPath(".//div[@class = \"objbox\"]//tr[" + rowIndex + "]"), this);
            WERefreshed cell = GetDriver().WaitForElement(By.XPath("./td[" + colIndex + "]"), rowElem);
            WEGDHtmlxComboBox unitCombobox = new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[starts-with(@id,\"idDivComboPoint\")]"),cell);
            return unitCombobox.Read();
        }

        [TxAction("Source", "Click on Source button and check the tracking modifications.")]
        public WDWindow<RForm> Source(int column)
        {
            //column = column*2 - 2;
            //column = (column-1)*2;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivGrid\")]/div[2]/table/tbody/tr[2]/td[" + column + "]/div/div[1]/img"));

            //Problem with getFirstTab for SourceWindow.

            //RForm rform = new RForm(GetDriver(), By.XPath(".//*[@ida = \"dhxMainCont\"]"));
            RForm rform = new RForm(GetDriver(), By.XPath(".//*[@ida = \"dhxMainCont\"]"), null, true);

            return new WDWindow<RForm>(GetDriver(), rform);
        }

        [TxAction("SpecificationList", "")]
        public WEGDHtmlxComboBox SpecificationList()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@class,\"trBlockSpecification\")]"), this);
        }
        [TxAction("ManageUnitinHeader", "")]
        public WEGDHtmlxComboBox ManageUnitinHeader(int colIndex, int rowIndex)
        {
            WERefreshed cell = GetDriver().WaitForElement(By.XPath(".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + colIndex + "]"), this);
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@class,\"dhxcombo_material\")]"), cell);
        }
        [TxAction("ExpandColoumn", "")]
        public void ExpandColoumn(int colIndex, int rowIndex)
        {
            WERefreshed cell = GetDriver().WaitForElement(By.XPath(".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + colIndex + "]"), this);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"plus\")]"), cell);
        }
        [TxAction("CollapseColoumn", "")]
        public void CollapseColoumn(int colIndex, int rowIndex)
        {
            WERefreshed cell = GetDriver().WaitForElement(By.XPath(".//div[@class = \"xhdr\"]//tr[" + (rowIndex + 1) + "]/td[" + colIndex + "]"), this);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"minus\")]"), cell);
        }
        [TxAction("ReadCellBackgoundColor", "")]
        public string ReadCellBackgoundColor(int colIndex, int rowIndex)
        {
            IWebElement cell = GetDriver().WaitForElement(By.XPath(".//div[@class = \"objbox\"]//tr[" + rowIndex + "]/td[" + colIndex + "]"), this).GetElement();
            string color = cell.GetAttribute("style").Split('(').Last().Split(')').First();
            return color;
        }
        [TxAction("ReadDecimalValue2", "Read Decimal Secoand Type")]
        public string ReadDecimalValue2(int colIndex, int rowIndex)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class = \"objbox\"]//tr[" + rowIndex + "]//td[" + colIndex + "]"), this).GetElement().Text.Replace("\r\n", " ");
        }

    }
}
