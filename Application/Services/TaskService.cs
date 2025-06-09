using Domain.Interfaces;
using Tarefando.Application.DTOs;
using Tarefando.Domain.Entities;

namespace Tarefando.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByProjectAsync(int projectId)
        {
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);
            return tasks.Select(TaskDto.FromEntity);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetTaskWithHistoryAsync(id);
            return task != null ? TaskDto.FromEntity(task) : null;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createDto, int projectId, int userId)
        {
            var project = await _projectRepository.GetProjectWithTasksAsync(projectId);

            if (project == null)
                throw new ArgumentException("Projeto não encontrado");

            if (!project.CanAddTask())
                throw new InvalidOperationException("O projeto já atingiu o limite máximo de 20 tarefas");

            var task = new TaskItem
            {
                Titulo = createDto.Titulo,
                Descricao = createDto.Descricao,
                Priority = createDto.Priority,
                ProjectId = projectId
            };

            task.AddHistoryEntry("Tarefa criada", userId);

            var createdTask = await _taskRepository.AddAsync(task);
            return TaskDto.FromEntity(createdTask);
        }

        public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateDto, int userId)
        {
            var task = await _taskRepository.GetTaskWithHistoryAsync(id);

            if (task == null)
                return null;

            if (updateDto.Status.HasValue)
                task.UpdateStatus(updateDto.Status.Value, userId);

            if (!string.IsNullOrEmpty(updateDto.Titulo) || !string.IsNullOrEmpty(updateDto.Descricao))
                task.UpdateDetails(updateDto.Titulo ?? task.Titulo, updateDto.Descricao ?? task.Descricao, userId);

            await _taskRepository.UpdateAsync(task);
            return TaskDto.FromEntity(task);
        }

        public async Task<TaskDto?> AddCommentAsync(int taskId, CreateCommentDto commentDto, int userId)
        {
            var task = await _taskRepository.GetTaskWithHistoryAsync(taskId);

            if (task == null)
                return null;

            task.AddComment(commentDto.Comentario, userId);
            await _taskRepository.UpdateAsync(task);

            return TaskDto.FromEntity(task);
        }

        public async Task<string> DeleteTask(int id)
        {
            var entidadeProjeto = await _taskRepository.GetByIdAsync(id);
            if (entidadeProjeto == null)
            {
                return "Task não existe!";
            }
            await _taskRepository.DeleteAsync(id);
            return "Operação realizada com sucesso!";
        }
    }
}