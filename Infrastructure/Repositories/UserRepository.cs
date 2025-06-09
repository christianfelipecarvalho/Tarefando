using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tarefando.Domain.Entities;
using Tarefando.Infrastructure.Data;

namespace Tarefando.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TarefandoDbContext context) : base(context) { }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        async Task<List<User>> IRepositoryBase<User>.GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

    }
}