using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.Handlers;

namespace TxSelenium.GenericTests.WebDav
{
    public class EditFileForm : WERefreshed
    {
        public EditFileForm(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("Value", "Gets the value of a specified cell and type...")]
        public T GetValue<T>(int colIndex, int rowIndex) where T : IReadableData
        {
            WERefreshed cell = new WERefreshed(GetDriver(), By.XPath(".//div[@class = \"objbox\"]//tr[@class][" + rowIndex + "]//td[" + colIndex + "]"), this);

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
    }
}
