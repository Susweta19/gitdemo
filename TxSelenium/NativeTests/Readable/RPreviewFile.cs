using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.GenericTests;

namespace TxSelenium.NativeTests.Readable
{
    public class RPreviewFile : WERefreshed
    {
        public RPreviewFile(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("IsPreviewButtonPresent", "")]
        public bool IsEnvelopButtonPresent(string fileName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//a[@class=\"clFilesAttributes\" and starts-with(text(),\"" + fileName + "\")]/..//i[@data-id-archived-file]"), this.GetElement());
        }

        [TxAction("IsDocumentViewerPresent", "")]
        public bool IsDocumentViewerPresent(string fileName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\""+fileName+"\")]"), this.GetElement());
        }

        [TxAction("ClickOnPreviewButton", "")]
        public RPreviewFile ClickOnPreviewButton(string fileName)
        {
          //fileName = "! (1).pdf";
            GetDriver().ClickAt(By.XPath(".//a[@class=\"clFilesAttributes\" and starts-with(text(),\"" + fileName + "\")]/..//i[@data-id-archived-file]"), this);
            Sleep(2);
            return new RPreviewFile(GetDriver(), By.XPath(".//div[starts-with(@class,\"documentViewerLayer\")]"));
        }

        [TxAction("ClickOnFile", "")]
        public void ClickOnFile(string fileName)
        {
            GetDriver().ClickAt(By.XPath(".//a[@class=\"clFilesAttributes\" and starts-with(text(),\"" + fileName + "\")]"), this);
        }
        
        [TxAction("OpenInNewTab", "")]
        public GTTab<RPreviewFile> OpenInNewTab(string fileName=null)
        {
            WERefreshed parent;
            
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerOpenInTab\")]"), parent);
            Sleep(2);
            RPreviewFile previewFile = new RPreviewFile(GetDriver(),By.TagName("body"));
            return new GTTab<RPreviewFile>(GetDriver(),previewFile);
        }

        [TxAction("Print", "")]
        public void Print(string fileName=null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerPrintIcon\")]"), parent);
        }

        [TxAction("Download", "")]
        public void Download(string fileName=null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerDlIcon\")]"), parent);
        }
        
        [TxAction("ClosePreview", "")]
        public void ClosePreview()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerPrevIcon\")]"));
        }

        //documentViewerCurrentPage
        [TxAction("ReadCurrentPageNumber", "")]
        public string ReadCurrentPageNumber(string fileName=null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            return GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"documentViewerCurrentPage\")]"), parent).GetElement().Text;
        }
        //documentViewerLastPage
        [TxAction("ReadLastPageNumber", "")]
        public string ReadLastPageNumber(string fileName = null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            return GetDriver().WaitForElement(By.XPath(".//span[starts-with(@id,\"documentViewerLastPage\")]"), parent).GetElement().Text;
        }
        //documentViewerZoomOut
        [TxAction("ZoomOut", "")]
        public void ZoomOut(string fileName = null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerZoomOut\")]"), parent);
        }
        //documentViewerFit
        [TxAction("FitToWidth", "")]
        public void FitToWidth(string fileName = null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerFit\")]"), parent);
        }
        //documentViewerResetZoom
        [TxAction("ResetZoom", "")]
        public void ResetZoom(string fileName = null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerResetZoom\")]"), parent);
        }
        //documentViewerZoomIn
        [TxAction("ZoomIn", "")]
        public void ZoomIn(string fileName = null)
        {
            WERefreshed parent;
            if (!string.IsNullOrEmpty(fileName))
                parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and starts-with(text(),\"" + fileName + "\")]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
            else
                parent = this;
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerZoomIn\")]"), parent);
        }

        //public bool IsZoomedIn(string fileName = null)
        //{
        //    WERefreshed parent;
        //    if (!string.IsNullOrEmpty(fileName))
        //        parent = GetDriver().WaitForElement(By.XPath(".//span[@class=\"documentViewerName\" and text()=\"" + fileName + "\"]/ancestor::div[starts-with(@id,\"idDivPdfFileView\")]"), this);
        //    else
        //        parent = this;
        //    GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"documentViewerZoomIn\")]"), parent);
        //}
    }
}
