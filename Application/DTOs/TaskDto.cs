using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;

namespace Tarefando.Application.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Domain.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int ProjectId { get; set; }
        public DateTime CriadaEm { get; set; }
        public DateTime? ConcluidaEm { get; set; }
        public List<TaskHistoryDto> History { get; set; } = new();
        public List<TaskCommentDto> Comments { get; set; } = new();

        public static TaskDto FromEntity(TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Titulo = task.Titulo,
                Descricao = task.Descricao,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                CriadaEm = task.CriadaEm,
                ConcluidaEm = task.ConcluidaEm,
                History = task.History.Select(h => TaskHistoryDto.FromEntity(h)).ToList(),
                Comments = task.Comments.Select(c => TaskCommentDto.FromEntity(c)).ToList()
            };
        }
    }

    public class CreateTaskDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Media;
    }

    public class UpdateTaskDto
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public Domain.Enums.TaskStatus? Status { get; set; }
    }
}