using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Queo.Commons.Persistence.EntityFramework.Generic;

namespace Queo.Commons.Persistence.EntityFramework
{
    public class EntityDao<TEntity> : GenericDao<TEntity, int>, IEntityDao<TEntity> where TEntity : Entity
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public EntityDao(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        ///     Sucht nach <see cref="T" /> anhand einer Liste mit Ids.
        /// </summary>
        /// <param name="businessIds">
        ///     Liste mit Guids in denen die <see cref="Entity.BusinessId" /> einer <see cref="T" />
        ///     enthalten sein muss, damit sie gefunden wird.
        /// </param>
        /// <returns></returns>
        public virtual IList<TEntity> FindByBusinessIds(IList<Guid> businessIds)
        {
            return DbContext.Set<TEntity>().Where(x => businessIds.Contains(x.BusinessId)).ToList();
        }

        /// <summary>
        ///     Liefert das Entity mit der entsprechenden BusinessId.
        /// </summary>
        /// <param name="businessId">die BusinessId</param>
        /// <returns>Das Entity</returns>
        public virtual TEntity GetByBusinessId(Guid businessId)
        {
            return DbContext.Set<TEntity>().Single(x => x.BusinessId == businessId);
        }
    }
}
