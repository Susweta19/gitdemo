using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using TxSelenium.NativeTests.WebElements;
using System.Linq;
using System.Threading;
using TxSelenium.Utils;
using TxSelenium.GenericTests.TXSpecification;
using System.Collections.ObjectModel;
using TxSelenium.GenericTests.TxCharts;

namespace TxSelenium
{
    /// <summary>
    /// This class is the main interface between the selenium specific code and the interaction code.
    /// </summary>
    public class TxWebDriver : IDisposable
    {
        public IWebDriver Driver { get; private set; }
        private string _baseUrl;
        public By CurrentFrameBy { get; private set; }
        public string Language { get; private set; }

        internal object FindElement(By by, GTAddNewCurve gTAddNewCurve)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The contructor.
        /// </summary>
        /// <param name="driver">The selenium driver to be used</param>
        /// <param name="baseUrl">The url the driver should go to</param>
        /// <param name="language"></param>
        public TxWebDriver(IWebDriver driver, string baseUrl, string language)
        {
            this.Driver = driver;
            this._baseUrl = baseUrl;

            INavigation navigation = driver.Navigate();
            try {
                navigation.GoToUrl(baseUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine("goToUrl Function Fail URL : " + baseUrl);
                Console.WriteLine("goToUrl Function Fail Driver is null ? : " + (driver == null));
                Console.WriteLine("goToUrl Function Fail navigation is null ? : " + (navigation == null));
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                driver.Quit();
                driver.Dispose();
                throw new Exception("goToUrl Function Fail", e);
            }
            Language = language ?? Translator.EnLang;
        }

        public void Dispose()
        {
            if (Driver != null)
            {
                try
                {
                    //kill process dllHost.exe
                    Console.WriteLine("Killing DllHost.exe");
                    ExecuteScript<object>(@"J.ajax({
                url: _url('/code/StartupAjax.aspx'),
                async: false,
                cache: false,
                data: {
                    sFunctionName: 'logout'
                }
                });");


                }
                catch (Exception e)
                {
                    Console.WriteLine("Driver dispose failed : " + e.Message);
                }
                finally
                {
                    Driver.Quit();
                    Driver.Dispose();
                }

            }
        }

        public string GetTitle()
        {
            return Driver.Title;
        }

        /// <summary>
        /// Does a right click on an element.
        /// </summary>
        /// <param name="elem">the element the right click should occur on</param>
        public void RightClick(WERefreshed elem)
        {
            Actions actions = new Actions(Driver);
            Thread.Sleep(5000);
            actions.MoveToElement(elem.GetElement()).ContextClick().Build().Perform();
        }

        public void RightClick(IWebElement elem, int xOffSet, int yOffSet)
        {
            Actions actions = new Actions(Driver);
            actions.MoveToElement(elem, xOffSet, yOffSet).ContextClick().Build().Perform();
        }

        /// <summary>
        /// Click on an element
        /// </summary>
        /// <param name="element"></param>
        public void ClickAt(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            Exception except = null;

            bool success = wait.Until((d) =>
            {
                try
                {
                    element.Click();
                    //element.Click();
                    return true;
                }
                catch (Exception e)
                {
                    except = e;
                    return false;
                }
            });
            if ( (! success) && (except != null) )
            {
                Console.WriteLine(except.Message);
                Console.WriteLine(except.StackTrace);
            }
        }

        /// <summary>
        /// Click on an element
        /// </summary>
        /// <param name="element"></param>
        public void ClickAt(WERefreshed element)
        {
            ClickAt(element.GetElement());
        }

