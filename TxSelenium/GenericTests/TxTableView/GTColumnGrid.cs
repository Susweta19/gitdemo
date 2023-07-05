using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTableView
{
    public class GTColumnGrid : WERefreshed
    {
        public GTColumnGrid(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("SelectRow", "SelectRow")]
        public void SelectRow(int orderNum)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class='objbox']//tr[@class]["+orderNum+"]"),this);
        }

        [TxAction("ReadOrder", "ReadOrder")]
        public string ReadOrder(int rowNum)
        {
            SelectRow(rowNum);
            return GetDriver().WaitForElement(By.XPath(".//div[@class='objbox']//tr[@class][" + rowNum + "]/td[1]"), this).GetElement().Text;
        }

        [TxAction("NumberOfRow", "NumberOfRow")]
        public int NumberOfRow()
        {
            return GetDriver().FindElement(this.GetElement(),By.XPath(".//div[@class='objbox']")).FindElements(By.XPath(".//tr[@class]")).Count;
        }

        [TxAction("EditLabel", "EditLabel")]
        public WEGInput EditLabel(int orderNum)
        {
            SelectRow(orderNum);
            GetDriver().DoubleClickAt(By.XPath(".//div[@class='objbox']//tr[@class][" + orderNum + "]/td[2]"),this);
            return new WEGInput(GetDriver(), By.XPath(".//div[@class='objbox']//tr[@class][" + orderNum + "]/td[2]/input"), this);
        }

        [TxAction("ReadLabel", "ReadLabel")]
        public string ReadLabel(int orderNum)
        {
            SelectRow(orderNum);
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivLayoutEditConfigCellD\")]//div[@class='objbox']//tr[@class][" + orderNum + "]/td[2]")).GetElement().Text;
        }
    

        [TxAction("EditWidth", "EditWidth")]
        public WEGInput EditWidth(int orderNum)
        {
            SelectRow(orderNum);
            GetDriver().DoubleClickAt(By.XPath(".//div[@class='objbox']//tr[@class][" + orderNum + "]/td[3]"), this);
            return new WEGInput(GetDriver(), By.XPath(".//div[@class='objbox']//tr[@class][" + orderNum + "]/td[3]/input"), this);
        }

        [TxAction("ReadWidth", "ReadWidth")]
        public string ReadWidth(int orderNum)
        {
            SelectRow(orderNum);
            return GetDriver().WaitForElement(By.XPath(".//div[@class='objbox']//tr[@class][" + orderNum + "]/td[3]"), this).GetElement().Text;
        }

        [TxAction("MoveColumnUp", "MoveColumnUp")]
        public void MoveColumnUp(int orderNum)
        {
            SelectRow(orderNum);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,'-Up.png')]"),this);
        }

        [TxAction("MoveColumnDown", "MoveColumnDown")]
        public void MoveColumnDown(int orderNum)
        {
            SelectRow(orderNum);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,'Down.png')]"), this);
        }

        [TxAction("ShowCheckBoxCol", "ShowCheckBoxCol")]
        public WEGCheckBox ShowCheckBoxCol()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[@class=\"dhx_toolbar_btn dhxtoolbar_btn_def\"]//img[contains(@src,'chk0.gif')]"), this);
        }

        [TxAction("ShowObjectCol", "ShowObjectCol")]
        public WEGCheckBox ShowObjectCol()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[contains(@id,\"checkboxSpeCol1\")]/parent::div//img"), this);
            //(".//input[starts-with(@id,'checkboxSpeCol1')]")
        }

        [TxAction("EditCell", "EditLabel")]
        public WEGInput EditCell(int Row, int Column)
        {            
            GetDriver().DoubleClickAt(By.XPath(".//div[@class='objbox']//tr[@class][" + Row + "]/td[" + Column + "]"), this);
            return new WEGInput(GetDriver(), By.XPath(".//div[@class='objbox']//tr[@class][" + Row + "]/td[" + Column + "]/input"), this);
        }

        [TxAction("ReadCell", "Read any cell")]
        public string ReadCell(int Row, int Column)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class='objbox']//tr[@class][" + Row + "]/td[" + Column + "]"), this).GetElement().Text;
        }
    }
}
