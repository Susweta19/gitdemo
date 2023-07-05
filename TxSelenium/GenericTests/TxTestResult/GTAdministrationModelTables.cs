using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using TxSelenium.Utils;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxTestResult
{
    public class GTAdministrationModelTables : WERefreshed
    {

        public GTAdministrationModelTables(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("DeleteAllColumn", "Delete all column")]
        public void DeleteAllColumn()
        {
            WERefreshed tableHeader = GetDriver().WaitForElement(By.XPath("(.//div[starts-with(@id,\"divTableAdminGrid\")]//div[@class=\"xhdr\"])[2]"), this);
            WERefreshed table = GetDriver().WaitForElement(By.XPath("(.//div[starts-with(@id,\"divTableAdminGrid\")]//div[@class=\"objbox\"])[2]"), this);
            int count = table.GetElement().FindElements(By.XPath(".//tbody/tr[2]/td[not(contains(@style,\"display: none\"))]")).Count;

            for (int i = 0; i < count; i++)
            {
                WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//tbody/tr[2]/td[not(contains(@style,\"display: none\"))][1]/div"), tableHeader);
                GetDriver().RightClick(elem);
                string text = Translator.GetTranslation(GetDriver().Language, Translator.Deletecolumntxresult);
                GetDriver().ClickAt(By.XPath(".//tr[starts-with(@class,\"sub_item\")]//div[text()=\"" + text + "\"]"));
                new WESPopUp(GetDriver()).ClosePopup(true);
            }
        }

        [TxAction("ConditionTable", "Condition Table")]
        public void ConditionTable()
        {
            GetDriver().ClickAt(By.XPath(".//li[@idatt=\"96\"]"), this);

        }

        [TxAction("ResultTable", "Result Table")]
        public void ResultTable()
        {
            GetDriver().ClickAt(By.XPath(".//li[@idatt=\"93\"]"), this);
        }

        [TxAction("AddColumn", "Add a new Column")]
        public WDValidatedWindow<GTColumnModification> AddColumn()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminToolbar\")]//img[contains(@src,\"48x48-AddColumn.png\")]"), this);
            GTColumnModification columnModification = new GTColumnModification(GetDriver(), By.XPath(".//div[contains(@id,\"divAddColumnAdminGrid\")]"), null);
            return new WDValidatedWindow<GTColumnModification>(GetDriver(), columnModification);
        }
       
        /// <summary>
        /// For first column columIndex=1
        /// </summary>
        /// <param name="columIndex"></param>
        [TxAction("ModifyColumn", "Modify Column")]
        public WDValidatedWindow<GTColumnModification> ModifyColumn(int columIndex)
        {
            By elm = By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/div[1]/table/tbody/tr[2]/td[" + columIndex + "]/div");
            WERefreshed table = GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"divTableAdminGrid\")]//div[@class=\"objbox\"]"), this);
            GetDriver().ScrollHorizontal(table.GetElement(), elm, null, true);
            WERefreshed elem = new WERefreshed(GetDriver(), elm, this);
            GetDriver().RightClick(elem);
            GetDriver().ClickAt(By.XPath(".//tr[starts-with(@class,\"sub_item\")]//div[text()=\"Edit column\"]"));
            GTColumnModification columnModification = new GTColumnModification(GetDriver(), By.XPath(".//div[contains(@id,\"divAddColumnAdminGrid\")]"), null);
            return new WDValidatedWindow<GTColumnModification>(GetDriver(), columnModification);
        }
        [TxAction("InsertColumnContextMenu", "Insert Column Context Menu")]
        public WDValidatedWindow<GTColumnModification> InsertColumnContextMenu(int columIndex)
        {
            WERefreshed tb = GetDriver().WaitForElement(By.XPath(".//div[@class='xhdr']/table[contains(@id,'headerGridId')]/.."), this);
            GetDriver().ScrollHorizontal(tb.GetElement(), By.XPath(".//tbody/tr[2]/td[15]/div/div"), this, true);
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[@class='xhdr']/table[contains(@id,'headerGridId')]/..//tbody/tr[2]/td["+columIndex+"]/div/div"), this);
            GetDriver().RightClick(elem);

            GetDriver().ClickAt(By.XPath(".//tr[starts-with(@class,\"sub_item\")]//div[text()=\"Insert a column\"]"));
            GTColumnModification columnModification = new GTColumnModification(GetDriver(), By.XPath(".//div[contains(@id,\"divAddColumnAdminGrid\")]"), null);
            return new WDValidatedWindow<GTColumnModification>(GetDriver(), columnModification);
        }
        [TxAction("DeletecolumnContextMenu","Delete Column Context Menu")]
        public void DeletecolumnContextMenu(Boolean result, int columIndex)
        {
            
            WERefreshed tb = GetDriver().WaitForElement(By.XPath(".//div[@class='xhdr']/table[contains(@id,'headerGridId')]/.."), this);
            GetDriver().ScrollHorizontal(tb.GetElement(), By.XPath(".//tbody/tr[2]/td[15]/div/div"), this, true);
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//div[@class='xhdr']/table[contains(@id,'headerGridId')]/..//tbody/tr[2]/td["+columIndex+"]/div/div"), this);
            GetDriver().RightClick(elem);
            string text = Translator.GetTranslation(GetDriver().Language, Translator.Deletecolumntxresult);
            GetDriver().ClickAt(By.XPath(".//tr[starts-with(@class,\"sub_item\")]//div[text()=\""+ text + "\"]"));
            //new WESPopUp(GetDriver()).ClosePopup(result);
        }
        /// <summary>
        /// if delete=false then it will create new label as given label values
        /// if delete=true then it will delte one by one label as given label values
        /// </summary>
        /// <param name="label"></param>
        /// <param name="delete"></param>
        [TxAction("LabelOfDefaultLines", "Write  Label")]
        public void LabelOfDefaultLines(ICollection<String> label, Boolean delete = false)
        {
            if (delete)
            {
                foreach (String l in label)
                    GetDriver().ClickAt(By.XPath(".//ul[@class=\"tagit ui-widget ui-widget-content ui-corner-all\"]//li//span[text()=\"" + l + "\"]/../a"), this);
            }
            else
            {
                WEGInput input = new WEGInput(GetDriver(), By.XPath(".//input[@class=\"ui-widget-content ui-autocomplete-input\"]"), this);
                foreach (String l in label)
                {
                    input.FillWithoutClear(l);
                    input.Enter();
                }
            }
        }
        [TxAction("Record", "Click on Record Button")]
        public void Record()
        {
            GetDriver().ClickAt(By.XPath(".//input[contains(@id,\"idTrBtnSave\")]"), this);
        }
        /// <summary>
        /// This function will help you to modify rule for Text, LinkedData, CalculatedFormula and Link
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("EditRuleForTextLinkDataCalculatedFormulaLinkBoolean", "Edit Rule For Text Link Data Calculated formula and Link")]
        public WDValidatedWindow<GTTxEditRuleForTextLinkDataFormulaLinkBoolean> EditRule(int rowIndex,int columnIndex)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            GTTxEditRuleForTextLinkDataFormulaLinkBoolean PathtxtWindow = new GTTxEditRuleForTextLinkDataFormulaLinkBoolean(GetDriver(), By.XPath(".//div[contains(@id,\"divEditRulesAdminGrid\")]"), this);
            return new WDValidatedWindow<GTTxEditRuleForTextLinkDataFormulaLinkBoolean>(GetDriver(), PathtxtWindow);
        }
        [TxAction("EditRuleForDate", "Edit Rule For Date")]
        public WDValidatedWindow<GTTxEditRuleForDate> EditRuleForDate(int rowIndex,int columnIndex)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            GTTxEditRuleForDate pathDatewindow = new GTTxEditRuleForDate(GetDriver(), By.XPath(".//div[contains(@id,\"divEditRulesAdminGrid\")]"));
            return new WDValidatedWindow<GTTxEditRuleForDate>(GetDriver(), pathDatewindow);
        }

        [TxAction("EditRuleForDigital", "Edit Rule For Text")]
        public WDValidatedWindow<GTTxEditRuleForDigital> EditRuleForDigital(int rowIndex, int columnIndex)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            GTTxEditRuleForDigital PathDigitalWindow = new GTTxEditRuleForDigital(GetDriver(), By.XPath(".//div[contains(@id,\"divEditRulesAdminGrid\")]"), this);
            return new WDValidatedWindow<GTTxEditRuleForDigital>(GetDriver(), PathDigitalWindow);
        }
        /// <summary>
        /// For first row, rowIndex=1
        /// For first column columnIndex=1
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        [TxAction("EditRuleForListChoices", "Edit Rule For Text")]
        public WDValidatedWindow<GTTxEditRuleListChoicesWindow> EditRuleForListChoices(int rowIndex, int columnIndex)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/div[2]/table/tbody/tr[" + (rowIndex + 1) + "]/td[" + columnIndex + "]"), this);
            GTTxEditRuleListChoicesWindow PathDigitalWindow = new GTTxEditRuleListChoicesWindow(GetDriver(), By.XPath(".//div[contains(@id,\"divEditRulesAdminGrid\")]"), this);
            return new WDValidatedWindow<GTTxEditRuleListChoicesWindow>(GetDriver(), PathDigitalWindow);
        }
        /// <summary>
        /// For first row, rowIndex=1
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        [TxAction("EditSpecification", "Edit EditSpecification value")]
        public WEGInput EditSpecification(int rowIndex)
        {
            GetDriver().DoubleClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[1]//table[@class=\"obj row20px\"]//tr["+(rowIndex+1)+"]/td"), this);
            return new WEGInput(GetDriver(), By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[1]//table[@class=\"obj row20px\"]//tr[" + (rowIndex + 1) + "]/td/input"), this);
        }

        [TxAction("AddSpecification", "Add a new Specification")]
        public void AddSpecification()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminToolbar\")]//img[contains(@src,\"48x48-AddRow.png\")]"), this);
        }
        /// <summary>
        /// rowIndex=1 for first row
        /// </summary>
        /// <param name="rowIndex"></param>
        [TxAction("DeleteSpecification", "Delete a Specification")]
        public void DeleteSpecification(int rowIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@id,\"divTableAdminGrid\")]/div[1]//tr["+rowIndex+"]"), this);
            GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"DeleteRow.png\")]"), this);

        }
    }
}


/* [TxAction("AddSpecification", "Add a new Specification")]
        public void AddSpecification()
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminToolbar\")]//img[contains(@src,\"48x48_AddRow.png\")]"), this);
        }*/
/// <summary>
/// rowIndex=1 for first row
/// </summary>
/// <param name="rowIndex"></param>
/*[TxAction("DeleteSpecification", "Delete a Specification")]
public void DeleteSpecification(int rowIndex)
{
    GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/table/tbody/tr["+(rowIndex+1) +"]/td[1]"), this);
    GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminToolbar\")]//img[contains(@src,\"48x48_DeleteRow.png\")]"), this);

}*/
/*
  /// <summary>
        /// For first column columIndex=1
        /// </summary>
        /// <param name="columIndex"></param>
        [TxAction("DeleteColumn", "Add a new Column")]
        public void DeleteColumn(int columIndex)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminGrid\")]/div[2]/table/tbody/tr[2]/td["+ columIndex + "]"), this);
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\"divTableAdminToolbar\")]//img[contains(@src,\"48x48_DeleteColumn.png\")]"), this);
        }
*/
