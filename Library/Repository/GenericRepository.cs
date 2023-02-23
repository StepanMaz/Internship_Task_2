using Microsoft.EntityFrameworkCore;
using Database;
using Database.Entities;

namespace Repositories
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

        public IQueryable<TEntity> Get()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Get().ToListAsync();
        }
         
        public async Task <TEntity> FindById(int id)
        {
            return await _dbSet.FindAsync(id);
        }
 
        public async Task Create(TEntity item)
        {
            _dbSet.Add(item);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task Update(TEntity item)
        {
            _libraryContext.Entry(item).State = EntityState.Modified;
            await _libraryContext.SaveChangesAsync();
        }

        public async Task Remove(TEntity item)
        {
            _dbSet.Remove(item);
            await _libraryContext.SaveChangesAsync();
        }
    }
}