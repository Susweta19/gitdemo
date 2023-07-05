using System;
using System.Linq;
using OpenQA.Selenium;
using TxSelenium.NativeTests.Handlers;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.DataTypes;
using XmlExecutor.Attributes;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.Windows;
using TxSelenium.Utils;
using System.Threading;
using System.Collections.Generic;

namespace TxSelenium.NativeTests.Writable
{
    /// <summary>
    /// A form representation while the write mode is active.
    /// </summary>
    public class WForm : WDForm
    {
        public static readonly By WriteFormDiv = By.ClassName("dhx_cell_wins");

        public WForm(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// Reads the value currently displayed in a specified input.
        /// </summary>
        /// <typeparam name="T">the data type of the input</typeparam>
        /// <param name="id">the id of the attribute the data should be read from</param>
        /// <returns>the read value</returns>
        [TxAction("Read", "Reads the specified attribute current input value.")]
        public T ReadInput<T>(int id) where T : IWritableData
        {
            return HandlersFactory.GetWritingHandler<T>().ReadInputContent(GetAttributeElem(id));
        }

        /// <summary>
        /// Writes a data object to its interface representation using corresponding handler.
        /// </summary>
        /// <typeparam name="T"> the type of the data to be written</typeparam>
        /// <param name="id">the id of the attribute the data should be written to</param>
        /// <param name="value">the value to write</param>
        [TxAction("Write", "Writes in the specified attribute.")]
        public void Write<T>(int id, T value) where T : IWritableData
        {
            HandlersFactory.GetWritingHandler<T>().WriteForm(GetAttributeElem(id), id, value);
        }

        [TxAction("AttributeName", "Read and check the name of any attribute.")]
        public string AttributeName(int attrId)
        {
            WERefreshed attribute = GetDriver().WaitForElement(By.XPath(".//*[starts-with(@id , \"idLabelAttNameLabel" + attrId + "\")]"), this);
            return attribute.GetElement().Text;
        }

        //[TxAction("FillAttribute", "")]
        //public void FillAttribute(string attrId, string value)
        //{
        //    new WEGInput(GetDriver(), By.XPath(".//textarea[starts-with(@id,\"stringdiv" + attrId + "\")]"), null, By.XPath(".//iframe[contains(@url,\"/5.aspx\")]|.//iframe[contains(@url,\"/5_fr.aspx\")]|.//iframe[contains(@url,\"/3287.aspx\")]")).Fill(value);
        //}
        /// <summary>
        /// Writes a data object to its interface representation using corresponding handler.
        /// </summary>
        /// <typeparam name="T"> the type of the data to be written</typeparam>
        /// <param name="id">the id of the attribute the data should be written to</param>
        /// <param name="value">the value to write</param>
        [TxAction("DeleteData", "Writes in the specified attribute.")]
        public void DeleteData<T>(int id) where T : IWritableData
        {
            HandlersFactory.GetWritingHandler<T>().DeleteData(GetAttributeElem(id));
        }

        /// <summary>
        /// Gets the table object of specified attribute.
        /// </summary>
        /// <param name="attrId">the attribute's id</param>
        /// <returns>the table object</returns>
        [TxAction("Table", "Gets the specifed table in write mode.")]
        public WTable GetTable(int attrId)
        {
            return new WTable(GetDriver(), GetAttrDiv(attrId, CurrentTab), this);
        }


        /// <summary>
        /// Get the associative object corresponding to a specified attribute.
        /// </summary>
        /// <param name="attrId">the attribute's id</param>
        /// <returns>the associative object</returns>
        [TxAction("Associative", "Gets the specifed associative class in write mode.")]
        public WAssociative GetAssociative(int attrId)
        {
            return new WAssociative(GetDriver(), WForm.GetAttrDiv(attrId, CurrentTab), this);
        }

        /// <summary>
        /// To manage the url and mail attribute.
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("MailAndURL", "Gets the specifed associative class in write mode.")]
        public WEGMailUrl MailAndURL(int attrId)
        {
            return new WEGMailUrl(GetDriver(), WForm.GetAttrDiv(attrId, CurrentTab), this);
        }
        
