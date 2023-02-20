using Microsoft.EntityFrameworkCore;
using Database;
using Database.Entities;

namespace Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        LibraryContext _libraryContext;
        DbSet<TEntity> _dbSet;

        public GenericRepository(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
            _dbSet = libraryContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }
         
        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }
 
        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _libraryContext.SaveChanges();
        }
        
        public void Update(TEntity item)
        {
            _libraryContext.Entry(item).State = EntityState.Modified;
            _libraryContext.SaveChanges();
        }

        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
            _libraryContext.SaveChanges();
        }
    }
}