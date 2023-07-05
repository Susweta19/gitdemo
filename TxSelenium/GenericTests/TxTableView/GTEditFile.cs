using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTableView
{
   public class GTEditFile : WERefreshed
    {
        public GTEditFile(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("ReadImageName", "......")]
        public string ReadImageName()
        {
           // string aa = GetDriver().WaitForElement(By.XPath(".//div[@class='dhx_cell_cont_wins']//td[@valign='middle'][1]")).GetElement().Text;
            return GetDriver().WaitForElement(By.XPath(".//td[@valign='middle'][1]"),this).GetElement().Text.Trim();
        }


        [TxAction("ValidateCancel", "ValidateCancel")]
        public void ValidateCancel(bool validate)
        {
            if (validate == true)
            {
                GetDriver().ClickAt(By.XPath(".//div[contains(@id,'idDivBtns')]//input[@id='idBtnValidate']"));
            }
            else
                GetDriver().ClickAt(By.XPath(".//div[contains(@id,'idDivBtns')]//input[@id='idBtnCancel']"));
            //By button = validate ? By.Id("idBtnValidate") : By.Id("idBtnCancel");
            //GetDriver().ClickAt(button, this);
        }
        [TxAction("UploadDocumentFile", "Publish Document File")]
        public void UploadDocumentFile(string pathFile)
        {
            if (LinuxUtils.IsLinux)
            {
                pathFile = pathFile.Replace("\\", "/");
            }
            Console.WriteLine("File path : " + pathFile);
           
            WERefreshed uploadElem = new WERefreshed(GetDriver(), By.XPath(".//input[contains(@id,\"idInpFile\")]"), this);

            GetDriver().UploadFile(uploadElem, pathFile);
            Sleep(5);
            Console.WriteLine("Sleep Given from PublishDocumentFile");
        }
        [TxAction("RenameFile", "")]
        public RenameFile FinalName()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"idImgEditFile\")]"));

            return new RenameFile(GetDriver(), By.XPath(".//div[contains(@id,\"idDivFileNameFields\")]"));
        }
        [TxAction("Delete", "")]
        public void Delete()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@id,\"idImgDelete\")]"));
        }
    }
}
