using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TxSelenium.NativeTests.DataTypes;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using OpenQA.Selenium.Support.PageObjects;
using TxSelenium.Utils;
using System.Threading;

namespace TxSelenium.NativeTests.WebElements
{
    /// <summary>
    /// class managing all the tree strucure in teexma...
    /// </summary>
    public class WESTree : WERefreshed
    {
        /*=============================================Filtered Tree==========================================================*/

        private readonly By fitlerTree1 = By.XPath(".//*[@id = \"table_mcs_view_filtered\" and contains(@style, \"display: block\")]");
        private readonly By fitlerTree2 = By.XPath(".//*[@id = \"tree2_1_0\" and contains(@style, \"display: block\")]");
        private readonly By fitlerTree3 = By.XPath(".//*[@id = \"table_linear_view\" and contains(@style, \"display: block\")]");

        /*======================  getClickableByTree and getEntityByTree to set all the tree types ===========================*/

        private static string entityPathTree1 = ".//following-sibling::*//*[text() = \"{0}\"]/ancestor::tr[ position() = 1]";
        public static Func<string, By> getEntityByTree1 = entityName => By.XPath(string.Format(entityPathTree1, entityName));

        private static string entityPathTree2 = ".//following-sibling::*//*[text() = \"{0}\"]/ancestor::div[ position() = 1 and not(contains(@class, \"containerTableStyle\" ))]";
        public static Func<string, By> getEntityByTree2 = entityName => By.XPath(string.Format(entityPathTree2, entityName));

        private static string clickablePathTree = ".//*[text() = \"{0}\"]";
        private static Func<string, By> getClickableByTree = entityName => By.XPath(string.Format(clickablePathTree, entityName));

        /*================================= Public expandBy according the tree type ==========================================*/

        public static readonly By expandByClassicTree = By.XPath("./td[1]/div");
        public static readonly By expandByTableViewTree = By.XPath(".//div/img[1]");
        public static readonly By expandByDefCriteriaTree = By.XPath(".//a");
        public static readonly By expandByModifHistTree = By.XPath("./td/div/img");

        /*===================================================================================================*/

        public static By GetNodeBy(int i)
        {
            return By.XPath("./tr[" + (i + 1) + "]/td[@colspan=\"3\"]/table/tbody");
        }

        public static readonly By scrollableContainer = By.XPath(".//div[contains(@class,\"containerTableStyle\")] | .//div[contains(@class,\"reeview\")] | .//div[@class = \"objbox\"] ");

        /*===================================================================================================*/

        protected By entityExpand;
        protected Func<string, By> getClickableBy;
        protected Func<string, By> getEntityBy;

        /// <summary>
        /// Constructor for WETree
        /// </summary>
        /// <param name="driver">The driver selected</param>
        /// <param name="elemIdentifier">ElementIdentifier which allows to localise WEEntitry object</param>
        /// <param name="entityExpand"> The By expand to deploy a father entity, this by changes according the tree type </param>
        /// <param name="parent">The parent object</param>
        public WESTree(TxWebDriver driver, By elemIdentifier, By entityExpand, WERefreshed parent = null, int treeType = 1)
               : base(driver, elemIdentifier, parent)
        {
            if (treeType == 1)
                this.getEntityBy = getEntityByTree1;
            else if (treeType == 2)
            {
                this.getEntityBy = getEntityByTree2;
            }

            this.entityExpand = entityExpand;
            this.getClickableBy = getClickableByTree;
        }

        //TODO this only works if the parent nodes of the entity are all closed
        /// <summary>
        /// Gets the element corresponding to an entity using the passed collection as a path.
        /// The first entry being the path begining.
        /// Every parent on the path will be opened and should be closed
        /// for this to work.
        /// </summary>
        /// <param name="path">A collection, which should be ordered, containing the path.</param>
        /// <returns>the element corresponding to the entity</returns>
        public WERefreshed GetEntity(ICollection<string> path)
        {
            string entityName;
            WERefreshed entity = null;
            WERefreshed parent = FilteredTreeOrNot();

            for (int i = 0; i < path.Count; i++)
            {
                entityName = path.ElementAt(i);

                if (GetDriver().IsElementPresent(getEntityBy(entityName), parent.GetElement()))
                    entity = GetDriver().WaitForElement(getEntityBy(entityName), parent);
                else
                {
                    LoadNode(getEntityBy(entityName), parent);
                    entity = GetDriver().WaitForElement(getEntityBy(entityName), parent);
                }

                if (i < (path.Count - 1))
                {
                    string childPath = path.ElementAt(i + 1);
                    if (!GetDriver().IsElementPresent(getEntityBy(childPath), entity.GetElement()))
                    {
                        LoadNode(getEntityBy(childPath), entity);
                        if (!GetDriver().IsElementPresent(getEntityBy(childPath), entity.GetElement()))
                            GetDriver().ClickAt(entityExpand, entity);
                    }
                    parent = entity;
                }
            }
            return entity;
        }

