using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using TxSelenium.Utils;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.WebElements
{
    public class WESPopUp

    {
        public static By GetPopupButton(string result)
        {
            return By.XPath(".//div[@dhxbox]//div[@result=\"" + result + "\"]");
        }

        public static By GetPopupMessage()
        {
            return By.XPath(".//div[@dhxbox]//div[@class=\"dhtmlx_popup_text\"]/span");
        }

        private TxWebDriver Driver;

        public WESPopUp(TxWebDriver driver)
        {
            Driver = driver;
        }

        [TxAction("SelectExportName", "okNoCancel = ok, no or cancel")]
        public void SelectExportName(string option, bool yesOrCancelOrNo)
        {
            Driver.ClickAt(By.XPath(".//div[starts-with(@id, \"idDivQueryCombo\")]"));
            Driver.ClickAt(By.XPath(".//div[@class= \"dhxcombo_option_text\"and text()=\"" + option + "\"]"));
            if (yesOrCancelOrNo)
            {
                Driver.ClickAt(By.XPath(".//div[@class=\"dhxwin_active\"]//*[@id=\"idBtnValidate\"]"));
            }
            else
            {
                Driver.ClickAt(By.XPath(".//div[@class=\"dhxwin_active\"]//*[@id=\"idBtnCancel\"]"));
            }
        }

        /// <summary>
        /// Closes a popup with a specified index and result if there is only one popup the index is one.
        /// </summary>
        /// <param name="popupIndex">the index of the popup to close</param>
        /// <param name="result">the result value of the button that should be clicked to close the popup</param>
        [TxAction("ClosePopUp", "Close the Popup window.")]
        public void ClosePopup(bool result)
        {
            Driver.ClickAt(GetPopupButton(result.ToString().ToLower()));
            
        }
        [TxAction("ClosePopupWithinFrame", "Close the Popup window.")]
        public void ClosePopupWithinFrame(bool result)
        {
            Driver.ClickAt(By.XPath(".//div[@dhxbox]//div[@result=\"" + result.ToString().ToLower() + "\"]"), null, By.XPath(".//iframe[@cd_frame_id_]"));
        }

        [TxAction("CloseWindowPopup", "")]
        public void CloseWindowPopup(bool validate=true)
        {
            By userChoice;
            List<IWebElement> headers = Driver.FindElement(By.TagName("body"))
                .FindElements(By.XPath(".//div[@class = \"dhxwin_text_inside\"]")).ToList();
            if (validate)
                userChoice = By.XPath(".//input[starts-with(@id, \"idBtnValidate\")]");
            else
                userChoice = By.XPath(".//input[starts-with(@id, \"idBtnCancel\") or @value='Cancel']");
            Driver.ClickAt(userChoice,Driver.WaitForElement(By.XPath("(.//div[@class = \"dhxwin_active\"])[" + headers.Count+"]")));
        }

        [TxAction("Next", "")]
        public void Next()
        {
            Driver.ClickAt(By.XPath(".//input[@value='Next']"));
        }

        [TxAction("CloseConsecutivePopUp", "Close the Popup window when there is a bug.")]
        public void CloseConsecutivePopUp(int numberOfPopup)
        {

            for (int i = numberOfPopup; i > 0; i--)
            {
                Driver.ClickAt(By.XPath(".//div[@dhxbox][" + i + "]//div[@result=\"true\"]"), null, null);
            }
        }

        /// <summary>
        /// used for advanced deletion
        /// </summary>
        /// <param name="popupIndex">the index of the popup to close</param>
        /// <param name="result">the result value of the button that should be clicked to close the popup</param>
        [TxAction("AdvancedOption", "okNoCancel = ok, no or cancel")]
        public void AdvancedOption(string advDelete, string yesOrCancelOrNo, int windowIndex = 1)
        {
            By userChoice;
            if (yesOrCancelOrNo == "Yes")
                userChoice = By.XPath(".//input[starts-with(@id, \"idBtnValidate\")]");
            else if (yesOrCancelOrNo == "Cancel")
                userChoice = By.XPath(".//input[starts-with(@id, \"idBtnCancel\")]");
            else
                userChoice = By.XPath(".//input[starts-with(@id, \"idBtnNo\")]");

            new WEGDHtmlxComboBox(Driver, By.XPath(".//div[contains(@id,\"idDivQueryCombo\")]")).SelectOption(advDelete);
            Driver.ClickAt(new ByChained(By.XPath("(.//div[starts-with(@id, \"idDivBtns\")])[" + windowIndex + "]"), userChoice));
        }

        [TxAction("DropDownList", "okNoCancel = ok, no or cancel")]
        public WEGDHtmlxComboBox DropDownList()
        {
            return new WEGDHtmlxComboBox(Driver, By.XPath(".//div[contains(@id,\"idDivQueryCombo\")]"));
        }

        /// <summary>
        /// Closes a popup with a specified index and result if there is only one popup the index is one.
        /// </summary>
        /// <param name="popupIndex">the index of the popup to close</param>
        /// <param name="result">the result value of the button that should be clicked to close the popup</param>
        [TxAction("AddLink", "okNoCancel = ok, no or cancel")]
        public WDValidatedWindow<WForm> AddLink(string linkObject = null, bool okOrCancel = false, bool editForm = true)
        {
           
            if (linkObject != null)
            {
                By userChoice;
                if (okOrCancel == true)
                    userChoice = By.Id("idBtnValidate");
                else
                    userChoice = By.Id("idBtnCancel");

                new WEGDHtmlxComboBox(Driver, By.XPath(".//div[starts-with(@id,\"idDivQueryCombo\")]")).SelectOption(linkObject);
                Driver.ClickAt(new ByChained(By.XPath(".//div[starts-with(@id,\"idDivBtns\")]"), userChoice));
            }

            if (!editForm)
                return null;
            else
            {
                WForm formCreator = new WForm(Driver, WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(Driver, formCreator);
            }
        }
        [TxAction("ReadPopUpMessage", "Close the Popup window.")]
        public ActionColl<string> ReadPopUpMessage()
        {
            ICollection<string> strCollection = new List<string>();
            string popUpMessagedata = Driver.WaitForElement(GetPopupMessage()).GetElement().Text;

            string[] arraystr = popUpMessagedata.Split('\n');
            int i = 0;
            foreach (string data in arraystr)
            {
                arraystr[i] = data.Replace("\r", "");
                i++;
            }

            strCollection = arraystr.ToList();
            return new ActionColl<string>(strCollection);
        }


        //[TxAction("ReadConfirmationPopUpMessage", "Read message of confirmation Popup window.")]
        //public ActionColl<string> ReadConfirmationPopUpMessage(bool currentFrame = true)
        //{
        //    ICollection<string> strCollection = new List<string>();
        //    By framePopUp;

        //    if (!currentFrame)
        //        framePopUp = null;
        //    else
        //        framePopUp = Driver.CurrentFrameBy;

        //    string popUpMessagedata = Driver.WaitForElement(By.XPath(".//div[@dhxbox and contains(@class,\"dhtmlx-confirm\")]//span"), null, framePopUp).GetElement().Text;

        //    string[] arraystr = popUpMessagedata.Split('\n');
        //    int i = 0;
        //    foreach (string data in arraystr)
        //    {
        //        arraystr[i] = data.Replace("\r", "");
        //        i++;
        //    }

        //    strCollection = arraystr.ToList();
        //    return new ActionColl<string>(strCollection);
        //}
        [TxAction("ReadConfirmationPopUpMessage", "Read message of confirmation Popup window.")]
        public ActionColl<string> ReadConfirmationPopUpMessage()
        {
            ICollection<string> strCollection = new List<string>();
            string popUpMessagedata = Driver.WaitForElement(By.XPath(".//div[@dhxbox and contains(@class,\"dhtmlx-confirm\")]//span")).GetElement().Text;

            string[] arraystr = popUpMessagedata.Split('\n');
            int i = 0;
            foreach (string data in arraystr)
            {
                arraystr[i] = data.Replace("\r", "");
                i++;
            }

            strCollection = arraystr.ToList();
            return new ActionColl<string>(strCollection);
        }

        [TxAction("CloseConfirmationPopup", "Close the confirmation Popup window.")]
        public void CloseConfirmationPopup(bool result)
        {
            Driver.WaitForAjax();
            Driver.ClickAt(result ? By.XPath(".//div[@dhxbox and contains(@class,\"dhtmlx-confirm\")]//div[text()=\"Ok\" or text()=\"Yes\" or text()=\"Oui\"]") : By.XPath(".//div[@dhxbox and contains(@class,\"dhtmlx-confirm\")]//div[text()=\"Cancel\" or text()=\"No\"or text()=\"Non\"or text()=\"Annuler\"]"));
        }

        [TxAction("ReadHeader", "")]
        public string ReadHeader()
        {
            List<IWebElement> headers = Driver.FindElement(By.TagName("body"))
                .FindElements(By.XPath(".//div[@class = \"dhxwin_text_inside\"]")).ToList() ;
            return headers.Last().Text;
        }

        [TxAction("CloseInformationPopup", "Close Information Popup")]
        public void CloseInformationPopup(int numberOfPopup = 1, bool currentFrame = true)
        {
            By framePopUp;

            if (!currentFrame)
                framePopUp = null;
            else
                framePopUp = Driver.CurrentFrameBy;
            if (numberOfPopup == 1)
                Driver.ClickAt(By.XPath(".//div[@class=\"dhtmlx_popup_controls\"]//div[@class=\"dhtmlx_popup_button\"]"), null, framePopUp);
            else
            {
                for (int i = numberOfPopup; i >= 1; i--)
                    Driver.ClickAt(By.XPath(".//div[@class=\" dhtmlx_modal_box dhtmlx-alert\"][" + i + "]//div[@class=\"dhtmlx_popup_button\"]"), null, framePopUp);
            }

        }

        [TxAction("ManageInputBox", "")]
        public void ManageInputBox(string value, bool okOrCancel = false)
        {
            new WEGInput(Driver, By.XPath(".//input[starts-with(@id,\"idTextQueryString\")]")).Fill(value);
            By userChoice;
            if (okOrCancel == true)
                userChoice = By.Id("idBtnValidate");
            else
                userChoice = By.Id("idBtnCancel");
            Thread.Sleep(2000);
            Driver.ClickAt(new ByChained(By.XPath(".//div[starts-with(@id,\"idDivBtns\")]"), userChoice));
            Thread.Sleep(1000);
        }

        [TxAction("ReadInputBox", "")]
        public string ReadInputBox()
        {
            return new WEGInput(Driver, By.XPath(".//input[starts-with(@id,\"idTextQueryString\")]")).Read();
        }

        [TxAction("DoNotAskAgain", "")]
        public WEGCheckBox DoNotAskAgain()
        {
            return new WEGCheckBox(Driver, By.XPath(".//input[starts-with(@id,'inputWFPAAsk')]"));
        }
    }
}