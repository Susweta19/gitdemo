using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.WebElements
{
    /// <summary>
    /// class managing all the combobox in teexma
    /// </summary>
    public class WEGDHtmlxComboBox : WERefreshed
    {
        private static readonly By comboList = By.XPath(".//div[contains(@class , \"dhx_combo_list\") and not(contains(@style ,\"display: none\"))] | "+
            ".//div[contains(@class , \"dhxcombolist_dhx_skyblue\") and not(contains(@style ,\"display: none\"))] | "+
            ".//div[contains(@class , \"dhxcombolist_material\") and not(contains(@style ,\"display: none\"))] | .//div[@id=\"cmb_liste\"]");
        public static readonly By comboImg = By.XPath(".//div[@class = \"dhxcombo_select_img\"] |.//img[@id = \"cmb_img\"]|.//img[@class=\"clDivFictiveTxComboImg\"] | .//div[contains(@class,\"dhxcombo_checkbox\")]");

        /// <summary>
        /// Using for list element of combo box (located up in the frame not as a child of WEGDHtmlxComboBox) 
        /// </summary>
        private By currentFrameBy;

        public WEGDHtmlxComboBox(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By currentFrameBy = null)
            : base(driver, elemIdentifier, parent)
        {
            this.currentFrameBy = currentFrameBy;
            base.WaitForElement();
        }

        [TxAction("Select", "....")]
        public void SelectOption(string value)
        {
            GetDriver().ClickAt(comboImg, this);
            //can we use _driver.currentFrameBy instead of currentFrameBy ????
            WaitForAjax();
            WERefreshed comboListElem = new WERefreshed(GetDriver(), comboList, null, currentFrameBy);
            //to catch the space in dom
            //value = value.Replace(' ', '\u00a0');
            if (value.Contains('\''))
                GetDriver().ClickAt(By.XPath(".//div[text() =\"" + value + "\"]"), comboListElem);
            else
                GetDriver().ClickAt(By.XPath(".//div[text() = '" + value + "' ]"), comboListElem);
        }

        [TxAction("SelectV2", "....")]
        public void SelectV2(string value)
        {
            Thread.Sleep(2000);
            GetDriver().ClickAt(comboImg, this);
            GetDriver().FindElement(By.XPath(".//div[starts-with(@class,'dhxcombo_option')]/div[text() = \"" + value + "\" ]")).Click();
            Thread.Sleep(2000);
            Console.WriteLine("Sleep inside SelectV2. Need to remove later");
            //GetDriver().ClickAt(By.XPath(".//div[@class='dhxcombo_option']/div[text() = \"" + value + "\" ]"));
        }

        [TxAction("SelectV3", "....")]
        public void SelectV3(string value)
        {
            Thread.Sleep(2000);
            this.GetElement().FindElement(By.XPath(".//img[@class=\"clDivFictiveTxComboImg\"]")).Click();
            WERefreshed comboListElem = new WERefreshed(GetDriver(), comboList, this);
            GetDriver().ClickAt(By.XPath(".//div[text() =\"" + value + "\"]"), comboListElem);
            Thread.Sleep(2000);
            Console.WriteLine("Sleep inside SelectV3. Need to remove later");
            //GetDriver().ClickAt(By.XPath(".//div[@class='dhxcombo_option']/div[text() = \"" + value + "\" ]"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedObjectType"></param>
        [TxAction("Read", "....")]
        public string Read()
        {
            GetDriver().ClickAt(comboImg, this);
            
            WERefreshed comboListElem = GetDriver().WaitForElement(comboList, null, currentFrameBy);
            string objectTypeName;
            try {
                WERefreshed objectTypeSelected = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhxcombo_option_selected\")]//div[contains(@class,\"dhxcombo_option_text\")]"), comboListElem);
                objectTypeName = objectTypeSelected.GetElement().Text;
            }
            catch(Exception)
            {
                Console.WriteLine("problem combobox scroll");
                objectTypeName = "cm";
            }       
            
            GetDriver().ClickAt(comboImg, this);
            return objectTypeName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedObjectType"></param>
        [TxAction("ReadV2", "Only for combo box in multicriteria selection")]
        public string ReadV2()
        {
            Thread.Sleep(2000);
            WERefreshed valueSelected = GetDriver().WaitForElement(By.XPath(".//input[@class=\"dhxcombo_input\"]"));
            return valueSelected.GetElement().GetAttribute("value");
        }

        [TxAction("IsComboBoxDisabled", "")]
        public bool IsComboBoxDisabled()
        {
            IWebElement elem = this.GetElement();
            return elem.GetAttribute("class").Contains("dhxcombo_disabled");
        }

        [TxAction("ReadAllListItem", "Use to read list of link we can click.")]
        public ActionColl<string> ReadAllListItem()
        {
            GetDriver().ClickAt(comboImg, this);
            WERefreshed comboListElem = GetDriver().WaitForElement(comboList, null, currentFrameBy);
            ICollection<IWebElement> listItem = comboListElem.GetElement().FindElements(By.XPath(".//div[contains(@class,\"dhxcombo_option\") and not(contains(@style,\"display: none\"))]//div[contains(@class,\"dhxcombo_option_text\")]"));
            ICollection<string> item = new List<string>(listItem.Count);
            for (int i = 1; i <= listItem.Count; i++)
            {
                item.Add(GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"dhxcombo_option\") and not(contains(@style,\"display: none\"))][" + i + "]//div[contains(@class,\"dhxcombo_option_text\")]"), comboListElem).GetElement().Text);
            }
            return new ActionColl<string>(item);
        }

        [TxAction("SelectV4", "....")]
        public void SelectV4(string value)
        {
            Thread.Sleep(2000);
            GetDriver().ClickAt(By.XPath(".//span[text() =\"" + value + "\"]"), this);
            Thread.Sleep(2000);
        }
    }
}
