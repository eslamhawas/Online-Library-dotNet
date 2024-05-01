
using Microsoft.EntityFrameworkCore;

namespace Online_Library.Data
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        private readonly OnlineLibraryContext _context;
        private readonly DbSet<T> table;
        public DataRepository(OnlineLibraryContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await table.FindAsync(id);
        }

        public IQueryable<T> GetQueryable()
        {
            return table;
        }

        public void Insert(T entity)
        {
           table.AddAsync(entity);
        }


        public void Update(T entity)
        {
            table.Update(entity);
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
        }

        public  Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
