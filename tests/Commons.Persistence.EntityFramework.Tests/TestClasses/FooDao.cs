using System.Linq;

using Microsoft.EntityFrameworkCore;

using Queo.Commons.Persistence.EntityFramework.Generic;

namespace Queo.Commons.Persistence.EntityFramework.Tests.TestClasses {
    public class FooDao : GenericDao<Foo, int> {
        public FooDao(DbContext dbContext) : base(dbContext) {
        }

        protected override IQueryable<Foo> DbSetWithIncludedProperties() {
            return base.DbSetWithIncludedProperties().Include(foo=>foo.Boo);
        }
    }
}