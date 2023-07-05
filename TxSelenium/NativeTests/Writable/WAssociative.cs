using OpenQA.Selenium;
using System.Linq;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Writable
{
    /// <summary>
    /// Represents an associative class in a form in write mode.
    /// </summary>
    public class WAssociative : WERefreshed
    {
        private WMultipleLink tree; 

        public WAssociative(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            this.tree = new WMultipleLink(GetDriver(), elemIdentifier, WESTree.expandByTableViewTree, WSingleLink.entitySelectorTableViewBy, parent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addData"></param>
        /// <param name="modifyLink"></param>
        /// <returns></returns>
        [TxAction("Add", "Adds an element to the associtative class.")]
        public WDValidatedWindow<WDAddAssoLink> Add()
        {
            GetDriver().ClickAt(ElemByPictureName.AddButton, this);

            WDAddAssoLink addAssoLink =  new WDAddAssoLink(GetDriver (), By.ClassName("dhx_cell_wins"));
            return new WDValidatedWindow<WDAddAssoLink>(GetDriver(), addAssoLink);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="WithContextMenu"></param>
        /// <returns></returns>
        [TxAction("Edit", "Edits the line of specified path using contextMenu ou button, true =ContextMenu , false = Button")]
        public WDValidatedWindow<WForm> Edit(DTEntityNode nodes, bool WithContextMenu = true)
        {
            if (!WithContextMenu)
            {
                tree.SelectEntity(nodes);
                GetDriver().ClickAt(ElemByPictureName.EditButton, this);
            }
            else
                tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.modifyPathCM) });

            WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv, null);
            return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TxAction("TreeLink", "Use the treeLink")]
        public WMultipleLink treeLink()
        {
            return tree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [TxAction("Delete", "Delets an element from the associtative class.")]
        public void Delete(DTEntityNode nodes)
        {          
            tree.OpenMenu(nodes).SelectEntry( new string[] { Translator.GetTranslation(GetDriver().Language, Translator.deleteLinePathCM) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        [TxAction("SelectObjectsCM", "Select objects by context menu")]
        public void Select(ActionColl<DTEntityNode> collection)
        {
            tree.SelectEntities(collection);
            tree.OpenMenu(collection.Last()).SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.selectAllobjectsCM) });
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="collection"></param>
        [TxAction("UncheckObjectsCM", "Uncheck objects by context menu")]
        public void Uncheck(ActionColl<DTEntityNode> collection)
        {
            tree.SelectEntities(collection);
            tree.OpenMenu(collection.Last()).SelectEntry(new string[] { Translator.GetTranslation(GetDriver().Language, Translator.unCheckAllObjectsCM) });
        }

        /// <summary>
        /// Click on an header to sort the table view.
        /// </summary>
        /// <param name="headerName">the name to the specified header</param>
        [TxAction("SelectHeader", "Click on the header selected to sort the table view.")]
        public void Headers(string headerName)
        {
            WERefreshed table = GetDriver().WaitForElement( WESTree.scrollableContainer, this);

            GetDriver().ScrollHorizontal(table.GetElement(), WDTableView.GetHeaderBy(headerName), this);

            GetDriver().ClickAt(WDTableView.GetHeaderBy(headerName), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The node path selected to compare to objects</param>
        /// <param name="collection">Collection of objects compared</param>
        [TxAction("FullScreen", "Option in context menu.")]
        public void FullScreen()
        {
            GetDriver().ClickAt(ElemByPictureName.FullScreen, this);
        }

    }
}
