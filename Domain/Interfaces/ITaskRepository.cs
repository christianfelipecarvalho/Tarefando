using System.Threading.Tasks;
using Tarefando.Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskRepository : IRepositoryBase<TaskItem>
{
    Task<List<TaskItem>> GetTasksByProjectIdAsync(int idProject);
    Task<TaskItem> GetTaskByIdAsync(int id);
    Task<TaskItem> GetTaskWithHistoryAsync(int id);
    Task<int> GetCompletedTasksCountByUserAsync(int id, DateTime intervalo);
    Task<bool> GetProjectByTaskPendentes(int idProjeto);

}
