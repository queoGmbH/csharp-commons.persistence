using System;
using System.Collections.Generic;
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
        [SetUp]
        protected void Setup()
        {

            contextOptions = GetDbContextOptions<TestDbContext>();
            expectedEntity = new EntityWithStringKey("Die ID");
            expectedEntity.Name = "Testobjekt";

            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

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

        [Test]
        public void TestFindByBusinessIds()
        {
            Guid[] idsToFind = new Guid[2];
            List<EntityWithStringKey> expectedEntities = new List<EntityWithStringKey>();

            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey entity1 = new EntityWithStringKey("Erste Id");
                EntityWithStringKey entity2 = new EntityWithStringKey("Zweite Id");
                EntityWithStringKey entity3 = new EntityWithStringKey("Dritte Id");
                EntityWithStringKey entity4 = new EntityWithStringKey("Vierte Id");
                List<EntityWithStringKey> entities = new List<EntityWithStringKey>() { entity1, entity2, entity3, entity4 };
                entityDao.Save(entities);
                idsToFind[0] = entity1.BusinessId;
                idsToFind[1] = entity4.BusinessId;
                expectedEntities.Add(entity1);
                expectedEntities.Add(entity4);
                context.SaveChanges();

            }
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);

                IList<EntityWithStringKey> actualEntites = entityDao.FindByBusinessIds(idsToFind);
                int expectedEntityCount = 2;

                Assert.AreEqual(expectedEntityCount, actualEntites.Count);

                actualEntites.Should().BeEquivalentTo(expectedEntities);
            }
        }
        [Test]
        public async Task TestFindByBusinessIdsAsync()
        {
            Guid[] idsToFind = new Guid[2];
            List<EntityWithStringKey> expectedEntities = new List<EntityWithStringKey>();

            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey entity1 = new EntityWithStringKey("Erste Id");
                EntityWithStringKey entity2 = new EntityWithStringKey("Zweite Id");
                EntityWithStringKey entity3 = new EntityWithStringKey("Dritte Id");
                EntityWithStringKey entity4 = new EntityWithStringKey("Vierte Id");
                List<EntityWithStringKey> entities = new List<EntityWithStringKey>() { entity1, entity2, entity3, entity4 };
                entityDao.Save(entities);
                idsToFind[0] = entity1.BusinessId;
                idsToFind[1] = entity4.BusinessId;
                expectedEntities.Add(entity1);
                expectedEntities.Add(entity4);
                context.SaveChanges();

            }
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);

                IList<EntityWithStringKey> actualEntites = await entityDao.FindByBusinessIdsAsync(idsToFind);
                int expectedEntityCount = 2;

                Assert.AreEqual(expectedEntityCount, actualEntites.Count);

                actualEntites.Should().BeEquivalentTo(expectedEntities);
            }
        }
    }
}
