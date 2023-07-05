using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Linq;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.Utils;
using TxSelenium.GenericTests.TxFormulation;
using System;
using System.Threading;

namespace TxSelenium.NativeTests.WebElements
{
    public class WESNavigationTree : WERefreshed
    {
        private static readonly By entityCombo = By.Id("idDivNavigationCombo");
        private static readonly By entityTree = By.Id("idDivNavigationTree");
        public static readonly By comboImg = By.XPath(".//div[@class = \"dhxcombo_select_img\"] |.//img[@id = \"cmb_img\"]|.//img[@class=\"clDivFictiveTxComboImg\"]");
        
        private WESTree tree;

        public WESNavigationTree(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
            this.tree = new WESTree(driver, entityTree, WESTree.expandByClassicTree, this);
        }

        /*======================================= Function for the write/read mode ===========================================*/


        /// <summary>
        /// Select a type of entity from the entity type selector.
        /// </summary>
        /// <param name="ot">the id of the entity type</param>
        [TxAction("SelectOT", "Selects the specified entity type")]
        public WEGDHtmlxComboBox SelectOT()
        {
            return new WEGDHtmlxComboBox(GetDriver(), By.ClassName("dhxcombo_material"), this);
        }
        [TxAction("IsOTPresent", "")]
        public bool IsOTPresent(string value,bool byclick=true)
        {
            GetDriver().WaitForAjax();
            //GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"24x24-wMainOptions.png\")]"));
            IWebElement ele = GetDriver().FindElement(By.XPath(".//div[contains(@class , \"dhx_combo_list\") and not(contains(@style ,\"display: none\"))] | " +
            ".//div[contains(@class , \"dhxcombolist_dhx_skyblue\") and not(contains(@style ,\"display: none\"))] | " +
            ".//div[contains(@class , \"dhxcombolist_material\") and not(contains(@style ,\"display: none\"))] | .//div[@id=\"cmb_liste\"]"));
            //  GetDriver().ClickAt(comboImg, this);
            int i = 0;
            if (byclick)
            {
                GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"24x24-wMainOptions.png\")]"));
            }
                WaitForAjax();
            if (value.Contains('\''))
             return   GetDriver().IsElementPresent(By.XPath(".//div[text() =\"" + value + "\"]"),ele);
            else
             return   GetDriver().IsElementPresent(By.XPath(".//div[text() = '" + value + "' ]"),ele);

        }



        /// <summary>
        /// Gets the main entity tree.
        /// </summary>
        /// <returns>the main entity tree</returns>
        [TxAction("EntityTree", "Gets the handle for the main entity tree.")]
        public WESTree GetEntityTree()
        {
            return new WESTree(GetDriver(), ElemIdentifier, WESTree.expandByClassicTree, this);
        }

        /// <summary>
        /// 
        /// </summary>
        [TxAction("SwitchReadWrite", "Switch into write mode.")]
        public void Switch()
        {
            WERefreshed TreeDiv = GetDriver().WaitForElement(By.Id("idDivNavigationTree"), this);
            if (TreeDiv.GetElement().GetCssValue("display") != "none")
                GetDriver().ClickAt(ElemByPictureName.SwitchReadOrWrite, this);
            else
                GetDriver().ClickAt(new ByChained(By.Id("idDivNavigationBusinessToolbar"), ElemByPictureName.SwitchReadOrWrite), this);
        }

        /*=================================== Function for the write mode by context menu ====================================*/

        /// <summary>
		/// Opens the context menu.
        /// </summary>

        [TxAction("TreeContextMenu", "Open the context menu.")]
        public WESTreeContextMenu WContextMenu()
        {
            return new WESTreeContextMenu(GetDriver(), this.tree, new WESBlankAera(GetDriver(), By.Id("idDivNavigationTree"), this, 175, 200));
        }

        [TxAction("ScrollToBottom", "Gets the handle for the main entity tree.")]
        public void ScrollToBottom()
        {
            GetDriver().ScrollToBottom(GetDriver().WaitForElement(By.ClassName("containerTableStyle"), this).GetElement());
        }



        /*======================================= Function for the write mode by click =======================================*/

        /// <summary>
        /// Add an object with the button
        /// </summary>
        /// GetDriver().ClickAt(Translator.GetButtonByTitle_Img(GetDriver(), Translator.addObjectButton), this);
        [TxAction("AddObject", "Add a new object.")]
        public WDValidatedWindow<WForm> AddObject(DTEntityNode nodes = null, string advancedCreation = null, bool useForm = true)
        {
            if (useForm == true)
            {
                if (nodes != null)
                {
                    //maybe useless
                    if (nodes.GetPath().Count != 0)
                        this.tree.SelectEntity(nodes);
                    GetDriver().ClickAt(ElemByPictureName.AddObjectButton, this);
                }
                else
                    GetDriver().ClickAt(ElemByPictureName.AddObjectButton, this);

                if (advancedCreation != null)
                    new WESPopUp(GetDriver()).AdvancedOption(advancedCreation, "Yes");

                WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
            }
            else
            {
                GetDriver().ClickAt(ElemByPictureName.AddObjectButton, this);
                return null;
            }
        }

        [TxAction("LaunchFormulation", "Add a new object.")]
        public WDValidatedWindow<IngredientWindow> LaunchFormulation(DTEntityNode nodes = null)
        {

            //maybe useless
            Thread.Sleep(2000);
            this.tree.OpenMenu(nodes).SelectEntry(new String[] { "Launch Formulation - Tree mode" });
            //if (nodes.GetPath().Count != 0)
            //            this.tree.SelectEntity(nodes);
            //        GetDriver().ClickAt(By.XPath(".//div[text()=\"Launch Formulation - Tree mode\"]"), this);


            IngredientWindow win = new IngredientWindow(GetDriver(), By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<IngredientWindow>(GetDriver(), win);
        }
           
        
        /// <summary>
        /// Add an object with the button and not return  a window
        /// </summary>
        [TxAction("AddSimpleObject", "Add a new object without window.")]
        public void AddSimpleObject(string advancedCreation = null)
        {
            GetDriver().ClickAt(ElemByPictureName.AddObjectButton, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [TxAction("AddChildObject", "add a child object and open a Wform if you select only one entity.")]
        public WDValidatedWindow<WForm> AddChildObject(ActionColl<DTEntityNode> collection = null, string advancedCreation = null, bool useForm = true)
        {
            if (useForm == true)
            {
                if (collection == null)
                {
                    GetDriver().ClickAt(ElemByPictureName.AddChildObjectButton, this);
                }
                else
                {
                    this.tree.SelectEntities(collection);
                    GetDriver().ClickAt(ElemByPictureName.AddChildObjectButton, this);
                }

                if (advancedCreation != null)
                    new WESPopUp(GetDriver()).AdvancedOption(advancedCreation, "Yes");

                WForm formCreator = new WForm(GetDriver(), WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(GetDriver(), formCreator);
            }
            else
            {
                this.tree.SelectEntities(collection);
                GetDriver().ClickAt(ElemByPictureName.AddChildObjectButton, this);
                return null;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        [TxAction("DeleteObject", "delete object(s) selected.")]
        public WESPopUp DeleteObject(ActionColl<DTEntityNode> collection = null)
        {
            if (collection != null)
                this.tree.SelectEntities(collection);
            GetDriver().ClickAt(ElemByPictureName.DeleteObjectButton, this);

            return new WESPopUp(GetDriver());
        }

        /// <returns></returns>
        [TxAction("ClickOnBlankArea", "Option by context menu.")]
        public void ClickOnBlankArea()
        {
            WERefreshed blankAera = new WERefreshed(GetDriver(), WEGDHtmlxComboBox.comboImg, this);
            GetDriver().ClickAt(blankAera, 0, 150);
        }
        //ClickAt(blankAeraElem, 0, 200)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        [TxAction("RenameF2", "Rename the selected entity.")]
        public void RenameF2(DTEntityNode nodes, string newName)
        {
            if (nodes != null)
                this.tree.SelectEntity(nodes);

            GetDriver().PressKey(Keys.F2);
            WEGInput input = new WEGInput(GetDriver(), new ByChained(By.ClassName("containerTableStyle"), By.XPath(".//input")), tree);
            input.Fill(newName);
            input.Enter();
        }

        [TxAction("RenameInput", "Rename the selected entity.")]
        public void RenameInput(string newName)
        {
            WEGInput input = new WEGInput(GetDriver(), new ByChained(By.ClassName("containerTableStyle"), By.XPath(".//input")), tree);
            input.Fill(newName);
            input.Enter();
        }

        /// <summary>
        /// Move Object(s) according the index if is positive 
        /// we move up and if negative we move down
        /// </summary>
        ///<param name="collection"></param>
        ///<param name="index"></param>
        [TxAction("MoveObject", "Move up or down object(s) selected according index.")]
        public void MoveObject(ActionColl<DTEntityNode> collection, int index)
        {
            this.tree.SelectEntities(collection);

            if (index < 0)
            {
                for (int i = 0; i < -index; i++)
                    GetDriver().ClickAt(ElemByPictureName.DownButton, this);
            }
            else
            {
                for (int i = 0; i < index; i++)
                    GetDriver().ClickAt(ElemByPictureName.UpButton, this);
            }
        }

        /// <summary>
        /// Performs a drag and drop between two entity
        /// </summary>
        /// <param name="dragPath">the path to the dragged entity</param>
        /// <param name="dropPath">the path to the entity where the drop is performed</param>

        [TxAction("DragAndDrop", "Drags an entity to another one. If drop under is true the entity will be dropped under the targeted entity not on it")]
        public void DragAndDrop(DTEntityNode dragNodes, DTEntityNode dropNodes, bool asChild = false, bool placeOver = false)
        {
            WERefreshed dragElem = GetEntitySpan(dragNodes);
            GetDriver().ClickAndHold(dragElem.GetElement());

            WERefreshed dropElem = GetEntitySpan(dropNodes);
            GetDriver().MouseOver(dropElem);
            if (asChild)
                GetDriver().Release();
            else
            {
                if (placeOver)
                {
                    GetDriver().MouseMove(-2, -7);
                    GetDriver().Release();
                }
                else
                {
                    GetDriver().MouseMove(-2, 7);
                    GetDriver().Release();
                }
            }
        }

        /// <summary>
        /// Gets the span of an entity row. This is used for click based interaction
        /// since the row is too large and Selenium tries to click outside of the tree.
        /// </summary>
        private WERefreshed GetEntitySpan(DTEntityNode nodes)
        {
            string nodeText = nodes.GetPath().Last();
            WERefreshed entity = this.tree.GetEntity(nodes.GetPath());
            WERefreshed entitySpan = GetDriver().WaitForElement(By.XPath(".//span[text() = \"" + nodeText + "\"]"), entity);
            return entitySpan;
        }

        [TxAction("AddLinkedObject", "add object(s) selected.")]
        public WESPopUp AddLinkedObject(DTEntityNode nodes)
        {
            this.tree.SelectEntity(nodes);
            GetDriver().ClickAt(ElemByPictureName.AddLinkObjectButton, this);
            return new WESPopUp(GetDriver());
        }

        [TxAction("DeleteLinkedObject", "delete object(s) selected.")]
        public WESPopUp DeleteLinkedObject(DTEntityNode nodes)
        {
            this.tree.SelectEntity(nodes);
            GetDriver().ClickAt(ElemByPictureName.DeleteLinkObjectButton, this);

            return new WESPopUp(GetDriver());
        }
        [TxAction("IsAddLinkObjectsDisabled", "Add Linked Objects disabled")]
        public bool IsAddLinkObjectsDisabled()
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//img[contains(@src,'20x20-AddLink.png')]/.."), this);
            string className = elem.GetElement().GetAttribute("class");
            if (className.Contains("dhxtoolbar_btn_dis"))
                return false;
            else
                return true;
            //return GetDriver().IsElementPresent(By.XPath(".//img[contains(@src,'20x20-AddLinkDisabled.png')]"), this.GetElement());
        }
        [TxAction("IsButtonDisabled", "Check any Button in toolbar disabled or not")]
        public bool IsButtonDisabled(string imgName)
        {
            WERefreshed elem = GetDriver().WaitForElement(By.XPath(".//img[contains(@src,\"" + imgName + "\")]/.."), this);           
            return elem.GetElement().GetAttribute("class").Contains("dhxtoolbar_btn_dis");            
        }

        [TxAction("SearchBox", "Use the searchbox")]
        public WEGInput SearchBox()
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[starts-with(@class,\"dhxtoolbar_input\")]"), this);
        }
        
    }
}
