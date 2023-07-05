using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Writable;
using TxSelenium.NativeTests.Windows;
using System.Threading;
using OpenQA.Selenium.Support.PageObjects;

namespace TxSelenium.GenericTests.TxVisualDesign
{
    public class GTVisualDesign : WERefreshed
    {
        public GTVisualDesign(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ReadHeaderName", "")]
        public void ReadHeaderName(string name)
        {
            WERefreshed elem;
            elem = GetDriver().WaitForElement(By.Id("txVDNameSelectedObject"));
            Console.WriteLine(elem.GetElement().Text.ToString());
            Console.WriteLine(name);
            if (!elem.GetElement().Text.Contains(name))
                throw new Exception("Header name does not match");
        }

        [TxAction("Settings", " Settings")]
        public GTSettings Settings()
        {
            return new GTSettings(GetDriver(), By.Id("idTxVDAction"), this);
        }

        [TxAction("SaveTheConfiguration", " SaveTheConfiguration")]
        public void SaveTheConfiguration()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Save.png\")]"), this);
        }

        [TxAction("SelectDesignElement", "")]
        public void SelectDesignElement(int xCanvas, int yCanvas)
        {
            WERefreshed canvasElem = GetDriver().WaitForElement(new ByChained(By.Id("idTxVDScene"), By.TagName("canvas")), this);
            GetDriver().ClickOn(canvasElem.GetElement(), xCanvas, yCanvas);
        }

        /// <summary>
        /// To drag an item from its starting position and drop it to the ending position within the Canvas area.
        /// </summary>
        /// <param name="x1">Starting point at x axis</param>
        /// <param name="y1">Starting point at y axis</param>
        /// <param name="x2">Finishing point at x axis</param>
        /// <param name="y2">Finishing point at y axis</param>
        [TxAction("MoveDesignElement", "To drag an item from its starting position and drop it to the ending position within the Canvas area.")]
        public void MoveDesignElement(int x1, int y1, int x2, int y2)
        {
            WERefreshed canvasElem = GetDriver().WaitForElement(new ByChained(By.Id("idTxVDScene"), By.TagName("canvas")), this);
            GetDriver().MoveTo(canvasElem.GetElement(), x1, y1, x2, y2);
        }

        [TxAction("DragAndDropOnCanvas", "To drag an item from one Canvas area and drop it to the other Canvas area.")]
        public void DragAndDropOnCanvas(int x1, int y1, int x2, int y2)
        {
            WERefreshed canvasElem = GetDriver().WaitForElement(By.XPath(".//*[@id='idTxVDMainLayout']/div/div[1]/div/div/table/tbody/tr[3]"), this);
            WERefreshed canvasElem2 = GetDriver().WaitForElement(By.XPath(".//*[@id=\"idTxVDScene\"]/canvas"), this);
            GetDriver().MouseOver(canvasElem, x1, y1);
            Thread.Sleep(250);
            GetDriver().ClickAndHold();
            //Thread.Sleep(5000);
            //GetDriver().MouseOver(canvasElem2, 10, 10);
            Thread.Sleep(250);
            GetDriver().Release(canvasElem2.GetElement());
            GetDriver().Release();
            // GetDriver().MoveTo(canvasElem.GetElement(), x1, y1, x2, y2);
        }

        [TxAction("GetPointerPosition", "")]
        public void GetPointerPosition(int xCanvas, int yCanvas)
        {
            //WERefreshed canvasElem = GetDriver().WaitForElement(new ByChained(By.Id("idTxVDScene"), By.TagName("canvas")), this);
            WERefreshed canvasElem = GetDriver().WaitForElement(By.XPath(".//*[@id='idTxVDMainLayout']/div/div[1]/div/div/table/tbody/tr[3]"), this);
            GetDriver().RightClick(canvasElem.GetElement(), xCanvas, yCanvas);
        }

        [TxAction("ReadTitleOfRightPanel", "")]
        public void ReadTitleOfRightPanel(string paneTitle)
        {
            WERefreshed elem;
            elem = GetDriver().WaitForElement(By.XPath(".//*[@id='idTxVDAction']/div[2]/div[1]"));
            Console.WriteLine(elem.GetElement().Text.ToString());
            Console.WriteLine(paneTitle);
            if (!elem.GetElement().Text.Contains(paneTitle))
                throw new Exception("Header name does not match");
            else
                Console.WriteLine("Expected value : " + paneTitle + "is equal to Actual value : " + elem.GetElement().Text);
        }

        //

        [TxAction("RightPanelVisibility", "")]
        public void RightPanelVisibility()
        {
            IWebElement elem;
            try
            {
                elem = GetDriver().FindElement(By.XPath("//*[@id='idTxVDMainLayout']/div/div[1]/div/div/table/tbody/tr[3]/td[5]/div/div[@class='dhxcont_global_content_area' and contains(@style,'display: none;')]"));
            }
            catch (Exception)
            {
                elem = null;
            }
            if (elem == null)
                Console.WriteLine("The panel is displayed.");
            else
                Console.WriteLine("The panel is hiddeen.");
        }

        [TxAction("New", " New")]
        public void New()
        {
            GetDriver().ClickAt(By.XPath(".//img[@id=\"idNewDrawingBtn\"]"), this);
        }

        //AutomaticRepositioning is still disabled
        [TxAction("AutomaticRepositioning", " AutomaticRepositioning")]
        public void AutomaticRepositioning()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Workflows.png\")]"), this);
        }

        //ChangeSourceObject
        [TxAction("ChangeSourceObject", " ChangeSourceObject")]
        public WDValidatedWindow<WMultipleLink> ChangeSourceObject()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Folder.png\")]"), this);
            WMultipleLink window = new WMultipleLink(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this, 1);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), window);
        }

        //ReturnObjectSelection
        [TxAction("ReturnObjectSelection", " ObjectSelection")]
        public WDValidatedWindow<WMultipleLink> ReturnObjectSelection()
        {
            WMultipleLink singleLink = new WMultipleLink(GetDriver(), By.ClassName("dhtmlx_wins_body_outer"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), singleLink);
        }

        //we need to click somewhere but for the moment there is nothing for dragpath
        [TxAction("FreeForm", "FreeForm")]
        public void FreeForm()
        {
            WERefreshed dragPath = GetDriver().WaitForElement(By.XPath(".//"), this);
            WERefreshed dropPath = GetDriver().WaitForElement(By.XPath(".//*[@id=\"idTxVDScene\"]/canvas"), this);
            GetDriver().DragAndDrop(dragPath, dropPath);
        }
    }
}
