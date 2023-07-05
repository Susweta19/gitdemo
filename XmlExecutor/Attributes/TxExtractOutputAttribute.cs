using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExecutor.Attributes
{
    /// <summary>
    /// Marks a method so it's return value will be stored by the executor to be used by it's calling code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class TxExtractOutputAttribute : Attribute
    {
        public string Name { get; private set; }

        /// <summary>
        /// Marks a method so it's return value will be stored by the executor to be used by it's calling code.
        /// For each call of this method the value will be added to an array accessible from a dictionnary the key to get the array is the name passed in this constructor.
        /// </summary>
        /// <param name="name">the name as a key to get the value returned by the associated method.</param>
        public TxExtractOutputAttribute(string name)
        {
            Name = name;
        }
    }
}
