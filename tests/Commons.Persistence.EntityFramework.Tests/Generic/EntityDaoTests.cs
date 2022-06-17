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
        private DbContextOptions<TestDbContext> contextOptions;
        private EntityWithStringKey expectedEntity;

        //GIVEN: <comment of assumptions>
        [OneTimeSetUp]
        protected void Setup()
        {
            contextOptions = GetDbContextOptions<TestDbContext>();
            expectedEntity = new EntityWithStringKey("Die ID");
            expectedEntity.Name = "Testobjekt";
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                entityDao.Save(expectedEntity);
                entityDao.Flush();
            }
        }

        [Test]
        public void TestSaveAndLoadEntityWithStringKey()
        {
            //WHEN: <comment on execution>
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey actualEntity = entityDao.Get(expectedEntity.Id);

                //THEN: <comments on expectations>
                actualEntity.Should().BeEquivalentTo(expectedEntity);
            }
        }

        [Test]
        public void TestLoadByBusinessId()
        {
            //WHEN: <comment on execution>
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey actualEntity = entityDao.GetByBusinessId(expectedEntity.BusinessId);

                //THEN: <comments on expectations>
                actualEntity.Should().BeEquivalentTo(expectedEntity);
            }
        }

        [Test]
        public async Task TestLoadByBusinessIdAsync()
        {
            //WHEN: <comment on execution>
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey actualEntity = await entityDao.GetByBusinessIdAsync(expectedEntity.BusinessId);

                //THEN: <comments on expectations>
                actualEntity.Should().BeEquivalentTo(expectedEntity);
            }
        }
    }
}
