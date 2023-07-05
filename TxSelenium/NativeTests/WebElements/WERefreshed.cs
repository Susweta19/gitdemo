using System;
using OpenQA.Selenium;
using XmlExecutor.Attributes;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using TxSelenium.Utils;
using System.IO;
using System.Linq;
using TxSelenium.NativeTests.WebElements.Specific;
using TxSelenium.NativeTests.WebElements.General;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.WebElements
{
    /// <summary>
    /// This class is the base for every interactive web element in TxWebTest.
    /// Its purpose is to fetch the underlying IWebelement every time it is asked to
    /// avoid StaleElementException on refresh.
    /// </summary>
    public class WERefreshed
    {
        public By ElemIdentifier { get; private set; }
        public WERefreshed Parent { get; set; }
        private readonly TxWebDriver _driver;
        public By FrameBy;
        private IWebElement _currentElement;

        /// <summary>
        /// The constructor for the refreshed element, it will not check 
        /// wether or not the IWebelement pointed by the By identifier is correct, accessible ...
        /// </summary>
        /// <param name="driver"> the driver </param>
        /// <param name="elemIdentifier"> the identifier relative to the parent</param>
        /// <param name="parent"> the parent of this element, can be null</param>
        public WERefreshed(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
        {
            _driver = driver;
            ElemIdentifier = elemIdentifier;
            Parent = parent;
            FrameBy = frameBy;
            //ParseAllAjax();
        }

        [TxAction("ReadNotification", "")]
        public string ReadNotification()
        {
            try
            {
                return GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhtmlx_message_area\"] | .//div[starts-with(@id,\"toaster\")]/div"), this).GetElement().Text.Split('\n').Last();
            }
            catch (Exception)
            {
                return "Not able to get notification";
            }
        }

        /// <summary>
        /// Function use inside the constructor of inherited class when 
        /// we need to wait the element. UseFull when the test is too fast.
        /// </summary>
        /// <returns></returns>
        public WERefreshed WaitForElement()
        {
            return _driver.WaitForElement(ElemIdentifier, Parent, FrameBy);
        }

        protected static By ReturnFrameBy(string srcAtt)
        {
            return By.XPath(".//iframe[contains(@url, \"" + srcAtt + "\")]");
        }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        /// <returns> the driver </returns>
        public TxWebDriver GetDriver()
        {
            return _driver;
        }

        [TxAction("IfConfirmationPopUpPresent", "*****")]
        public void IfConfirmationPopUpPresent(bool result = true)
        {
            WaitForAjax();
            try
            {
                new WESPopUp(GetDriver()).CloseConfirmationPopup(result);
                Sleep(2);
            }
            catch (Exception)
            {
                Console.WriteLine("No PopUp appears..");
            }
            WaitForAjax();
        }

        [TxAction("ReadDataValidationError", "*****")]
        public string ReadDataValidationError()
        {
            return GetDriver().WaitForElement(By.XPath(".//span[@class=\"textError\"]")).GetElement().Text;
        }

        [TxAction("TimeToWaitForAjax", "Execution stop according time you give")]
        public void TimeToWaitForAjax(int timeToWait, string frameName = null)
        {
            try
            {
                if (frameName != null)
                    GetDriver().Driver.SwitchTo().Frame(GetDriver().Driver.FindElement(By.XPath(".//iframe[contains(@url,\"" + frameName + "\")]")));

                WebDriverWait wait = new WebDriverWait(GetDriver().Driver, TimeSpan.FromSeconds(timeToWait));
                bool waited = wait.Until<bool>((d) =>
                {
                    Console.WriteLine("waiting");
                    return (bool)(GetDriver().Driver as IJavaScriptExecutor).ExecuteScript("return jQuery ? jQuery.active == 0 : true;");
                });
            }
            catch (Exception)
            {
                Console.WriteLine("jQuery.active still more than 0");
            }
        }


        /**************************Ajax Stuff*******************************/
        /// <summary>
        /// Todo
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ICollection<IWebElement> WaitFrameAjax(IWebElement parent = null)
        {
            if (parent != null)
            {
                GetDriver().Driver.SwitchTo().Frame(parent);
            }
            else
                GetDriver().Driver.SwitchTo().DefaultContent();

            WebDriverWait wait = new WebDriverWait(GetDriver().Driver, TimeSpan.FromSeconds(30));
            try
            {
                bool waited = wait.Until<bool>((d) =>
                {
                    return (bool)(GetDriver().Driver as IJavaScriptExecutor).ExecuteScript("return jQuery ? jQuery.active == 0 : true;");
                });
            }
            catch(Exception)
            {
                Console.WriteLine("exception Ajax Stuff");
            }
            return GetDriver().Driver.FindElements(By.XPath(".//iframe[not(@src = \"javascript:false;\") or not(contains(@style, \"display: none;\"))]"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public void ParseAllAjax(IWebElement parent = null)
        {
            By frameby = GetDriver().CurrentFrameBy;
            if (frameby != null)
                GetDriver().SwitchToContent();
            
            if(WaitFrameAjax(parent).Count == 0)
            {
                Console.WriteLine("No Frame found");
            }
            else
                foreach (IWebElement elem in WaitFrameAjax(parent))
                {
                    ParseAllAjax(elem);
                }
        }
        /**************************************************************/

        /// <summary>
        /// Gets the driver.
        /// </summary>
        /// <returns> the driver </returns>
        [TxAction("Sleep", "Execution stop according time you give")]
        public void Sleep(int timeOut)
        {
            Console.WriteLine("Sleep during " + timeOut);
            Thread.Sleep(TimeSpan.FromSeconds(timeOut));
        }
        [TxAction("IfPopUpPresent", "*****")]
        public void IfPopUpPresent()
        {
            WaitForAjax();
            try
            {
                new WESPopUp(GetDriver()).ClosePopup(true);
            }
            catch (Exception)
            {
                Console.WriteLine("No PopUp appears..");
            }
            WaitForAjax();
        }

        [TxAction("WaitForAjax", "Execution stop according time you give")]
        public void WaitForAjax()
        {
            GetDriver().WaitForAjax();
        }

        [TxAction("GoBack", "*****")]
        public void GoBack()
        {
            _driver.GoBack();
        }
        [TxAction("RefreshWebPage", "*****")]
        public void RefreshWebPage()
        {
            _driver.Refresh();
        }

        [TxAction("GoForward", "*****")]
        public void GoForward()
        {
            _driver.GoForward();
        }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        /// <returns> the driver </returns>
        [TxAction("ManagePopUp", "*****")]
        public WESPopUp ManagePopUp()
        {
            return new WESPopUp(GetDriver());
        }

        [TxAction("ManageJavaScriptAlert", "*****")]
        public WEGJavascriptAlert ManageJavaScriptAlert()
        {
            return new WEGJavascriptAlert(GetDriver());
        }

        /// <summary>
        /// Fetches the element pointed by the identifier added in the constructor.
        /// This method will fetch the element from its parent if it has one and wait 
        /// for the element pointed by its identifier.
        /// The resulting IWebElement should not be stored but fetched by this method 
        /// for every use to avoid a StaleElementException. 
        /// On the other hand it can have a high computing cost.
        /// </summary>
        /// <returns>the pointed element</returns>
        public IWebElement GetElement()
        {
            IWebElement parentElem = null;
            if (Parent != null)
                parentElem = Parent.GetElement();

            if (Parent == null)
                GetDriver().SwitchToContent();

            if (FrameBy != null)
                GetDriver().SwitchToFrame(FrameBy);

            if (ElemIdentifier != null)
            {
                if (parentElem != null && FrameBy == null)
                    _currentElement = _driver.FindElement(parentElem, ElemIdentifier);
                else
                    _currentElement = _driver.FindElement(ElemIdentifier);
            }

            return _currentElement;
        }


        [TxAction("ManageCommonMethods", "")]
        public CommonMethods ManageCommonMethods()
        {
            return new CommonMethods();
        }

        [TxAction("ManageDownloadedFiles", "")]
        public ManageDownloadedFiles ManageDownloadedFiles()
        {
            return new ManageDownloadedFiles();
        }

        [TxAction("PressKeyFromKeyBoard", "")]
        public PressKeyFromKeyBoard PressKeyFromKeyBoard()
        {
            return new PressKeyFromKeyBoard(GetDriver());
        }

        [TxAction("DownloadFileChecking", "Check if the filename is present or not  in the downloaddirectory.")]
        public bool DownloadFileChecking(string DownloadDirectory, string fileName, bool contains = false)
        {
            Thread.Sleep(2000);
            DirectoryInfo dirInfo = new DirectoryInfo(DownloadDirectory);
            IEnumerable<FileInfo> files = dirInfo.EnumerateFiles();
            bool isPresent = false;
            foreach (FileInfo fileInfo in files)
            {
                if (contains)
                {
                    if (fileInfo.Name.Contains(fileName))
                    {
                        isPresent = true;
                        break;
                    }
                }
                else
                {
                    if (fileInfo.Name == fileName)
                    {
                        isPresent = true;
                        break;
                    }
                }
            }

            //Use this part while debuging only 

            //if (!isPresent)
            //{
            //    Console.WriteLine("List of Present Files:");
            //    //foreach (FileInfo fileInfo in files)
            //    //{
            //    //    Console.WriteLine(fileInfo.Name);
            //    //}
            //}
            return isPresent;
        }

        [TxAction("NotificationManager", "*****")]
        public ManageNotification NotificationManager()
        {
            return new ManageNotification(GetDriver());
        }

        [TxAction("TakeScreenshotNow", "*****")]
        public void TakeScreenshotNow(string screenshotName)
        {
            Screenshot screenshot = GetDriver().TakeScreenShot();
            screenshot.SaveAsFile(screenshotName + ".Png", ScreenshotImageFormat.Png);
            Console.WriteLine("Screenshot Taken : " + screenshotName + ".Png");
        }

        [TxAction("IsButtonPresentByTitle", "")]
        public bool IsButtonPresentByTitle(string buttonName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@title,\""+buttonName+ "\") and not(@style=\"display: none;\")]|.//*[contains(@title,\"" + buttonName + "\") and not(@style=\"display: none;\")]"), this.GetElement());
        }

        [TxAction("IsButtonPresentByValue", "")]
        public bool IsButtonPresentByValue(string buttonName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@value,\"" + buttonName + "\") and not(@style=\"display: none;\")]|.//*[contains(@value,\"" + buttonName + "\") and not(@style=\"display: none;\")]"), this.GetElement());
        }

        [TxAction("ReadNotificationList", "")]
        public ActionColl<string> ReadNotificationList()
        {
            ICollection<IWebElement> element;
            element= GetDriver().WaitForElement(By.XPath(".//div[@class=\"dhtmlx_message_area\"]")).GetElement().FindElements(By.XPath(".//div[@class]"));
            ICollection<string> configNames = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                configNames.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(configNames);            
        }

        [TxAction("ReadValidationMessage", "To read data validation messages")]
        public string ReadValidationMessage()
        {
            return GetDriver().WaitForElement(By.XPath(".//span[starts-with(@class,\"message\")]")).GetElement().Text;
        }
    }
}
