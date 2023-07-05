using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TXSpecification
{
    public class ManageDashBoard : WERefreshed
        {
            public ManageDashBoard(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
                : base(driver, elemIdentifier, parent, frameBy)
            {
            }

        [TxAction("ObjectType", "")]
        public WEGDHtmlxComboBox ObjectType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"spcDBMObjTypeCombo\")]"), this);
        }
        [TxAction("ControlCard", "")]
        public WEGInput ControlCard()
        {
          //  if (!GetDriver().IsElementPresent(By.XPath(".//div[text()=\"Control Card\"]/../../..//tr[3]/td[1]"), this.GetElement()))
           //     GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-search.png\")]"));
            return new WEGInput(GetDriver(), (By.XPath(".//div[text()=\"Control Card\"]/../../..//tr[3]/td[1]//input")));
        }
        [TxAction("Description", "")]
        public WEGInput Description()
        {
            //  if (!GetDriver().IsElementPresent(By.XPath(".//div[text()=\"Control Card\"]/../../..//tr[3]/td[1]"), this.GetElement()))
            //     GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"fal_fa-search.png\")]"));
            return new WEGInput(GetDriver(), (By.XPath(".//div[text()=\"Control Card\"]/../../..//tr[3]/td[2]//input")));
        }

        [TxAction("PushButton", "")]
        public void PushButton()
        {
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id,\"spcDBMPushButton\")]"),this);
        }

        [TxAction("RemoveButton", "")]
        public void RemoveButton()
        {
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id,\"spcDBMRemoveButton\")]"),this);
          
        }
        [TxAction("SelectRowInLeftSide", "")]
        public void SelectRowInLeftSide(int row)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"spcDBMListCardsGrid\")]//div[@class=\"objbox\"]//tr[" + (row + 1) + "]"), this);
        }
        [TxAction("SelectRowInRightSide", "")]
        public void SelectRowInRightSide(int row)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"spcDBMSelectedCardsGrid\")]//div[@class=\"objbox\"]//tr[" + (row + 1) + "]"), this);
        }
        [TxAction("ReadNumberOfRowInRightSide", "")]
        public int ReadNumberOfRowInRightSide()
        {
          int row= GetDriver().FindElement(By.XPath(".//div[starts-with(@id,\"spcDBMSelectedCardsGrid\")]//div[@class=\"objbox\"]")).FindElements(By.XPath(".//tr")).Count;
            return row - 1;
        }
    }
}
