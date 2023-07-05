using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDSearchBox : WERefreshed
    {
        private static readonly By advancedSettingsButton = By.XPath(".//input[starts-with(@id , \"idBtnShowAdvancedSettings\")]");
        private static readonly By hideAdvancedSettingsButton = By.XPath(".//input[starts-with(@id , \"idBtnHideAdvancedSettings\")]");
        private static readonly By searchButton = By.XPath(".//input[starts-with(@id , \"idBtnSearchInTeexma\")]");
        private static readonly By closeButton = By.XPath(".//input[starts-with(@id , \"idBtnCloseSearch\")]");
        private static readonly By classicSearch = By.XPath(".//input[starts-with(@id,\"idTextAndWord\")]");
        private static readonly By atLeastSearch = By.XPath(".//input[starts-with(@id,\"idTextOrWord\")]");
        private static readonly By noneSearch = By.XPath(".//input[starts-with(@id,\"idTextWithoutWord\")]");
        private static readonly By checkboxData = By.XPath(".//input[starts-with(@id,\"idCheckboxData\")]");

        public WDSearchBox(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("AllOfTheFollowingWords", "Fill the input and tick checkbox.")]
        public WEGInput AllOfTheFollowingWords()
        {
            return new WEGInput(GetDriver(), WDSearchBox.classicSearch, this);
        }

        [TxAction("AtLeastOneOfTheFollowingWords", "Fill the input and tick checkbox.")]
        public WEGInput AtLeastOneOfTheFollowingWords()
        {
            return new WEGInput(GetDriver(), WDSearchBox.atLeastSearch, this);
        }

        [TxAction("NoneOfTheFollowingWords", "Fill the input and tick checkbox.")]
        public WEGInput NoneOfTheFollowingWords()
        {
            return new WEGInput(GetDriver(), WDSearchBox.noneSearch, this);
        }

        /// <summary>
        /// Click on Search button.
        /// </summary>
        [TxAction("SearchBothDataAndObjectNames", "")]
        public WEGCheckBox SearchBothDataAndObjectNames()
        {
            return new WEGCheckBox(GetDriver(), WDSearchBox.checkboxData, this);
        }

        /// <summary>
        /// Click on Search button.
        /// </summary>
        [TxAction("SearchButton", "Click on search button")]
        public void SearchButton()
        {
            GetDriver().ClickAt(WDSearchBox.searchButton, this);
        }

        /// <summary>
        /// Click on Search button.
        /// </summary>
        [TxAction("DisplayAdavancedSettings", "Click on Display Adavanced Settings button")]
        public void DisplayAdavancedSettings()
        {
            GetDriver().ClickAt(WDSearchBox.advancedSettingsButton, this);
        }

        /// <summary>
        /// Click on Search button.
        /// </summary>
        [TxAction("HideAdavancedSettings", "Click on Hide Adavanced Settings button")]
        public void HideAdavancedSettings()
        {
            GetDriver().ClickAt(WDSearchBox.hideAdvancedSettingsButton, this);
        }

        /// <summary>
        /// Close the search box window by the close Button
        /// </summary>
        [TxAction("CloseButton", "close the searchbox window by the close button")]
        public void CloseButton()
        {
            GetDriver().ClickAt(WDSearchBox.closeButton, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ot"></param>
        /// <param name="entity"></param>
        [TxAction("Results", "Select entity in result column")]
        public WESTree Results()
        {
            return new WESTree(GetDriver(), By.XPath(".//div[starts-with(@id,\"idDivTreeObjects\")]"), WESTree.expandByClassicTree, this);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ot"></param>
        /// <param name="entity"></param>
        [TxAction("PageResultsManagement", "Select entity in result column")]
        public void PageResultsManagement(string pageNumber = null, string typeDisplay = null)
        {
            if (pageNumber != null)
            {
                GetDriver().ClickAt(By.XPath(".//div[2]/div[contains(@class , \"arwimg\")]"), this);
                GetDriver().ClickAt(By.XPath(".//div[@class = \"btn_sel_text\" and text() = \"" + pageNumber + "\"]"));
            }

            if (typeDisplay != null)
            {
                GetDriver().ClickAt(By.XPath(".//div[4]/div[contains(@class , \"arwimg\")]"), this);
                GetDriver().ClickAt(By.XPath(".//div[@class = \"btn_sel_text\" and text() = \"" + typeDisplay + "\"]"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjectTypeName"></param>
        [TxAction("SelectObjectType", "Select OT by clicking in OT column")]
        public void SelectObjectType(string ObjectTypeName)
        {
            GetDriver().ClickAt(new ByChained(By.XPath(".//div[starts-with(@id,\"idDivGridOT\")]"), By.XPath(".//td[contains(text(), \"" + ObjectTypeName + "\")]")), this);

        }

        [TxAction("DisplayNextPage", "Display the next page")]
        public void DisplayNextPage()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Display next page\"]"), this);
        }

        [TxAction("DisplayLastPage", "Display the Last page")]
        public void DisplayLastPage(int NextPrevious)
        {
            if (NextPrevious == 0)
                GetDriver().ClickAt(By.XPath(".//div[@title=\"Display the last page\"]"), this);
            if (NextPrevious == 1)
                GetDriver().ClickAt(By.XPath(".//div[@title=\"Display the first page\"]"), this);
        }

        [TxAction("DisplayPreviousPage", "Display the Last page")]
        public void DisplayPreviousPage()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title=\"Show previous page\"]"), this);
        }

       
        [TxAction("DisplayedObjects", "Check the results number of displayed objects.")]
        public string DisplayedObjects(string otName = "All Object Types")
        {
            GetDriver().WaitForAjax();
            WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//td[contains(text(),\"" + otName + "\")]/../td[@align=\"right\"]"), this);
            return elem.GetElement().Text;
        }
    }
}
