using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Queo.Commons.Persistence.Exceptions;
using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence.EntityFramework.Generic
{
    public class EntityDao<TEntity, TKey> : GenericDao<TEntity, TKey>, IEntityDao<TEntity, TKey> where TEntity : Entity<TKey>
    {
        public EntityDao(DbContext dbContext) : base(dbContext)
        {
        }

        public IList<TEntity> FindByBusinessIds(IList<Guid> businessIds)
        {
            return DbContext.Set<TEntity>().Where(x => businessIds.Contains(x.BusinessId)).ToList();
        }
        public async Task<IList<TEntity>> FindByBusinessIdsAsync(IList<Guid> businessIds)
        {
            return await DbContext.Set<TEntity>().Where(x => businessIds.Contains(x.BusinessId)).ToListAsync();
        }

        public TEntity GetByBusinessId(Guid businessId)
        {
            try
            {
                TEntity entity = DbSetWithIncludedProperties().Single(e => e.BusinessId == businessId);
                return entity;
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(TEntity), businessId.ToString());
            }

        }
        public async Task<TEntity> GetByBusinessIdAsync(Guid businessId)
        {
            try
            {
                TEntity entity = await DbSetWithIncludedProperties().SingleAsync(e => e.BusinessId == businessId);
                return entity;
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(TEntity), businessId.ToString());
            }
        }
    }
}
