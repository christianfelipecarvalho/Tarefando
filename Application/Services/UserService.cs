using Domain.Entities;
using Domain.Interfaces;
using Tarefando.Application.DTOs;

namespace Tarefando.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(UserDto.FromEntity);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? UserDto.FromEntity(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? UserDto.FromEntity(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(createDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("E-mail já está em uso");

            var user = new User
            {
                Email = createDto.Email,
                Nome = createDto.Nome,
                Role = createDto.Role
            };

            var createdUser = await _userRepository.AddAsync(user);

            return UserDto.FromEntity(createdUser);
        }
    }
}