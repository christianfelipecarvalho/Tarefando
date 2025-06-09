using Microsoft.AspNetCore.Mvc;
using Tarefando.Application.DTOs;
using Tarefando.Application.Services;
using Tarefando.Domain.Enums;
using Tarefando.Infrastructure.Data;

namespace Tarefando.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;
    private readonly TarefandoDbContext _context;

    public ReportsController(ReportService reportService, TarefandoDbContext context)
    {
        _reportService = reportService;
        _context = context;
    }

    [HttpGet("performance")]
    public async Task<ActionResult<IEnumerable<PerformanceReportDto>>> GetPerformanceReport([FromHeader(Name = "User-Id")] int userId)
    {
        if (userId <= 0)
            return BadRequest("User-Id header é obrigatório");

        // Verificar se o usuário tem permissão de gerente
        var user = await _context.Users.FindAsync(userId);
        if (user == null || user.Role != UserRole.Gerente)
            return Forbid("Acesso negado. Apenas gerentes podem acessar relatórios de desempenho.");

        var reports = await _reportService.GetPerformanceReportAsync();
        return Ok(reports);
    }
}