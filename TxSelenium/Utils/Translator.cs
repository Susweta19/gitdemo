using OpenQA.Selenium;
using System.Collections.Generic;

namespace TxSelenium.Utils
{
    public class Translator
    {
        public static readonly string EnLang = "en";
        public static readonly string FrLang = "fr";

        /*****************************************  BUTTON  *********************************************/

        //MainWindowButton        
        public static readonly string listOfAvailableActionsButton = "listOfAvailableActionsButton";
        public static readonly string listOfAvailableModificationsButton = "listOfAvailableModificationsButton";

        //TabResultsButton
        public static readonly string extractValueButton = "extractValueButton";

        //Validate and Cancel buttons for ValidatedWindow
        public static readonly string validateButton = "validateButtonValue";
        public static readonly string cancelButton = "cancelButtonValue";

        //Close button for ExtractionWindow
        public static readonly string closeButton = "closeButtonValue";

        /**************************************  CONTEXT MENU  ******************************************/

        //WENavigationTree
        public static readonly string compareCM = "Compare Objects";
        public static readonly string advancedCM = "Advanced";
        public static readonly string expandBranchCM = "Expand the selected branch";
        public static readonly string collapseBranchCM = "Collapse the selected branch";
        public static readonly string propertiesCM = "Properties";
        public static readonly string renamingCM = "Renaming";
        public static readonly string addObjectCM = "Add Object";
        public static readonly string duplicateCM = "Duplicate";
        public static readonly string advDuplicateCM = "advancedDuplicate";
        public static readonly string insertObjectCM = "Insert Object";
        public static readonly string addFolderCM = "Add a Folder";
        public static readonly string changeFolderObjectCM = "Change into Folder/Object";
        public static readonly string sortTreeCM = "Sort the entire tree";
        public static readonly string ascendingCM = "Ascending";
        public static readonly string descendingCM = "Descending";
        public static readonly string addChildObjectCM = "Add a child Object";
        public static readonly string LaunchFormulation = "Launch Formulation - Tree mode";
        public static readonly string deleteObjectCM = "Delete the Object";
        public static readonly string renameCM = "Rename";
        public static readonly string addLinkedObjectCM = "Add a linked object";
        public static readonly string deselectBranchCM = "Deselect the selected branch";
        public static readonly string selectBranchCM = "Select the selected branch";
        public static readonly string advancedCreationCM = "Advanced creation(s)";
        public static readonly string advancedComparisonCM = "Advanced Comparison(s)";
        public static readonly string Deletecolumntxresult = "Delete the column";


        //WAssociative
        public static readonly string deleteLinePathCM = "Delete the selection";
        public static readonly string modifyPathCM = "Modifier une donnée associative";
        public static readonly string selectAllobjectsCM = "Select all objects";
        public static readonly string unCheckAllObjectsCM = "Uncheck all objects";

        //WTable
        public static readonly string addColumnPathCM = "Add a column";
        public static readonly string insertColumnPathCM = "Insert a column";
        public static readonly string eraseColumnPathCM = "Erase the column";
        public static readonly string deleteColumnPathCM = "Delete the column";
        public static readonly string addSeriesPathCM = "Add a Series";
        public static readonly string insertSeriesPathCM = "Insert a Series";
        public static readonly string duplicateSeriesPathCM = "Dupliquer une Série";
        public static readonly string eraseSeriesPathCM = "Erase the Series";
        public static readonly string deleteSeriesPathCM = "Delete the Series";


        private static IDictionary<string, IDictionary<string, string>> translations;

        public static string GetTranslation(string language, string itemName)
        {
            return translations[language][itemName];
        }

