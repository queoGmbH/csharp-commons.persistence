using NUnit.Framework;
using Queo.Commons.Persistence.Exceptions;
using System.Globalization;
using System.Threading;

namespace Queo.Commons.Persistence.Tests.Exceptions
{
    [TestFixture]

    public class EntityNotFoundExceptionTests
    {
        [Test]
        public void EntityNotFoundEmptyConstructorCultureDe()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
            EntityNotFoundException exception = new EntityNotFoundException();
            Assert.AreEqual("Die Entität wurde nicht gefunden.", exception.Message);
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        [Test]
        public void EntityNotFoundEmptyConstructorCultureEn()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            EntityNotFoundException exception = new EntityNotFoundException();
            Assert.AreEqual("The entity was not found.", exception.Message);
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        [Test]
        public void EntityNotFoundExceptionWithParamsCultureEn()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            EntityNotFoundException exception = new EntityNotFoundException(typeof(EntityNotFoundException), "42");
            Assert.AreEqual("The entity was not found. (Type: EntityNotFoundException, ID: 42)", exception.Message);
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }
    }
}
