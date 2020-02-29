using Commons.Persistence.Generic;

namespace Commons.Persistence.EntityFramework.Tests.TestClasses {
    public class Boo : Entity<int> {
        public string Name { get; set; }
    }
}