using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        ///     Search for <see cref="T" /> from a list of Ids.
        /// </summary>
        /// <param name="businessIds">
        ///     List of Guids in which the <see cref="Entity.BusinessId" /> of a <see cref="T" />
        ///     must be included for it to be found.
        /// </param>
        /// <returns></returns>
        public virtual IList<TEntity> FindByBusinessIds(IList<Guid> businessIds)
        {
            return DbContext.Set<TEntity>().Where(x => businessIds.Contains(x.BusinessId)).ToList();
        }

        /// <summary>
        ///     Search asynchronously for <see cref="T" /> from a list of Ids.
        /// </summary>
        /// <param name="businessIds"></param>
        ///     List of Guids in which the <see cref="Entity.BusinessId" /> of a <see cref="T" />
        ///     must be included for it to be found.
        /// <returns></returns>
        public virtual async Task<IList<TEntity>> FindByBusinessIdsAsync(IList<Guid> businessIds)
        {
            return await DbContext.Set<TEntity>().Where(x => businessIds.Contains(x.BusinessId)).ToListAsync();
        }

        /// <summary>
        ///     Returns the entity with the appropriate BusinessId.
        /// </summary>
        /// <param name="businessId">the BusinessId</param>
        /// <returns>The Entity</returns>
        public virtual TEntity GetByBusinessId(Guid businessId)
        {
            return DbContext.Set<TEntity>().Single(x => x.BusinessId == businessId);
        }

        /// <summary>
        ///     Returns asynchronously the entity with the appropriate BusinessId.
        /// </summary>
        /// <param name="businessId">the BusinessId</param>
        /// <returns>The Entity</returns>
        public virtual async Task<TEntity> GetByBusinessIdAsync(Guid businessId)
        {
            return await DbContext.Set<TEntity>().SingleAsync(x => x.BusinessId == businessId);
        }
    }
}
