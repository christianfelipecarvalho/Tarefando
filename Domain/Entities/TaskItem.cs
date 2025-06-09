
using Tarefando.Domain.Enums;

namespace Tarefando.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pendente;
    public TaskPriority Priority { get; set; } = TaskPriority.Media;
    public int ProjectId { get; set; }
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
    public DateTime? ConcluidaEm { get; set; }

    public virtual Project Project { get; set; } = null!;
    public virtual ICollection<TaskHistory> History { get; set; } = new List<TaskHistory>();
    public virtual ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();

    public void UpdateStatus(Enums.TaskStatus newStatus, int userId)
    {
        var oldStatus = Status;
        Status = newStatus;

        if (newStatus == Enums.TaskStatus.Concluida)
            ConcluidaEm = DateTime.UtcNow;

        AddHistoryEntry($"Status alterado de {oldStatus} para {newStatus}", userId);
    }

    public void UpdateDetails(string titulo, string descricao, int userId)
    {
        var changes = new List<string>();

        if (Titulo != titulo)
        {
            changes.Add($"Título alterado de '{Titulo}' para '{titulo}'");
            Titulo = titulo;
        }

        if (Descricao != descricao)
        {
            changes.Add($"Descrição alterada");
            Descricao = descricao;
        }

        if (changes.Any())
            AddHistoryEntry(string.Join("; ", changes), userId);
    }

    public void AddComment(string comentario, int userId)
    {
        Comments.Add(new TaskComment
        {
            Comentario = comentario,
            UserId = userId,
            CriadoEm = DateTime.UtcNow
        });

        AddHistoryEntry($"Comentário adicionado: {comentario}", userId);
    }

    public void AddHistoryEntry(string alteracao, int userId)
    {
        History.Add(new TaskHistory
        {
            Alteracao = alteracao,
            UserId = userId,
            DataAlteracao = DateTime.UtcNow
        });
    }
}