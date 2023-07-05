using System;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.DataTypes
{
    /// <summary>
    /// Represents a boolean in teexma.
    /// </summary>
    public class DTBool : IReadableData, IWritableData
    {
        public static readonly DTBool True = new DTBool("1");
        public static readonly DTBool False = new DTBool("");
        public static readonly DTBool Undefined = new DTBool("-1");

        private readonly string value;

        //TODO Impossible to know the value in write mode.
        /// <summary>
        /// Gets a TxBool from a value attribute content.
        /// </summary>
        /// <param name="elemValue">the value</param>
        /// <returns>the TxBool</returns>
        public static DTBool GetTxBool(string elemValue)
        {
            if (elemValue == DTBool.True.ToString()|| elemValue== "yes")
            {
                return DTBool.True;
            }
            else if (elemValue == DTBool.False.ToString() || elemValue == "no")
            {
                return DTBool.False;
            }
            else if (elemValue == DTBool.Undefined.ToString())
            {
                return DTBool.Undefined;
            }
            else
            {
                throw new Exception("Invalid elemValue : " + elemValue);
            }
        }

        private DTBool(string value)
        {
            this.value = value;
        }

        [TxConst]
        public DTBool(bool value)
        {
            if (value == true)
            {
                this.value = DTBool.True.value;
            }
            else if (value == false)
            {
                this.value = DTBool.False.value;
            }

        }

        public override bool Equals(object obj)
        {
            if (obj == null && GetType() == obj.GetType()) return false;

            return this.value == (obj as DTBool).value;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                if (value != null)
                {
                    hash = hash * 23 + value.GetHashCode();
                }
                return hash;
            }
        }

        public override string ToString()
        {
            return this.value;
        }

    }
}
