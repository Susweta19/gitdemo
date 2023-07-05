using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    /// <summary>
    /// This interface represents a class able to write data of a certain type to its corresponding interface element.
    /// Every implementing class must have an argumentless constructor.
    /// For a form to be able to write this data type a class implementing this interface with that type must be implemented.
    /// Only one specific implementation of this interface can exist, this is only checked at runtime.
    /// </summary>
    /// <typeparam name="T">the data type the implementing class is able to write</typeparam>
    public interface IWritingHandler<T>
    {
        /// <summary>
        /// Reads the content of an input.
        /// </summary>
        /// <param name="elem">the element representing the input</param>
        /// <returns>the parsed data</returns>
        T ReadInputContent(WERefreshed elem);

        void DeleteData(WERefreshed elem);

        /// <summary>
        /// Writes data to an input.
        /// </summary>
        /// <param name="elem">the element representing the input</param>
        /// <param name="attrId">the id of the parse attribute</param>
        /// <param name="value">the value to write</param>
        void WriteForm(WERefreshed elem, int attrId, T value);
        
    }
}
