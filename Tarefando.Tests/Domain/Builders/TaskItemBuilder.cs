// TaskItemBuilder.cs
using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;
using TaskStatus = Tarefando.Domain.Enums.TaskStatus;

namespace Tarefando.Tests.Domain.Builders;

public class TaskItemBuilder
{
    private TaskItem _task = new();

    public static TaskItemBuilder New() => new();

    public TaskItemBuilder WithId(int id)
    {
        _task.Id = id;
        return this;
    }

    public TaskItemBuilder WithTitle(string titulo)
    {
        _task.Titulo = titulo;
        return this;
    }

    public TaskItemBuilder WithDescription(string descricao)
    {
        _task.Descricao = descricao;
        return this;
    }

    public TaskItemBuilder WithStatus(TaskStatus status)
    {
        _task.Status = status;
        return this;
    }

    public TaskItemBuilder WithPriority(TaskPriority priority)
    {
        _task.Priority = priority;
        return this;
    }

    public TaskItemBuilder WithProjectId(int projectId)
    {
        _task.ProjectId = projectId;
        return this;
    }

    public TaskItemBuilder WithConcluidaEm(DateTime? concluidaEm)
    {
        _task.ConcluidaEm = concluidaEm;
        return this;
    }

    public TaskItemBuilder WithCriadaEm(DateTime criadaEm)
    {
        _task.CriadaEm = criadaEm;
        return this;
    }
}


