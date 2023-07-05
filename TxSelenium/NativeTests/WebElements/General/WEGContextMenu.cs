using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.WebElements
{
    public class WEGContextMenu
    {

        private static By activeMenusBy = By.XPath("//div[contains(@class,\"dhtmlxMenu_material_SubLevelArea_Polygon\") and not(contains(@style,\"display: none;\"))]");

        private static By GetEntryBy(string entryText)
        {
            return new ByChained(activeMenusBy, By.XPath(".//div[text()='" + entryText + "']"));
        }

        private TxWebDriver driver;

        public WEGContextMenu(TxWebDriver driver)
        {
            this.driver = driver;
        }

        [TxAction("SelectEntry", "Selects an entry in a context menu and clicks on it. This will most likely close the menu.")]
        public void SelectEntry(ICollection<string> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                string entryText = path.ElementAt(i);
                By entryBy = WEGContextMenu.GetEntryBy(entryText);
                WERefreshed entryElem = driver.WaitForElement(entryBy);
                driver.MouseOver(entryElem);
                if (i != path.Count - 1)
                {
                    //The path is not finished it means that this element opens a submenu.
                    //If selenium is asked to directly move the mouse over an element in that submenu
                    //it might pass over an element of the current menu making the sub menu disappear.
                    //To avoid that the cursor is first moved to the right over the submenu.
                    int xOffset = (entryElem.GetElement().Size.Width + 10) / 2;
                    driver.MouseMove(xOffset, 0);
                }
                else
                    driver.ClickCursor();
            }
        }
    }
}
