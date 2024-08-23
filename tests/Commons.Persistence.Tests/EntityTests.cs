using System;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Queo.Commons.Persistence.Tests
{
    public class EntityTests
    {

        [Test]
        public void TestToString()
        {
            int id = 3;
            Guid businessId = Guid.Parse("{B9CCE1CE-AD89-4999-B85B-42F05776A85E}");
            Entity entity = new Entity(id, businessId);

            string expectedString = $"Type: Entity, Id: 3, Bid: {businessId.ToString()}";
            string actualString = entity.ToString();
            ClassicAssert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void TestEqualsForReference()
        {
            Entity firstReference = new Entity();
            Entity secondReference = firstReference;

            Assert.That(firstReference.Equals(secondReference));
        }

        [Test]
        public void TestEqualsSameBusinessIdDifferentInstances()
        {
            int id = 3;
            Guid businessId = Guid.NewGuid();
            Entity firstObject = new Entity(id, businessId);
            Entity secondObject = new Entity(id, businessId);
            Assert.That(firstObject.Equals(secondObject));
        }

        [Test]
        public void TestDifferentBusinessIdNotEqual()
        {
            Guid firstBusinessId = Guid.Parse("{D98C1D4D-BEA8-44DC-A827-A46550249D92}");
            Guid secondBusinessId = Guid.Parse("{A3D0810D-3803-4302-B2B4-06F67686AD38}");
            Entity firstObject = new Entity(firstBusinessId);
            Entity secondObject = new Entity(secondBusinessId);

            ClassicAssert.IsFalse(firstObject.Equals(secondObject));
        }

        [Test]
        public void TestDifferentTypeForSameBusinessIdAreNotEqual()
        {
            Guid businessId = Guid.Parse("{D98C1D4D-BEA8-44DC-A827-A46550249D92}");
            Entity entity = new Entity(businessId);
            Foo foo = new Foo(businessId);

            ClassicAssert.IsFalse(entity.Equals(foo));
            ClassicAssert.IsFalse(foo.Equals(entity));
        }
    }

    internal class Foo : Entity
    {
        public Foo(Guid businessId) : base(businessId)
        {
        }
    }
}
