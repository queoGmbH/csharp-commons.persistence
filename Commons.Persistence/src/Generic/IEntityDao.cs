using System;
using System.Collections.Generic;

namespace Commons.Persistence.Generic {
    public interface IEntityDao<T, TKey> : IGenericDao<T, TKey> where T : Entity<TKey> {
        /// <summary>
        ///     Sucht nach <see cref="T" /> anhand einer Liste mit Ids.
        /// </summary>
        /// <param name="businessIds">
        ///     Liste mit Guids in denen die <see cref="Entity.BusinessId" /> einer <see cref="T" />
        ///     enthalten sein muss, damit sie gefunden wird.
        /// </param>
        /// <returns></returns>
        IList<T> FindByBusinessIds(IList<Guid> businessIds);

        /// <summary>
        ///     Liefert das Entity mit der entsprechenden BusinessId.
        /// </summary>
        /// <param name="businessId">die BusinessId</param>
        /// <returns>Das Entity</returns>
        T GetByBusinessId(Guid businessId);
    }
}