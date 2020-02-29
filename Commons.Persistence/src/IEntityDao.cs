using Commons.Persistence.Generic;

namespace Commons.Persistence {
    public interface IEntityDao<T> : IEntityDao<T, int> where T : Entity<int> {

    }
}