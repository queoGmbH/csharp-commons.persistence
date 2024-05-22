using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Queo.Commons.Persistence.Infrastructure.TransactionManager;
using Queo.Commons.Persistence.Filter.FilterAttributes;

using System.Linq;
using System.Reflection;

namespace Queo.Commons.Persistence.Filter
{
    /// <summary>
    /// TransactionFilter is used to start a transaction before the request is processed.
    /// </summary>
    public class TransactionFilter : IResourceFilter
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<TransactionFilter> _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="transactionManager"></param>
        /// <param name="logger"></param>
        public TransactionFilter(ITransactionManager transactionManager, ILogger<TransactionFilter> logger)
        {
            _transactionManager = transactionManager;
            _logger = logger;
        }

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
            bool readOnlyTransaction = context.HttpContext.Request.Method == "GET";

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                TransactionAttribute? transactionAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes<TransactionAttribute>(inherit: true).FirstOrDefault();

                if (transactionAttribute != null)
                {
                    readOnlyTransaction = transactionAttribute.ReadOnly;
                }
            }

            return readOnlyTransaction;
        }
    }
}
