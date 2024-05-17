using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Queo.Commons.Persistence.Infrastructure.TransactionManager;

namespace Queo.Commons.Persistence.Filter
{
    /// <summary>
    /// ResourceFilter is used to start a transaction before the request is processed.
    /// </summary>
    public class ResourceFilter : IResourceFilter
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<ResourceFilter> _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="transactionManager"></param>
        public ResourceFilter(ITransactionManager transactionManager, ILogger<ResourceFilter> logger)
        {
            _transactionManager = transactionManager;
            _logger = logger;
        }
        // Get endpunkt hier als Readonly, mittels attribut am controler kann diese auf schreibbar gesetzt werden
        // Mittels Attribut kann man die Methode auf schreibbar setzen, obwohl sie get ist (readonly)

        /// <inheritdoc />
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            bool readOnlyTransaction = context.HttpContext.Request.Method == "GET";
            _transactionManager.BeginTransaction(readOnlyTransaction);
        }

        /// <inheritdoc />
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
