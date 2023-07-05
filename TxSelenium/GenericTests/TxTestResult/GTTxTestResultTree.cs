using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTTxTestResultTree : WESTree
    {
        public GTTxTestResultTree(TxWebDriver driver, By elemIdentifier, By entityExpand, WERefreshed parent = null)
            :base(driver , elemIdentifier, entityExpand, parent,1)
        {

        }


        [TxAction("DeleteSubGridValue", "Delete Sub Grid Value")]
        public WESPopUp DeleteSubGridValue(ICollection<string> path)
        {

            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"16x16_false.png\")]"), GetEntity(path));
            return new WESPopUp(GetDriver());
        }


        /// <summary>
        /// Write value in cell.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        [TxAction("WriteCell", "Edit Cell")]
        public WEGInput WriteCell(ICollection<string> path, int Column)
        {
            GetDriver().DoubleClickAt(By.XPath(".//td[" + (Column + 1) + "]"), GetEntity(path));
            By Path = By.XPath(".//td[" + (Column + 1) + "]/ input");
            return new WEGInput(GetDriver(), Path, this);
        }



        [TxAction("ChangeUnit", "change the unit")]
        public WEGDHtmlxComboBox ChangeUnit(int gridIndex, int rowIndex, int colIndex)
        {
            rowIndex++;
            WERefreshed gridElem = GetDriver().WaitForElement(By.XPath(".//div[@class= \"objbox\"]/*[starts-with(@id,\"cgrid2\") and @class = \"dhx_sub_row\"][" + gridIndex + "]"), this);
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//tr[" + rowIndex + "]/td[" + colIndex + "]"), gridElem);
        }
    }

}