        //TODO should be managed using a handler
        /// <summary>
        /// Gets the dhtmlx combo corresponding to a specified attribute.
        /// </summary>
        /// <param name="attrId">the attribute's id</param>
        /// <returns>the dhtmlx combo</returns>
        [TxAction("DropdownList", "Use the drop down list.")]
        public WEGDHtmlxComboBox DropdownList(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id, \"idDivCombodiv"+attrId+"\")]"), attrElem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("Source", "Click on Source button and fill the formularay.")]
        public WDValidatedWindow<WForm> Source(int attrId)
        {
            GetDriver().ClickAt(By.XPath("./..//div[starts-with(@id , \"idDisplaySourceLabel\")]/img"), GetAttributeElem(attrId));
            WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }

        [TxAction("RichText", "Write values in the specified RichText attribute.")]
        public WRichText RichText(int attrId, int idGroup = 0)
        {
            if (GetDriver().IsElementPresent(By.XPath(".//*[starts-with(@id,\"html_text_" + attrId + "\")]|.//div[starts-with(@id,\"div"+attrId+"\")]//div[@class=\"clDivRichTextTemporary\"]")))
            {
                GetDriver().ClickAt(By.XPath(".//*[starts-with(@id,\"html_text_" + attrId + "\")]|.//div[starts-with(@id,\"div" + attrId + "\")]//div[@class=\"clDivRichTextTemporary\"]"));
              
                return new WRichText(GetDriver(), WForm.GetAttrDiv(attrId, CurrentTab,idGroup), this);
            }
            else
                return new WRichText(GetDriver(), WForm.GetAttrDiv(attrId, CurrentTab, idGroup), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [TxAction("Delete", "Delete date, decimal value and dropdwon list.")]
        public void Delete(int attrId, int index = 1)
        {
            index++;
            WERefreshed attrElem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id, \"cancel\")] | .//tr[\""+ index +"\"]//img[starts-with(@id, \"idImgDelete\")] |"+
                " .//img[starts-with(@id, \"idImgSwitchTreeSize\")] | .//img[contains(@src,\"16x16-False.png\")]"), attrElem);
        }

        /// <summary>
        /// Click on delete button for information of *** windows"
        /// Window returned by the previous function.
        /// </summary>
        /// <returns></returns>
        //[TxAction("DeleteButton", "Click on delete button for information of *** windows")]
        //public void DeleteButton()
        //{
        //    GetDriver().ClickAt(By.XPath(".//input[starts-with(@id, \"delete\")]"), this);
        //}

