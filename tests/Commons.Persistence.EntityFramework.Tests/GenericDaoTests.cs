using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using Queo.Commons.Persistence.EntityFramework.Generic;
using Queo.Commons.Persistence.EntityFramework.Tests.TestClasses;

namespace Queo.Commons.Persistence.EntityFramework.Tests
{
    [TestFixture]
    public class GenericDaoTests : PersistenceBaseTest
    {
        [Test]
        public void TestFindByIds()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int[] fooIdsToFind = new int[2];
            List<Foo> expectedFoos = new List<Foo>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                Foo foo4 = new Foo { Email = "test4@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3, foo4 };
                genericDao.Add(foos);
                fooIdsToFind[0] = foo1.Id;
                fooIdsToFind[1] = foo4.Id;
                expectedFoos.Add(foo1);
                expectedFoos.Add(foo4);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IList<Foo> actualFoos = genericDao.FindByIds(fooIdsToFind);
                int expectedFooCount = 2;

                //THEN: <comments on expectations>
                Assert.AreEqual(expectedFooCount, actualFoos.Count);
                actualFoos.Should().BeEquivalentTo(expectedFoos);
            }
        }

        [Test]
        public void TestExistsWithExistingEntry()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int fooIdToCheck = 2;
            List<Foo> expectedFoos = new List<Foo>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2 };
                genericDao.Save(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                bool fooExists = genericDao.Exists(fooIdToCheck);

                //THEN: <comments on expectations>
                fooExists.Should().BeTrue();
            }
        }

        [Test]
        public void TestExistsWithNotExistingEntry()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int fooIdToCheck = 5;
            List<Foo> expectedFoos = new List<Foo>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2 };
                genericDao.Save(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                bool fooExists = genericDao.Exists(fooIdToCheck);

                //THEN: <comments on expectations>
                fooExists.Should().BeFalse();
            }
        }

        [Test]
        public async Task TestFindByIdsAsync()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int[] fooIdsToFind = new int[2];
            List<Foo> expectedFoos = new List<Foo>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                Foo foo4 = new Foo { Email = "test4@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3, foo4 };
                await genericDao.AddAsync(foos);
                fooIdsToFind[0] = foo1.Id;
                fooIdsToFind[1] = foo4.Id;
                expectedFoos.Add(foo1);
                expectedFoos.Add(foo4);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IList<Foo> actualFoos = await genericDao.FindByIdsAsync(fooIdsToFind);
                int expectedFooCount = 2;

                //THEN: <comments on expectations>
                Assert.AreEqual(expectedFooCount, actualFoos.Count);
                actualFoos.Should().BeEquivalentTo(expectedFoos);
            }
        }

        /// <summary>
        ///     Testet dass laden von Nutzern mit Pagination
        /// </summary>
        [Test]
        public void TestFindWithPagination()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            IList<Foo> addedFoos;
            int expectedFooCount = 10;
            int fooCount = 15;
            PageRequest pageRequest = new PageRequest(1, expectedFooCount);

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                List<Foo> foos = new List<Foo>();
                for (int i = 0; i < fooCount; i++)
                {
                    Foo foo = new Foo { Email = $"test{i}@test.com" };
                    foos.Add(foo);
                }

                addedFoos = genericDao.Add(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IPage<Foo> actualFoos = genericDao.Find(pageRequest);

                //THEN: <comments on expectations>
                Assert.AreEqual(fooCount, addedFoos.Count);
                Assert.AreEqual(expectedFooCount, actualFoos.NumberOfElements);
            }
        }
        /// <summary>
        ///     Testet asynchron dass laden von Nutzern mit Pagination
        /// </summary>
        [Test]
        public async Task TestFindWithPaginationAsync()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            IList<Foo> addedFoos;
            int expectedFooCount = 10;
            int fooCount = 15;
            PageRequest pageRequest = new PageRequest(1, expectedFooCount);

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                List<Foo> foos = new List<Foo>();
                for (int i = 0; i < fooCount; i++)
                {
                    Foo foo = new Foo { Email = $"test{i}@test.com" };
                    foos.Add(foo);
                }

                addedFoos = await genericDao.AddAsync(foos);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IPage<Foo> actualFoos = await genericDao.FindAsync(pageRequest);


                //THEN: <comments on expectations>
                Assert.AreEqual(fooCount, addedFoos.Count);
                Assert.AreEqual(expectedFooCount, actualFoos.NumberOfElements);
            }
        }

        [Test]
        public void TestFindAll()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            IList<Foo> expectedFoos;
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3 };
                expectedFoos = genericDao.Add(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IList<Foo> actualFoos = genericDao.FindAll();

                //THEN: <comments on expectations>
                CollectionAssert.AreEquivalent(expectedFoos, actualFoos);
            }
        }

        [Test]
        public void TestFindAllWithNoEntitiesExisting()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IList<Foo> actualFoos = genericDao.FindAll();

                //THEN: <comments on expectations>
                actualFoos.Count.Should().Be(0);
            }
        }

        [Test]
        public async Task TestFindAllAsync()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            IList<Foo> expectedFoos;
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3 };
                expectedFoos = await genericDao.AddAsync(foos);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IList<Foo> actualFoos = await genericDao.FindAllAsync();

                //THEN: <comments on expectations>
                CollectionAssert.AreEquivalent(expectedFoos, actualFoos);
            }
        }

        [Test]
        public async Task TestFindAllAsyncNoEntitiesExisting()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                IList<Foo> actualFoos = await genericDao.FindAllAsync();

                //THEN: <comments on expectations>
                actualFoos.Count.Should().Be(0);
            }
        }

        [Test]
        public void TestGetCount()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                Foo foo4 = new Foo { Email = "test4@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3, foo4 };
                genericDao.Add(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                long actualCount = genericDao.GetCount();

                //THEN: <comments on expectations>
                int expectedCount = 4;
                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [Test]
        public async Task TestGetCountAsync()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                Foo foo4 = new Foo { Email = "test4@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3, foo4 };
                await genericDao.AddAsync(foos);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                long actualCount = await genericDao.GetCountAsync();

                //THEN: <comments on expectations>
                int expectedCount = 4;
                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [Test]
        public void TestLoadAssociatedEntity()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int fooId;
            Boo expectedBoo = new Boo { Name = "Testboo" };
            Foo expectedFoo = new Foo { Name = "Testfoo", Boo = expectedBoo };
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                FooDao fooDao = new FooDao(context);
                fooDao.Add(expectedFoo);
                context.SaveChanges();
                fooId = expectedFoo.Id;
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                FooDao fooDao = new FooDao(context);

                //WHEN: <comment on execution>
                Foo foo = fooDao.Get(fooId);

                //THEN: <comments on expectations>
                foo.Should().BeEquivalentTo(expectedFoo);
                foo.Boo.Should().BeEquivalentTo(expectedBoo);
            }
        }

        [Test]
        public async Task TestLoadAssociatedEntityAsync()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int fooId;
            Boo expectedBoo = new Boo { Name = "Testboo" };
            Foo expectedFoo = new Foo { Name = "Testfoo", Boo = expectedBoo };
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                FooDao fooDao = new FooDao(context);
                await fooDao.AddAsync(expectedFoo);
                await context.SaveChangesAsync();
                fooId = expectedFoo.Id;
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                FooDao fooDao = new FooDao(context);

                //WHEN: <comment on execution>
                Foo foo = await fooDao.GetAsync(fooId);

                //THEN: <comments on expectations>
                foo.Should().BeEquivalentTo(expectedFoo);
                foo.Boo.Should().BeEquivalentTo(expectedBoo);
            }
        }

        [Test]
        public void TestAddAndLoad()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            Foo expectedFoo;
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo = new Foo { Email = "test@test.com" };
                expectedFoo = genericDao.Add(foo);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                Foo actualFoo = genericDao.Get(expectedFoo.Id);

                //THEN: <comments on expectations>
                Assert.AreEqual(expectedFoo, actualFoo);
            }
        }

        [Test]
        public async Task TestAddAndLoadAsync()
        {
            //GIVEN: <comment of assumptions>
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            Foo expectedFoo;
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo = new Foo { Email = "test@test.com" };
                expectedFoo = await genericDao.AddAsync(foo);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);

                //WHEN: <comment on execution>
                Foo actualFoo = await genericDao.GetAsync(expectedFoo.Id);

                //THEN: <comments on expectations>
                Assert.AreEqual(expectedFoo, actualFoo);
            }
        }
    }
}
