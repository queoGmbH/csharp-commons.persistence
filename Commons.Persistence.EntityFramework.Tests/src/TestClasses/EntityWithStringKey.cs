using Commons.Persistence.Generic;

namespace Commons.Persistence.EntityFramework.Tests.TestClasses {
    public class EntityWithStringKey : Entity<string> {
        public EntityWithStringKey(string id):base(id) {
            
        }

        public string Name { get; set; }
    }
}