        /// <summary>
        /// Selects an entity using the passed collection as a path.
        /// The first entry being the path begining.
        /// Every parent on the path will be opened and should be closed 
        /// for this to work.
        /// </summary>
        /// <param name="path"> A collection , which should be ordered, containing the path.</param>
        [TxAction("SelectEntity", "Selects an entity.")]
        public void SelectEntity(DTEntityNode nodes)
        {
            WERefreshed entity = GetEntity(nodes.GetPath());
            By identifier = getClickableBy(nodes.GetPath().Last());

            GetDriver().ClickAt(getClickableBy(nodes.GetPath().Last()), entity);
        }

        [TxAction("SelectEntity2", "Selects an entity.")]
        public void SelectEntity2(string node)
        {
            WERefreshed entity = GetEntity(new List<string>() { node });
            GetDriver().ClickAt(entity);
        }

        /// <summary>
        /// Selects an entity using the passed collection as a path.
        /// The first entry being the path begining.
        /// Every parent on the path will be opened and should be closed 
        /// for this to work.
        /// </summary>
        /// <param name="path"> A collection , which should be ordered, containing the path.</param>
        [TxAction("SelectObjectByPosition", "Selects an entity.")]
        public void SelectObjectByPosition(string position)
        {
            int tmpPos = Convert.ToInt32(position) + 1;
            GetDriver().ClickAt(By.XPath(".//div[@id =\"idDivNavigationTree\"]/div/table/tbody/tr[" + tmpPos + "]//span"), this);
        }

        /// <summary>
        /// Selects an entity using the passed collection as a path.
        /// The first entry being the path begining.
        /// Every parent on the path will be opened and should be closed 
        /// for this to work.
        /// </summary>
        /// <param name="path"> A collection , which should be ordered, containing the path.</param>
        [TxAction("NumberOfObjects", "Selects an entity.")]
        public int NumberOfObjects(int gap = 0)
        {
            Thread.Sleep(2000);
            return this.GetElement().FindElements(By.XPath(".//div[@id =\"idDivNavigationTree\"]/div/table/tbody/tr")).Count - 1 + gap;
        }

