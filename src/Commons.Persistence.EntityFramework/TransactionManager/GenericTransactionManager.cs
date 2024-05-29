using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

/* Nicht gemergte Änderung aus Projekt "Commons.Persistence.EntityFramework (net6.0)"
Vor:
using Microsoft.Extensions.Logging;
Nach:
using Microsoft.Extensions.Logging;
using Queo;
using Queo.Commons;
using Queo.Commons.Persistence;
using Queo.Commons.Persistence.EntityFramework;
using Queo.Commons.Persistence.EntityFramework.Infrastructure;
using Queo.Commons.Persistence.EntityFramework.Infrastructure.TransactionManager;
using Queo.Commons.Persistence.EntityFramework.TransactionManager;
*/
using Microsoft.Extensions.Logging;
using Queo.Commons.Persistence.TransactionManager;

namespace Queo.Commons.Persistence.EntityFramework.TransactionManager
{
    /// <summary>
    /// TransactionManager class.
    /// </summary>
    public class GenericTransactionManager<T> : ITransactionManager where T : DbContext
    {
        private readonly T _dbContext;
        private IDbContextTransaction? _transaction;
        private bool _readOnly;
        private readonly ILogger<GenericTransactionManager<T>> _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="dbContext"></param>
        public GenericTransactionManager(T dbContext, ILogger<GenericTransactionManager<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public void BeginTransaction(bool readOnly)
        {
            _transaction = _dbContext.Database.BeginTransaction();
            _readOnly = readOnly;
        }

        /// <inheritdoc />
        public async Task BeginTransactionAsync(bool readOnly)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            _readOnly = readOnly;
        }

        /// <inheritdoc />
        public void SaveChangesAndCommitTransaction()
        {
            if (!TransactionIsActive())
            {
                _logger.LogWarning("No transaction to commit.");
                return;
            }

            if (_readOnly && _dbContext.ChangeTracker.HasChanges())
            {
                _logger.LogWarning("Transaction is read-only. No commit allowed.");
                return;
            }
            _dbContext.SaveChanges();
            _transaction!.Commit();
        }

        /// <inheritdoc />
        public async Task SaveChangesAndCommitTransactionAsync()
        {
            if (!TransactionIsActive())
            {
                _logger.LogWarning("No transaction to commit.");
                return;
            }

            if (_readOnly && _dbContext.ChangeTracker.HasChanges())
            {
                _logger.LogWarning("Transaction is read-only. No commit allowed.");
                return;
            }

            await _dbContext.SaveChangesAsync();
            await _transaction!.CommitAsync();
        }

        /// <inheritdoc />
        public void RollbackTransaction()
        {
            if (TransactionIsActive())
            {
                _transaction!.Rollback();
                DeleteTransaction();
            }
            else
            {
                _logger.LogWarning("No transaction to rollback.");
            }
        }

        /// <inheritdoc />
        public async Task RollbackTransactionAsync()
        {
            if (TransactionIsActive())
            {
                await _transaction!.RollbackAsync();
                DeleteTransaction();
            }
            else
            {
                _logger.LogWarning("No transaction to rollback.");
            }
        }

        /// <inheritdoc />
        public bool TransactionIsActive()
        {
            return _transaction != null;
        }

        private void DeleteTransaction()
        {
            _transaction = null;
        }
    }
}