        static Translator()
        {
            translations = new Dictionary<string, IDictionary<string, string>>();

            /*==========================================================================================*/
            /*=============================   FRENCH TRANSLATIONS  =====================================*/
            /*==========================================================================================*/

            translations[FrLang] = new Dictionary<string, string>();

            /***************************************  BUTTON  *******************************************/

            //MainWindows
            translations[FrLang][listOfAvailableActionsButton] = "Liste des actions disponibles*";
            translations[FrLang][listOfAvailableModificationsButton] = "Liste des modifications possibles *";

            //TabResultsButton, value not title!!!
            translations[FrLang][extractValueButton] = "Extraire ...";


            //Validate Cancel Button
            translations[FrLang][validateButton] = "Valider";
            translations[FrLang][cancelButton] = "Annuler";

            //Validate Close Button for ExtractionWindow
            translations[FrLang][closeButton] = "Fermer";
           
            /**************************************  CONTEXT MENU  **************************************/

            //WENavigationTree
            translations[FrLang][compareCM] = "Comparer cette Entité aux autres...";
            translations[FrLang][advancedCM] = "Avancé";
            translations[FrLang][expandBranchCM] = "Développer toute l'arborescence focalisée";
            translations[FrLang][collapseBranchCM] = "Réduire toute l'arborescence focalisée";
            translations[FrLang][propertiesCM] = "Propriétés...";
            translations[FrLang][renamingCM] = "Renommage";
            translations[FrLang][addObjectCM] = "Ajouter une Entité";
            translations[FrLang][duplicateCM] = "Dupliquer";
            translations[FrLang][advDuplicateCM] = "Duplication(s) avancée(s)";
            translations[FrLang][insertObjectCM] = "Insérer une Entité";
            translations[FrLang][addFolderCM] = "Ajouter un Dossier";
            translations[FrLang][changeFolderObjectCM] = "Changer en Dossier/Entité";
            translations[FrLang][sortTreeCM] = "Trier toute l'arborescence";
            translations[FrLang][ascendingCM] = "Croissant";
            translations[FrLang][descendingCM] = "Décroissant";
            translations[FrLang][addChildObjectCM] = "Ajouter une Entité enfant";
            translations[FrLang][LaunchFormulation] = "Lancer la formulation - Mode arborescence";
            translations[FrLang][deleteObjectCM] = "Supprimer l'Entité";
            translations[FrLang][renameCM] = "Renommer";
            translations[FrLang][addLinkedObjectCM] = "Ajouter une Entité liée";
            translations[FrLang][deselectBranchCM] = "Décocher toute l'arborescence focalisée";
            translations[FrLang][selectBranchCM] = "Cocher toute l'arborescence focalisée";
            translations[FrLang][advancedCreationCM] = "Création(s) avancée(s)";
            translations[FrLang][advancedComparisonCM] = "Comparaison(s) avancée(s)";
            translations[FrLang][Deletecolumntxresult] = "Supprimer la colonne";

            //WAssociative
            translations[FrLang][deleteLinePathCM] = "Supprimer la sélection";
            translations[FrLang][modifyPathCM] = "Modifier une donnée associative";
            translations[FrLang][selectAllobjectsCM] = "Cocher toutes les Entités";
            translations[FrLang][unCheckAllObjectsCM] = "Décocher toutes les Entités";

            //WTable
            translations[FrLang][addColumnPathCM] = "Ajouter une colonne";
            translations[FrLang][insertColumnPathCM] = "Insérer une colonne";
            translations[FrLang][eraseColumnPathCM] = "Effacer la column";
            translations[FrLang][deleteColumnPathCM] = "Suppirmer la colonne";
            translations[FrLang][addSeriesPathCM] = "Ajouter une Série";
            translations[FrLang][insertSeriesPathCM] = "Insérer une Série";
            translations[FrLang][duplicateSeriesPathCM] = "Dupliquer une Série";
            translations[FrLang][eraseSeriesPathCM] = "Effacer une Série";
            translations[FrLang][deleteSeriesPathCM] = "Supprimer la Série";

            /*==========================================================================================*/
            /*============================   ENGLISH TRANSLATION  ======================================*/
            /*==========================================================================================*/


            translations[EnLang] = new Dictionary<string, string>();

            /***************************************  BUTTON  *******************************************/

            //MainWindowButton           
            translations[EnLang][listOfAvailableActionsButton] = "Liste des actions disponibles*";
            translations[EnLang][listOfAvailableModificationsButton] = "Liste des modifications possibles *";

            //TabResultsButton,by value not title!!!
            translations[EnLang][extractValueButton] = "Extract ...";

            //Validate Cancel Button
            translations[EnLang][validateButton] = "Ok";
            translations[EnLang][cancelButton] = "Cancel";

            //Validate Close Button for Extraction Window
            translations[EnLang][closeButton] = "Close";

            /*************************************  CONTEXT MENU  ***************************************/

            //WENavigationTree
            translations[EnLang][compareCM] = "Compare Objects";
            translations[EnLang][advancedCM] = "Advanced";
            translations[EnLang][expandBranchCM] = "Expand the selected branch";
            translations[EnLang][collapseBranchCM] = "Collapse the selected branch";
            translations[EnLang][propertiesCM] = "Properties…";
            translations[EnLang][renamingCM] = "Renaming";
            translations[EnLang][addObjectCM] = "Add Object";
            translations[EnLang][duplicateCM] = "Duplicate";
            translations[EnLang][advDuplicateCM] = "Advanced duplication(s)";
            translations[EnLang][insertObjectCM] = "Insert Object";
            translations[EnLang][addFolderCM] = "Add a Folder";
            translations[EnLang][changeFolderObjectCM] = "Change into Folder/Object";
            translations[EnLang][sortTreeCM] = "Sort the entire tree";
            translations[EnLang][ascendingCM] = "Ascending";
            translations[EnLang][descendingCM] = "Descending";
            translations[EnLang][addChildObjectCM] = "Add a child Object";
            translations[EnLang][LaunchFormulation] = "Launch Formulation - Tree mode";
            translations[EnLang][deleteObjectCM] = "Delete the Object";
            translations[EnLang][renameCM] = "Rename";
            translations[EnLang][addLinkedObjectCM] = "Add a linked object";
            translations[EnLang][deselectBranchCM] = "Deselect the selected branch";
            translations[EnLang][selectBranchCM] = "Select the selected branch";
            translations[EnLang][advancedCreationCM] = "Advanced creation(s)";
            translations[EnLang][advancedComparisonCM] = "Advanced Comparison(s)";
            translations[EnLang][Deletecolumntxresult] = "Delete the column";


            //WAssociative
            translations[EnLang][deleteLinePathCM] = "Delete the selection";
            translations[EnLang][modifyPathCM] = "Edit associative data";
            translations[EnLang][selectAllobjectsCM] = "Select all Objects";
            translations[EnLang][unCheckAllObjectsCM] = "Uncheck all Objects";

            //WTable
            translations[EnLang][addColumnPathCM] = "Add a column";
            translations[EnLang][insertColumnPathCM] = "Insert a column";
            translations[EnLang][eraseColumnPathCM] = "Erase the column";
            translations[EnLang][deleteColumnPathCM] = "Delete the column";
            translations[EnLang][addSeriesPathCM] = "Add a Series";
            translations[EnLang][insertSeriesPathCM] = "Insert a Series";
            translations[EnLang][duplicateSeriesPathCM] = "Duplicate a Series";
            translations[EnLang][eraseSeriesPathCM] = "Erase the Series";
            translations[EnLang][deleteSeriesPathCM] = "Delete the Series";
        }

        /// <summary>
        /// Function To create a By with the title translated
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="button"></param>
        /// <returns>By Title</returns>
        public static By GetButtonByTitle(TxWebDriver driver, string button)
        {
            string title = GetTranslation(driver.Language, button);
            return By.XPath(".//*[@title=\"" + title + "\"]");
        }

        /// <summary>
        /// Function To create a By with the title translated
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static By GetButtonByTitle_Img(TxWebDriver driver, string button)
        {
            string title = GetTranslation(driver.Language, button);
            return By.XPath(".//*[@title=\"" + title + "\"]/img");
        }

        /// <summary>
        /// Function To create a By with the title translated
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static By GetButtonByStartsTitle(TxWebDriver driver, string button)
        {
            string title = GetTranslation(driver.Language, button);
            return By.XPath(".//*[starts-with(@title, \"" + title + "\")]");
        }

        /// <summary>
        /// Function To create a By with the title translated
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="button"></param>
        /// <returns>By Title</returns>
        public static By GetButtonByValue(TxWebDriver driver, string button)
        {
            string value = GetTranslation(driver.Language, button);
            return By.XPath(".//*[@value=\"" + value + "\"]");
        }
    }
}
