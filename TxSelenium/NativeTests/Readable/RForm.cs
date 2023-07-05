using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TxSelenium.NativeTests.Handlers;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.DataTypes;
using XmlExecutor.DataTypes;
using XmlExecutor.Attributes;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.NativeTests.Windows;
using TxSelenium.GenericTests;
using TxSelenium.GenericTests.TxCharts;
using TxSelenium.Utils;
using System.Linq;

namespace TxSelenium.NativeTests.Readable
{
    /// <summary>
    /// This class manages interactions with a form in read mode.
    /// </summary>
    public class RForm : WDForm
    {
        public RForm(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, bool formInsiddeWindow = false)
            : base(driver, elemIdentifier, parent, formInsiddeWindow)
        {
        }

        /// <summary>
        /// Reads data from this form of a certain type for a specified attribute.
        /// </summary>
        /// <typeparam name="T">the expected data type of the attribute</typeparam>
        /// <param name="attrId">the id of the attribute</param>
        /// <returns>the value</returns>
        [TxAction("Read", "Reads the value of an attribute identified by its id.")]
        public T Read<T>(int attrId, int groupId = 0, int tabId = 0) where T : IReadableData
        {
            if (groupId > 0)
                return HandlersFactory.GetReadingHandler<T>().ReadForm(GetAttributeElemByGroupId(attrId, groupId), attrId);
            else
            {
                if (tabId > 0)
                    return HandlersFactory.GetReadingHandler<T>().ReadForm(GetAttributeElemByTabId(attrId, tabId - 1), attrId);
                else
                    return HandlersFactory.GetReadingHandler<T>().ReadForm(GetAttributeElem(attrId), attrId);
            }
        }

        /// <summary>
        /// for testing translation of
        /// attribute name
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("AttributeName", "Read and check the name of any attribute.")]
        public string AttributeName(int attrId)
        {
            WERefreshed attribute = GetDriver().WaitForElement(By.XPath(".//label[starts-with(@id ,\"idLabelAttNameLabel" + attrId + "\")]"), this);
            string attributeName = attribute.GetElement().Text;
            if (GetDriver().IsElementPresent(By.XPath(".//span[contains(@class,\"clMandatorySpan\")]"), attribute.GetElement()))
                attributeName += GetDriver().WaitForElement(By.XPath(".//span[contains(@class,\"clMandatorySpan\")]"), attribute).GetElement().Text;
            return attributeName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("ReadListLink", "Use to read list of link we can click.")]
        public ActionColl<WEGLink> ReadListLink(int attrId)
        {
            WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
            ICollection<IWebElement> listLink = attrDiv.GetElement().FindElements(By.XPath(".//a[not(contains(@style, \"display: none;\"))]|.//tr[not(contains(@style,\"display: none;\"))]//span[@class=\"standartTreeRow\"]"));
            ICollection<WEGLink> values = new List<WEGLink>(listLink.Count);
            for (int i = 1; i <= listLink.Count; i++)
                values.Add(new WEGLink(GetDriver(), By.XPath(".//div/a[" + i + "]|.//div[" + i + "]/a | .//a[" + i + "]|.//tr[not(contains(@style,\"display: none;\")) and not(@idnode)][" + i + "]//span[@class=\"standartTreeRow\"]"), attrDiv));

            return new ActionColl<WEGLink>(values);
        }

