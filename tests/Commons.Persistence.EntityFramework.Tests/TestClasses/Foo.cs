using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence.EntityFramework.Tests.TestClasses
{
    public class Foo : Entity<int>
    {

        public Boo Boo { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
    }
}
