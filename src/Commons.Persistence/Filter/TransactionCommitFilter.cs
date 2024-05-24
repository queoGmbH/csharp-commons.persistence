using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

using Queo.Commons.Persistence.Infrastructure.TransactionManager;

namespace Queo.Commons.Persistence.Filter
{
    /// <summary>
    ///     AlwaysRunResultFilter
    /// </summary>
    public class TransactionCommitFilter : IAlwaysRunResultFilter
    {
        private readonly ITransactionManager _transactionManager;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="transactionManager"></param>
        public TransactionCommitFilter(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        /// <inheritdoc />
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // If no exception was thrown in the controller or layers below, the transaction is committed here.
            // If an exception was thrown, the transaction was rolled back by the ExceptionFilter.

            // If a transaction is active, try to commit it.
            if (_transactionManager.TransactionIsActive())
            {
                try
                {
                    _transactionManager.SaveChangesAndCommitTransaction();
                }
                catch
                {
                    // ExceptionResponseWriter should be configurable: Configuration options: how should the response be written? IExceptionResponseWriter
                    context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(JsonSerializer.Serialize("An error occurred while trying to save changes and commit the transaction.")) { StatusCode = 500 };
                }
            }
        }

        /// <inheritdoc />
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
