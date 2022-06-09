using System.Collections.Generic;

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
                genericDao.Save(foos);
                fooIdsToFind[0] = foo1.Id;
                fooIdsToFind[1] = foo4.Id;
                expectedFoos.Add(foo1);
                expectedFoos.Add(foo4);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                IList<Foo> actualFoos = genericDao.FindByIds(fooIdsToFind);
                int expectedFooCount = 2;
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

                addedFoos = genericDao.Save(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                IPage<Foo> actualFoos = genericDao.Find(pageRequest);

                Assert.AreEqual(fooCount, addedFoos.Count);
                Assert.AreEqual(expectedFooCount, actualFoos.NumberOfElements);
            }
        }

        [Test]
        public void TestGetAll()
        {
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            IList<Foo> expectedFoos;
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3 };
                expectedFoos = genericDao.Save(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                IList<Foo> actualFoos = genericDao.GetAll();
                CollectionAssert.AreEquivalent(expectedFoos, actualFoos);
            }
        }

        [Test]
        public void TestGetCount()
        {
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo1 = new Foo { Email = "test@test.com" };
                Foo foo2 = new Foo { Email = "test2@test.com" };
                Foo foo3 = new Foo { Email = "test3@test.com" };
                Foo foo4 = new Foo { Email = "test4@test.com" };
                List<Foo> foos = new List<Foo> { foo1, foo2, foo3, foo4 };
                genericDao.Save(foos);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                long actualCount = genericDao.GetCount();
                int expectedCount = 4;
                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [Test]
        public void TestLoadAssociatedEntity()
        {
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            int fooId;
            Boo expectedBoo = new Boo { Name = "Testboo" };
            Foo expectedFoo = new Foo { Name = "Testfoo", Boo = expectedBoo };
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                FooDao fooDao = new FooDao(context);
                fooDao.Save(expectedFoo);
                context.SaveChanges();
                fooId = expectedFoo.Id;
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                FooDao fooDao = new FooDao(context);
                Foo foo = fooDao.Get(fooId);
                foo.Should().BeEquivalentTo(expectedFoo);
                foo.Boo.Should().BeEquivalentTo(expectedBoo);
            }
        }

        [Test]
        public void TestSaveAndLoad()
        {
            DbContextOptions<TestDbContext> dbContextOptions = GetDbContextOptions<TestDbContext>();
            Foo expectedFoo;
            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo foo = new Foo { Email = "test@test.com" };
                expectedFoo = genericDao.Save(foo);
                context.SaveChanges();
            }

            using (TestDbContext context = new TestDbContext(dbContextOptions))
            {
                GenericDao<Foo, int> genericDao = new GenericDao<Foo, int>(context);
                Foo actualFoo = genericDao.Get(expectedFoo.Id);
                Assert.AreEqual(expectedFoo, actualFoo);
            }
        }
    }
}