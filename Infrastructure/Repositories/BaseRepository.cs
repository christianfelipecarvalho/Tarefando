using Microsoft.EntityFrameworkCore;
using Tarefando.Infrastructure.Data;

namespace Tarefando.Infrastructure.Repositories;

public class BaseRepository<T> where T : class
{
    protected readonly TarefandoDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(TarefandoDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}