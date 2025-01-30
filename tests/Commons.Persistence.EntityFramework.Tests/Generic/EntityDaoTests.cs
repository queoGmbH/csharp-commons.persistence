using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Queo.Commons.Persistence.EntityFramework.Generic;
using Queo.Commons.Persistence.EntityFramework.Tests.TestClasses;
using Queo.Commons.Persistence.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queo.Commons.Persistence.EntityFramework.Tests.Generic
{
    [TestFixture]
    public class EntityDaoTests : PersistenceBaseTest
    {
        private DbContextOptions<TestDbContext> contextOptions = null!;
        private EntityWithStringKey expectedEntity = null!;

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

                entityDao.Add(expectedEntity);
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
        public void TestGetByBusinessIdWithNonExistentBidShouldThrow()
        {
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                Guid nonExistentBusinessId = Guid.Empty;
                EntityNotFoundException? entityNotFoundException = Assert.Throws<EntityNotFoundException>(() => entityDao.GetByBusinessId(nonExistentBusinessId));
                ClassicAssert.AreEqual(typeof(EntityWithStringKey), entityNotFoundException!.EntityType);
                ClassicAssert.AreEqual(nonExistentBusinessId.ToString(), entityNotFoundException.Id);
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
        public void TestGetByBusinessIdAsyncWithNonExistentBidShouldThrow()
        {
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                Guid nonExistentBusinessId = Guid.Empty;
                EntityNotFoundException? entityNotFoundException = Assert.ThrowsAsync<EntityNotFoundException>(() => entityDao.GetByBusinessIdAsync(nonExistentBusinessId));
                ClassicAssert.AreEqual(typeof(EntityWithStringKey), entityNotFoundException!.EntityType);
                ClassicAssert.AreEqual(nonExistentBusinessId.ToString(), entityNotFoundException.Id);
            }
        }

        private IList<EntityWithStringKey> Save4Entities()
        {
            IList<EntityWithStringKey> the4Entities;
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityWithStringKey entity1 = new EntityWithStringKey("Erste Id");
                EntityWithStringKey entity2 = new EntityWithStringKey("Zweite Id");
                EntityWithStringKey entity3 = new EntityWithStringKey("Dritte Id");
                EntityWithStringKey entity4 = new EntityWithStringKey("Vierte Id");
                the4Entities = new List<EntityWithStringKey>() { entity1, entity2, entity3, entity4 };
                entityDao.Add(the4Entities);
                context.SaveChanges();
            }
            return the4Entities;
        }

        [Test]
        public void TestFindByBusinessIds()
        {
            IList<EntityWithStringKey> savedEntities = Save4Entities();

            IList<EntityWithStringKey> entitiesToFind = new List<EntityWithStringKey>()
            {
                savedEntities[0],
                savedEntities[3]
            };

            Guid[] idsToFind = entitiesToFind.Select(x => x.BusinessId).ToArray();

            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);

                IList<EntityWithStringKey> actualEntites = entityDao.FindByBusinessIds(idsToFind);
                int expectedEntityCount = 2;

                ClassicAssert.AreEqual(expectedEntityCount, actualEntites.Count);

                actualEntites.Should().BeEquivalentTo(entitiesToFind);
            }
        }

        [Test]
        public async Task TestFindByBusinessIdsAsync()
        {
            IList<EntityWithStringKey> savedEntities = Save4Entities();

            IList<EntityWithStringKey> entitiesToFind = new List<EntityWithStringKey>()
            {
                savedEntities[0],
                savedEntities[3]
            };

            Guid[] idsToFind = entitiesToFind.Select(x => x.BusinessId).ToArray();

            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);

                IList<EntityWithStringKey> actualEntites = await entityDao.FindByBusinessIdsAsync(idsToFind);
                int expectedEntityCount = 2;

                ClassicAssert.AreEqual(expectedEntityCount, actualEntites.Count);

                actualEntites.Should().BeEquivalentTo(entitiesToFind);
            }
        }

        [Test]
        public void TestGetWithWrongIdShouldThrow()
        {
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                string notExistentKey = "NotExistentKey";
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityNotFoundException? entityNotFoundException = Assert.Throws<EntityNotFoundException>(() =>
                {
                    entityDao.Get(notExistentKey);
                });
                ClassicAssert.AreEqual(typeof(EntityWithStringKey), entityNotFoundException!.EntityType);
                ClassicAssert.AreEqual(notExistentKey, entityNotFoundException.Id);
            }
        }

        [Test]
        public void TestGetAsyncWithWrongIdShouldThrow()
        {
            using (TestDbContext context = new TestDbContext(contextOptions))
            {
                string notExistentKey = "NotExistentKey";
                EntityDao<EntityWithStringKey, string> entityDao = new EntityDao<EntityWithStringKey, string>(context);
                EntityNotFoundException? entityNotFoundException = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                {
                    return entityDao.GetAsync(notExistentKey);
                });
                ClassicAssert.AreEqual(typeof(EntityWithStringKey), entityNotFoundException!.EntityType);
                ClassicAssert.AreEqual(notExistentKey, entityNotFoundException!.Id);
            }
        }
    }
}
