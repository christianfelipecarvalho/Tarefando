using System.Linq.Expressions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;
using Tarefando.Infrastructure.Data;

namespace Tarefando.Infrastructure.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(TarefandoDbContext context) : base(context) { }

        public async Task<Project> CreateProjectAsync(Project project, int userId)
        {
            await _dbSet.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;

        }
        public virtual async Task<Project> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var project = await GetByIdAsync(id);
            _dbSet.Remove(project);
            await _context.SaveChangesAsync();
        }

        //public async Task<bool> GetProjectByTaskPendentes(int idProjeto)
        //{
        //    var hasPendingTasks = await _dbSet.Tasks.AnyAsync(t => t.ProjectId == idProjeto && t.Status != Domain.Enums.TaskStatus.Concluida);

        //    return hasPendingTasks;
        //}

        public async Task<IEnumerable<Project>> GetProjectsByUserAsync(int userId)
        {
            return await _dbSet
               .Where(p => p.UserId == userId)
               .Include(p => p.Tasks)
               .ToListAsync();
        }

        public async Task<Project?> GetProjectWithTasksAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Tasks)
                .ThenInclude(t => t.History)
                .Include(p => p.Tasks)
                .ThenInclude(t => t.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<Project> UpdateAsync(Project entity)
        {
            throw new NotImplementedException();
        }

        async Task<List<Project>> IRepositoryBase<Project>.GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}