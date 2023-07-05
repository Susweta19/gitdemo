using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TxSelenium.GenericTests.TxTableView;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.GenericTests.TxPortal
{
    public class Portal : WERefreshed
    {
        public Portal(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }
        [TxAction("ReadSideBar", "Click on a selected link inside the main window.")]
        public ActionColl<string> ReadSideBar()
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idSubDivPortal\")]")).GetElement().FindElements(By.XPath(".//span[starts-with(@class,\"clPortalSidebar-itemName\")]")
);
            ICollection<string> sidebar = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
                sidebar.Add(elem.Text);
            return new ActionColl<string>(sidebar);
        }
        [TxAction("ReadNumberOfTiles", "Click on a selected link inside the main window.")]
        public string ReadNumberOfTiles()
        {
            return GetDriver().FindElement(By.XPath(".//div[starts-with(@class,\"portalDyTilesTextContainer\")]")).FindElements(By.XPath(".//div")).Count.ToString();
        }
        [TxAction("ExpandSlidebarItem", "Click on a selected link inside the main window.")]
        public void ExpandSlidebarItem(string title)
        {
            try
            {
                GetDriver().ClickAt(By.XPath(".//span[@class=\"clPortalSidebar-itemName\" and text()=\"" + title + " \"]/..|.//span[text()=\"" + title + "\"]/parent::span[@class=\"clPortalSidebar-itemName\"]/.."), this);
            }
            catch (Exception e)
            {
                GetDriver().ClickAt(By.XPath(".//span[@class=\"clPortalSidebar-itemName\" and text()=\"" + title + "\"]/..|.//span[text()=\"" + title + "\"]/parent::span[@class=\"clPortalSidebar-itemName\"]/.."));
            }
        }
        [TxAction("IsSlidebarItemOpened", "Check Slidebar Item is opened")]
        public bool IsSlidebarItemOpened(string title)
        {
            string classlist = GetDriver().WaitForElement(By.XPath($".//div[starts-with(@class,\"clPortalSidebar-item\") and contains(@title,\"{title}\")]"), this).GetElement().GetAttribute("Class");
            return classlist.Contains("clPortalSidebar-itemOpened");
        }
        [TxAction("ReadTileTitle", "Click on a selected link inside the main window.")]
        public ActionColl<string> ReadTilesTitle()
        {
            ICollection<IWebElement> elementList = this.GetElement().FindElements(By.XPath(".//div[starts-with(@id,\"tileTitle\")]"));
            ICollection<string> tileTiles = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
            {
                if (elem.Text != "")
                    tileTiles.Add(elem.Text);
            }
            return new ActionColl<string>(tileTiles);
        }
        [TxAction("ClickOnPortalSubLink", "Click on a selected link inside the main window.")]
        public void ClickOnPortalSubLink(string linkTitle)
        {
            GetDriver().ClickAt(By.XPath($".//span[@class=\"clPortalSidebar-subItemText\" and text()=\"{linkTitle}\"]"), this);
        }
        [TxAction("ReadPortalSublinks", "Click on a selected link inside the main window.")]
        public ActionColl<string> ReadPortalSublinks(string title)
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath($".//div[starts-with(@class,\"clPortalSidebar-item\") and contains(@title,\"{title}\")]/..")).GetElement().FindElements(By.XPath(".//span[@class=\"clPortalSidebar-subItemText\"]"));
            ICollection<string> Sublinks = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
                Sublinks.Add(elem.Text);
            return new ActionColl<string>(Sublinks);
        }
        [TxAction("ReturnTxTableView", "Click on a selected link inside the main window.")]
        public GTTxTableView ReturnTxTableView(string index = "1")
        {
            WERefreshed parrent = GetDriver().WaitForElement(By.XPath(".//div[@id=\"portalTemplateSidebarContent\"]/div[contains(@style,\"display: block;\")]//div[contains(@class,\"clPortalTVContainer\")]"), this);
            return new GTTxTableView(GetDriver(), By.XPath(".//div[contains(@class,\"clPortalTVWrapper\")][" + index + "]"), parrent, true);
        }
        [TxAction("IsQuickSearchbarPresent", "Click on a selected link inside the main window.")]
        public bool IsQuickSearchbarPresent(string title)
        {
            return GetDriver().IsElementPresent(By.XPath($".//div[starts-with(@class,\"clPortalSidebar-item\") and contains(@title,\"{title}\")]/..//div[@class=\"clPortalSidebar-subItem\"]//quicksearch"), this.GetElement());
        }
        [TxAction("ReadNoofSublinks", "Click on a selected link inside the main window.")]
        public string ReadNoofSublinks(string title)
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath($".//div[starts-with(@class,\"clPortalSidebar-item\") and contains(@title,\"{title}\")]/..")).GetElement().FindElements(By.XPath($".//span[@class=\"clPortalSidebar-subItemText\"]"));
            return elementList.Count.ToString();
        }
        [TxAction("ClickonLeftSliderArrow", "Click on a selected link inside the main window.")]
        public void ClickonLeftSliderArrow()
        {
            WERefreshed parrent = GetDriver().WaitForElement(By.XPath(".//div[@id=\"tileCarousel\"]"), this);
            GetDriver().MoveToElement(By.XPath(".//div[contains(@class,\"clPortalTextTile\") and contains(@style,\"display: block;\")]"), parrent);
            GetDriver().ClickAt(By.XPath(".//span[contains(@class,\"clPortalTile-SliderArrow left\")]"), parrent);
        }
        [TxAction("ClickonRightSliderArrow", "Click on a selected link inside the main window.")]
        public void ClickonRightSliderArrow()
        {
            WERefreshed parrent = GetDriver().WaitForElement(By.XPath(".//div[@id=\"tileCarousel\"]"), this);
            GetDriver().MoveToElement(By.XPath(".//div[contains(@class,\"clPortalTextTile\") and contains(@style,\"display: block;\")]"), parrent);
            GetDriver().ClickAt(By.XPath(".//span[contains(@class,\"clPortalTile-SliderArrow right\")]"), parrent);
        }
        [TxAction("ChangeSliderDot", "Click on a selected link inside the main window.")]
        public void ChangeSliderDot(string index)
        {
            WERefreshed parrent = GetDriver().WaitForElement(By.XPath(".//div[@id=\"tileCarousel\"]"), this);
            GetDriver().ClickAt(By.XPath($".//div[@class=\"clPortalTile-SliderBottom\"]//span[contains(@class,\"clPortalTile-SliderCircle\")][{index}]"), parrent);
        }
        [TxAction("ReadPortalButtons", "Read all Portal Buttons")]
        public ActionColl<string> ReadPortalButtons()
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath(".//portalbuttons")).GetElement().FindElements(By.XPath(".//button"));
            ICollection<string> buttons = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
                buttons.Add(elem.Text);
            return new ActionColl<string>(buttons);
        }
        [TxAction("ReadPortalLinkTileTitle", "Click on a selected link inside the main window.")]
        public ActionColl<string> ReadPortalLinkTileTitle()
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath(".//div[@class=\"portalDyTilesLinkContainer\"]"), this).GetElement().FindElements(By.XPath(".//div[@class=\"clPortalTile-header\"]"));
            ICollection<string> tileTiles = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
            {
                if (elem.Text != "")
                    tileTiles.Add(elem.Text);
            }
            return new ActionColl<string>(tileTiles);
        }
        [TxAction("ReadNumberofPortalLinkTileTitle", "Click on a selected link inside the main window.")]
        public string ReadNumberofPortalLinkTileTitle()
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath(".//div[@class=\"portalDyTilesLinkContainer\"]"), this).GetElement().FindElements(By.XPath(".//div[contains(@class,\"clPortalLinkTile\")]"));
            return elementList.Count.ToString();
        }
        [TxAction("ClickonLinkofPortalLinkTile", "Click on a selected link inside the main window.")]
        public void ClickonLinkofPortalLinkTile(string index, string LinkText)
        {
            GetDriver().MoveToElement(By.XPath($".//div[@class=\"portalDyTilesLinkContainer\"]//div[contains(@class,\"clPortalLinkTile\")][{index}]"), this);
            GetDriver().ClickAt(By.XPath($".//div[@class=\"portalDyTilesLinkContainer\"]//div[contains(@class,\"clPortalLinkTile\")][{index}]//span[text()=\"{LinkText}\"]"), this);
        }
        [TxAction("ClickonPortalButton", "Click on a Portal Button")]
        public void ClickonPortalButton(string text)
        {
            GetDriver().ClickAt(By.XPath($".//portalbuttons//button[text()=\"{text}\"]|.//portalbuttons//span[text()=\"{text}\"]/.."));
        }
        [TxAction("ClickonPortalTileButton", "Click on a Portal Button in Tile")]
        public void ClickonPortalTileButton(string text)
        {
            GetDriver().ClickAt(By.XPath($".//div[text()=\"{text}\" and @class=\"clPortalTile-Button\"]"));
        }
        [TxAction("ReadTileContent", "Click on a selected link inside the main window.")]
        public ActionColl<string> ReadTileContent()
        {
            ICollection<IWebElement> elementList = this.GetElement().FindElements(By.XPath(".//div[@class=\"clPortalTile-TextContent\"]"));
            ICollection<string> tileTiles = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
            {
                if (elem.Text != "")
                    tileTiles.Add(elem.Text);
            }
            return new ActionColl<string>(tileTiles);
        }
        [TxAction("PortalQuickSearch", "Manage Search")]
        public WEGInput PortalQuickSearch(string slidebarItemTitle)
        {
            return new WEGInput(GetDriver(), By.XPath($".//div[starts-with(@class,\"clPortalSidebar-item\") and contains(@title,\"{slidebarItemTitle}\")]/..//div[@class=\"clPortalSidebar-subItem\"]//quicksearch/input"), this);
        }
        [TxAction("PortalQuickSearchResults", "Read all Portal Buttons")]
        public ActionColl<string> PortalQuickSearchResults()
        {
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath(".//div[@class=\"clPortalQuickSearchResultsContainer\" and contains(@style,\"display: block;\")]")).GetElement().FindElements(By.XPath(".//li"));
            ICollection<string> buttons = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
                buttons.Add(elem.Text);
            return new ActionColl<string>(buttons);
        }
        [TxAction("IsQuickSearchResultsPresent", "Click on a selected link inside the main window.")]
        public bool IsQuickSearchResultsPresent(string title)
        {
            return this.GetElement().FindElement(By.XPath($".//div[starts-with(@class,\"clPortalSidebar-item\") and contains(@title,\"{title}\")]/..//div[@class=\"clPortalQuickSearchResultsContainer\"]")).Displayed;
        }
        [TxAction("IsEmojiPresent", "")]
        public bool IsEmojiPresent(string title, string emojiType)
        {
            return GetDriver().IsElementPresent(By.XPath($".//span[@class=\"clPortalSidebar-subItemText\" and text()=\"{title}\"]/..//span[contains(@class,'fal fa-{emojiType}')]"), this.GetElement());
        }
        [TxAction("ClickonPortalQuickSearchResult", "Click on a Portal Button in Tile")]
        public void ClickonPortalQuickSearchResult(string text, bool doubleClick = false)
        {
            GetDriver().ClickAt(By.XPath(".//div[@class=\"clPortalQuickSearchResultsContainer\" and contains(@style,\"display: block;\")]//div[text()=\"" + text + "\"]/.."), this);
            if (doubleClick)
                GetDriver().ClickAt(By.XPath(".//div[@class=\"clPortalQuickSearchResultsContainer\" and contains(@style,\"display: block;\")]//div[text()=\"" + text + "\"]"), this);
        }
        [TxAction("ReadLinksofPortalLinkTile", "Click on a selected link inside the main window.")]
        public ActionColl<string> ReadLinksofPortalLinkTile(string index)
        {
            GetDriver().MoveToElement(By.XPath($".//div[@class=\"portalDyTilesLinkContainer\"]//div[contains(@class,\"clPortalLinkTile\")][{index}]"), this);
            ICollection<IWebElement> elementList = GetDriver().WaitForElement(By.XPath($".//div[@class=\"portalDyTilesLinkContainer\"]//div[contains(@class,\"clPortalLinkTile\")][{index}]"), this).GetElement().FindElements(By.XPath(".//span[@class=\"clPortalTile-itemName\"]"));
            ICollection<string> links = new List<string>(elementList.Count);
            foreach (IWebElement elem in elementList)
                links.Add(elem.Text);
            return new ActionColl<string>(links);
        }

    }
}
