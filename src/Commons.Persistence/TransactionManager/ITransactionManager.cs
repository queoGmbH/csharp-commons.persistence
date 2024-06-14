using System.Threading.Tasks;

namespace Queo.Commons.Persistence.TransactionManager
{
    /// <summary>
    /// TransactionManager interface.
    /// </summary>
    public interface ITransactionManager
    {
        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        /// 
        void BeginTransaction(bool readOnly);

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        Task BeginTransactionAsync(bool readOnly);

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        void SaveChangesAndCommitTransaction();

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        Task SaveChangesAndCommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Returns whether a transaction is active.
        /// </summary>
        bool TransactionIsActive();
    }
}
