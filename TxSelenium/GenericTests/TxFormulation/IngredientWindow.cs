using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxFormulation
{
   public class IngredientWindow : WERefreshed
    {
        public IngredientWindow(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }
        [TxAction("SelectIngredient", "Passes Teexma in write mode.")]
        public WMultipleLink SelectIngredient()
        {
            // WEGDHtmlxComboBox win = new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WMultipleLink(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]//div[starts-with(@id,\"idDivMain\")]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
           // return new WEGDHtmlxComboBox(GetDriver(),By.XPath(".//div[@class=\"dhxwin_active\"]//div[starts-with(@id,\"idDivMain\")]"),this);
        }

        [TxAction("Search", "")]
        public void Search(string research, bool popup = true)
        {
            WEGInput input;
            try
            {
                input = new WEGInput(GetDriver(), By.XPath(".//input[contains(@placeholder,\"Search\")]"), this);
            }
            catch (Exception)
            {
                input = new WEGInput(GetDriver(), By.XPath(".//input[contains(@class,\"dhxtoolbar_input\")]"), this);
                Console.WriteLine("Exception handled within Search method...");
            }

            input.Fill(research);
            input.Enter();

            if (popup)
                if (research.Length == 1)
                    new WESPopUp(GetDriver()).ClosePopup(true);
        }
    }
}
