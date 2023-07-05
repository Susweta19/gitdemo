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

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTManageSeriesTable : WERefreshed
    {
        public GTManageSeriesTable(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ReadCellValue", "ReadCellValue")]
        public string ReadCellValue(int row,int col)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,'objbox')]//tr[@class][" + row + "]/td[" + col + "]"), this).GetElement().Text.Replace(" ",string.Empty);
        }

        [TxAction("EditCellValue", "EditCellValue")]
        public WEGInput EditCellValue(int row, int col)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[starts-with(@class,'objbox')]//tr[@class][" + row + "]/td[" + col + "]"),this);
            return new WEGInput(GetDriver(),By.XPath(".//tr[@class]["+row+"]/td["+col+"]//input[contains(@class,'edit')]"),this);
        }

        [TxAction("SelectTableRow", "SelectTableRow")]
        public void SelectTableRow(int row)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,'objbox')]//tr[@class][" + row + "]"),this);
        }

        [TxAction("ScrollToRowNumber", "ReadRowNumber")]
        public void ScrollToRowNumber(int toRowNumber)
        {
            IWebElement elem = new WERefreshed(GetDriver(), By.XPath(".//div[@class=\"sideBarItemContainer\" and contains(@style,\"display: block\")]//div[starts-with(@id,'chartData') and contains(@class,'chartData gridbox')]//div[@class='objbox']")).GetElement();
            GetDriver().ScrollToBottom(elem, By.XPath(".//div[starts-with(@id,'chartData') and contains(@class,'chartData gridbox')]//tr[@class][" + toRowNumber + "]"),null);
        }

        [TxAction("ReadRowNumber", "ReadRowNumber")]
        public int ReadRowNumber()
        { 
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'chartData') and contains(@class,'chartData gridbox')]"), this).GetElement().FindElements(By.XPath(".//tr[@class]")).Count;
        }

        [TxAction("ManageRightClickOptions", "ManageRightClickOptions")]
        public GTManageRightClickOptions ManageRightClickOptions()
        {
            WERefreshed elem = new WERefreshed(GetDriver(),By.XPath(".//tr[contains(@class,'rowselected')]"),this);
            GetDriver().RightClick(elem.GetElement(),10,10);
            return new GTManageRightClickOptions(GetDriver(),By.XPath(".//div[starts-with(@class,'dhtmlxMenu_dhx_skyblue_SubLevelArea_Polygon ') and not(contains(@style,'display: none;'))]"));
        }
        [TxAction("ColourValue", "colur value put on ")]
        public void ColourValue(string colourvalue)
        {
            GetDriver().FindElement(By.XPath(".//input[@class=\"dhxcp_value\"]")).SendKeys(colourvalue);
            GetDriver().ClickAt(By.ClassName("dhxcp_sub_area"), this);
          
        }
        [TxAction("SelectStyle", "Graph Style selected")]
        public void SelectStyle(string id)
        {
            GetDriver().ClickAt(By.XPath(".//span[@data-icon-id=\"" + id + "\"]"), this);
        }
        //   [TxAction("clickserries", "Graph Style selected")]
        //   public bool clickserries()
        //   {
        //       GetDriver().ClickAt(By.XPath(".//*[@transform='translate(8,3)']"), this);
        //return GetDriver().IsElementPresent(By.XPath(".//*[@style='color:#CCC;font-size:12px;font-weight:bold;cursor:pointer;fill:#CCC;']"));


        //   }


        [TxAction("IsSeriescolour", "ManageAdvanced")]
        public bool IsSeriescolour(int row, int col)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,'objbox')]//tr[@class][" + row + "]/td[" + col + "]"), this).GetElement().GetAttribute("bgcolor")== "#E0B6B6";
        }
        [TxAction("ReadCellColor", "ManageAdvanced")]
        public string ReadCellColor(int row, int col)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@class,'objbox')]//tr[@class][" + row + "]/td[" + col + "]"), this).GetElement().GetAttribute("bgcolor");
        }

    }
}
