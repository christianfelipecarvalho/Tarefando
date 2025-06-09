using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;
using Tarefando.Infrastructure.Data;

namespace Tarefando.Infrastructure.Repositories
{
    public class TaskRepository : BaseRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(TarefandoDbContext context) : base(context) { }

        public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.History)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskWithHistoryAsync(int id)
        {
            return await _dbSet
                .Include(t => t.History)
                .ThenInclude(h => h.User)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> GetCompletedTasksCountByUserAsync(int userId, DateTime fromDate)
        {
            return await _context.Tasks
                .Where(t => t.Project.UserId == userId &&
                           t.Status == Domain.Enums.TaskStatus.Concluida &&
                           t.ConcluidaEm >= fromDate)
                .CountAsync();
        }

        async Task<List<TaskItem>> ITaskRepository.GetTasksByProjectIdAsync(int idProject)
        {
            //return await _context.Tasks
            //    .Where(t => t.Project.Id == idProject).ToListAsync();
            return await _dbSet
               .Where(t => t.ProjectId == idProject)
               .Include(t => t.History)
               .Include(t => t.Comments)
               .ToListAsync();
        }

        public Task<TaskItem> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<TaskItem> UpdateAsync(TaskItem entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> GetProjectByTaskPendentes(int idProjeto)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(t => t.ProjectId == idProjeto && t.Status != Domain.Enums.TaskStatus.Concluida);
        }

        
        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        Task<List<TaskItem>> IRepositoryBase<TaskItem>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await GetByIdAsync(id);
            _dbSet.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
