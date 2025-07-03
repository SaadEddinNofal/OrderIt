using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OrderITDemo.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        T FindById(int id);
        IQueryable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
        Task<T> GetByIdAsync(int id);

    }
}
