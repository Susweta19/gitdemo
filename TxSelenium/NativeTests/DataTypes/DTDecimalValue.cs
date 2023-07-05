namespace TxSelenium.NativeTests.DataTypes
{
    /// <summary>
    /// Represents the data associated with a decimal value.
    /// </summary>
    public class DTDecimalValue : IWritableData, IReadableData
    {
        /// <summary>
        /// The min value if this a range or the value if this is a single value.
        /// </summary>
        public string Min { get; private set; }
        /// <summary>
        /// The max value if this a range or null if this is a single value.
        /// </summary>
        public string Max { get; private set; }
        /// <summary>
        /// The mean value if this a range with a mean or the null if this is a value without mean.
        /// </summary>
        public string Mean { get; private set; }
        /// <summary>
        /// The unit if this value has one null otherwise.
        /// </summary>
        public DTDecimalUnit Unit { get; private set; }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="min">The min value if this a range or the value if this is a single value.</param>
        /// <param name="max">The max value if this a range or null if this is a single value.</param>
        /// <param name="mean">The mean value if this a range with a mean, null otherwise.</param>
        /// <param name="unit">The unit if this value has one, null otherwise.</param>
        public DTDecimalValue(string min = null, string max = null, string mean = null, DTDecimalUnit unit = null)
        {
            this.Min = min;
            this.Max = max;
            this.Mean = mean;
            this.Unit = unit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            bool isEqual = true;
            DTDecimalValue castedObj = obj as DTDecimalValue;
            isEqual = isEqual && this.Min == castedObj.Min;
            isEqual = isEqual && this.Max == castedObj.Max;
            isEqual = isEqual && this.Mean == castedObj.Mean;

            if (this.Unit == null)
                isEqual = isEqual && castedObj.Unit == null;
            else
                isEqual = isEqual && this.Unit.UnitName == castedObj.Unit.UnitName;

            return isEqual;
        }

        public override string ToString()
        {
            if (Min != null && Max != null && Mean != null)
                return "Min : " + this.Min + " Max : " + this.Max + " Mean : " + this.Mean;
            else if (Min != null && Max != null && Mean == null)
                return "Min : " + this.Min + " Max : " + this.Max;
            else
                return "Min : " + this.Min;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                if (Min != null)
                    hash = hash * 23 + Min.GetHashCode();
                if (Max != null)
                    hash = hash * 23 + Max.GetHashCode();
                if (Mean != null)
                    hash = hash * 23 + Mean.GetHashCode();
                if (Unit != null)
                    hash = hash * 23 + Unit.GetHashCode();
                return hash;
            }
        }
    }
}