        /// <summary>
        /// Change Icollection<Path> to nodes in parameters
        /// </summary>
        /// <param name="path"> A collection , which should be ordered, containing the path.</param>
        [TxAction("SelectEntityBis", "Selects an entity.")]
        public void SelectEntityBis(DTEntityNode nodes)
        {
            WERefreshed entity = GetEntity(nodes.GetPath());
            By identifier = getClickableBy(nodes.GetPath().Last());

            GetDriver().ClickAt(getClickableBy(nodes.GetPath().Last()), entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="presence"></param>
        [TxAction("EntitytPresence", "Selects an entity.")]
        public void EntitytPresence(DTEntityNode nodes, bool presence)
        
        {
            try
            {
                WERefreshed entity = GetEntity(nodes.GetPath());
                if (entity != null && presence == false)
                    throw new Exception("Object should not be present");
            }
            catch (WebDriverTimeoutException)
            {
                if (presence == true)
                    throw new Exception("Object should be present");
            }
        }
        [TxAction("ReadHighlightedEntity", "To remove filters")]
        public string ReadHighlightedEntity()
        {
            WERefreshed objectSpan = GetDriver().WaitForElement(By.XPath(".//span[@class=\"selectedTreeRow\"]"), this);
            return objectSpan.GetElement().Text;
        }

        [TxAction("RenameCurrentObject", "Rename the selected entity.")]
        public void RenameCurrentObject(string newName)
        {
            GetDriver().ClickAt(By.ClassName("selectedTreeRowFull"), this);
            GetDriver().PressKey(Keys.F2);
            WEGInput input = new WEGInput(GetDriver(), new ByChained(By.ClassName("containerTableStyle"), By.XPath(".//input")), this);
            input.Fill(newName);
            input.Enter();
        }

        [TxAction("DoubleClickEntity", "Selects an entity.")]
        public void DoubleClickEntity(DTEntityNode nodes)
        {
            WERefreshed entity = GetEntity(nodes.GetPath());
            GetDriver().DoubleClickAt(getClickableBy(nodes.GetPath().Last()), entity);
        }

        /// <summary>
        /// Views the checked Objects.
        /// </summary>
        [TxAction("ReadEntityRow", "Views the checked Objects.")]
        public void ReadEntityRow(DTEntityNode nodes, ActionColl<string> ExpectedValue, ActionColl<string> NotExpectedValue = null)
        {
            WERefreshed entity = GetEntity(nodes.GetPath());
            GetDriver().ScrollToElement(entity.GetElement());
            string value = entity.GetElement().Text;

            foreach (string res in ExpectedValue)
                if (!value.Contains(res))
                    throw new Exception("Expected Value Not found");

            if (NotExpectedValue != null)
                foreach (string res in NotExpectedValue)
                    if (value.Contains(res))
                        throw new Exception("Expected Value Not found");
        }
        /// <summary>
        /// Selects an entity using the passed collection as a path.
        /// The first entry being the path begining.
        /// Every parent on the path will be opened and should be closed 
        /// for this to work.
        /// </summary>
        /// <param name="path"> A collection , which should be ordered, containing the path.</param>
        [TxAction("SelectEntities", "Selects multiple entities.")]
        public void SelectEntities(ActionColl<DTEntityNode> collection)
        {
            SelectEntity(collection.ElementAt(0));
            GetDriver().KeyDown(Keys.Control);
            for (int i = 1; i < collection.Count(); i++)
            {
                SelectEntity(collection.ElementAt(i));
            }
            GetDriver().KeyUp(Keys.Control);
        }

        /// <summary>
        /// Selects an entity using the passed collection as a path.
        /// The first entry being the path begining.
        /// Every parent on the path will be opened and should be closed 
        /// for this to work.
        /// </summary>
        /// <param name="path"> A collection , which should be ordered, containing the path.</param>
        [TxAction("SelectEntitiesBis", "Selects multiple entities.")]
        public void SelectEntitiesBis(ActionColl<DTEntityNode> collection)
        {
            SelectEntity(collection.ElementAt(0));
            GetDriver().KeyDown(Keys.Control);
            for (int i = 1; i < collection.Count(); i++)
            {
                SelectEntity(collection.ElementAt(i));
            }
            GetDriver().KeyUp(Keys.Control);
        }

        /// <summary>
        /// Views the checked Objects.
        /// </summary>
        [TxAction("ViewCheckedObjects", "Views the checked Objects.")]
        public void CheckedObjects()
        {
            GetDriver().ClickAt(ElemByPictureName.SelectedObjects, this);
        }

        /// <summary>
        /// Views the checked Objects.
        /// </summary>
        [TxAction("PressHereToSee", "Views the checked Objects.")]
        public void PressHereToSee()
        {
            GetDriver().ClickAt(ElemByPictureName.EntireListObjects, this);
        }

        /// <summary>
        /// Views the checked Objects.
        /// </summary>
        [TxAction("DisplayTheEntireTree", "Click here to see the entire list of Objects.")]
        public void DisplayTheEntireTree()
        {

            GetDriver().ClickAt(ElemByPictureName.EntireListObjects, this);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="checkResult"> should be expanded or collapsed</param>
        [TxAction("ExpandCollapseEntity", "Click on plus or minus button or check if expanded or collapsed")]
        public void ExtendContractEntity(DTEntityNode nodes, string checkResult = null)
        {
            //TODO remove this sleep with optimal Code
            //SomeTimes after selecting OT entity come slower than usual
            Thread.Sleep(2000);

            WERefreshed entity = GetEntity(nodes.GetPath());
            if (checkResult == null)
                GetDriver().ClickAt(entityExpand, entity);
            else
            {
                WERefreshed standartTreeImage = GetDriver().WaitForElement(entityExpand, entity);
                string treeImage = standartTreeImage.GetElement().GetAttribute("src");

                string value;
                if (checkResult == "expanded")
                    value = "minus";
                else
                    value = "plus";

                if (!treeImage.Contains(value))
                    throw new Exception("Expand or collapse problem.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="research"></param>
        [TxAction("Search", "Tree search.")]
             public void Search(string research, bool popup = true)
        {
            WEGInput input;
            try
            {
                input = new WEGInput(GetDriver(), By.XPath(".//div[contains(@id,\"Toolbar\") and starts-with(@class,\"dhx_toolbar_material\") and not(@id=\"idDivMainToolbar\")]//input[@class=\"dhxtoolbar_input\"] | .//input[starts-with(@id,\"inp_search_\")]"), this);
                input.Fill(research);
                input.Enter();
            }
            catch (Exception)
            {
                try
                {
                    input = new WEGInput(GetDriver(), By.XPath(".//div[starts-with(@id,\"IdFilterToolbar\") and starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"]|.//div[starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"]"), this);
                    input.Fill(research);
                    input.Enter();
                    Console.WriteLine("Exception handled within Search method...");
                }
                catch (Exception)
                {
                    IWebElement elem=this.GetElement().FindElement(By.XPath(".//div[starts-with(@id,\"IdFilterToolbar\") and starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"]|.//div[starts-with(@class,\"dhx_toolbar_material\")]//input[@class=\"dhxtoolbar_input\"]"));
                    elem.Clear();
                    elem.SendKeys(research);
                    elem.SendKeys(Keys.Enter);
                }
            }

            

            if (popup)
                if (research.Length == 1)
                    new WESPopUp(GetDriver()).ClosePopup(true);
        }
    

        //    WEGInput input = new WEGInput(GetDriver(), By.XPath(".//div[contains(@id,\"Toolbar\") and starts-with(@class,\"dhx_toolbar_material\") and not(@id=\"idDivMainToolbar\")]//input[@class=\"dhxtoolbar_input\"] | .//input[starts-with(@id,\"inp_search_\")]"), this);

        //    input.Fill(research);
        //    input.Enter();

        //    if (research.Length == 1)
        //        new WESPopUp(GetDriver()).ClosePopup(true);
        //}

        /// <summary>
        /// Use when tree is divided by two filtered and not filtered three table view
        /// ,launch calculation(multicriteria) and preselect tree...
        /// Otherwise return this.
        /// </summary>
        /// <returns></returns>
        private WERefreshed FilteredTreeOrNot()
        {
            if (GetDriver().IsElementPresent(fitlerTree1, this.GetElement()))
                return new WERefreshed(GetDriver(), fitlerTree1, this);
            else if (GetDriver().IsElementPresent(fitlerTree2, this.GetElement()))
                return new WERefreshed(GetDriver(), fitlerTree2, this);
            else if (GetDriver().IsElementPresent(fitlerTree3, this.GetElement()))
                return new WERefreshed(GetDriver(), fitlerTree3, this);
            else
                return this;
        }

        /// <summary>
        /// Opens the contextual menu from an entity.
        /// </summary>
        /// <param name="path">the path to the entity</param>
        [TxAction("OpenMenu", "Opens the contextual menu from an entity.")]
        public virtual WEGContextMenu OpenMenu(DTEntityNode nodes)
        {
            //Opening the path to the node           
            WERefreshed entity = GetEntity(nodes.GetPath());
            //The entity element is too large for a click so the span is taken
            string nodeText = nodes.GetPath().Last();
            WERefreshed entityText = GetDriver().WaitForElement(By.XPath(".//*[text() = \"" + nodeText + "\"]"), entity);
            GetDriver().RightClick(entityText);
            GetDriver().WaitForAjax();
            return new WEGContextMenu(GetDriver());
        }

        /// <summary>
        /// Scrolls this tree to the bottom forcing every nodes to be loaded.
        /// </summary>
        protected void LoadAllNodes()
        {
            WERefreshed table = GetDriver().WaitForElement(scrollableContainer, this);
            GetDriver().ScrollToBottom(table.GetElement());
        }

        /// <summary>
        /// Scrolls this tree to the bottom forcing every nodes to be loaded.
        /// </summary>
        protected void LoadNode(By by, WERefreshed parent = null)
        {
            WERefreshed table = GetDriver().WaitForElement(scrollableContainer, this);
            GetDriver().ScrollToBottom(table.GetElement(), by, parent);
        }

        /// <summary>
        /// Views the checked Objects.
        /// </summary>
		[TxAction("ClickHereTo", "Click here to see the entire list of Objects.")]
        public void ClickHereTo()
        {
            GetDriver().ClickAt(ElemByPictureName.EntireListObjects, this);
        }

        //[TxAction("ImportPreselection", "Clicks on the button Import a preselection")]
        //public void ImportPreselection()
        //{
        //    WaitForAjax();
        //    GetDriver().ClickAt(ElemByPictureName.Preselection, this);
        //}

        [TxAction("ImportPreselection", "Import document file")]
        public void ImportPreselection(string pathFile)
        {
            if (LinuxUtils.IsLinux)
            {
                pathFile = pathFile.Replace("\\", "/");
            }
            Console.WriteLine("File path : " + pathFile);
            //GetDriver().ClickAt(By.XPath(".//img[contains(@src,\"20x20-Tasks.png\")]"));
            WERefreshed uploadElem = new WERefreshed(GetDriver(), By.XPath(".//img[contains(@src,\"20x20-Tasks.png\")]"),this);
            uploadElem.GetElement().SendKeys(pathFile);
            //GetDriver().UploadFile(uploadElem, pathFile);
        }
        [TxAction("CancelSearch", "")]
        public void CancelSearch()
        {
            GetDriver().ClickAt(By.XPath(".//div[starts-with(@class,\"dhx_toolbar_material\")]//img[@class=\"clImgToolbarRemoveInputValue\" and not(contains(@style,\"display: none\"))]"), this);
        }

    }
}
