using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence {
    public interface IEntityDao<T> : IEntityDao<T, int> where T : Entity<int> {

    }
}