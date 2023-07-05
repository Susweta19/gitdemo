using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Linq;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.Windows;
using TxSelenium.NativeTests.Windows.Utils;
using TxSelenium.Utils;
using System.Threading;
using System;
using TxSelenium.GenericTests.TxFormulation;

namespace TxSelenium.NativeTests.WebElements
{

    public class WESTreeContextMenu
    {
        private string advancedCM;
        private WESTree tree;
        public TxWebDriver Driver { get; private set; }
        private WESBlankAera blankArea;

        public WESTreeContextMenu(TxWebDriver driver, WESTree tree, WESBlankAera blankArea)
        {
            this.tree = tree;
            this.Driver = driver;
            this.advancedCM = Translator.GetTranslation(Driver.Language, Translator.advancedCM);
            this.blankArea = blankArea;
        }

        /// <summary>
        /// Select a object and open the compareobject windows
        /// </summary>
        /// <param name="path"> object select in the entity tree</param>
        /// <returns></returns>
        [TxAction("CompareTheObject", "Compare The Objetct to others.")]
        public WDValidatedWindow<WDCompareObject> CompareTheObject(DTEntityNode nodes)
        {
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.compareCM) });
            Driver.WaitForAjax();
            WDCompareObject compareCreator = new WDCompareObject(Driver, By.ClassName("dhxlayout_cont"));
            return new WDValidatedWindow<WDCompareObject>(Driver, compareCreator);
        }


        [TxAction("ModificationsHistory", "Modification history of the object")]
        public WDWindow<WDModificationsHistory> ModificationsHistory(DTEntityNode nodes)
        {
            Thread.Sleep(2000);
            this.tree.OpenMenu(nodes).SelectEntry(new String[] { "Modifications history…" });

            WDModificationsHistory modifHistory = new WDModificationsHistory(Driver, By.Id("bodyTraceabilities"));
            return new WDWindow<WDModificationsHistory>(Driver, modifHistory);
        }
        [TxAction("ModificationsHistoryFR", "Modification history of the object")]
        public WDWindow<WDModificationsHistory> ModificationsHistoryFR(DTEntityNode nodes)
        {
            Thread.Sleep(2000);
            this.tree.OpenMenu(nodes).SelectEntry(new String[] { "Historique des modifications..." });

            WDModificationsHistory modifHistory = new WDModificationsHistory(Driver, By.Id("bodyTraceabilities"));
            return new WDWindow<WDModificationsHistory>(Driver, modifHistory);
        }
        /// <summary>
        /// Select a object and open the compareobject windows
        /// </summary>
        /// <param name="path"> object select in the entity tree</param>
        /// <returns></returns>
        [TxAction("Renaming", "Renaming The Objetct to others.")]
        public void Renaming(DTEntityNode nodes)
        {
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.renamingCM) });
        }

        public void ScrollObjectPopUpMenuToBottom(DTEntityNode nodes)
        {
            this.tree.OpenMenu(nodes);
            Thread.Sleep(1000);
            WERefreshed elem = new WERefreshed(Driver, By.XPath(".//div[starts-with(@id,'arrowdown_dhxId')]"));
            Driver.MouseOver(elem);
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Select a object and open the compareobject windows
        /// </summary>
        /// <param name="path"> object select in the entity tree</param>
        /// <returns></returns>
        [TxAction("ObjectPopUpMenu", "Select model directly in context menu.")]
        public void ModelApp(DTEntityNode nodes, string modelName, bool needToScrollBottom = false)
        {
            if (needToScrollBottom)
                ScrollObjectPopUpMenuToBottom(nodes);
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { modelName });
        }

        /// <summary>
        /// Collapse or expand, according value, a branch node.
        /// </summary>
        ///<param name="value">value : 0 for expand, 1 for collapse </param>
        /// <param name="collection">node collection who can be expand or collapse</param>
        [TxAction("ExpandOrCollapse", "value is 0 for expand or 1 for collapse.")]
        public void ExpandCollapse(int value, ActionColl<DTEntityNode> collection = null)
        {
            string expandBranch = Translator.GetTranslation(Driver.Language, Translator.expandBranchCM);
            string collapseBranch = Translator.GetTranslation(Driver.Language, Translator.collapseBranchCM);

            string[] option = new string[] { expandBranch, collapseBranch };
            string[] choice = new string[] { advancedCM, option[value] };

            if (collection != null)
            {
                if (collection.Count() > 1)
                {
                    this.tree.SelectEntities(collection);
                    this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(choice);
                }
                else
                {
                    this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(choice);
                }
            }
            else
            {
                blankArea.RightClick();
                new WEGContextMenu(Driver).SelectEntry(choice);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [TxAction("Properties", "Display the object properties.")]
        public void Properties(DTEntityNode nodes)
        {
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.propertiesCM) });
            Driver.ClickAt(WDFramedWindow<WERefreshed>.closeButton);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [TxAction("AddObject", "Add a new object.")]
        public WDValidatedWindow<WForm> AddObject(DTEntityNode nodes = null, string advancedCreation = null, bool useForm = true)
        {
            if (useForm == true)
            {
                if (nodes != null)
                {
                    this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addObjectCM) });
                }
                else
                {
                    blankArea.RightClick();
                    new WEGContextMenu(Driver).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addObjectCM) });
                }

                if (advancedCreation != null)
                    new WESPopUp(Driver).AdvancedOption(advancedCreation, "Yes");

                WForm formCreator = new WForm(Driver, WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(Driver, formCreator);
            }
            else
            {
                this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addObjectCM) });
                return null;
            }
        }
        [TxAction("LaunchFormulation", "Add a new object.")]
            public WDValidatedWindow<IngredientWindow>LaunchFormulation(ActionColl<DTEntityNode> collection = null, string advancedCreation = null, bool useForm = true)
            {
            if (useForm == true)
            {
                if (collection == null)
                {
                    blankArea.RightClick();
                    new WEGContextMenu(Driver).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.LaunchFormulation) });
                }
                else
                {
                    if (collection.Coll.Count == 1)
                    {
                        this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.LaunchFormulation) });
                    }
                    else
                    {
                        this.tree.SelectEntities(collection);
                        this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.LaunchFormulation) });
                    }
                }
            }
                    //end
             IngredientWindow win = new IngredientWindow(Driver, By.XPath(".//div[@class=\"dhxwin_active\"]"));
            return new WDValidatedWindow<IngredientWindow>(Driver, win);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [TxAction("AddChildObject", "add a child object and open a Wform if you select only one entity, if not add child objects without opening wform .")]
        public WDValidatedWindow<WForm> AddChildObject(ActionColl<DTEntityNode> collection = null, string advancedCreation = null, bool useForm = true)
        {
            if (useForm == true)
            {
                if (collection == null)
                {
                    blankArea.RightClick();
                    new WEGContextMenu(Driver).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addChildObjectCM) });
                }
                else
                {
                    if (collection.Coll.Count == 1)
                    {
                        this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addChildObjectCM) });
                    }
                    else
                    {
                        this.tree.SelectEntities(collection);
                        this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addChildObjectCM) });
                    }
                }


                if (advancedCreation != null)
                    new WESPopUp(Driver).AdvancedOption(advancedCreation, "Yes");

                WForm formCreator = new WForm(Driver, WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(Driver, formCreator);

            }
            else
            {
                this.tree.SelectEntities(collection);
                this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addChildObjectCM) });
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [TxAction("AddFolder", "Add a new folder.")]
        public void AddFolder(DTEntityNode nodes = null)
        {
            if (nodes != null)
                this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addFolderCM) });
            else
            {
                blankArea.RightClick();
                new WEGContextMenu(Driver).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addFolderCM) });
            }
        }


        [TxAction("Sleep", "Execution stop according time you give")]
        public void Sleep(int timeOut)
        {
            Console.WriteLine("Sleep during " + timeOut);
            Thread.Sleep(TimeSpan.FromSeconds(timeOut));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        [TxAction("DeleteObject", "delete object(s) selected.")]
        public WESPopUp DeleteObject(ActionColl<DTEntityNode> collection)
        {
            this.tree.SelectEntities(collection);
            this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.deleteObjectCM) });

            return new WESPopUp(Driver);
        }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="path"></param>
        [TxAction("Duplicate", "Duplicate an entity.")]
        public WDValidatedWindow<WDCompareObject> Duplicate(DTEntityNode nodes)
        {
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.duplicateCM) });

            WDCompareObject compareCreator = new WDCompareObject(Driver, By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<WDCompareObject>(Driver, compareCreator);
        }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="path"></param>
        [TxAction("AdvancedDuplicate", "Duplicate an entity with an advanced duplication.")]
        public WDValidatedWindow<WForm> AdvancedDuplicate(DTEntityNode nodes, string advDuplicateName, bool returnForm = false)
        {
            string[] command = new string[] {advancedCM,
                Translator.GetTranslation(Driver.Language, Translator.advDuplicateCM),advDuplicateName};

            this.tree.OpenMenu(nodes).SelectEntry(command);

            if (returnForm)
            {
                WForm wform = new WForm(Driver, WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(Driver, wform);
            }
            else
                return null;
        }


        /// <summary>
        /// 
        /// </summary>
        ///<param name="path"></param>
        [TxAction("InsertObject", "Insert an object below the entity selected.")]
        public WDValidatedWindow<WForm> InsertObject(ActionColl<DTEntityNode> collection)
        {
            if (collection.Count() > 1)
                this.tree.SelectEntities(collection);

            this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { advancedCM, Translator.GetTranslation(Driver.Language, Translator.insertObjectCM) });
            WForm formCreator = new WForm(Driver, WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(Driver, formCreator);
        }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="path"></param>
        [TxAction("ChangeFolderOrObject", "Change object(s) orfolder(s) selected to folder or object .")]
        public WESPopUp ChangeFolderObject(ActionColl<DTEntityNode> collection)
        {
            this.tree.SelectEntities(collection);
            this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { advancedCM, Translator.GetTranslation(Driver.Language, Translator.changeFolderObjectCM) });

            if (Driver.IsElementPresent(WESPopUp.GetPopupButton("true")))
                return new WESPopUp(Driver);
            else
                return null;
        }

        /// <summary>
        /// Sort the entire tree
        /// </summary>
        ///<param name="path"></param>
        ///<param name="sortType">tree type = ascending if treeType == 1, descending otherwise</param>
        [TxAction("SortEntireTree", "Tree type is ascending if treeType == 1, descending otherwise.")]
        public void SortTree(DTEntityNode nodes = null, int sortType = 1)
        {
            string SortEntireTree = Translator.GetTranslation(Driver.Language, Translator.sortTreeCM);
            string[] command;

            if (sortType == 1)
                command = new string[] { advancedCM, SortEntireTree, Translator.GetTranslation(Driver.Language, Translator.ascendingCM) };
            else
                command = new string[] { advancedCM, SortEntireTree, Translator.GetTranslation(Driver.Language, Translator.descendingCM) };

            if (nodes != null)
                this.tree.OpenMenu(nodes).SelectEntry(command);
            else
            {
                blankArea.RightClick();
                new WEGContextMenu(Driver).SelectEntry(command);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        [TxAction("Rename", "Rename the selected entity.")]
        public void Rename(DTEntityNode nodes, string newName)
        {
            string[] command = new string[] { advancedCM, Translator.GetTranslation(Driver.Language, Translator.renameCM) };

            //If the entity is not selected first an error might occur.
            //this.tree.SelectEntity(path);
            Thread.Sleep(3000);
            Console.WriteLine("Sleeping for 3 sec, inside TreeContexMenu--> Rename method");
            this.tree.OpenMenu(nodes).SelectEntry(command);

            WEGInput input = new WEGInput(Driver, new ByChained(By.ClassName("containerTableStyle"), By.XPath(".//input")), tree);
            input.Fill(newName);
            input.Enter();
        }

        /// <summary>
        /// Collapse or expand, according value, a branch node.
        /// </summary>
        /// <param name="collection">node collection which can be deselected  </param>
        [TxAction("AddLinkedObject", "Select the link objet by context menu.")]
        public WDValidatedWindow<WForm> LinkObject(DTEntityNode nodes, string objectLinkedName)
        {
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.addLinkedObjectCM), objectLinkedName });
            WForm formCreator = new WForm(Driver, WForm.WriteFormDiv);
            return new WDValidatedWindow<WForm>(Driver, formCreator);
        }

        /// <summary>
        /// Collapse or expand, according value, a branch node.
        /// </summary>
        ///<param name="value">value : 0 for expand, 1 for collapse </param>
        /// <param name="collection">node collection who can be expand or collapse</param>
        [TxAction("SelectOrDeselect", "value is 0 for select or 1 for deselect.")]
        public void SelectOrDeselect(ActionColl<DTEntityNode> collection, int value)
        {

            string selectBranch = Translator.GetTranslation(Driver.Language, Translator.selectBranchCM);
            string deselecBranch = Translator.GetTranslation(Driver.Language, Translator.deselectBranchCM);

            string[] option = new string[] { selectBranch, deselecBranch };
            string[] choice = new string[] { advancedCM, option[value] };

            this.tree.SelectEntities(collection);
            this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(new string[] { advancedCM, Translator.GetTranslation(Driver.Language, Translator.selectBranchCM) });
        }

        /// <summary>
        /// Collapse or expand, according value, a branch node.
        /// </summary>
        /// <param name="collection">node collection which can be deselected  </param>
        [TxAction("AdvancedCreation", "Select an advanced cretation by context menu.")]
        public WDValidatedWindow<WForm> AdvancedCreation(string creationOption, bool usesWindow, ActionColl<DTEntityNode> collection = null)
        {
            string[] choice = new string[] { Translator.GetTranslation(Driver.Language, Translator.advancedCreationCM), creationOption };

            if (collection == null)
            {
                blankArea.RightClick();
                new WEGContextMenu(Driver).SelectEntry(choice);
            }
            else
            {
                this.tree.SelectEntities(collection);
                this.tree.OpenMenu(collection.ElementAt(0)).SelectEntry(choice);
            }
            if (usesWindow)
            {
                WForm formCreator = new WForm(Driver, WForm.WriteFormDiv);
                return new WDValidatedWindow<WForm>(Driver, formCreator);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Select a object and open the compareobject windows
        /// </summary>
        /// <param name="path"> object select in the entity tree</param>
        /// <returns></returns>
        [TxAction("AdvancedComparison", "Compare The Objetct to others.")]
        public WDValidatedWindow<WDCompareObject> AdvancedComparison(DTEntityNode nodes, string advancedComparison)
        {
            this.tree.OpenMenu(nodes).SelectEntry(new string[] { Translator.GetTranslation(Driver.Language, Translator.advancedComparisonCM), advancedComparison });

            WDCompareObject compareCreator = new WDCompareObject(Driver, By.ClassName("dhtmlx_wins_body_inner"));
            return new WDValidatedWindow<WDCompareObject>(Driver, compareCreator);
        }

        [TxAction("IsCompareObjectDisabled", "Compare The Objetct to others.")]
        public bool IsCompareObjectDisabled()
        {

            WERefreshed ele = new WERefreshed(Driver, By.XPath(".//div[@class='containerTableStyle']"));
            Driver.RightClick(ele.GetElement(), 250, 200);
            return Driver.IsElementPresent(By.XPath(".//tr[contains(@id,'_optCompare')]"));
        }


        [TxAction("IsPropertiesObjectDisabled", "Compare The Objetct to others.")]
        public bool IsPropertiesObjectDisabled()
        {

            WERefreshed ele = new WERefreshed(Driver, By.XPath(".//div[@class='containerTableStyle']"));
            Driver.RightClick(ele.GetElement(), 250, 90);
            return Driver.IsElementPresent(By.XPath(".//tr[contains(@id,'_optProperty')]"));
        }
        [TxAction("ReadSelectedObject", "ReadSelectedObject.")]
        public string ReadSelectedObject(string attrId)
        {
            string qq = Driver.WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivTreediv" + attrId + "_\")]//span[@class=\"selectedTreeRow\"]")).GetElement().Text;
            int i = 0;

            return Driver.WaitForElement(By.XPath(".//div[starts-with(@id,\"idDivTreediv" + attrId + "_\")]//span[@class=\"selectedTreeRow\"]")).GetElement().Text;
        }
        [TxAction("IsOptionsDisable", "Any Options is Disable")]
        public bool IsOptionsDisable(string title,ActionColl<DTEntityNode> collection = null)
        {
            if (collection == null)
            {
                blankArea.RightClick();                
            }
            else
            { 
                this.tree.SelectEntities(collection);
                this.tree.OpenMenu(collection.ElementAt(0));
            }
            return Driver.IsElementPresent(By.XPath(".//tr[@class=\"sub_item_dis\" and @title=\"" + title + "\"]"));
        }
    }




        /// <summary>
        /// small class to manage the blank aera elem inside a tree
        /// </summary>
        public class WESBlankAera : WERefreshed
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public WESBlankAera(TxWebDriver driver, By elemId, WERefreshed parent, int x, int y)
                : base(driver, elemId, parent)
            {
                X = x;
                Y = y;
            }

            public void RightClick()
            {
                GetDriver().RightClick(this.GetElement(), X, Y);
            }


        }
    }


