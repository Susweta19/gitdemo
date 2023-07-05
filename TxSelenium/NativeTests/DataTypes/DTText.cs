namespace TxSelenium.NativeTests.DataTypes
{
    public class DTText : IWritableData, IReadableData
    {
        public string Value { get; private set; }

        public DTText(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return this.Value == (obj as DTText).Value;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                if (Value != null)
                    hash = hash * 23 + Value.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
