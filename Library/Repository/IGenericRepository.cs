using Database.Entities;

namespace Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public void Create(TEntity item);
        public TEntity FindById(int id);
        public IEnumerable<TEntity> Get();
        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        public void Remove(TEntity item);
        public void Update(TEntity item);
    }
}