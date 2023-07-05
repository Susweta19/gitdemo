using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;

namespace Lafarge
{
    public class AHWDInterface : WDInterface
    {
        public AHWDInterface(WDInterface baseInstance)
            : base(baseInstance.Driver)
        {
        }

        [TxOverload(typeof(WDInterface), typeof(AHWDInterface))]
        public static object Overload(object baseInstance)
        {
            return new AHWDInterface(baseInstance as WDInterface);
        }

        [TxAction("LoginForm", "Fills the login form an validates it.")]
        public void LoginForm(string login, string password, string langage = null)
        {
            try
            {
                new WEGInput(Driver, By.Id("idUsername")).Fill(login);

                new WEGInput(Driver, By.Id("idPassword")).Fill(password);

                if (langage != null)
                {
                    new WEGDHtmlxComboBox(Driver, By.XPath(".//div[@id=\"selectBoxLanguage\"]/div")).SelectOption(langage);
                }

                Driver.ClickAt(By.Id("idBtnConnect"));
                if (!Driver.WaitForElementToDisapear(By.Id("idPassword")))
                {
                    IWebElement info = Driver.WaitForElement(By.ClassName("info")).GetElement();
                    throw new Exception(info.Text);
                }
                Driver.WaitForAjax();
            }

            catch (Exception e)
            {
                throw new Exception("Login failed", e);
            }
        }
    }
}
