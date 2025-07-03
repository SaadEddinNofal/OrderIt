using OrderITDemo.Data;
using OrderITDemo.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace OrderITDemo.Repository
{
    public class MainRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public MainRepository(ApplicationDbContext context)
        {
            _context= context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        public void Delete(int id)
        {
           var item = _dbSet.Find(id);
            if (item != null) 
            {
                _dbSet.Remove(item);
            }
        }

        public T FindById(int id)
        {

            return _dbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
