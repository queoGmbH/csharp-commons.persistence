using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence.EntityFramework.Tests.TestClasses
{
    public class Boo : Entity<int>
    {
        public string Name { get; set; } = string.Empty;
    }
}
