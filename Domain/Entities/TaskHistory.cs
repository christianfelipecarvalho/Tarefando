using Domain.Entities;

namespace Tarefando.Domain.Entities;

public class TaskHistory
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Alteracao { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;

    public virtual TaskItem Task { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}