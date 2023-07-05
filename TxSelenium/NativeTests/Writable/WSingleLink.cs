using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.Utils;

namespace TxSelenium.NativeTests.Writable
{
    /// <summary>
    /// This class interacts with a single link in write mode.
    /// </summary>
    public class WSingleLink : WESTree
    {
        /*========================== Public EntitySelector to set a WSingleLink Tree ================================*/

        public static readonly By entitySelectorTableViewBy = By.XPath(".//td[1]/img");
        public static readonly By entitySelectorWLinkBy = By.XPath("./td[2]/div");
        public static readonly By entitySelectorPreselctBy = By.XPath(".//input");
        public static readonly By entitySelectorTreeGDC = By.XPath("./span/input");

        /*=========================================================================================================*/

        private  By EntitySelectorBy;

        public WSingleLink(TxWebDriver driver, By elemIdentifier, By entityExpand, By entitySelectorBy,  WERefreshed parent = null, int treeType = 1)
            : base(driver, elemIdentifier, entityExpand,  parent, treeType)
        {
            this.EntitySelectorBy = entitySelectorBy;
        }

        /// <summary>
        /// Toggles the selection of an entiy with the specified path.
        /// </summary>
        /// <param name="path">the path to the specified entity</param>
        [TxAction("SelectEntityBox", "Toggles the selection box of one entity identified by the path leading to it.")]
        public WEGCheckBox SelectEntityBox(DTEntityNode nodes)
        {
            WERefreshed elem = GetEntity(nodes.GetPath());            
            return new WEGCheckBox(GetDriver(), EntitySelectorBy, elem);
        }

        [TxAction("SelectEntityBoxBis", "Toggles the selection box of one entity identified by the path leading to it.")]
        public WEGCheckBox SelectEntityBoxBis(string node)
        {
            GetDriver().WaitForAjax();
            WERefreshed elem = GetEntity(new List<string>() { node });
            return new WEGCheckBox(GetDriver(), EntitySelectorBy, elem);
        }

        /// <summary>
        /// Opens the context menu.
        /// </summary>
        /// <param name="path">The node path selected to open the menu</param>
        [TxAction("TreeContextMenu", "Open the context menu.")]
		public WESTreeContextMenu ContextMenu()
		{  
			return new WESTreeContextMenu(GetDriver(), this, new WESBlankAera(GetDriver(), By.XPath(".//input[@class=\"dhxtoolbar_input\"]"), this, 100, -50));
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
        [TxAction("IsFullScreenPresent", "")]
        public bool IsFullScreenPresent()
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[not(contains(@style,'display: none'))]/img[contains(@src,'Fullscreen.png')]"), this.GetElement());
        }
        /// <summary>
        /// Uses the select all functionnality.
        /// </summary>
        [TxAction("DeselectAll", "Deselect All all checked entities.")]
        public void DeselectAll()
        {
            GetDriver().ClickAt(ElemByPictureName.UnCheckAll, this);
        }

        /// <summary>
        /// Uses the uncheck all functionnality.
        /// </summary>
        [TxAction("UncheckAll", "Unchecks all checked entities.")]
        public void UncheckAll()
        {
            GetDriver().ClickAt(ElemByPictureName.UnCheckAll, this);
        }
        /// <summary>
        /// Uses the uncheck all child object.
        /// </summary>
        [TxAction("UncheckChildObject", "Unchecks all checked child entities.")]
        public void UncheckChildObject()
        {
            GetDriver().ClickAt(ElemByPictureName.UnCheckChildObject, this);
        }

        [TxAction("ReadToolbarMessage", "")]
        public string ReadToolbarMessage()
        {
            WaitForAjax();
            return GetDriver().WaitForElement(By.XPath(".//div[starts-with(@id,\"seriesToolbarMessage\")]"), this).GetElement().Text;
        }

        /// <summary>
        /// Checks whether or not the tree is in filtered mode.
        /// </summary>
        /// <returns>true if the tree is in filtered mode false otherwise</returns>
        public bool isFiltered()
        {
            return this.GetElement().GetCssValue("display") == "none";            
        }

        ///// <summary>
        ///// Sets the tree into a specified filter mode.
        ///// If the tree is already in the specified mode then nothing happens.
        ///// </summary>
        ///// <param name="value">true if the tree should be passed to filtered mode false otherwise</param>
        public void SetFiltered(bool value)
        {
            if (isFiltered() != value)
           {
                GetDriver().ClickAt(ElemByPictureName.SelectedObjects, this);
            }
        }

        /// <summary>
        /// Gets the a node whose children are the data representation of this tree content.
        /// This method does not open entities with children so only the children of already
        /// opened entites will be parsed.
        /// </summary>
        /// <returns>An entity node with no name (it is the "root" node)</returns>
        public DTEntityNode GetDisplayedEntities()
        {
            WERefreshed tableBody = GetDriver().WaitForElement( By.TagName("tbody"), this);
            LoadAllNodes();
            DTEntityNode rootNode = ExtractNodes(tableBody, null);
            return rootNode;
        }

        /// <summary>
        /// This method recursively parse a node and, if it is opened, its children.
        /// </summary>
        /// <param name="tableBody">the element corresponding to the node's table body</param>
        /// <param name="parentNode">the parent node</param>
        /// <returns>the node</returns>
        private DTEntityNode ExtractNodes(WERefreshed tableBody, DTEntityNode parentNode)
        {
            ICollection<IWebElement> children = tableBody.GetElement().FindElements(By.XPath("./tr"));
            //The first row is the current node.
            IWebElement label = children.ElementAt(0).FindElement(By.XPath(".//span[@class=\"standartTreeRow\"]|.//span[@class=\"selectedTreeRow\"]"));
            DTEntityNode node = new DTEntityNode(label.Text, parentNode);
            for (int i = 1; i < children.Count; i++)
            {
                ExtractNodes(new WERefreshed(tableBody.GetDriver(), WESTree.GetNodeBy(i), tableBody), node);
            }
            return node;
        }
        [TxAction("IsButtonDisabled", "")]
        public bool IsButtonDisabled(string btnName)
        {
            return GetDriver().IsElementPresent(By.XPath(".//div[contains(@id,\"idDivBtnBar\")]//input[@value=\"" + btnName + "\" and @disabled=\"disabled\"]"));
        }
        [TxAction("Minimize", "")]//need to delete
        public void Minimize()
        {
            GetDriver().ClickAt(ElemByPictureName.Minimize);
        }

        [TxAction("CheckBox", "Get control over the checkbox")]
        public WEGCheckBox CheckBox(int Row, int Column)
        {
            return new WEGCheckBox(GetDriver(), By.XPath(".//div[contains(@id,\"tvDivGrid\")]//div[@class=\"objbox\"]//tr[" + (Row + 1) + "]/td[" + Column + "]//img"), this);
        }

    }
}
