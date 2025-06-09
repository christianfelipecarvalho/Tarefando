using Domain.Entities;

namespace Tarefando.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public bool CanBeDeleted() => !Tasks.Any(t => t.Status != Enums.TaskStatus.Concluida);
    public bool CanAddTask() => Tasks.Count < 20;
}