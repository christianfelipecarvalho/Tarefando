using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;
using TaskStatus = Tarefando.Domain.Enums.TaskStatus;
using FluentAssertions;

namespace Tarefando.Tests.Domain.Entities;

public class ProjectTests
{
    [Fact]
    public void Project_WhenCreated_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var project = new Project();

        // Assert
        project.Id.Should().Be(0);
        project.Nome.Should().BeEmpty();
        project.Descricao.Should().BeEmpty();
        project.UserId.Should().Be(0);
        project.CriadoEm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        project.Tasks.Should().BeEmpty();
    }

    [Fact]
    public void CanBeDeleted_WithAllTasksConcluidas_ShouldReturnTrue()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>
            {
                new() { Status = TaskStatus.Concluida },
                new() { Status = TaskStatus.Concluida },
                new() { Status = TaskStatus.Concluida }
            }
        };

        // Act
        var result = project.CanBeDeleted();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanBeDeleted_WithNoTasks_ShouldReturnTrue()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>()
        };

        // Act
        var result = project.CanBeDeleted();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanBeDeleted_WithPendenteTasks_ShouldReturnFalse()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>
            {
                new() { Status = TaskStatus.Concluida },
                new() { Status = TaskStatus.Pendente }, // Esta impede a exclusão
                new() { Status = TaskStatus.Concluida }
            }
        };

        // Act
        var result = project.CanBeDeleted();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanBeDeleted_WithEmAndamentoTasks_ShouldReturnFalse()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>
            {
                new() { Status = TaskStatus.Concluida },
                new() { Status = TaskStatus.EmAndamento }, // Esta impede a exclusão
                new() { Status = TaskStatus.Concluida }
            }
        };

        // Act
        var result = project.CanBeDeleted();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanBeDeleted_WithMixedNonConcluidaTasks_ShouldReturnFalse()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>
            {
                new() { Status = TaskStatus.Pendente },
                new() { Status = TaskStatus.EmAndamento },
                new() { Status = TaskStatus.Concluida }
            }
        };

        // Act
        var result = project.CanBeDeleted();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanAddTask_WithLessThan20Tasks_ShouldReturnTrue()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = Enumerable.Range(1, 19)
                .Select(i => new TaskItem { Id = i })
                .ToList()
        };

        // Act
        var result = project.CanAddTask();

        // Assert
        result.Should().BeTrue();
        project.Tasks.Count.Should().Be(19);
    }

    [Fact]
    public void CanAddTask_WithExactly20Tasks_ShouldReturnFalse()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = Enumerable.Range(1, 20)
                .Select(i => new TaskItem { Id = i })
                .ToList()
        };

        // Act
        var result = project.CanAddTask();

        // Assert
        result.Should().BeFalse();
        project.Tasks.Count.Should().Be(20);
    }

    [Fact]
    public void CanAddTask_WithMoreThan20Tasks_ShouldReturnFalse()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = Enumerable.Range(1, 25)
                .Select(i => new TaskItem { Id = i })
                .ToList()
        };

        // Act
        var result = project.CanAddTask();

        // Assert
        result.Should().BeFalse();
        project.Tasks.Count.Should().Be(25);
    }

    [Fact]
    public void CanAddTask_WithEmptyTasks_ShouldReturnTrue()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>()
        };

        // Act
        var result = project.CanAddTask();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(19)]
    public void CanAddTask_WithVariousTaskCounts_ShouldReturnTrue(int taskCount)
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = Enumerable.Range(1, taskCount)
                .Select(i => new TaskItem { Id = i })
                .ToList()
        };

        // Act
        var result = project.CanAddTask();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(30)]
    [InlineData(100)]
    public void CanAddTask_WithTaskCountAtOrAboveLimit_ShouldReturnFalse(int taskCount)
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = Enumerable.Range(1, taskCount)
                .Select(i => new TaskItem { Id = i })
                .ToList()
        };

        // Act
        var result = project.CanAddTask();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void BusinessRules_CombinedScenario_ShouldWorkCorrectly()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Nome = "Projeto Teste",
            Descricao = "Descrição do projeto",
            Tasks = new List<TaskItem>
            {
                new() { Id = 1, Status = TaskStatus.Concluida },
                new() { Id = 2, Status = TaskStatus.Concluida },
                new() { Id = 3, Status = TaskStatus.Pendente }
            }
        };

        // Act & Assert
        project.CanAddTask().Should().BeTrue(); // Menos de 20 tasks
        project.CanBeDeleted().Should().BeFalse(); // Tem task pendente
    }

    [Fact]
    public void BusinessRules_ProjectReadyForDeletion_ShouldAllowDeletion()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = new List<TaskItem>
            {
                new() { Status = TaskStatus.Concluida },
                new() { Status = TaskStatus.Concluida }
            }
        };

        // Act & Assert
        project.CanBeDeleted().Should().BeTrue();
        project.CanAddTask().Should().BeTrue();
    }

    [Fact]
    public void BusinessRules_ProjectAtMaxCapacity_ShouldNotAllowMoreTasks()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Tasks = Enumerable.Range(1, 20)
                .Select(i => new TaskItem
                {
                    Id = i,
                    Status = TaskStatus.Concluida
                })
                .ToList()
        };

        // Act & Assert
        project.CanAddTask().Should().BeFalse(); // Limite de 20 atingido
        project.CanBeDeleted().Should().BeTrue(); // Todas concluídas
    }

    [Fact]
    public void Project_WithNullTasks_ShouldInitializeEmptyCollection()
    {
        // Arrange & Act
        var project = new Project();

        // Assert
        project.Tasks.Should().NotBeNull();
        project.Tasks.Should().BeEmpty();
    }

    [Fact]
    public void Project_PropertiesAssignment_ShouldWorkCorrectly()
    {
        // Arrange
        var project = new Project();
        const string nome = "Projeto Teste";
        const string descricao = "Descrição detalhada";
        const int userId = 10;

        // Act
        project.Nome = nome;
        project.Descricao = descricao;
        project.UserId = userId;

        // Assert
        project.Nome.Should().Be(nome);
        project.Descricao.Should().Be(descricao);
        project.UserId.Should().Be(userId);
    }
}