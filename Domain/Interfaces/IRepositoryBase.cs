using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
