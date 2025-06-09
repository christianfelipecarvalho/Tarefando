using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Tarefando.Application.DTOs;
using Tarefando.Filters;

namespace Tarefando.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [UserValidationFilter]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os projetos do usuário
        /// </summary>
        /// <returns>Lista de projetos do usuário</returns>
        [HttpGet("ProjetoPorUsuario")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects([FromHeader(Name = "User-Id")] int userId)
        {
            _logger.LogInformation("Buscando projetos para o usuário ", userId);
            var projects = await _projectService.GetProjectsByUserAsync(userId);
            return Ok(projects);
        }

        /// <summary>
        /// Obtém todos os projetos
        /// </summary>
        /// <returns>Lista de projetos</returns>
        [HttpGet("BuscarTodos")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects([FromHeader(Name = "User-Id")] int userId)
        {
            _logger.LogInformation("Buscando projetos para o usuário ", userId);
            var projects = await _projectService.GetAllProjects();
            return Ok(projects);
        }

        /// <summary>
        /// Obtém um projeto específico por ID
        /// </summary>
        /// <param name="id">ID do projeto</param>
        /// <returns>Dados do projeto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id, [FromHeader(Name = "User-Id")] int userId)
        {
            var projetoId = await _projectService.ObterPorId(id);

            return Ok(projetoId);
        }

        /// <summary>
        /// Cria um novo projeto
        /// </summary>
        /// <param name="createDto">Dados para criação do projeto</param>
        /// <returns>Projeto criado</returns>
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectDto createDto, [FromHeader(Name = "User-Id")] int userId)
        {
            _logger.LogInformation("Criando novo projeto para o usuário {UserId}", userId);
            var project = await _projectService.CreateProjectAsync(createDto, userId);
            _logger.LogInformation($"Projeto {project.Id} criado com sucesso para o usuário {userId}");

            return Ok(project);
        }

        /// <summary>
        /// exclui um projeto
        /// </summary>
        /// <param name = "id" > id do projeto</param>
        /// <returns>resultado da operação</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProjeto(int id)
        {
            try
            {
                var message = await _projectService.RemoverProjeto(id);
                return Ok(message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

    }
}