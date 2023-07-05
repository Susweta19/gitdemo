using System;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    /// <summary>
    /// This interface represents a class able to parse data of a certain type from its corresponding interface element
    /// Every implementing class must have an argumentless constructor.
    /// For a form to be able to read this data type a class implementing this interface with that type must be implemented.
    /// Only one specific implementation of this interface can exist this is only checked at runtime.
    /// </summary>
    /// <typeparam name="T">the data type the implementing class is able to parse</typeparam>
    public interface IReadingHandler<T>
    {
        /// <summary>
        /// Parses the data from an element in a form.
        /// If the element can be integrated in a list the index parameter should be taken into account.
        /// </summary>
        /// <param name="elem">the element to parse from a form</param>
        /// <param name="attrId">the attribute id of this element</param>
        /// <param name="index">the index of this element if it is parse from a list</param>
        /// <returns>the value</returns>
        T ReadForm(WERefreshed elem, int attrId);

        /// <summary>
        /// Gets a function able to parse the data from a cell in an associative class representation.
        /// The RefreshedElement parameter is the cell.
        /// </summary>
        /// <returns>the parser</returns>
        Func<WERefreshed, T> GetAssoParser();
    }
}
