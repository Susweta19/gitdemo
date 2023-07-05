using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxCalendar
{
    public class AddTask : WERefreshed
    {
        public AddTask(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }
        [TxAction("ManageStartDate", ".....")]
        public WDate ManageStartDate()
        {
            GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivStartDate\")]//img[starts-with(@src,\"/temp_resources/theme/img/icons/calendar.png\")]"), this).GetElement().Click();
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));
        }
        [TxAction("ManageEndDate", ".....")]
        public WDate ManageEndtDate()
        {
            GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivEndDate\")]//img[starts-with(@src,\"/temp_resources/theme/img/icons/calendar.png\")]"), this).GetElement().Click();
            return new WDate(GetDriver(), By.XPath(".//div[@class=\"dhtmlxcalendar_material dhtmlxcalendar_in_input\"]"));
        }
        [TxAction("AssignMoreResources", ".....")]
        public WDValidatedWindow<WMultipleLink> AssignMoreResources()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"AddObject.png\")]"), this);
            GetDriver().WaitForAjax();
            WMultipleLink multiplelink = new WMultipleLink(GetDriver(), WForm.WriteFormDiv, WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), multiplelink);
        }

        [TxAction("ReadResourceDetails", ".....")]
        public string ReadResourceDetails(int row, int col)
        {

            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"objbox\"]//tr[@class][" + row + "]//td[" + col + "]"), this).GetElement().Text;
        }
        [TxAction("RemoveResource", ".....")]
        public void RemoveResource(int row)
        {
            GetDriver().WaitForElement(By.XPath(".//div[@class=\"objbox\"]//tr[@class][" + row + "]//td[3]/..//img"), this).GetElement().Click();
        }
        [TxAction("ReadNumberOfResource", ".....")]
        public int ReadNumberOfResource()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"objbox\"]"), this).GetElement().FindElements(By.XPath(".//tr[@class]")).Count;
        }

        [TxAction("ReadCurrentTab", ".....")]
        public string ReadCurrentTab()
        {
            return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhxtabbar_tab dhxtabbar_tab_actv\"]"), this).GetElement().Text;
        }

        [TxAction("AttributeName", "Read and check the name of any attribute.")]
        public string AttributeName(int attrId)
        {
            WERefreshed attribute = GetDriver().WaitForElement(By.XPath(".//*[starts-with(@id , \"idLabelAttNameLabel" + attrId + "\")]"), this);
            return attribute.GetElement().Text;
        }

        [TxAction("GetDayOfWeek", ".....")]
        public int GetDayOfWeek(string date)
        {
            DateTime currentDateTime = Convert.ToDateTime(date);
            int week = (int)currentDateTime.DayOfWeek;

            return week;
        }
        [TxAction("EditResourceDetails", ".....")]
        public WEGInput EditResourceDetails(string row, string column)
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[@id=\"idDivGrid\"]//div[@class=\"objbox\"]//tr[@class][" + row + "]//td[" + column + "]"), this);
            GetDriver().DoubleClickAt(By.XPath(".//div[@id=\"idDivGrid\"]//div[@class=\"objbox\"]//tr[@class][" + row + "]//td[" + column + "]"), this);
            return new WEGInput(GetDriver(), By.XPath(".//input"), elem);
        }

        [TxAction("ReadAttributeValue", "......")]
        public string ReadAttributeValue(int attrId)
        {
            WERefreshed attribute = GetDriver().WaitForElement(By.XPath(".//*[starts-with(@slabelhtmlid , \"Label" + attrId + "\")]//input"), this);
            return attribute.GetElement().Text;
        }

        [TxAction("IsAttributeValueContains", "....")]
        public bool IsAttributeValueContains(int attrId, string value)
        {
           // WEGInput attribute = new WEGInput(GetDriver(), By.XPath(".//*[starts-with(@slabelhtmlid , \"Label" + attrId + "\")]//input"), this);
           string aa= GetDriver().FindElement(By.XPath(".//*[starts-with(@slabelhtmlid , \"Label" + attrId + "\")]//input")).GetAttribute("value");
            int i = 0;
            if (aa.Contains(value))
            {
                return true;
            }
            else
                return false;
        }
        [TxAction("WriteValue", "....")]
        public WEGInput WriteValue(int attrId)
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"minValdiv" + attrId + "\")]"), this);
        }

        [TxAction("RemoveStartDate", "....")]
        public void RemoveStartDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivStartDate\")]//img[contains(@src,\"16x16-False.png\")]"), this);
        }

        [TxAction("RemoveEndDate", "....")]
        public void RemoveEndDate()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivEndDate\")]//img[contains(@src,\"16x16-False.png\")]"), this);
        }
    }
}
