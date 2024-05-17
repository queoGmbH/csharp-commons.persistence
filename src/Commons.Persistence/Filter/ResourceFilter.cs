using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Queo.Commons.Persistence.Infrastructure.TransactionManager;
using Queo.Commons.Persistence.Filter.FilterAttributes;

using System.Linq;

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
        /// <param name="logger"></param>
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
            bool readOnlyTransaction = TransactionIsReadOnly(context);
            _transactionManager.BeginTransaction(readOnlyTransaction);
        }

        /// <inheritdoc />
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        private bool TransactionIsReadOnly(ResourceExecutingContext context)
        {
            // Check if the request is a GET request
            bool readOnlyTransaction = context.HttpContext.Request.Method == "GET";

            bool notReadOnlyTransactionIsDefined = false;

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                // Check if the TransactionNotReadOnlyAttribute is defined on the method
                notReadOnlyTransactionIsDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(TransactionNotReadOnlyAttribute)));
            }

            return readOnlyTransaction && !notReadOnlyTransactionIsDefined;
        }
    }
}
