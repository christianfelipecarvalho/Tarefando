using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Tarefando.Application.DTOs;
using Tarefando.Domain.Entities;

namespace Application.Services;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;

    public ProjectService(IProjectRepository projectRepository, ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;

    }
    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createDto, int userId)
    {
        Project projeto = new Project();
        projeto.UserId = userId;
        projeto.Nome = createDto.Nome;
        projeto.Descricao = createDto.Descricao;

        var projetoCriado = await _projectRepository.CreateProjectAsync(projeto, userId);
        return ProjectDto.FromEntity(projetoCriado);
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjects()
    {
        var todosProjetos = await _projectRepository.GetAllAsync();

        return todosProjetos.Select(ProjectDto.FromEntity);
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsByUserAsync(int userId)
    {
       var projetosUsuario = await _projectRepository.GetProjectsByUserAsync(userId);

        return projetosUsuario.Select(ProjectDto.FromEntity);
    }

    public async Task<ProjectDto> ObterPorId(int id)
    {
        var projetoDto = await _projectRepository.GetByIdAsync(id);

        return ProjectDto.FromEntity(projetoDto);
    }
    public async Task<string> RemoverProjeto(int idProjeto)
    {
        var entidadeProjeto = await _projectRepository.GetByIdAsync(idProjeto);
        if (entidadeProjeto == null)
        {
            return "Projeto não existe!";
        }
        var temTarefasPendentes = await _taskRepository.GetProjectByTaskPendentes(idProjeto);

        if(temTarefasPendentes)
        {
            return "Falha ao deletar projeto, existem tarefas não concluidas. Conclua as tarefas para conseguir deletar";
        }
         await _projectRepository.DeleteAsync(idProjeto);

        return "Operação realizada com sucesso!";
    }
}