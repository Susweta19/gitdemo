using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxGantt
{
    public class ManageRightOption : WERefreshed
    {
        public ManageRightOption(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("NameAssending", "")]
        public void NameAssending()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Name ascending\"]"));
        }
        [TxAction("NameDescending", "")]
        public void Namedescending()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Name descending\"]"));
        }
        [TxAction("StartAateAscending", "")]
        public void StartAateAscending()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Start date ascending\"]"));
        }
        [TxAction("StartDateDescending", "")]
        public void StartDateDescending()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Start date descending\"]"));
        }
        [TxAction("Zones", "")]
        public void Zones()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Zones\"]"));
        }
        [TxAction("Technician", "")]
        public void Technician()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Technician\"]"));
        }
        [TxAction("Deadline", "")]
        public void Deadline()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Deadline\"]"));
        }
        [TxAction("Machine", "")]
        public void Machine()
        {
            GetDriver().WaitForElement(By.XPath(".//div[text()=\"Machine\"]"));
        }
    }
}
