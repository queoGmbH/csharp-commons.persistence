using System;

namespace Queo.Commons.Persistence.Filter.FilterAttributes
{
    /// <summary>
    /// TransactionAttribute class.
    /// </summary>
    public class TransactionAttribute : Attribute
    {
        /// <summary>
        /// Readonly property for the TransactionAttribute.
        /// </summary>
        public bool ReadOnly { get; set; } = false;
    }
}
