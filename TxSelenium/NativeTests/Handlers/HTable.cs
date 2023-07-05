using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    class HTable : IReadingHandler<DTTable>
    {
        private static readonly By tableLine = By.TagName("tr");
        private static readonly By tableCell = By.XPath(".//th|.//td");

        public DTTable ReadForm(WERefreshed elem, int attrId)
        {
            return ReadTable(elem);
        }

        public Func<WERefreshed, DTTable> GetAssoParser()
        {
            return ReadTable;
        }

        private DTTable ReadTable(WERefreshed elem)
        {
            string[,] tableContent;
            ICollection<IWebElement> tableRows = elem.GetElement().FindElements(tableLine);
            ICollection<IWebElement> lineCells;
            int columnCount = 0;
            if (tableRows.Count > 0)
            {
                columnCount = tableRows.ElementAt(0).FindElements(tableCell).Count;
            }
            tableContent = new string[tableRows.Count, columnCount];
            for (var i = 0; i < tableRows.Count; i++)
            {
                lineCells = tableRows.ElementAt(i).FindElements(tableCell);
                for (var j = 0; j < lineCells.Count; j++)
                {
                    tableContent[i,j] = lineCells.ElementAt(j).Text;
                }
            }
            return new DTTable(tableContent, elem);
        }      
    }
}
