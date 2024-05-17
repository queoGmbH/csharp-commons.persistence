using Microsoft.AspNetCore.Mvc.Filters;
using Queo.Commons.Persistence.Infrastructure.TransactionManager;
using Microsoft.Extensions.Logging;

namespace Queo.Commons.Persistence.Filter
{
    /// <summary>
    /// ExceptionFilter is used to rollback the transaction if there is a transaction active and an exception occurs.
    /// </summary>
    public class TransactionRollbackOnExceptionFilter : IExceptionFilter
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<TransactionRollbackOnExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFilter"/> class.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        public TransactionRollbackOnExceptionFilter(ITransactionManager transactionManager, ILogger<TransactionRollbackOnExceptionFilter> logger)
        {
            _transactionManager = transactionManager;
            _logger = logger;
        }

        /// <summary>
        /// Handles the exception and performs the necessary actions.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public void OnException(ExceptionContext context)
        {
            if (_transactionManager.TransactionIsActive())
            {
                _transactionManager.RollbackTransaction();
                _logger.LogWarning("Transaction rolled back due to exception.");
            }
        }
    }
}