        [TxAction("ReadVisualizationActivatedForPDF", "Read RichText Value")]
        public void ReadVisualizationActivatedForPDF(int attrId, string fileName)
        {
            WERefreshed pdf = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivPdfFileView\")]/object"), GetAttributeElem(attrId));
            String data = pdf.GetElement().GetAttribute("data");
            if (!pdf.GetElement().GetAttribute("data").Contains(fileName))
                throw new Exception("PDF is not displaying...");

        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="attrId"></param>
        ///// <returns></returns>
        //[TxAction("ReadListListing", "Use to read List of link that we can not click")]
        //public ActionColl<DTText> ReadListLinkText(int attrId)
        //{

        //    WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
        //    ICollection<IWebElement> listLink = attrDiv.GetElement().FindElements(By.XPath(".//label[@iindexobject] | .//tr//td[@colspan]//span[@class=\"standartTreeRow\"]"));
        //    ICollection<DTText> values = new List<DTText>(listLink.Count);

        //    for (int i = 1; i <= listLink.Count; i++)
        //        values.Add(new DTText(GetDriver().FindElement(attrDiv.GetElement(), By.XPath(".//label[@iindexobject][" + i + "] | .//tr[@idnode=\"" + i + "\" and not(@title)]//span")).Text));

        //    return new ActionColl<DTText>(values);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        [TxAction("DisplayMoreLessLinks", "Display more or less links/listings")]
        public void AdjustListLink(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//*[@id=\"idAMoreLinks\"]|.//*[starts-with(@id,\"idDivClickToSeeMorediv\")]"), elem);
        }

        /// <summary>
        /// Reads an associative class from this form.
        /// </summary>
        /// <param name="attrId">the id of the associative class attribute</param>
        /// <returns>the associative class</returns>
        [TxAction("ReadAsso", "Gets an associative class in read mode.")]
        public RAssociative ReadAssoClass(int attrId)
        {
            WERefreshed attribute = GetAttributeElem(attrId);
            return new RAssociative(GetDriver(), By.TagName("div"), attribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("Source", "Click on Source button and fill the formularay.")]
        public WDWindow<RForm> Source(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath("./..//img[@title = \"Source\"]"), elem);
            RForm rform = new RForm(GetDriver(), By.XPath(".//div[starts-with(@id,'divTabbar')]"));
            return new WDWindow<RForm>(GetDriver(), rform);
        }

        [TxAction("IsSourcePresent", "Check Is Source Present")]
        public bool IsSourcePresent(int attrId)
        {
            return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,\"ExistingSource\")]/parent::div[contains(@id,\"idDisplaySourceLabel" + attrId + "\")]"));
        }

        [TxAction("IsInformationPresent", "Check Is Information Present")]
        public bool IsInformationPresent(int attrId)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//label[starts-with(@id ,\"idLabelAttNameLabel" + attrId + "\")]"), this);
            return elem.GetElement().GetAttribute("style").Contains("underline");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("Information", "Click on Source button and fill the formularay.")]
        public WDWindow<RForm> Information(int attrId)
        {
            //WERefreshed elem = GetAttributeElem(attrId);
            //GetDriver().ClickAt(By.XPath("./..//img[@title = \"Information\"]"), elem);
            GetDriver().ClickAt(By.XPath(".//label[starts-with(@id ,\"idLabelAttNameLabel" + attrId + "\")]"), this);
            RForm rform = new RForm(GetDriver(), By.XPath(".//div[starts-with(@id,'divTabbar')]"));
            return new WDWindow<RForm>(GetDriver(), rform);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        [TxAction("ClickOnGraph", "Open on graph and open txcharts in new tab")]
        public GTTab<GTTxCharts> ClickOnPicture(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            try
            {
                GetDriver().ClickAt(new ByChained(By.ClassName("cl_div_table_picture"), By.TagName("img")), elem);
            }
            catch
            {
                GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"TxCharts\")]"),this);
            }
            GTTxCharts txCharts = new GTTxCharts(GetDriver(), By.TagName("body"));
            return new GTTab<GTTxCharts>(GetDriver(), txCharts);

        }

        /// <summary>
        /// Changes the unit of an attribute which type is decimal value and has a unit.
        /// </summary>
        /// <param name="attrId">the id of the attribute</param>
        /// <param name="unit">the id of the unit</param>
        [TxAction("ChangeUnit", "Change Unit using name.")]
        public void ChangeUnit(int attrId, string unit)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[contains(@class,\"dhxcombo_material\")]"), elem).SelectOption(unit);
        }

        [TxAction("RichText", "Read RichText Value")]
        public RRichText ReadRichText(int attrId)
        {
            return new RRichText(GetDriver(), By.TagName("html"), null, new ByChained(GetAttrDiv(attrId, CurrentTab), By.XPath(".//iframe[starts-with(@id,\"idFrameText\")]")));
        }

        [TxAction("ExportData", "Export the data in Excel or tsv format.")]
        public void ExportData(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            GetDriver().ClickAt(ElemByPictureName.ExportRf, elem);
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// For checking if website is displayed
        /// after activating url display
        /// </summary>
        /// <param name="attrId"></param>
        [TxAction("ReadUrlActivated", "Read RichText Value")]
        public string ReadUrlActivated(int attrId)
        {
            bool IsURLActivated = GetDriver().IsElementPresent(By.XPath(".//div[@class='clDivForPreviewURL']"), GetAttributeElem(attrId).GetElement());

            if (IsURLActivated)
                return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,'previewURLMainContainer')]"), GetAttributeElem(attrId)).GetElement().GetAttribute("title");
            else
                return "URL is not activated";
        }

        [TxAction("AttributeIsDiplayed", "Read RichText Value")]
        public bool AttributeIsDiplayed(int attrId, int idGroup = 0)
        {
            bool res = GetDriver().IsElementPresent(GetAttrDiv(attrId, CurrentTab, idGroup), this.GetElement());
            return res;
        }
        [TxAction("ReadListValue", "")]
        public ActionColl<string> ReadListValue(string attributeId)
        {
            GetDriver().WaitForAjax();
            ICollection<IWebElement> element;

            element = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"div" + attributeId + "\")]")).GetElement().FindElements(By.XPath(".//label"));

            ICollection<string> configNames = new List<string>(element.Count);
            for (int i = 0; i < element.Count; i++)
                configNames.Add(element.ElementAt(i).Text);
            return new ActionColl<string>(configNames);
        }
        [TxAction("ClcikHereToDisplayRemainingList", "ClcikHereToDisplayRemainingList")]
        public void ClcikHereToDisplayRemainingList(int attrId)
        {
            WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'idDivClickToSeeMorediv')]"), attrDiv);
            WERefreshed listDiv = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivLinksdiv" + attrId + "\")]"), attrDiv);
            GetDriver().ScrollToElement(GetDriver().WaitForElement(By.XPath(".//tr[not(@idNode)][last()]"), listDiv).GetElement());
        }
        [TxAction("ReadListListing", "Use to read List of link that we can not click")]
        public ActionColl<DTText> ReadListListing(int attrId, int groupId = 0)
        {
            WERefreshed attrDiv;
            if (groupId > 0)
                attrDiv = GetAttributeElemByGroupId(attrId, groupId);
            else
                attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
            ICollection<IWebElement> listLink = attrDiv.GetElement().FindElements(By.XPath(".//label[@iindexobject]|.//tr[@idnode and not(contains(@style,'display: none'))]"));
            ICollection<DTText> values = new List<DTText>(listLink.Count);

            for (int i = 1; i <= listLink.Count; i++)
                values.Add(new DTText(GetDriver().FindElement(attrDiv.GetElement(), By.XPath(".//tr[not(@idnode)][" + i + "]//tr[@idnode]/td[4]/span|.//label[@iindexobject][" + i + "]")).Text));

            return new ActionColl<DTText>(values);
        }

        protected virtual WERefreshed GetAttributeElemByTabId(int id, int tabId)
        {
            WERefreshed attributeElem = GetDriver().WaitForElement(GetAttrDiv(id, tabId), this);

            return attributeElem;
        }

        private WERefreshed GetAttributeElemByGroupId(int attrId, int groupId)
        {
            throw new NotImplementedException();
        }

        [TxAction("ReadVisualizationActivated", "Read RichText Value")]
        public void ReadVisualizationActivated(int attrId, string pictureName)
        {
            WERefreshed img = GetDriver().WaitForElement(By.XPath(".//img[contains(@id,\"idImgVisualisationFile\")]"), GetAttributeElem(attrId));
            string test = img.GetElement().GetAttribute("title");
            if (!img.GetElement().GetAttribute("title").Contains(pictureName))
                throw new Exception("image not displayed ! ");

        }
        [TxAction("ReadListImageForDocument", "Use to read list of link we can click.")]
        public ActionColl<string> ReadListImageForDocument(int attrId)
        {
            WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
            ICollection<IWebElement> listLink = attrDiv.GetElement().FindElements(By.XPath(".//div[@class]"));
            ICollection<string> values = new List<string>(listLink.Count);
            for (int i = 1; i <= listLink.Count; i++)
            {

                IWebElement element1 = GetDriver().WaitForElement(By.XPath(".//div[@class][" + i + "]//img"), attrDiv).GetElement();

                values.Add(element1.GetAttribute("title"));
            }
            return new ActionColl<string>(values);
        }

        //[TxAction("ClcikHereToDisplayRemainingList", "ClcikHereToDisplayRemainingList")]
        //public void ClcikHereToDisplayRemainingList(int attrId)
        //{
        //    WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
        //    GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,'idDivClickToSeeMorediv')]"), attrDiv);
        //    WERefreshed listDiv = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivLinksdiv" + attrId + "\")]"), attrDiv);
        //    GetDriver().ScrollToElement(GetDriver().WaitForElement(By.XPath(".//tr[not(@idNode)][last()]"), listDiv).GetElement());
        //}

        [TxAction("IsValueContains", "To check total value of a attribute contains the given value or not")]
        public bool IsValueContains(int attrId, string value)
        {
            WERefreshed elem = GetAttributeElem(attrId);
          
            string attrValue = GetDriver().WaitForElement(By.XPath(".//label | .//a[text()]"), elem).GetElement().Text;
        
            return attrValue.Contains(value);
            
        }
        [TxAction("DecrementIndex", "")]
        public string DecrementIndex(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            string attrValue = GetDriver().WaitForElement(By.TagName("label"), elem).GetElement().Text;
            return (Convert.ToInt32(attrValue) - 1).ToString();
        }

        [TxAction("ExpandLink", "")]
        public void ExpandLink(int attrId)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"idDivClickToSeeMorediv" + attrId + "\")]"));
        }

        [TxAction("FullScreenLink", "")]
        public void FullScreenLink(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"Fullscreen.png\")]"), elem);
        }

        [TxAction("ReadLinkByName", "")]
        public WEGLink ReadLinkByName(int attrId, string name)
        {
            WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
            if (GetDriver().IsElementPresent(By.XPath(".//div[starts-with(@id,\"idDivClickToSeeMorediv" + attrId + "\")]"), attrDiv.GetElement()))
            {
                GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idDivClickToSeeMorediv" + attrId + "\")]"), attrDiv);
                WEGInput search = new WEGInput(GetDriver(), By.ClassName("dhxtoolbar_input"), attrDiv);
                search.Fill(name);
                search.PressKeyFromKeyBoard().PressEnter();
            }
            return new WEGLink(GetDriver(), By.XPath(".//a[@iindexobject]|.//tr[@idnode and not(contains(@style,'display: none'))]//span[text()=\"" + name + "\"]"), attrDiv);
        }
        [TxAction("ReadListLinkForDocument", "Use to read list of link we can click.")]
        public ActionColl<WEGLink> ReadListLinkForDocument(int attrId)
        {
            WERefreshed attrDiv = GetDriver().WaitForElement(GetAttrDiv(attrId, CurrentTab), this);
            ICollection<IWebElement> listLink = attrDiv.GetElement().FindElements(By.XPath(".//a[not(contains(@style, \"display: none;\"))]"));
            ICollection<WEGLink> values = new List<WEGLink>(listLink.Count);
            for (int i = 1; i <= listLink.Count; i++)
                values.Add(new WEGLink(GetDriver(), By.XPath(".//div/a[" + i + "]|.//div[" + i + "]/a | .//a[" + i + "]"), attrDiv));

            return new ActionColl<WEGLink>(values);
        }
        [TxAction("ReadValue", "Read value for Webfrom")]
        public string ReadValue(int attributeId)
        {

            return GetDriver().FindElement(this.GetElement(), By.XPath(".//div[starts-with(@id,\"div" + attributeId + "\")]//label")).Text;
        }
        [TxAction("IsEnvelopButtonPresent", "")]
        public bool IsEnvelopButtonPresent(int id)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@id,\"Label" + id + "\")]/..//img[contains(@src,\"SendEmail.png\")]"), this.GetElement());
        }
        [TxAction("IsGroupPresent", "")]
        public bool IsGroupPresent(string groupName,int Id)
        {
            return GetDriver().IsElementPresent(By.XPath(".//fieldset[contains(@id,\"group-"+Id+"\")]//legend[text()=\""+groupName+ "\"]|.//fieldset[contains(@id,\"group" + Id + "\")]//legend[text()=\"" + groupName + "\"]"),this.GetElement());
        }

        [TxAction("ManagePreviewFile", "")]
        public RPreviewFile ManagePreviewFile(int attrId)
        {
            WERefreshed elem = GetAttributeElem(attrId);
            Sleep(1);
            return new RPreviewFile(GetDriver(), elem.ElemIdentifier, elem.Parent);
        }

        [TxAction("ReadBlackBox", "")]
        public RAssociative ReadBlackBox(string blackBoxId)
        {
            //WERefreshed blackBoxDiv = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"BlackBoxe"+ blackBoxId + "_\")]"),this);
            return new RAssociative(GetDriver(), By.XPath(".//div[starts-with(@id,\"BlackBoxe" + blackBoxId + "_\")]"), this);
        }
    }
}
