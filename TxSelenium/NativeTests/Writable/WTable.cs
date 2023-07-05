using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows.Utils;
using TxSelenium.NativeTests.Windows;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Writable
{
    /// <summary>
    /// This class interacts with a table in write mode.
    /// </summary>
    public class WTable : WERefreshed
    {
        private static By GetTableColumn(int index)
        {
            //int indexReal = index + 2;
            return By.XPath(".//tr[@class=\" ev_material\"]//td[" + index + "]");
        }

        private static By GetTableSeries(int id)
        {
            int idReal = id + 1;
            return By.XPath(".//div[@class=\"objbox\"]//tr[" + idReal + "]/td[1]");
        }

        private static By GetTableCell(int series, int column)
        {
            int seriesReal = series + 1;
            int columnReal = column + 1;
            return By.XPath(".//div[@class=\"objbox\"]//tr[" + seriesReal + "]/td[" + columnReal + "]");
        }

        public WTable(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("DeleteData", "Delete the array.")]
        public WESPopUp DeleteData()
        {
            GetDriver().ClickAt(ElemByPictureName.DeleteData, this);
            return new WESPopUp(GetDriver());
        }


        /// <summary>
        /// Adds a column with the contextMenu
        /// </summary>
        /// <param name="index">
        /// The index of the column to be added.
        /// Beware this index should be superior or equal to 1.</param>
        [TxAction("AddColumn", "Adds a column.")]
        public void AddColumn(int index)
        {
            WERefreshed addColumn = GetDriver().WaitForElement(GetTableColumn(index), this);
            GetDriver().RightClick(addColumn);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.addColumnPathCM) });
        }
        

        /// <summary>
        /// Deletes a column. If the index is less than 2 then an exception is thrown.
        /// </summary>
        /// <param name="index">
        /// The index of the column to be deleted.
        /// Beware this index should be superior or equal to 1.</param>
        [TxAction("DeleteColumn", "Deletes the specified column.")]
        public WESPopUp DeleteColumn(int index)
        {
            WERefreshed deleteColumn = GetDriver().WaitForElement(GetTableColumn(index), this);
            GetDriver().RightClick(deleteColumn);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.deleteColumnPathCM) });

            return new WESPopUp(GetDriver());
        }

        /// <summary>
        /// Erase a column.
        /// </summary>
        /// <param name="index">
        /// The index of the column to be erased.
        /// Beware this index should be superior or equal to 1.</param>
        [TxAction("EraseColumn", "Erase the specified column.")]
        public WESPopUp EraseColumn(int index)
        {
            WERefreshed eraseColumn = GetDriver().WaitForElement(GetTableColumn(index), this);
            GetDriver().RightClick(eraseColumn);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.eraseColumnPathCM) });

            return new WESPopUp(GetDriver());
        }

        /// <summary>
        /// Insert a column.
        /// </summary>
        /// <param name="index">
        /// A column is inserted just before the column selected with the index.
        /// Beware this index should be superior or equal to 1.</param>
        [TxAction("InsertColumn", "Insert a column just before the selected column.")]
        public void InsertColumn(int index)
        {
            WERefreshed insertColumn = GetDriver().WaitForElement(GetTableColumn(index), this);
            GetDriver().RightClick(insertColumn);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.insertColumnPathCM) });
        }


        /// <summary>
        /// Add a series in the table.
        /// </summary>
        /// <param name="idSerie">the series type id</param>
        [TxAction("AddSeries", "Add a series in the table.")]
        public void AddSeries(int idSerie)
        {
            WERefreshed addSeries = GetDriver().WaitForElement(GetTableSeries(idSerie), this);
            GetDriver().RightClick(addSeries);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.addSeriesPathCM) });
        }

        /// <summary>
        /// Insert a series in the table.
        /// </summary>
        /// <param name="idSerie">the serie type id</param>
        [TxAction("InsertSeries", "Insert a series in the table just before the selected series")]
        public void InsertSeries(int idSerie)
        {
            WERefreshed insertSeries = GetDriver().WaitForElement(GetTableSeries(idSerie), this);
            GetDriver().RightClick(insertSeries);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.insertSeriesPathCM) });
        }

        /// <summary>
        /// Duplicate a series in the table.
        /// </summary>
        /// <param name="idSerie">the serie type id</param>
        [TxAction("DuplicateSeries", "Duplicate the selected series in the table")]
        public void DuplicateSeries(int idSerie)
        {
            WERefreshed duplicateSeries = GetDriver().WaitForElement(GetTableSeries(idSerie), this);
            GetDriver().RightClick(duplicateSeries);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.duplicateSeriesPathCM) });
        }

        /// <summary>
        /// Erase a series in the table.
        /// </summary>
        /// <param name="idSerie">the serie type id</param>
        [TxAction("EraseSeries", "Erase the selected series in the table")]
        public WESPopUp EraseSeries(int idSerie)
        {
            WERefreshed eraseSeries = GetDriver().WaitForElement(GetTableSeries(idSerie), this);
            GetDriver().RightClick(eraseSeries);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.eraseSeriesPathCM) });

            return new WESPopUp(GetDriver());
        }



        /// <summary>
        /// Delete a series in the table.
        /// </summary>
        /// <param name="idSerie">the serie type id</param>
        [TxAction("DeleteSeries", "Delete the selected series in the table")]
        public WESPopUp DeleteSeries(int idSerie)
        {
            WERefreshed deleteSeries = GetDriver().WaitForElement( GetTableSeries(idSerie), this);
            GetDriver().RightClick(deleteSeries);

            WEGContextMenu menu = new WEGContextMenu(GetDriver());
            menu.SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.deleteSeriesPathCM) });

            return new WESPopUp(GetDriver());
        }

        /// <summary>
        /// Sets the value of a specific cell.
        /// </summary>
        /// <param name="seriesIndex">the cell's series index</param>
        /// <param name="colIndex">the cell's column index</param>
        /// <param name="value">the value to be entered</param>
        [TxAction("SetValue", "Sets the value for a specific cell.")]
        public void SetCellValue(int seriesIndex, int colIndex, string value)
        {
            WERefreshed setCell = GetDriver().WaitForElement(GetTableCell(seriesIndex, colIndex), this);
            setCell.GetElement().Click();
            GetDriver().ClickAt(GetTableCell(seriesIndex, colIndex), this);
            WEGInput input = new WEGInput(GetDriver(), By.XPath(".//input|.//textarea"), setCell);
            input.Fill(value);
            input.Enter();
        }

        [TxAction("ExportData", "Data Will be export")]
        public void ExportData(int attrId)
        {
            GetDriver().ClickAt(By.XPath(".//div[contains(@id,\""+attrId+ "\")]//div[@title=\"Export Data in Excel or tsv format\"]//img|.//div[contains(@id,\"" + attrId + "\")]//div[@title=\"Export the Data in Excel format\"]//img"));
            GetDriver().WaitForAjax();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The node path selected to compare to objects</param>
        /// <param name="collection">Collection of objects compared</param>
        [TxAction("FullScreen", "Full screen the table of the attributes.")]
        public void FullScreen()
        {
            GetDriver().ClickAt(ElemByPictureName.FullScreen, this);
        }

        [TxAction("ReadcellValue", "click on import data button return a import data window")]
        public string ReadcellValue(int row, int col)
        {
            GetDriver().WaitForAjax();
            row = row + 1;
            return GetDriver().FindElement(this.GetElement(), By.XPath(".//div[@class='objbox']//tr[" + row + "]//td[" + col + "]")).Text;
        }

        [TxAction("ImportDataTable", "click on import data button return a import data window")]
        public WDValidatedWindow<WDImportDataTable> ImportDataTable()
        {
            GetDriver().ClickAt(ElemByPictureName.ImportButton, this);
            WDImportDataTable importDataTable = new WDImportDataTable(GetDriver(), By.ClassName("dhtmlx_wins_body_inner"));

            return new WDValidatedWindow<WDImportDataTable>(GetDriver(), importDataTable);
        }

        [TxAction("DropdownList", "click on import data button return a import data window")]
        public WEGDHtmlxComboBox DropdownList(int row, int col)
        {
            WERefreshed cell = GetDriver().WaitForElement(By.XPath(".//div[contains(@class,\"objbox\")]//tr[" + (row + 1) + "]/td[" + (col + 1) + "]"), this);
            GetDriver().ClickAt(cell);
            GetDriver().ClickAt(cell);
            return new WEGDHtmlxComboBox(GetDriver(), cell.ElemIdentifier, cell.Parent);
        }
    }
}
