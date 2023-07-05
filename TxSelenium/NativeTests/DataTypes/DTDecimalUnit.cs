namespace TxSelenium.NativeTests.DataTypes
{
    /// <summary>
    /// Represents the data associated with the unit of a decimal value.
    /// </summary>
    public class DTDecimalUnit
    {
        public string UnitName { get; private set; }

        /// <summary>
        /// The constructor.
        /// While each parameter can be null they should not be both null.
        /// </summary>
        /// <param name="unitId">the id of the unit</param>
        /// <param name="unitName">the string representation of the unit</param>
        public DTDecimalUnit(string unitName)
        {
            UnitName = unitName;
        }
    }
}
