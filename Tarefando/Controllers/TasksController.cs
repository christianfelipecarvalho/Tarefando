using Microsoft.AspNetCore.Mvc;
using Tarefando.Application.DTOs;
using Tarefando.Application.Services;

namespace Tarefando.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }
    /// <summary>
    /// Buscar tarefas de um projeto
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByProject(int projectId, [FromHeader(Name = "User-Id")] int userId)
    {
        var tasks = await _taskService.GetTasksByProjectAsync(projectId);
        return Ok(tasks);
    }
    /// <summary>
    /// Buscar tarefa por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    /// <summary>
    /// Criar tarefa vinculada a um projeto
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="createDto"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("project/{projectId}")]
    public async Task<ActionResult<TaskDto>> CreateTask(int projectId, [FromBody] CreateTaskDto createDto, [FromHeader(Name = "User-Id")] int userId)
    {
        if (userId <= 0)
            return BadRequest("User-Id header é obrigatório");

        if (string.IsNullOrWhiteSpace(createDto.Titulo))
            return BadRequest("Título da tarefa é obrigatório");

        try
        {
            var task = await _taskService.CreateTaskAsync(createDto, projectId, userId);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Atualizar tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateDto"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(int id, [FromBody] UpdateTaskDto updateDto, [FromHeader(Name = "User-Id")] int userId)
    {
        if (userId <= 0)
            return BadRequest("User-Id header é obrigatório");

        try
        {
            var task = await _taskService.UpdateTaskAsync(id, updateDto, userId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Deleta tarefa por id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id, [FromHeader(Name = "User-Id")] int userId)
    {
        var result = await _taskService.DeleteTask(id);
        return Ok(result);
    }

    /// <summary>
    /// Adicionar comentario a uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <param name="commentDto"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<TaskDto>> AddComment(int id, [FromBody] CreateCommentDto commentDto, [FromHeader(Name = "User-Id")] int userId)
    {
        if (userId <= 0)
            return BadRequest("User-Id header é obrigatório");

        if (string.IsNullOrWhiteSpace(commentDto.Comentario))
            return BadRequest("Comentário não pode estar vazio");

        var task = await _taskService.AddCommentAsync(id, commentDto, userId);
        if (task == null)
            return NotFound();

        return Ok(task);
    }
}