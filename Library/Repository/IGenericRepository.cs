using Database.Entities;

namespace Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task Create(TEntity item);
        public Task<TEntity> FindById(int id);
        public IQueryable<TEntity> Get();
        public Task<IEnumerable<TEntity>> GetAll();
        public Task Remove(TEntity item);
        public Task Update(TEntity item);
    }
}