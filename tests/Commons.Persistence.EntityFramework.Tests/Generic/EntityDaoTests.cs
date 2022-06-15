using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using Queo.Commons.Persistence.EntityFramework.Generic;
using Queo.Commons.Persistence.EntityFramework.Tests.TestClasses;

namespace Queo.Commons.Persistence.EntityFramework.Tests.Generic
{
    [TestFixture]
    public class EntityDaoTests : PersistenceBaseTest
    {
        [Test]
        public void TestSaveAndLoadEntityWithStringKey()
        {
            DbContextOptions<TestDbContext> contextOptions = GetDbContextOptions<TestDbContext>();
            EntityWithStringKey expectedEntity = new EntityWithStringKey("Die ID");
            expectedEntity.Name = "Testobjekt";
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                entityDao.Save(expectedEntity);
                entityDao.Flush();
            }

            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey actualEntity = entityDao.Get(expectedEntity.Id);
                actualEntity.Should().BeEquivalentTo(expectedEntity);
            }
        }

        [Test]
        public void TestLoadByBusinessId()
        {
            DbContextOptions<TestDbContext> contextOptions = GetDbContextOptions<TestDbContext>();
            EntityWithStringKey expectedEntity = new EntityWithStringKey("Eine ID");
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                entityDao.Save(expectedEntity);
                entityDao.Flush();
            }
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey actualEntity = entityDao.GetByBusinessId(expectedEntity.BusinessId);
                actualEntity.Should().BeEquivalentTo(expectedEntity);
            }
        }

        [Test]
        public async Task TestLoadByBusinessIdAsync()
        {
            DbContextOptions<TestDbContext> contextOptions = GetDbContextOptions<TestDbContext>();
            EntityWithStringKey expectedEntity = new EntityWithStringKey("Eine ID");
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                entityDao.Save(expectedEntity);
                entityDao.Flush();
            }
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey actualEntity = await entityDao.GetByBusinessIdAsync(expectedEntity.BusinessId);
                actualEntity.Should().BeEquivalentTo(expectedEntity);
            }
        }
    }
}
