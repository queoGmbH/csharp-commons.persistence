using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queo.Commons.Persistence.Generic
{
    public interface IEntityDao<T, TKey> : IGenericDao<T, TKey> where T : Entity<TKey>
    {
        /// <summary>
        ///     Searches for <see cref="T" /> using a list of Ids.
        /// </summary>
        /// <param name="businessIds">
        ///     List of Guids in which the <see cref="Entity.BusinessId" /> of a <see cref="T" />
        ///     must be included for it to be found.
        /// </param>
        /// <returns></returns>
        IList<T> FindByBusinessIds(IList<Guid> businessIds);

        /// <summary>
        ///     Searches asynchronously for <see cref="T" /> from a list of Ids.
        /// </summary>
        /// <param name="businessIds">
        ///     List of Guids in which the <see cref="Entity.BusinessId" /> of a <see cref="T" />
        ///     must be included for it to be found.
        /// </param>
        /// <returns></returns>
        Task<IList<T>> FindByBusinessIdsAsync(IList<Guid> businessIds);

        /// <summary>
        ///     Returns the entity with the appropriate BusinessId.
        /// </summary>
        /// <param name="businessId">the BusinessId</param>
        /// <returns>The entity</returns>
        T GetByBusinessId(Guid businessId);

        /// <summary>
        ///     Returns asynchronously the entity with the appropriate BusinessId.
        /// </summary>
        /// <param name="businessId">the BusinessId</param>
        /// <returns>The entity</returns>
        Task<T> GetByBusinessIdAsync(Guid businessId);
    }
}
