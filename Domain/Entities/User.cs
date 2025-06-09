using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;

namespace Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CriadoEm { get; set; }
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}