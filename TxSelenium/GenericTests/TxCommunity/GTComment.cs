using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.Utils;

namespace TxSelenium.GenericTests.TxCommunity
{
    public class GTComment : WERefreshed
    {
        public GTComment(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("UserName", "To read SenderName comment")]
        public string UserName()
        {
            WERefreshed attribute = GetDriver().WaitForElement(By.ClassName("Comment_Information"), this);
            return attribute.GetElement().Text;
        }

        [TxAction("ReadComment", "To read comment")]
        public string ReadComment()
        {
            WERefreshed attribute = GetDriver().WaitForElement(By.ClassName("Parent_Comment"), this);
            return attribute.GetElement().Text;
        }

        [TxAction("ManageChildComment", "To read child comment")]
        public GTComment ChildComment(int childCommentIndex)
        {
            childCommentIndex++;
            return new GTComment(GetDriver(), By.XPath(".//table/tbody/tr[" + childCommentIndex + "]"), this);
        }

        [TxAction("Reply", "To give Reply")]
        public GTAddNewThread Reply()
        {
            GetDriver().ClickAt(By.XPath(".//span[@class=\"ReplyButton\"]"));
            return new GTAddNewThread(GetDriver(), By.XPath(".//div[@class=\"dhtmlx_wins_body_outer\"]"), this);
        }

        [TxAction("ModifyComment", "Modify a Comment and index start from 1")]
        public GTAddNewThread ModifyComment()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"EditChildComment\")] | .//img[contains(@id,\"EditComment\")] "), this);
            return new GTAddNewThread(GetDriver(), By.XPath(".//div[@class=\"dhtmlx_wins_body_outer\"]"), this);
        }

        [TxAction("DeleteComment", "Delete a Comment and index start from 1")]
        public void DeleteComment()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"DeleteComment\")]"), this);
        }

        [TxAction("ExpandCollapseComment", "Expand or Collapse a Comment")]
        public void ExpandCollapseComment()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@style, \"18x18-Comment\")]"), this);
        }
        [TxAction("IsButtonAvailable", "")]
        public bool IsButtonAvailable(string imageName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,\"" + imageName + ".png\")]"), this.GetElement());
        }
    }
}
