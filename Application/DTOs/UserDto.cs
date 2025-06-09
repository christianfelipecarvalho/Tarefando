using Domain.Entities;
using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;

namespace Tarefando.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CriadoEm { get; set; }

        public static UserDto FromEntity(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Nome = user.Nome,
                Role = user.Role,
                CriadoEm = user.CriadoEm
            };
        }
    }

    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Usuario;
    }
}