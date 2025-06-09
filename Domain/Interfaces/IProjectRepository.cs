using Domain.Entities;
using Tarefando.Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository : IRepositoryBase<Project>
{
    Task<Project> GetProjectWithTasksAsync(int projetctId);
    Task<IEnumerable<Project>> GetProjectsByUserAsync(int userId);
    Task<Project> CreateProjectAsync(Project project,int userId);
}
