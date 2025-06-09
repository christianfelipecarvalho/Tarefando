using Tarefando.Domain.Entities;

namespace Tarefando.Application.DTOs
{
    public class TaskHistoryDto
    {
        public int Id { get; set; }
        public string Alteracao { get; set; } = string.Empty;
        public string UsuarioNome { get; set; } = string.Empty;
        public DateTime DataAlteracao { get; set; }

        public static TaskHistoryDto FromEntity(TaskHistory history)
        {
            return new TaskHistoryDto
            {
                Id = history.Id,
                Alteracao = history.Alteracao,
                UsuarioNome = history.User?.Nome ?? "Sistema",
                DataAlteracao = history.DataAlteracao
            };
        }
    }
}