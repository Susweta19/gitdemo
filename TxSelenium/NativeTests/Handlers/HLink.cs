using OpenQA.Selenium;
using System;
using System.Linq;
using TxSelenium.NativeTests.DataTypes;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Writable;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.Handlers
{
    class HLink : IWritingHandler<DTEntityNode>, IReadingHandler<WEGLink>
    {

		public static readonly By treeWriteFormBy = By.XPath("./div");


        public static By GetLinkReadBy(int index)
        {
            return By.XPath("(.//a)[" + index + "]");
        }

        public DTEntityNode ReadInputContent(WERefreshed elem)
        {
            WSingleLink writableLink = new WSingleLink(elem.GetDriver(), HLink.treeWriteFormBy, WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, elem);
            writableLink.SetFiltered(true);
            OrderedSet<DTEntityNode> set = writableLink.GetDisplayedEntities().Children;

            if (set.Count > 1) 
            {
                throw new Exception("This link tree contains more than one entry, it should not."); 
            }

            if (set.Count == 0)
            {
                return null;
            }
            else
            {
                return set.ElementAt(0);
            }
        }

        public void WriteForm(WERefreshed elem, int attrId, DTEntityNode value)
        {
            WSingleLink writableLink = new WSingleLink(elem.GetDriver(), HLink.treeWriteFormBy, WESTree.expandByClassicTree, WSingleLink.entitySelectorWLinkBy, elem);
            writableLink.SetFiltered(false);
            writableLink.SelectEntityBox(value);
        }

        public WEGLink ReadForm(WERefreshed elem, int attrId)
        {
            return new WEGLink(elem.GetDriver(), By.XPath(".//tr[not(contains(@style,\"display: none;\"))]//span[@class=\"standartTreeRow\"]|.//a"), elem);
        }

        public Func<WERefreshed, WEGLink> GetAssoParser()
        {
            return (elem) => Parser(elem, 1);
        }

        private WEGLink Parser(WERefreshed elem, int index)
        {
            return new WEGLink(elem.GetDriver(), HLink.GetLinkReadBy(index), elem);
        }

        public void DeleteData(WERefreshed elem)
        {

        }
    }
}
