using System;
using System.Collections.Generic;
using System.Linq;

using Commons.Persistence.Generic;

using Microsoft.EntityFrameworkCore;

namespace Commons.Persistence.EntityFramework.Generic {
    public class EntityDao<TEntity, TKey> : GenericDao<TEntity, TKey>, IEntityDao<TEntity, TKey> where TEntity : Entity<TKey> {
        public EntityDao(DbContext dbContext) : base(dbContext) {
        }

        public IList<TEntity> FindByBusinessIds(IList<Guid> businessIds) {
            throw new NotImplementedException();
        }

        public TEntity GetByBusinessId(Guid businessId) {
            TEntity entity = DbSetWithIncludedProperties().Single(e => e.BusinessId == businessId);
            return entity;
        }
    }
}