using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;

using Queo.Commons.Persistence.EntityFramework.Tests.TestClasses;

namespace Queo.Commons.Persistence.EntityFramework.Tests
{
    public abstract class PersistenceBaseTest
    {
        internal DbContextOptions<TestDbContext> ContextOptions { get; private set; }

        /// <summary>
        ///     Stellt in Abhängigkeit der aufrufenden Methode einen Namen für die In-Memory-DB zur Verfügung
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        protected string GetDbName([CallerMemberName] string dbName = "default")
        {
            return $"{GetType()}-{dbName}";
        }

        /// <summary>
        /// Liefert die DbContextOptions für eine InMemory-DB mit einem vom Aufrufernamen abhängigen DB-Namen.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="dbName"></param>
        /// <returns></returns>
        protected DbContextOptions<TDbContext> GetDbContextOptions<TDbContext>([CallerMemberName] string dbName = "default")
            where TDbContext : DbContext
        {
            DbContextOptions<TDbContext> dbContextOptions = new DbContextOptionsBuilder<TDbContext>()
                .UseInMemoryDatabase(databaseName: GetDbName(dbName))
                .UseLazyLoadingProxies()
                .Options;
            return dbContextOptions;
        }
    }
}
