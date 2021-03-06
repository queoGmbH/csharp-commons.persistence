using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }
        public Task<IList<TEntity>> FindByBusinessIdsAsync(IList<Guid> businessIds)
        {
            throw new NotImplementedException();
        }

        public TEntity GetByBusinessId(Guid businessId)
        {
            return DbSetWithIncludedProperties().Single(e => e.BusinessId == businessId);

        }
        public async Task<TEntity> GetByBusinessIdAsync(Guid businessId)
        {
            return await DbSetWithIncludedProperties().SingleAsync(e => e.BusinessId == businessId);
        }
    }
}
