using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using System.Threading;


namespace TxSelenium.GenericTests.TxCommunity
{
    public class GTTxCommunity : WERefreshed
    {
        public GTTxCommunity(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        //Add a new thread Button
        [TxAction("AddNewThread", "Add a new thread")]
        public GTAddNewThread AddNewThread()
        {
            if (GetDriver().IsElementPresent(By.XPath(".//div[@class=\"dhxlayout_arrow dhxlayout_arrow_vb\"]/../..//div[@class=\"dhx_cell_layout dhxlayout_collapsed_v\"]")))
            {
                GetDriver().ClickAt(By.XPath(".//*[@id =\"idDivMainToolbar\"]//img[contains(@src,\"24x24-wDisplayTxCommunity.png\")]"));
                GetDriver().ClickAt(By.XPath(".//*[@id =\"IdDivTreeCommentToolBar\"]//img[contains(@src,\"20x20-AddObject.png\")]"));
                return new GTAddNewThread(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"), this);
            }
            else
            {
               
                GetDriver().ClickAt(By.XPath(".//*[@id =\"IdDivTreeCommentToolBar\"]//img[contains(@src,\"20x20-AddObject.png\")]"));
                return new GTAddNewThread(GetDriver(), By.XPath(".//div[@class=\"dhxlayout_cont\"]"), this);
            }
        }

        [TxAction("ManageComment", "To Read Comment.")]
        public GTComment ReadComment(int commentRow)
        {
            Thread.Sleep(1000);
            commentRow++;
            return new GTComment(GetDriver(), By.XPath(".//div[starts-with(@id,'IdDivTreeComment')]/div[@class='containerTableStyle']/table[@cellspacing='0']/tbody/tr[not(contains(@idnode,'0'))][" + commentRow + "]"));
            //return new GTComment(GetDriver(), By.XPath(".//*[@id = \"IdDivTreeComment\"]//tr[" + commentRow + "]"));
        }
        [TxAction("SelectComment", "To Read Comment.")]
        public void SelectComment(int commentRow)
        {
            Thread.Sleep(1000);

            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'IdDivTreeComment')]/div[@class='containerTableStyle']/table[@cellspacing='0']/tbody/tr[not(contains(@idnode,'0'))][" + (commentRow + 1) + "]//table"));
        }
    }
}
