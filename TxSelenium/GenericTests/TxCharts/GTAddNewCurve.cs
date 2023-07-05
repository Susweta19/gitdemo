using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.GenericDevs.TxMCS;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxCharts
{
    public class GTAddNewCurve : WERefreshed
    {
        public GTAddNewCurve(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
                : base(driver, elemIdentifier, parent)
        {
        }

        /*================================== BUTTON ==========================================*/

        [TxAction("Next", "Button to Next")]
        public void Next()
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id, \"idBtnNext\")]"),this);
            GetDriver().WaitForAjax();
        }
        [TxAction("ReadCurrentTab", "Read Tab Name")]
        public string ReadCurrentTab(string TabName)
        {
            TabName= GetDriver().FindElement(By.XPath(".//div[contains(@class,'active') and not(contains(@class,'inactive'))]/span")).Text;
            return TabName;
        }



        [TxAction("Finish", "Button to Finish")]
        public void Finish()
        {
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id, \"idBtnValidate\")]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("Cancel", "Button to Cancel")]
        public void Cancel()
        {
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id, \"idBtnCancel\")]"), this);
            GetDriver().WaitForAjax();
        }

        [TxAction("Previous", "Selects previous button")]
        public void Previous()
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id, \"idBtnPrevious\")]"), this);
            GetDriver().WaitForAjax();
        }

        /*================================== SOURCE TYPE ==========================================*/

        [TxAction("SourceType", "Select Source Type")]
        public void SourceType(string typeName)
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt( By.XPath(".//div[starts-with(@id, \"sideBarItem\") and text()='"+ typeName + "']"), this);
            GetDriver().WaitForAjax();
            //(".//div[starts-with(@class, \"sidebarSide\") and not(@id)]")
        }

        [TxAction("IsTypeSelected", "Is Type Selected")]
        public bool IsTypeSelected(string typeName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@class, \"active\") and text()='"+ typeName + "']"),this.GetElement());
        }

        /*================================== SETTINGS ==========================================*/

        [TxAction("CurveName", "Set Curvename")]
        public WEGInput Curvename()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[@name = \"name\"]"), this);
        }

        [TxAction("FunctionEquation", "Set Curvename")]
        public WEGInput FunctionEquation()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]//input[starts-with(@id,'eq')]"), this);
        }

        [TxAction("SetA", "Set the value of A")]
        public WEGInput SetA()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]//input[starts-with(@id,'a')]"), this);
        }

        [TxAction("SetB", "Set the value of B")]
        public WEGInput SetB()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]//input[starts-with(@id,'b')]"), this);
        }

        [TxAction("AbscissasLowerbound", "Set the value of Abscissas - Lower bound")]
        public WEGInput AbscissasLowerbound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]//input[starts-with(@id,'min')]"), this);
        }

        [TxAction("AbscissasUpperbound", "Set the value of Abscissas - Upper bound")]
        public WEGInput AbscissasUpperbound()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]//input[starts-with(@id,'max')]"), this);
        }
        //[TxAction("NumberOfPoints", "Set the NumberOfPoints")]
        //public WEGInput NumberOfPoints()
        //{
        //    return new WEGInput(GetDriver(), By.XPath(".//*[@id = \"nbpoint\"] | .//*[@id = \"rowcount\"] "), this);
        //}

        [TxAction("Degree", "Set the Degree")]
        public WEGInput Degree()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,'degree')]"), this);

        }

        [TxAction("CoefficientValues", "Set the Coefficient according index number and value.")]
        public void CoefficientValues(int index, string value)
        {
            new WEGInput(GetDriver(), By.XPath(".//*[contains(@id,'coeffs')]//input[@name='"+index+"']"), this).Fill(value);
        }

        [TxAction("FileType", "Select the type of File")]
        public WEGDHtmlxComboBox FileType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idFileType\")]"), this);
        }

        [TxAction("Separator", "Select the Separator")]
        public WEGDHtmlxComboBox Separator()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idSeparator\")]"), this);
        }
        [TxAction("SetHeaderLinesToSkip", "Number of header lines to skip")]
        public WEGInput SetHeaderLinesToSkip()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"linesToIgnore\")]"), this);
        }


        [TxAction("CommentsTag", "Set Comments tag")]
        public WEGInput CommentsTag()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@id,\"comments\")]"), this);
        }

        [TxAction("Tranpose", "Tick Tranpose")]
        public WEGCheckBox Tranpose()
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//input[starts-with(@id,\"transpose\")]"), this);
        }

        [TxAction("ObjectType", "***")]
        public WEGDHtmlxComboBox ObjectType()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idTxTableObjectType\")]|.//div[starts-with(@id,\"idObjectType\")]|.//div[starts-with(@id,\"idTxObjectsCorrelationObjectType\")]"), this);
            //(".//*[@id = \"idDivComboOT\"] | .//*[@id = \"id_div_OT\"] ")
        }

        [TxAction("Attributes", "***")]
        public WEGDHtmlxComboBox Attributes()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idTxTableAttribute\")]|.//div[starts-with(@id,\"idAttribute\")]"), this);
            //(".//*[@id = \"idDivComboAttTxTable\"] | .//*[@id = \"select_atr\"] | .//*[@id = \"idDivComboAttributes\"] ")
        }

        [TxAction("AttributeType", "***")]
        public void AttributeType(int index)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"idAttributeTypeFilter\")]/span["+ index + "]"),this);
            GetDriver().WaitForAjax();
        }

        [TxAction("Abscissa", "***")]
        public WEGDHtmlxComboBox Abscissa()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[@id = \"idDivComboAbsTxTable\"] | .//div[starts-with(@id,\"idTxTableAbscissa\")] |.//div[starts-with(@id,'idTxObjectsCorrelationAbscissa')]|.//div[@id=\"select_xAttr\"] "), this);
        }

        [TxAction("Ordinate", "***")]
        public WEGDHtmlxComboBox Ordinate()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[@id = \"idDivComboOrdTxTable\"] | .//div[starts-with(@id,\"idTxTableOrdonate\")] |.//div[starts-with(@id,\"idTxObjectsCorrelationOrdonate\")]|.//div[@id=\"select_yAttr\"] "), this);
        }

        [TxAction("Files", "***")]
        public WEGDHtmlxComboBox Files()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//div[starts-with(@id,\"idFile\") and not(contains(@id,'Type'))]"), this);
        }

        [TxAction("LabelBox", "***")]
        public WEGDHtmlxComboBox LabelBox()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[@id =\"select_labelserie\"] | .//div[starts-with(@id,\"idTxTableLabel\")]"), this);
            //(".//div[contains(@class,'txTableObjects objectsTree dhxtree_dhx_skyblue')]"), this);
            //(".//*[@id =\"select_labelserie\"] | .//div[starts-with(@id,\"idTxTableLabel\")]"), this);
        }

        [TxAction("LabelObjectLink", "Label Object Link")]
        public WMultipleLink LabelObjectLink()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idTxObjectsCorrelationObjects\")]/.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("DeleteLabelBox", "***")]
        public void DeleteLabelBox()
        {
            GetDriver().ClickAt(new ByChained(By.Id("idLinkClearLabelCombo"), By.TagName("img")), this);
        }

        //tree 1
        [TxAction("Objects", "***")]
        public  WMultipleLink Objects()
        {
            //probleme with selectall !!!!

           return new WMultipleLink(GetDriver(), By.XPath(".//div[starts-with(@id,\"idTxTableObjects\")]/..|.//div[starts-with(@id,\"idObjects\")]/.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            //return new WDValidatedWindow<WMultipleLink>(GetDriver(),multiLink);
        }

        //tree 2 
        [TxAction("ObjectOrLabelsTree", "***")]
        public WMultipleLink ObjectOrLabelsTree()
        {
            //probleme with selectall !!!!
            return new WMultipleLink(GetDriver(), By.Id("id_div_object"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
        }

        [TxAction("ChooseFile", "Tick Tranpose")]
        public void ChooseFile(string pathFile)
        {
            WERefreshed button = GetDriver().WaitForElement(By.XPath(".//input[starts-with(@id,\"idLocalFile\")]"), this);
            GetDriver().UploadFile(button, pathFile);
        }

        /*================================== APPEARANCES ==========================================*/

        [TxAction("FormatAbcissa", ".....")]
        public WEGDHtmlxComboBox FormatAbcissae()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[starts-with(@id, \"abscissa_format\")]"), this);
        }

        [TxAction("FormatOridnate", ".....")]
        public WEGDHtmlxComboBox FormatOridnate()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.XPath(".//*[starts-with(@id, \"ordinate_format\")]"), this);
        }

        [TxAction("SignificantFiguresAbscissa", "............")]
        public WEGInput SignificantFiguresAbscissa()
        {
            return new WEGInput(GetDriver(), By.XPath(".//*[starts-with(@id, \"abscissa_precision\")]"), this);
        }

        [TxAction("SignificantFiguresOridnate", "............")]
        public WEGInput SignificantFiguresOridnate()
        {
            return new WEGInput(GetDriver(), By.XPath(".//*[starts-with(@id, \"ordinate_precision\")]"), this);
        }

        [TxAction("SelectPointsStyle", "1  for points ,  2 for lines 3 for ponits and lines")]
        public void SelectPointsStyle(int style)
        {
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//span[@data-icon-id= \""+style+"\"]/img"), this);
        }

        [TxAction("SelectCurveColor", "Selects a curve color")]
        public GTSelectColor SelectCurveColor()
        {
            WaitForAjax();
            return new GTSelectColor(GetDriver(), By.XPath(".//*[starts-with(@id, \"colorPickerContainer\")]"), this);
        }

        /*================================== VALUES ==========================================*/

        [TxAction("SetValue", "SetValue")]
        public WEGInput SetValue(int colIndex, int rowIndex)
        {
            return new WEGInput(GetDriver(), new ByChained(By.ClassName("ValueTable"), By.XPath(".//tr[" + rowIndex + "]//td[" + colIndex + "]//input")), this);
        }

        [TxAction("ReadValue", "SetValue")]
        public string ReadValue(int colIndex, int rowIndex)
        {
            string text = GetDriver().WaitForElement(new ByChained(By.ClassName("ValueTable"), By.XPath(".//tr[" + rowIndex + "]//td[" + colIndex + "]")), this).GetElement().Text;
            return text;
        }

        /*================================== TAB ==========================================*/
        [TxAction("TabChartType", "Tab Chart Type")]
        public GTTabChartType TabChartType()
        {
            return new GTTabChartType(GetDriver(),By.XPath(".//div[starts-with(@id,\"idChartTypeTab\")]"));
        }
        
        [TxAction("AddSeries", "AddSeries")]
        public WDValidatedWindow<WMultipleLink> AddSeries()
        {
            GetDriver().ClickAt(By.XPath(".//div[@title='Add series']"),this);
            GetDriver().WaitForAjax();
            WMultipleLink multiplelink=new WMultipleLink (GetDriver(), By.XPath(".//div[starts-with(@class,\"dhx_popup_area\")]/.."), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, this);
            return new WDValidatedWindow<WMultipleLink>(GetDriver(), multiplelink);
            //return new WDValidatedWindow<WMultipleLink>(GetDriver(),multiLink);
        }
        [TxAction("ValidateCancelPopup", "")]
        public void ValidateCancelPopup(bool validate = true)
        {
            By button;
            button=validate? By.XPath(".//div[starts-with(@id,\"idTxTableObjectsButtonBar\")]//input[@id=\"btnValidate\"]"):By.XPath(".//div[starts-with(@id,\"idTxTableObjectsButtonBar\")]//input[@id=\"btnCancel\"]");
            GetDriver().ClickAt(button);
        }
        

        

        [TxAction("DeleteSeries", "DeleteSeries")]
        public void DeleteSeries(string series)
        {
            GetDriver().ClickAt(By.XPath(".//td[@class='standartTreeRow' and not(contains(@style,'display: none'))]//span[text()='"+ series + "']"));
            GetDriver().WaitForAjax();
            GetDriver().ClickAt(By.XPath(".//div[@title='Delete Series']"));//delete series change xpath
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// MCS is required only for TxCharts.
        /// </summary>
        /// <returns></returns>
        [TxAction("DisplayMulticriteriaSelection", "Display the multicriteria selection module...")]
        public GTTxMCS DisplayMulticriteriaSelection()
        {
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"MCS.png\")]"), this);
            GetDriver().WaitForAjax();
            return new GTTxMCS(GetDriver(), By.XPath(".//div[@class=\"dhx_popup_dhx_skyblue\"]"), this);
        }
        [TxAction("CheckAllObject", "Label Object Link")]
        public WMultipleLink CheckAllObject()
        {
            return new WMultipleLink(GetDriver(), By.XPath(".//div[@class='dhx_popup_dhx_skyblue'and not(contains(@style,\"display: none\"))]"), WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy);
        }

        [TxAction("isCoEfficientValuesPrasent", "")]
        public bool isCoEfficientValuesPrasent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//*[contains(@id,'coeffs')]//input[@name='1']"),this.GetElement());
        }


        [TxAction("numberOfCoefficientInputBox", "ReadCellValue")]
        public int numberOfCoefficientInputBox()
        {
            return GetDriver().FindElement(By.XPath(".//form[starts-with(@id,'coeff')]")).FindElements(By.XPath(".//input")).Count;
        }
        [TxAction("IsTabSelected", "ManageAdvanced")]
        public bool IsTabSelected(string TabName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@id,'idAttributeTypeFilter')]//span[@class='selected' and text()='"+TabName+"']"),this.GetElement());
        }

        [TxAction("IsTabEnabled", "ManageAdvanced")]
        public bool IsTabEnabled(string TabName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class='dhxtabbar_tab dhxtabbar_tab_actv']//div[text()='"+TabName+"']"), this.GetElement());
        }

        [TxAction("IsFunctionSelected", "ManageAdvanced")]
        public bool IsFunctionSelected(string FunctionName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[@class='sidebarSideItem active'and text()='"+ FunctionName + "']"), this.GetElement());
        }
        [TxAction("EditNumberOfPoint", "")]
        public WEGInput EditNumberOfPoint()
        {
            return new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,'sideBarContainer') and contains(@style,'display: block;')]//input[starts-with(@id,'numberOfPoints')]"), this);
        }
    }
}
