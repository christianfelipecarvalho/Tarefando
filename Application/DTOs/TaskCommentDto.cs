using Tarefando.Domain.Entities;

namespace Tarefando.Application.DTOs
{
    public class TaskCommentDto
    {
        public int Id { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public string UsuarioNome { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }

        public static TaskCommentDto FromEntity(TaskComment comment)
        {
            return new TaskCommentDto
            {
                Id = comment.Id,
                Comentario = comment.Comentario,
                UsuarioNome = comment.User?.Nome ?? "Sistema",
                CriadoEm = comment.CriadoEm
            };
        }
    }

    public class CreateCommentDto
    {
        public string Comentario { get; set; } = string.Empty;
    }
}