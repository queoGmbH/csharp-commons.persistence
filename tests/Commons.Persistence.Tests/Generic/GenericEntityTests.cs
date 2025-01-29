using Castle.DynamicProxy;
using NUnit.Framework;
using Queo.Commons.Persistence.Generic;
using System;

namespace Queo.Commons.Persistence.Tests.Generic
{
    public class GenericEntityTests
    {
        [Test]
        public void TestGetTypeUnproxied()
        {
            // Unproxied type
            Entity<int> entity = new();

            // Generate a proxy
            ProxyGenerator proxyGenerator = new();
            Entity<int> proxy = (Entity<int>) proxyGenerator.CreateClassProxy(entity.GetType(), new object[] { });

            Type type = proxy.GetType();
            Type unproxiedType = proxy.GetTypeUnproxied();

            Assert.That(type, Is.Not.EqualTo(unproxiedType));
            Assert.That(type.Namespace, Is.EqualTo("Castle.Proxies"));
            Assert.That(unproxiedType, Is.EqualTo(typeof(Entity<int>)));
        }
    }
}
