using Microsoft.EntityFrameworkCore;

namespace Queo.Commons.Persistence.EntityFramework.Tests.TestClasses
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Foo> Foos => Set<Foo>();

        public DbSet<EntityWithStringKey> EntityWithStringKeys => Set<EntityWithStringKey>();
    }
}