        /// <summary>
        /// Click on element identified by his By and his parent
        /// </summary>
        /// <param name="elemIdentifier"></param>
        /// <param name="parent"></param>
        /// <param name="frameBy"></param>
        public void ClickAt(By elemIdentifier, WERefreshed parent = null, By frameBy = null)
        {
            WERefreshed button = WaitForElement(elemIdentifier, parent, frameBy);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until((d) =>
            {
                try
                {
                    button.GetElement().Click();
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// click in x and y position according the origin elem that user gives.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ClickAt(WERefreshed elem, int x, int y)
        {
            Actions actions = new Actions(Driver);
            actions.MoveToElement(elem.GetElement(), x, y).Click().Build().Perform();
        }

        public void DoubleClickAt(By elemIdentifier, WERefreshed parent = null)
        {
            WERefreshed button = WaitForElement(elemIdentifier, parent);
            Actions action = new Actions(Driver);
            action.DoubleClick(button.GetElement()).Perform();
        }

        public void ClickOn(IWebElement elem, int xOffSet, int yOffSet)
        {
            Actions actions = new Actions(Driver);
            actions.MoveToElement(elem, xOffSet, yOffSet).Click().Build().Perform();
        }

        public void MoveTo(IWebElement elem, int x1, int y1, int xOffset = 0, int yOffset = 0)
        {
            Actions builder = new Actions(Driver);
            builder.MoveToElement(elem, x1, y1).ClickAndHold().MoveByOffset(xOffset, yOffset).Release().Build().Perform();
        }

        public void MouseOver(WERefreshed elem, int xOffSet, int yOffSet)
        {
            Actions actions = new Actions(Driver);
            actions.MoveToElement(elem.GetElement(), xOffSet, yOffSet).Build().Perform();
        }

        public void Release(IWebElement elem)
        {
            Actions action = new Actions(Driver);
            action.Release(elem).Build().Perform();
        }

        public void ClickAndHold()
        {
            Actions action = new Actions(Driver);
            action.ClickAndHold().Build().Perform();
        }

        /// <summary>
        /// Push a selected key in the keyboard.
        /// </summary>
        public void KeyDown(string key)
        {
            Actions actions = new Actions(Driver);
            actions.KeyDown(key).Perform();
        }

        /// <summary>
        /// Pull a selected key in the keyboard.
        /// </summary>
        /// <param name="key"></param>
        public void KeyUp(string key)
        {
            Actions actions = new Actions(Driver);
            actions.KeyUp(key).Perform();
        }

        public void PressKey(string key)
        {
            Actions actions = new Actions(Driver);
            actions.SendKeys(key).Perform();
       }

        /// <summary>
        /// replace the previous button
        /// present in the toobar previously
        /// </summary>
        public void GoBack()
        {
            this.Driver.Navigate().Back();
        }

        public void Refresh()
        {
            this.Driver.Navigate().Refresh();
        }

        /// <summary>
        /// replace the next button
        /// present in the toobar previously
        /// </summary>
        public void GoForward()
        {
            this.Driver.Navigate().Forward();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
            Actions action = new Actions(Driver);
            action.Release().Perform();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void ClickAndHold(IWebElement element)
        {
            Actions action = new Actions(Driver);
            action.ClickAndHold(element).Perform();
        }

        /// <summary>
        /// Moves the mouse over a specified element.
        /// </summary>
        /// <param name="elem">the element</param>
        public void MouseOver(WERefreshed elem)
        {
            Actions actions = new Actions(Driver);
            actions.MoveToElement(elem.GetElement()).Build().Perform();
        }

        /// <summary>
        /// Switches the driver focus back to the main frame "idTxAsp".
        /// </summary>
        public void SwitchToContent()
        {
            Driver.SwitchTo().DefaultContent();
            CurrentFrameBy = null;
        }

        /// <summary>
        /// Switches to a specified frame, usefull in GetElement() function.
        /// </summary>
        /// <param name="frameBy">the identifers of the frame the driver's focus should switch to</param>
        public void SwitchToFrame(By frameBy)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            IWebElement frameElem = wait.Until<IWebElement>((d) =>
            {
                IWebElement frameElement = FindElement(frameBy);
                return frameElement;
            });
            //TODO switch to default contect
            //IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            //var currentFrame = (IWebElement)(jsExecutor.ExecuteScript("return window.frameElement;"));
            this.CurrentFrameBy = frameBy;
            Driver.SwitchTo().Frame(frameElem);
        }

        /*****************************  New functions two manage tab in browser *****************************************/
        public string GetCurrentUrl()
        {
            return Driver.Url;
        }

        public string GetCurrentFrame()
        {
            return Driver.CurrentWindowHandle;
        }


        public void SwitchNewWindow()
        {
            string currentHandle = Driver.CurrentWindowHandle;
            string newHandle = Driver.WindowHandles.Last();
            int retryCount = 0;
            //The window may take a moment to open especially when the browser is IE11
            while (Driver.WindowHandles.Count == 1 && retryCount < 5)
            {
                retryCount++;
                Thread.Sleep(retryCount * 1000);
                Console.WriteLine("Handles count : " + Driver.WindowHandles.Count);
            }
            newHandle = Driver.WindowHandles.Last();
            if (newHandle == currentHandle)
            {
                throw new Exception("Failed to switch window : no new window found");
            }
            Driver.SwitchTo().Window(newHandle);
        }

        public void SwitchOldWindow()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.First());
        }

        public void SwitchToWindow(int frameIndex)
        {
            Driver.SwitchTo().Window(Driver.WindowHandles[frameIndex]);
        }

        /****************************************************************************************************************/

        public void Close()
        {
            Driver.Close();
        }

        public void UploadFile(WERefreshed input, string path)
        {
            WaitForAjax();
            input.GetElement().SendKeys(path);
        }

        /// <summary>
        /// Scrolls an element to ist bottom if it can scroll, does nothing otherwise.
        /// </summary>
        /// <param name="elem">the element to scroll down</param>
        public void ScrollToBottom(IWebElement elem)
        {
            long height = ExecuteScript<long>(@"return arguments[0].scrollHeight;", new object[] { elem });

            for (int i = 0; i < ((height / 200) + 2); i++)
            {
                ExecuteScript<object>(@"arguments[0].scrollTop = arguments[0].scrollTop + 200;", new object[] { elem });
                WaitForAjax();
            }
        }

        /// <summary>
        /// Scroll to the elemnt defined with by and parent.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="by"></param>
        /// <param name="parent"></param>
        public void ScrollToBottom(IWebElement elem, By by, WERefreshed parent)
        {
            long height = ExecuteScript<long>(@"return arguments[0].scrollHeight;", new object[] { elem });

            for (int i = 0; i < ((height / 50) + 2); i++)
            {
                ExecuteScript<object>(@"arguments[0].scrollTop = arguments[0].scrollTop + 50;", new object[] { elem });
                WaitForAjax();
                //Maybe better little wait until inside isclickable function
                Thread.Sleep(500);
                try
                {
                    if (IsClickable(by, parent))
                    {
                        break;
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("This is the strange issue generated from  IsClickable of ScrollToBottom\n Locally we can not reproduce it!");
                   // Console.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Scrolls an element to ist bottom if it can scroll, does nothing otherwise.
        /// </summary>
        /// <param name="elem">the element to scroll down</param>
        public void ScrollHorizontal(IWebElement elem)
        {
            long width = ExecuteScript<long>(@"return arguments[0].scrollWidth;", new object[] { elem });

            for (int i = 0; i < ((width / 200) + 2); i++)
            {
                ExecuteScript<object>(@"arguments[0].scrollLeft = arguments[0].scrollLeft + 200;", new object[] { elem });
                WaitForAjax();
            }
        }

        /// <summary>
        /// Scroll and click to the element defined With by and parent
        /// </summary>
        /// <param name="table"></param>
        /// <param name="by"></param>
        /// <param name="parent"></param>
        public void ScrollHorizontal(IWebElement table, By by, WERefreshed parent, bool click = true,bool tryOneExtraScroll=false)
        {
            long width = ExecuteScript<long>(@"return arguments[0].scrollWidth;", new object[] { table });

            for (int i = 0; i < ((width / 50) + 2); i++)
            {
                ExecuteScript<object>(@"arguments[0].scrollLeft = arguments[0].scrollLeft + 50;", new object[] { table });
                WaitForAjax();
                Thread.Sleep(1000);
                if (IsClickable(by, parent))
                {
                    if (tryOneExtraScroll)
                    {
                        try
                        {
                            ExecuteScript<object>(@"arguments[0].scrollLeft = arguments[0].scrollLeft + 50;", new object[] { table });
                            WaitForAjax();
                        }
                        catch (Exception) { }
                    }
                    if (click)
                        ClickAt(by, parent);
                    break;
                }
            }
        }

        /// <summary>
        /// Waits for an element to be available for 10 seconds.
        /// If the element is not found in the allotted time an exception will be thrown.
        /// </summary>
        /// <param name="parent">the parent of the element</param>
        /// <param name="by">the identifier of the element</param>
        /// <returns>the element </returns>
        public WERefreshed WaitForElement(By by, WERefreshed parent = null, By frameBy = null)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));

             
            return wait.Until<WERefreshed>((d) =>
            {
                try
                {
                    IWebElement webElem = null;
                    IWebElement parentElem = null;
                    if (parent != null)
                        parentElem = parent.GetElement();

                    if (parent == null)
                        SwitchToContent();

                    if (frameBy != null)
                        SwitchToFrame(frameBy);

                    if (by != null)
                    {
                        if (parentElem != null && frameBy == null)
                            webElem = parentElem.FindElement(by);
                        else
                            webElem = Driver.FindElement(by);
                    }

                    if (webElem != null && webElem.Displayed && webElem.Size.Height > 0 && webElem.Size.Width > 0)
                    {
                        return new WERefreshed(this, by, parent, frameBy);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }

            });
        }

        /// <summary>
        /// Waits for a parentless element to be available.
        /// If the element is not found in the allotted time an exception will be thrown.
        /// </summary>
        /// <param name="by">the identifier of the element</param>
        /// <returns>the element</returns>
        public IWebElement FindElement(By by)
        {
            try
            {
                return Driver.FindElement(by);
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Waits for an element to be available for 10 seconds.
        /// If the element is not found in the allotted time an exception will be thrown.
        /// </summary>
        /// <param name="parent">the parent of the element</param>
        /// <param name="by">the identifier of the element</param>
        /// <returns>the element </returns>
        public IWebElement FindElement(IWebElement parent, By by)
        {
            try
            {
                IWebElement webElem = null;
                webElem = parent.FindElement(by);
                if (webElem != null)
                    return webElem;
                else
                    return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks wether or not a specific element has at least one child.
        /// </summary>
        /// <param name="elem">the element</param>
        /// <returns>true if the element has at least one child, false otherwise</returns>
        public bool HasChildren(IWebElement elem)
        {
            return elem.FindElements(By.XPath("child::*")).Count != 0;
        }

        /// <summary>
        /// Waits for an element to no longer be found in the DOM.
        /// </summary>
        /// <param name="by">the element identifier</param>
        /// <returns>true if the element is not in the DOM before the allotted time</returns>
        public bool WaitForElementToDisapear(By by)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30)); //was 10
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            bool elem;
            try
            {
                elem = wait.Until<bool>((d) =>
                {
                    try
                    {
                        d.FindElement(by);
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }
                    return false;
                });
            }
            catch (WebDriverTimeoutException)
            {
                elem = false;
            }
            return elem;
        }

        /// <summary>
        /// Waits for all ajax calls on the page to end.
        /// This function will not work if the ajax call starts after this function is called,
        /// which is a common problem.
        /// </summary>
        /// <returns>true if all ajax calls have ended before the allotted time throws an exception otherwise</returns>
        public bool WaitForAjax()
        {
            try
            {
                By currentFrame = null;
                //If the driver is focused on an embedded iframe JQuery might not be available
                //but ajax calls can be made from the main window and change the window content,
                //in this case the driver needs to switch back to the main window wait and 
                //go back to the frame afterwards
                if ((bool)(Driver as IJavaScriptExecutor).ExecuteScript("return jQuery == undefined || jQuery == null;"))
                {
                    SwitchToContent();
                    currentFrame = this.CurrentFrameBy;
                }

                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(1));
                bool waited = wait.Until<bool>((d) =>
                {
                    return (bool)(Driver as IJavaScriptExecutor).ExecuteScript("return jQuery ? jQuery.active == 0 : true;");
                });
                if (currentFrame != null)
                    SwitchToFrame(currentFrame);
                return waited;
            }
            catch (Exception e)
            {
                Console.WriteLine("Wait for ajax failed");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks whether or not an element is present on the DOM.
        /// </summary>
        /// <param name="by">the identifier of hte elemetn to look for</param>
        /// <param name="parent">the parent of the element can be null if there is no parent</param>
        /// <returns>true if the element is found</returns>
        public bool IsElementPresent(By by, IWebElement parent = null)
        {
            try
            {
                if (parent != null)
                    parent.FindElement(by);
                else
                    Driver.FindElement(by);

                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if element is clickable or not, Use to scroll function
        /// </summary>
        /// <param name="by"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsClickable(By by, IWebElement parent = null)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(2));

            try
            {
                bool waited = wait.Until<bool>((d) =>
                {
                    try
                    {
                        if (parent != null)
                        {
                            if (IsElementPresent(by, parent) &&parent.FindElement(by).Displayed && parent.FindElement(by).Enabled)
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            if (IsElementPresent(by) && Driver.FindElement(by).Displayed && Driver.FindElement(by).Enabled)
                                return true;
                            else
                                return false;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                }
                );
                return waited;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsClickable(By by, WERefreshed parent = null)
        {

            try
            {
                if (parent != null)
                {
                    IWebElement elem = parent.GetElement().FindElement(by);
                    
                    if ( elem.Displayed && elem.Enabled)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if ( Driver.FindElement(by).Displayed && Driver.FindElement(by).Enabled)
                        return true;
                    else
                        return false;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }


        }

        /// <summary>
        /// Checks whether or not an attribute is present in an element.
        /// </summary>
        /// <param name="attribute">The name of attribute</param>
        /// <param name="elem">The element tested</param>
        /// <returns></returns>
        public bool IsAttributePresent(string attribute, WERefreshed elem)
        {
            bool result = false;
            try
            {
                string value = elem.GetElement().GetAttribute(attribute);
                if (value != null)
                    result = true;
            }
            catch (StaleElementReferenceException) { }

            return result;
        }
       
        /// <summary>
        /// Executes a piece of javascript code.
        /// Check the ExecuteScript method in the Selenium API 
        /// for available return types and parameters types.
        /// Use object type if the script has no return value.
        /// </summary>
        /// <typeparam name="T">the expected return type of the code,
        /// </typeparam>
        /// <param name="script">The script to be executed</param>
        /// <param name="args">An array containing the arguments for the script</param>
        /// <returns>the object returned by the script</returns>
        public T ExecuteScript<T>(string script, object[] args = null)
        {
            var returnValue = ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);
            return (T)returnValue;
        }

        /// <summary>
        /// Takes a screenshot and returns it as an object.
        /// </summary>
        /// <returns>A screenshot object</returns>
        public Screenshot TakeScreenShot()
        {
            return ((ITakesScreenshot)Driver).GetScreenshot();
        }

        /// <summary>
        /// Drags an element and drops it on another element.
        /// </summary>
        /// <param name="dragElem">the element to drag</param>
        /// <param name="dropElem">the element where the dragged element will be dropped</param>
        /// <param name="xOffset">the horizontal drop offset relative to the drop element along</param>
        /// <param name="yOffset">the vertical drop offset relative to the drop element along</param>
        public void DragAndDrop(WERefreshed dragElem, WERefreshed dropElem, int xOffset = 0, int yOffset = 0)
        {
            Actions builder = new Actions(Driver);
            IAction dragAndDrop = builder.ClickAndHold(dragElem.GetElement())
               .MoveToElement(dropElem.GetElement())
               .MoveByOffset(xOffset, yOffset)
               .Release()
               .Build();
            dragAndDrop.Perform();
        }

        /// <summary>
        /// Moves the mouse to the specified coordinate.
        /// </summary>
        /// <param name="xOffset">the x coordinate, relative to the current position, in pixels</param>
        /// <param name="yOffset">the y coordinate, relative to the current position, in pixels</param>
        public void MouseMove(int xOffset, int yOffset)
        {
            Actions actions = new Actions(Driver);
            actions.MoveByOffset(xOffset, yOffset).Build().Perform();
        }

        public void MoveToElement(By by, WERefreshed parent)
        {
            IWebElement element = WaitForElement(by, parent).GetElement();
            Actions actions = new Actions(Driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        /// <summary>
        /// Click where the cursor is.
        /// Useful after a mouse hover or mouse move.
        /// </summary>
        public void ClickCursor()
        {
            Actions actions = new Actions(Driver);
            actions.Click().Build().Perform();
        }

        public void ScrollToElement(IWebElement elem)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", elem);
        }
        public void ScrollByPosition(IWebElement Element, string xposition, string yposition)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].scrollTo(arguments[1],arguments[2]);", Element, xposition, yposition);
        }
    }
}
