using Domain.Interfaces;
using Tarefando.Application.DTOs;

namespace Tarefando.Application.Services
{
    public class ReportService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public ReportService(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<PerformanceReportDto>> GetPerformanceReportAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var users = await _userRepository.GetAllAsync();
            var reports = new List<PerformanceReportDto>();

            foreach (var user in users)
            {
                var completedTasks = await _taskRepository.GetCompletedTasksCountByUserAsync(user.Id, thirtyDaysAgo);
                var avgTasksPerDay = completedTasks / 30.0;

                reports.Add(new PerformanceReportDto
                {
                    UserId = user.Id,
                    UsuarioNome = user.Nome,
                    TarefasConcluidas = completedTasks,
                    MediaTarefasPorDia = Math.Round(avgTasksPerDay, 2),
                    PeriodoInicio = thirtyDaysAgo,
                    PeriodoFim = DateTime.UtcNow
                });
            }

            return reports.OrderByDescending(r => r.MediaTarefasPorDia);
        }
    }
}