using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queo.Commons.Persistence.Generic
{
    public interface IGenericDao<T, in TKey>
    {
        /// <summary>
        ///     Leert die Session.
        /// </summary>
        /// <remarks>
        ///     Die Methode sollte nur in Testfällen verwendet werden.
        /// </remarks>
        void Clear();

        /// <summary>
        ///     Löscht die übergebene Entität
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        ///     Checks whether there is an entity with the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>
        ///     <code>true</code>, if an entity with the specified primary key exists, otherwise <code>false</code>
        /// </returns>
        bool Exists(TKey primaryKey);

        /// <summary>
        ///     Checks asynchronously whether there is an entity with the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>
        ///     <code>true</code>, if an entity with the specified primary key exists, otherwise <code>false</code>
        /// </returns>
        Task<bool> ExistsAsync(TKey primaryKey);

        /// <summary>
        ///     Transfers all open changes to the database.
        /// </summary>
        /// <remarks>
        ///     In general, this method does not need to be called, as the control
        ///     is implicitly via the session or the transaction and via the FlushMode. 
        ///     In certain cases, however, it is helpful, e.g. for test cases.
        /// </remarks>
        void Flush();

        /// <summary>
        ///     Transfers all open changes to the database asynchronously.
        /// </summary>
        /// <remarks>
        ///     In general, this method does not need to be called, as the control
        ///     is implicitly via the session or the transaction and via the FlushMode. 
        ///     In certain cases, however, it is helpful, e.g. for test cases.
        /// </remarks>
        Task FlushAsync();

        /// <summary>
        ///     Returns the entity with the specified primary key.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        T Get(TKey primaryKey);

        /// <summary>
        ///     Returns the entity with the specified primary key asynchronously.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        Task<T> GetAsync(TKey primaryKey);

        /// <summary>
        ///     Returns a list with all entities.
        /// </summary>
        /// <returns>List with all entities.</returns>
        IList<T> GetAll();

        /// <summary>
        ///     Returns a list with all entities asynchronously.
        /// </summary>
        /// <returns>List with all entities.</returns>
        Task<IList<T>> GetAllAsync();

        /// <summary>
        ///     Returns the number of all objects.
        /// </summary>
        /// <returns>Number of objects.</returns>
        long GetCount();

        /// <summary>
        ///     Returns the number of all objects asynchronously.
        /// </summary>
        /// <returns>Number of objects.</returns>
        Task<long> GetCountAsync();

        /// <summary>
        ///     Saves the passed entity.
        /// </summary>
        /// <param name="entity">The entity to be saved</param>
        /// <returns>The stored entity</returns>
        T Save(T entity);

        /// <summary>
        ///     Saves the passed entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be saved</param>
        /// <returns>The stored entity</returns>
        Task<T> SaveAsync(T entity);

        /// <summary>
        ///     Saves all entities contained in the passed list.
        /// </summary>
        /// <param name="entities">List of entities to be saved.</param>
        /// <returns>List with stored entities</returns>
        IList<T> Save(IList<T> entities);

        /// <summary>
        ///     Saves asynchronously all entities that are contained in the passed list.
        /// </summary>
        /// <param name="entities">Liste mit zu speichernden Entities.</param>
        /// <returns>Liste mit gespeicherten Entities</returns>
        Task<IList<T>> SaveAsync(IList<T> entities);

        /// <summary>
        ///     Searches for <see cref="T" /> using a list of Ids.
        /// </summary>
        /// <param name="ids">
        ///     List of Ids in which the <see cref="Entity.Id" /> of a <see cref="T" /> must be contained in order to be found.
        /// </param>
        /// <returns></returns>
        IList<T> FindByIds(TKey[] ids);

        /// <summary>
        ///     Searches asynchronously for <see cref="T" /> using a list of Ids.
        /// </summary>
        /// <param name="ids">
        ///     List of Ids in which the<see cref= "Entity.Id" /> of a<see cref="T" /> must be contained in order to be found.
        /// </param>
        /// <returns></returns>
        Task<IList<T>> FindByIdsAsync(TKey[] ids);
    }
}
