using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.DataTypes;
using TxSelenium.NativeTests.DataTypes;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements.General;
using TxSelenium.Utils;
using System;

namespace TxSelenium.NativeTests.Writable
{
    /// <summary>
    /// This class interacts with a multiple link in write mode.
    /// </summary>
    public class WMultipleLink : WSingleLink
    {
        public WMultipleLink(TxWebDriver driver, By elemIdentifier, By entityExpand, By entitySelectorBy, WERefreshed parent = null, int treeType = 1)
            : base(driver, elemIdentifier, entityExpand, entitySelectorBy, parent, treeType)
        {
        }

        [TxAction("IsTaskSelected", "Checks weather a task is ticked or not")]
        public bool IsTaskSelected(string taskName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//span[text()=\"" + taskName + "\"]/ancestor::tr[@idnode]//div[contains(@style,\"CheckAll.gif\")] | //span[text()=\"" + taskName + "\"]/ancestor::tr[@idnode]//div[contains(@style,\"CheckDis.gif\")] "), this.GetElement());
        }

        [TxAction("IsTaskPresent", "Checks weather a task is present or not in the Add Task window")]
        public bool IsTaskPresent(string taskName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath("//span[starts-with(@class,\"standartTreeRow\") and contains(text(),\"" + taskName + "\")]"), this.GetElement());
        }

        /// <summary>
        /// 
        /// </summary>
        ///// <param name="collection">Path collection</param>
        [TxAction("SelectEntitiesBox", "Toggles the selection box of  entities identified by the collection leading to it.")]
        public WEGCollection<WEGCheckBox> SelectEntitiesBox(ActionColl<DTEntityNode> collection)
        {
            ICollection<WEGCheckBox> ColChkBox = new List<WEGCheckBox>(collection.Coll.Count);
            
            for (int i = 0; i < collection.Coll.Count; i++)
                ColChkBox.Add(SelectEntityBox(collection.Coll.ElementAt(i)));

            return new WEGCollection<WEGCheckBox>(ColChkBox);
        }

        /// <summary>
        /// Uses the select all functionnality.
        /// </summary>
        [TxAction("SelectAll", "Select all checked entities.")]
        public void SelectAll()
        {
            try
            {
                GetDriver().ClickAt(ElemByPictureName.SelectAll, this);
            }
            catch (Exception)
            {
                Console.WriteLine("There is multiple window");
                GetDriver().ClickAt(ElemByPictureName.SelectAll,
                       GetDriver().WaitForElement(this.ElemIdentifier));
            }
        }

        [TxAction("SelectObjectandchildren", "check selected object & children.")]
        public void SelectObjectandchildren()
        {
            GetDriver().ClickAt(ElemByPictureName.SelectObjectandchildren, this);
        }

        [TxAction("DeselectObjectandchildren", "uncheck selected object & children.")]
        public void DeselectObjectandchildren()
        {
            GetDriver().ClickAt(ElemByPictureName.DeselectObjectandchildren, this);
        }

        /// <summary>
        /// Checks weather the radio button for a particular entity is ticked or not
        /// Returns true is radio button is ticked
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        [TxAction("IsRadioButtonTicked", "Checks weather the radio button for a particular entity is ticked or not")]
        public bool IsRadioButtonTicked(string entityName)
        {
            WaitForAjax();
            return GetDriver().IsElementPresent(By.XPath(".//span[text()=\"" + entityName + "\"]/ancestor::tbody//div[starts-with(@class,\"dhx_bg_img_fix\") and contains(@style,\"radio_on.gif\")]"), this.GetElement());
        }


    }
}
