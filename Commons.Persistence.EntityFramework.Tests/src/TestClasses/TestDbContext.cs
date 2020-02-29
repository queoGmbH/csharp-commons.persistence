using Microsoft.EntityFrameworkCore;

namespace Commons.Persistence.EntityFramework.Tests.TestClasses {
    public class TestDbContext : DbContext {
        public TestDbContext(DbContextOptions options) : base(options) {
        }

        public DbSet<Foo> Foos { get; set; }

        public DbSet<EntityWithStringKey> EntityWithStringKeys { get; set; }
    }
}