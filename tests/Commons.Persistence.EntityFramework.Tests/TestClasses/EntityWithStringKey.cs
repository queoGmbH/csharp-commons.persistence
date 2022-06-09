using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence.EntityFramework.Tests.TestClasses {
    public class EntityWithStringKey : Entity<string> {
        public EntityWithStringKey(string id):base(id) {
            
        }

        public string Name { get; set; }
    }
}