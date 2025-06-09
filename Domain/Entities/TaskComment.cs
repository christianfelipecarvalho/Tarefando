using Domain.Entities;

namespace Tarefando.Domain.Entities;

public class TaskComment
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public virtual TaskItem Task { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}