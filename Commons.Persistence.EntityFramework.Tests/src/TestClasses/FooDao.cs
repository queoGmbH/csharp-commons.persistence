using System.Linq;

using Commons.Persistence.EntityFramework.Generic;

using Microsoft.EntityFrameworkCore;

namespace Commons.Persistence.EntityFramework.Tests.TestClasses {
    public class FooDao : GenericDao<Foo, int> {
        public FooDao(DbContext dbContext) : base(dbContext) {
        }

        protected override IQueryable<Foo> DbSetWithIncludedProperties() {
            return base.DbSetWithIncludedProperties().Include(foo=>foo.Boo);
        }
    }
}