      [TxAction("SwitchButton", "Click on this button to select true false option")]
       public void SwitchButton(int attrId)
        {            
            GetDriver().ClickAt(By.XPath(".//button[starts-with(@id, \"boolSwitchdiv" + attrId + "\")]"), this);           
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("Information", "*****")]
        public WDValidatedWindow<WForm> Information(int attrId)
        {
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id , \"idLabelAttNameLabel" + attrId + "\")]"), this);
            WForm form = new WForm(GetDriver(), WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(GetDriver(), form);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("GetLink", "Gets the specifed link in write mode.")]
        public WMultipleLink GetLink(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            return new WMultipleLink(GetDriver(), By.XPath("./div"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, attrElem);
        }

        /// <summary>
        /// Get the associative object corresponding to a specified attribute.
        /// </summary>
        /// <param name="attrId">the attribute's id</param>
        /// <returns>the associative object</returns>
        [TxAction("Date", "Gets the date and time in write mode.")]
        public WDate Date(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"calendar.png\")]"), attrElem);
            bool check = GetDriver().IsElementPresent(By.XPath(".//div[contains(@class,\"dhtmlxcalendar_in_input\")]"));
            return new WDate(GetDriver(),By.XPath(".//div[contains(@class,\"dhtmlxcalendar_in_input\")]"));
        }

        [TxAction("TodayOrNow", "")]
        public void TodayOrNow(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            By removeButton = By.XPath(".//div[not(contains(@style,\"display: none;\"))]/img[contains(@src,\"16x16-False.png\")]");
            if (GetDriver().IsElementPresent(removeButton, attrElem.GetElement()))
                GetDriver().ClickAt(removeButton, attrElem);
            GetDriver().ClickAt(By.XPath(".//input[starts-with(@id,\"datediv" + attrId + "\")]"), attrElem);
        }

        /*========================================FUNCTIONS INDIAN TEAM==========================================================*/

        /// <summary>
        /// 
        /// </summary>
        [TxAction("EnableDisableDocumentVisualization", "Gets the date and time in write mode.")]
        public void EnableDisableDocumentVisualization(int attrId,int index = 1)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            index++;
            if(index==100)
            {
                WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"idDivGrid\")]//div[@class=\"objbox\"]/table/tbody"), attrElem);
                index=  elem.GetElement().FindElements(By.XPath("./tr")).Count();
            }

            GetDriver().ClickAt(By.XPath(".//tr[" + index + "]/td[3]/img[starts-with(@id,\"idImgView\")]"), attrElem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        [TxAction("DeActivateURL", "Gets the date and time in write mode.")]
        public void ActivateURL(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);

            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id,\"viewdiv\")]"), attrElem);
        }


      

        [TxAction("RenameTheFile", "Rename The File")]
        public WDValidatedWindow<RenameFile> RenameTheFile(int attrId, int index = 1)
        {
            index++;
            WERefreshed attrElem = GetAttributeElem(attrId);
           GetDriver().ClickAt(By.XPath(".//tr[" + index + "]/td[3]/img[starts-with(@id,\"idImgEditFile\")]"), attrElem);
            RenameFile renameFile = new RenameFile(GetDriver(), By.XPath(".//div[contains(@id,\"idDivFileNameFields\")]"));
            return new WDValidatedWindow<RenameFile>(GetDriver(), renameFile);
        }

        [TxAction("IsRenameButtonEnabled", "Rename The File")]
        public bool IsRenameButtonEnabled(int attrId, int index = 1)
        {
            WaitForAjax();
            bool result = false;
            WERefreshed attrElem = GetAttributeElem(attrId);

            GetDriver().ClickAt(By.XPath(".//tr[@class][" + index + "]/td[3]/img[starts-with(@id,\"idImgEditFile\")]"), attrElem);
            Thread.Sleep(1000);
            WaitForAjax();
            //WERefreshed baseNameInput = new WERefreshed(GetDriver(), By.XPath(".//input[contains(@id,\"idTextBaseName\")]"));
            result = GetDriver().IsElementPresent(By.XPath(".//input[contains(@id,\"idTextBaseName\")]"));
            //string x = baseNameInput.GetElement().GetAttribute("disabled");
            if (result)
                GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"idBtnCancel\")]"));
            return result;
        }

        [TxAction("IsElementPresentInAddReference", "Add a referance to a published File")]
        public bool IsElementPresentInAddReference(int attrId, string reference)
        {
            bool isPresent = false;
            WERefreshed attrElem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id, \"idImgRefFiles\")]"), attrElem);

            WERefreshed list = GetDriver().WaitForElement(new ByChained(By.XPath(".//div[starts-with(@id,\"idDivQueryGrid\")]"), By.ClassName("objbox")));
            GetDriver().ScrollToBottom(list.GetElement(), By.XPath(".//td[text() = \"" + reference + "\"]"), list);

            isPresent = GetDriver().IsElementPresent(By.XPath(".//td[text() = \"" + reference + "\"]"), list.GetElement());

            GetDriver().ClickAt(By.XPath(".//div[@class=\"dhxwin_active\"]//div[@title=\"Close\"]"));

            return isPresent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        [TxAction("AddReference", "Add a referance to a published File")]
        public void AddReference(int attrId, string reference, bool ok)

        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//img[starts-with(@id, \"idImgRefFiles\")]"), attrElem);
            WaitForAjax();
            WERefreshed list = GetDriver().WaitForElement(new ByChained(By.XPath(".//div[starts-with(@id,\"idDivQueryGrid\")]"), By.ClassName("objbox")));            
            GetDriver().ScrollToBottom(list.GetElement(), By.XPath(".//td[(text()=\""+ reference + "\")]"), list);
            bool isPresent = GetDriver().IsElementPresent(By.XPath(".//td[text() = \"" + reference + "\"]"), list.GetElement());
            GetDriver().ClickAt(By.XPath(".//td[text() = \"" + reference + "\"]//img"), list);
            WaitForAjax();
            Sleep(2);
            if (ok)
                GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"idBtnValidate\")]"));
            else
                GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"idBtnCancel\")]"));
        }

        /// <summary>
        /// Use for testing if button is disable 
        /// when it should be.
        /// </summary>
        [TxAction("OkDisable", "Check if ok button if disable.")]
        public void OkDisable() 
        {
            Thread.Sleep(1500);
            WERefreshed okButton = GetDriver().WaitForElement(Translator.GetButtonByValue(GetDriver(), Translator.validateButton), this);
            string disable = okButton.GetElement().GetAttribute("disabled");
            if (disable == null)
                throw new Exception("Ok button enable");
        }

        [TxAction("DeleteAllUploadedFile", "")]
        public void DeleteAllUploadedFile(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            ICollection<IWebElement> deleteFileButton = attrElem.GetElement().FindElements(By.XPath(".//img[starts-with(@id, \"idImgDelete\")]"));
            foreach (IWebElement button in deleteFileButton)
            {
                GetDriver().ClickAt(button);
                WaitForAjax();
                IfPopUpPresent();
            }
        }

        [TxAction("PublishDocumentFile", "Publish Document File")]
        public void PublishDocumentFile(int attrId, string pathFile)
        {
            if (LinuxUtils.IsLinux)
            {
                pathFile = pathFile.Replace("\\", "/");
            }
            Console.WriteLine("File path : "+ pathFile);
            WERefreshed attrElem = GetAttributeElem(attrId);
            WERefreshed uploadElem = new WERefreshed(GetDriver(), By.XPath(".//input[starts-with(@id, \"idInpFile\")]"), attrElem);

            GetDriver().UploadFile(uploadElem, pathFile);
            Sleep(10);
            Console.WriteLine("Sleep Given from PublishDocumentFile");
        }

        /// <summary>
        /// check object is display when url activated
        /// </summary>
        /// <param name="attrId"></param>
        [TxAction("AttributeIsDiplayed", "")]
        public bool AttributeIsDiplayed(int attrId)
        {
            bool res = GetDriver().IsClickable(WForm.GetAttrDiv(attrId, CurrentTab), this.GetElement());
            return res;
        }
        [TxAction("IsFullScreened", "")]
        public bool IsFullScreened(int attrId)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,'divTabbar')]/div[starts-with(@id,'div" + attrId + "')]"),
                this.GetElement());
        }
        [TxAction("CrossDisable", "Check if cancel button if disable.")]
        public bool CrossDisable()
        {
            Thread.Sleep(1500);
            WERefreshed okButton = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,'dhxwin_button_close')]"));
            string data = okButton.GetElement().GetAttribute("class");
            return data.Contains("_dis");
        }

        [TxAction("IsInformationPresent", "Check Is Information Present")]
        public bool IsInformationPresent(int attrId)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//label[starts-with(@id ,\"idLabelAttNameLabel" + attrId + "\")]"),this);
            return elem.GetElement().GetAttribute("style").Contains("underline");
        }
        [TxAction("CancelDisable", "Check if cancel button if disable.")]
        public bool CancelDisable()
        {
            Thread.Sleep(1500);
            WERefreshed okButton = GetDriver().WaitForElement(Translator.GetButtonByValue(GetDriver(), Translator.cancelButton), this);
            string disable = okButton.GetElement().GetAttribute("disabled");
            return disable != null;
        }
        [TxAction("OkButtonDisable", "Check if ok button if disable.")]
        public bool OkButtonDisable()
        {
            Thread.Sleep(1500);
            WERefreshed okButton = GetDriver().WaitForElement(Translator.GetButtonByValue(GetDriver(), Translator.validateButton), this);
            string disable = okButton.GetElement().GetAttribute("disabled");
            return disable != null;
        }
        [TxAction("IsSourcePresent", "Check Is Source Present")]
        public bool IsSourcePresent(int attrId)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'idDisplaySourceLabel" + attrId + "')]/img"),this);
            return elem.GetElement().GetAttribute("src").Contains("ExistingSource");
        }

        [TxAction("IsRichTextEditable", " ")]
        public bool IsRichTextEditable(int attrId)
        {
            return !GetDriver().IsElementPresent(By.XPath(".//div[@class=\"clDivRichTextTemporary\"]"), GetAttributeElem(attrId).GetElement());
        }

        [TxAction("ReadFieldColor", "")]
        public XmlExecutor.DataTypes.ActionColl<string> ReadFieldColor(int attrId)
        {
            List<IWebElement> fields = GetAttributeElem(attrId).GetElement().FindElements(By.XPath(".//input[@id]")).ToList();
            List<string> colorList = new List<string>();
            foreach (IWebElement elem in fields)
            {
                string attribute = elem.GetAttribute("style").Split('(').Last().Split(')').First();
                colorList.Add(attribute);
            }

            return new XmlExecutor.DataTypes.ActionColl<string>(colorList);
        }

        [TxAction("EnableDocumentVisualization", "Gets the date and time in write mode.")]
        public void EnableDocumentVisualization(int attrId, int index = 1)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            index++;
            if (index == 100)
            {
                WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"idDivGrid\")]//div[@class=\"objbox\"]/table/tbody"), attrElem);
                index = elem.GetElement().FindElements(By.XPath("./tr")).Count();
            }

            WERefreshed elemDocumentVisualization = GetDriver().WaitForElement(By.XPath(".//tr[" + index + "]/td[3]/img[starts-with(@id,\"idImgView\")]"), attrElem);
            if (!elemDocumentVisualization.GetElement().GetAttribute("class").Contains("clIconPressedExt"))
                GetDriver().ClickAt(By.XPath(".//tr[" + index + "]/td[3]/img[starts-with(@id,\"idImgView\")]"), attrElem);
        }

        [TxAction("ReadSelectedObject", "ReadSelectedObject.")]
        public string ReadSelectedObject(string attrId)
        {
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivTreediv" + attrId + "_\")]//span[@class=\"selectedTreeRow\"]")).GetElement().Text;
        }

        [TxAction("DisableDocumentVisualization", "")]
        public void DisableDocumentVisualization(int attrId, int index = 1)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            index++;
            if (index == 100)
            {
                WERefreshed elem = new WERefreshed(GetDriver(), By.XPath(".//div[contains(@id,\"idDivGrid\")]//div[@class=\"objbox\"]/table/tbody"), attrElem);
                index = elem.GetElement().FindElements(By.XPath("./tr")).Count();
            }
            WERefreshed elemDocumentVisualization = GetDriver().WaitForElement(By.XPath(".//tr[" + index + "]/td[3]/img[starts-with(@id,\"idImgView\")]"), attrElem);
            if (elemDocumentVisualization.GetElement().GetAttribute("class").Contains("clIconPressedExt"))
                GetDriver().ClickAt(By.XPath(".//tr[" + index + "]/td[3]/img[starts-with(@id,\"idImgView\")]"), attrElem);
        }

        [TxAction("ClickOnRenameTheFile", "Will click only one the button.")]
        public void ClickOnRenameTheFile(int attrId)
        {
            WERefreshed attrElem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//tr//img[starts-with(@id,\"idImgEditFile\")]"), attrElem);
        }
    }
}
