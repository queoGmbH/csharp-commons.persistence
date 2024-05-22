using Microsoft.AspNetCore.Mvc.Filters;

namespace Queo.Commons.Persistence.Filter.FilterAttributes
{
    /// <summary>
    /// TransactionAttribute class.
    /// </summary>
    public class TransactionAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// Readonly property for the TransactionAttribute.
        /// </summary>
        public bool ReadOnly { get; set; } = true;
    }
}
