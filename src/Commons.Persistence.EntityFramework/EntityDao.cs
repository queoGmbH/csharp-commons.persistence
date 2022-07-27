using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Queo.Commons.Persistence.EntityFramework.Generic;

namespace Queo.Commons.Persistence.EntityFramework
{
    public class EntityDao<TEntity> : EntityDao<TEntity, int>, IEntityDao<TEntity> where TEntity : Entity
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public EntityDao(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
