using OpenQA.Selenium;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements
{
    /// <summary>
    /// Represents a clickable link.
    /// </summary>
    public class WEGLink : WERefreshed, IReadableData
    {
        /// <summary>
        /// The text of the link.
        /// </summary>
        public string Text {
            [TxAction("GetText", "Gets the displayed text value.")]
            get; 
            private set;
        }

        public WEGLink(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            Text = driver.WaitForElement(elemIdentifier, parent).GetElement().Text;
        }

        /// <summary>
        /// Clicks on the link.
        /// </summary>
        [TxAction("Click", "Clicks the link.")]
        public void Click()
        {
            GetDriver().ClickAt(ElemIdentifier, Parent);
        }
        [TxAction("TextContains", "")]
        public bool TextContains(string value)
        {
            return Text.Contains(value);
        }
      
        [TxAction("EmailShapeButton", "Open Email Window")]
        public void EmailButton()
        {
            GetDriver().ClickAt(ElemByPictureName.SendMailLink, this);
        }
        [TxAction("DoubleClick", "Clicks the link.")]
        public void DoubleClick()
        {
            GetDriver().DoubleClickAt(ElemIdentifier, Parent);
        }
          [TxAction("LinkList", "")]
        public XmlExecutor.DataTypes.ActionColl<string> LinkList()
        {
            System.Collections.Generic.List<string> links = new System.Collections.Generic.List<string>();
            System.Collections.Generic.ICollection<IWebElement> elems = this.Parent.GetElement().FindElements(By.XPath(".//a"));
            foreach (var elem in elems)
                links.Add(elem.Text);
            return new XmlExecutor.DataTypes.ActionColl<string>(links);
        }
    }
}
