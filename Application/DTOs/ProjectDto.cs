using System.ComponentModel.DataAnnotations;
using Tarefando.Domain.Entities;

namespace Tarefando.Application.DTOs;

/// <summary>
/// DTO para representar um projeto
/// </summary>
public class ProjectDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Arquivado { get; set; }
    public int UsuarioId { get; set; }
    public int TotalTarefas { get; set; }
    public int TarefasConcluidas { get; set; }
    public int TarefasPendentes { get; set; }
    public double PercentualConclusao => TotalTarefas > 0 ? (double)TarefasConcluidas / TotalTarefas * 100 : 0;


    public static ProjectDto FromEntity(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Nome = project.Nome,
            Descricao = project.Descricao,
            DataCriacao = project.CriadoEm,
            UsuarioId = project.UserId

        };
    }

}

 

/// <summary>
/// DTO para criação de um novo projeto
/// </summary>
public class CreateProjectDto
{
    [Required(ErrorMessage = "Nome do projeto é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string? Descricao { get; set; }
}

/// <summary>
/// DTO para atualização de um projeto
/// </summary>
public class UpdateProjectDto
{
    [Required(ErrorMessage = "Nome do projeto é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string? Descricao { get; set; }
}

/// <summary>
/// DTO para duplicação de um projeto
/// </summary>
public class DuplicateProjectDto
{
    [Required(ErrorMessage = "Nome do novo projeto é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string NovoNome { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string? NovaDescricao { get; set; }

    /// <summary>
    /// Indica se as tarefas do projeto original devem ser copiadas
    /// </summary>
    public bool CopiarTarefas { get; set; } = true;

    /// <summary>
    /// Indica se apenas tarefas não concluídas devem ser copiadas
    /// </summary>
    public bool ApenasNaoConcluidas { get; set; } = false;
}

/// <summary>
/// DTO para arquivamento/desarquivamento de um projeto
/// </summary>
public class ArchiveProjectDto
{
    [Required]
    public bool Archived { get; set; }
}

/// <summary>
/// DTO para estatísticas detalhadas de um projeto
/// </summary>
public class ProjectStatsDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int TotalTarefas { get; set; }
    public int TarefasConcluidas { get; set; }
    public int TarefasPendentes { get; set; }
    public int TarefasAtrasadas { get; set; }
    public double PercentualConclusao { get; set; }
    public DateTime? DataUltimaTarefa { get; set; }
    public DateTime? ProximoDeadline { get; set; }

    /// <summary>
    /// Estatísticas por prioridade
    /// </summary>
    public Dictionary<string, int> TarefasPorPrioridade { get; set; } = new();

    /// <summary>
    /// Estatísticas por status
    /// </summary>
    public Dictionary<string, int> TarefasPorStatus { get; set; } = new();

    /// <summary>
    /// Produtividade dos últimos 30 dias
    /// </summary>
    public List<DailyProductivityDto> ProdutividadeUltimos30Dias { get; set; } = new();
}

/// <summary>
/// DTO para produtividade diária
/// </summary>
public class DailyProductivityDto
{
    public DateTime Data { get; set; }
    public int TarefasConcluidas { get; set; }
    public int TarefasCriadas { get; set; }
}

/// <summary>
/// DTO para resumo de todos os projetos do usuário
/// </summary>
public class ProjectsSummaryDto
{
    public int TotalProjetos { get; set; }
    public int ProjetosAtivos { get; set; }
    public int ProjetosArquivados { get; set; }
    public int TotalTarefas { get; set; }
    public int TarefasConcluidas { get; set; }
    public int TarefasPendentes { get; set; }
    public int TarefasAtrasadas { get; set; }
    public double PercentualConclusaoGeral { get; set; }

    /// <summary>
    /// Resumo dos projetos mais ativos
    /// </summary>
    public List<ProjectSummaryItemDto> ProjetosMaisAtivos { get; set; } = new();

    /// <summary>
    /// Projetos com maior número de tarefas atrasadas
    /// </summary>
    public List<ProjectSummaryItemDto> ProjetosComTarefasAtrasadas { get; set; } = new();

    /// <summary>
    /// Projetos recentemente atualizados
    /// </summary>
    public List<ProjectSummaryItemDto> ProjetosRecentementeAtualizados { get; set; } = new();
}

/// <summary>
/// DTO para item de resumo de projeto
/// </summary>
public class ProjectSummaryItemDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalTarefas { get; set; }
    public int TarefasConcluidas { get; set; }
    public int TarefasAtrasadas { get; set; }
    public double PercentualConclusao { get; set; }
    public DateTime? UltimaAtualizacao { get; set; }
}