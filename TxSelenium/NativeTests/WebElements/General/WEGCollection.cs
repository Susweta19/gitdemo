using System.Collections.Generic;
using System.Linq;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.WebElements.General
{
    public class WEGCollection<T>
    {
        ICollection<T> Collection;

        public WEGCollection(ICollection<T> collection)
        {
            this.Collection = collection;
        }

        [TxAction("Tick", "...")]
        public void Tick()
        {
            for (int i = 0; i < Collection.Count; i++)
                (Collection.ElementAt(i) as WEGCheckBox).Tick();
        }

        [TxAction("Read", "Use the drop down list.")]
        public ActionColl<bool> ReadValue()
        {
            ICollection<bool> boolColl = new List<bool>();
            for (int i = 0; i < Collection.Count; i++)
            {
                boolColl.Add((Collection.ElementAt(i) as WEGCheckBox).ReadValue());
            }

            return new ActionColl<bool>(boolColl);
        }
    }
}
