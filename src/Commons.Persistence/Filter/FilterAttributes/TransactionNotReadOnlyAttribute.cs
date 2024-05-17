using Microsoft.AspNetCore.Mvc.Filters;

namespace Queo.Commons.Persistence.Filter.FilterAttributes
{
    /// <summary>
    /// Attribute to mark a transaction as not read only.
    /// </summary>
    public class TransactionNotReadOnlyAttribute : ResultFilterAttribute
    {
    }
}
