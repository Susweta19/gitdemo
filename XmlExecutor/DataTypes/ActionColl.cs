using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XmlExecutor.Attributes;

namespace XmlExecutor.DataTypes
{
    /// <inheritdoc />
    /// <summary>
    /// This class is a collection wrapper with action functions.
    /// </summary>
    /// <typeparam name="T">the wrapped collection inner type</typeparam>
    public class ActionColl<T> : IEnumerable<T>
    {
        public ICollection<T> Coll { get; private set; }

        public ActionColl(ICollection<T> collection)
        {
            Coll = collection;
        }

        [TxAction("ElementAt", "Get the element at the specified index begins at 0")]
        public T ElementAt(int index)
        {
            return Coll.ElementAt(index);
        }

        [TxAction("ElementAtFromLast", "Get the element from last at the specified index begins at 1")]
        public T ElementAtFromLast(int index)
        {
            return Coll.ElementAt(Coll.Count-index);
        }

        [TxAction("LastElement", "Get the element at the specified index begins at 0")]
        public T LastElement()
        {
            return Coll.Last();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Coll.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
