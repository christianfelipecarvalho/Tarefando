using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;
using TaskStatus = Tarefando.Domain.Enums.TaskStatus;
using FluentAssertions;

namespace Tarefando.Tests.Domain.Entities;

public class TaskItemTests
{
    [Fact]
    public void TaskItem_WhenCreated_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var task = new TaskItem();

        // Assert
        task.Id.Should().Be(0);
        task.Titulo.Should().BeEmpty();
        task.Descricao.Should().BeEmpty();
        task.Status.Should().Be(TaskStatus.Pendente);
        task.Priority.Should().Be(TaskPriority.Media);
        task.CriadaEm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        task.ConcluidaEm.Should().BeNull();
        task.History.Should().BeEmpty();
        task.Comments.Should().BeEmpty();
    }

    [Fact]
    public void UpdateStatus_FromPendenteToAndamento_ShouldUpdateStatusAndAddHistory()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Status = TaskStatus.Pendente
        };
        const int userId = 10;

        // Act
        task.UpdateStatus(TaskStatus.EmAndamento, userId);

        // Assert
        task.Status.Should().Be(TaskStatus.EmAndamento);
        task.ConcluidaEm.Should().BeNull();
        task.History.Should().HaveCount(1);
        task.History.First().Alteracao.Should().Contain("Status alterado de Pendente para EmAndamento");
        task.History.First().UserId.Should().Be(userId);
    }

    [Fact]
    public void UpdateStatus_ToConcluida_ShouldSetConcluidaEmAndAddHistory()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Status = TaskStatus.EmAndamento
        };
        const int userId = 10;

        // Act
        task.UpdateStatus(TaskStatus.Concluida, userId);

        // Assert
        task.Status.Should().Be(TaskStatus.Concluida);
        task.ConcluidaEm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        task.History.Should().HaveCount(1);
        task.History.First().Alteracao.Should().Contain("Status alterado de EmAndamento para Concluida");
        task.History.First().UserId.Should().Be(userId);
    }

    [Fact]
    public void UpdateStatus_FromConcluidaToPendente_ShouldNotAffectConcluidaEm()
    {
        // Arrange
        var originalConcluidaEm = DateTime.UtcNow.AddDays(-1);
        var task = new TaskItem
        {
            Id = 1,
            Status = TaskStatus.Concluida,
            ConcluidaEm = originalConcluidaEm
        };
        const int userId = 10;

        // Act
        task.UpdateStatus(TaskStatus.Pendente, userId);

        // Assert
        task.Status.Should().Be(TaskStatus.Pendente);
        task.ConcluidaEm.Should().Be(originalConcluidaEm); // Não deve alterar
        task.History.Should().HaveCount(1);
    }

    [Fact]
    public void UpdateDetails_WithBothChanges_ShouldUpdateBothFieldsAndAddHistory()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Titulo = "Título Original",
            Descricao = "Descrição Original"
        };
        const int userId = 10;
        const string novoTitulo = "Novo Título";
        const string novaDescricao = "Nova Descrição";

        // Act
        task.UpdateDetails(novoTitulo, novaDescricao, userId);

        // Assert
        task.Titulo.Should().Be(novoTitulo);
        task.Descricao.Should().Be(novaDescricao);
        task.History.Should().HaveCount(1);
        task.History.First().Alteracao.Should().Contain("Título alterado");
        task.History.First().Alteracao.Should().Contain("Descrição alterada");
        task.History.First().UserId.Should().Be(userId);
    }

    [Fact]
    public void UpdateDetails_WithOnlyTitleChange_ShouldUpdateOnlyTitleAndAddHistory()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Titulo = "Título Original",
            Descricao = "Descrição Original"
        };
        const int userId = 10;
        const string novoTitulo = "Novo Título";
        const string mesmaDescricao = "Descrição Original";

        // Act
        task.UpdateDetails(novoTitulo, mesmaDescricao, userId);

        // Assert
        task.Titulo.Should().Be(novoTitulo);
        task.Descricao.Should().Be(mesmaDescricao);
        task.History.Should().HaveCount(1);
        task.History.First().Alteracao.Should().Contain("Título alterado");
        task.History.First().Alteracao.Should().NotContain("Descrição alterada");
    }

    [Fact]
    public void UpdateDetails_WithOnlyDescriptionChange_ShouldUpdateOnlyDescriptionAndAddHistory()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Titulo = "Título Original",
            Descricao = "Descrição Original"
        };
        const int userId = 10;
        const string mesmoTitulo = "Título Original";
        const string novaDescricao = "Nova Descrição";

        // Act
        task.UpdateDetails(mesmoTitulo, novaDescricao, userId);

        // Assert
        task.Titulo.Should().Be(mesmoTitulo);
        task.Descricao.Should().Be(novaDescricao);
        task.History.Should().HaveCount(1);
        task.History.First().Alteracao.Should().NotContain("Título alterado");
        task.History.First().Alteracao.Should().Contain("Descrição alterada");
    }

    [Fact]
    public void UpdateDetails_WithNoChanges_ShouldNotAddHistory()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Titulo = "Título Original",
            Descricao = "Descrição Original"
        };
        const int userId = 10;

        // Act
        task.UpdateDetails("Título Original", "Descrição Original", userId);

        // Assert
        task.History.Should().BeEmpty();
    }

    [Fact]
    public void AddComment_ShouldAddCommentAndHistory()
    {
        // Arrange
        var task = new TaskItem { Id = 1 };
        const int userId = 10;
        const string comentario = "Este é um comentário de teste";

        // Act
        task.AddComment(comentario, userId);

        // Assert
        task.Comments.Should().HaveCount(1);
        var comment = task.Comments.First();
        comment.Comentario.Should().Be(comentario);
        comment.UserId.Should().Be(userId);
        comment.CriadoEm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        task.History.Should().HaveCount(1);
        task.History.First().Alteracao.Should().Contain($"Comentário adicionado: {comentario}");
        task.History.First().UserId.Should().Be(userId);
    }

    [Fact]
    public void AddComment_WithEmptyComment_ShouldStillAddCommentAndHistory()
    {
        // Arrange
        var task = new TaskItem { Id = 1 };
        const int userId = 10;
        const string comentarioVazio = "";

        // Act
        task.AddComment(comentarioVazio, userId);

        // Assert
        task.Comments.Should().HaveCount(1);
        task.Comments.First().Comentario.Should().BeEmpty();
        task.History.Should().HaveCount(1);
    }

    [Fact]
    public void AddHistoryEntry_ShouldAddEntryWithCorrectData()
    {
        // Arrange
        var task = new TaskItem { Id = 1 };
        const int userId = 10;
        const string alteracao = "Alteração de teste";

        // Act
        task.AddHistoryEntry(alteracao, userId);

        // Assert
        task.History.Should().HaveCount(1);
        var historyEntry = task.History.First();
        historyEntry.Alteracao.Should().Be(alteracao);
        historyEntry.UserId.Should().Be(userId);
        historyEntry.DataAlteracao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void MultipleOperations_ShouldMaintainHistoryOrder()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Titulo = "Título Original",
            Status = TaskStatus.Pendente
        };
        const int userId = 10;

        // Act
        task.UpdateStatus(TaskStatus.EmAndamento, userId);
        Thread.Sleep(10); // Pequena pausa para garantir ordem temporal
        task.UpdateDetails("Novo Título", "Nova Descrição", userId);
        Thread.Sleep(10);
        task.AddComment("Comentário teste", userId);

        // Assert
        task.History.Should().HaveCount(3);
        task.History.Should().BeInAscendingOrder(h => h.DataAlteracao);

        task.History.ElementAt(0).Alteracao.Should().Contain("Status alterado");
        task.History.ElementAt(1).Alteracao.Should().Contain("Título alterado");
        task.History.ElementAt(2).Alteracao.Should().Contain("Comentário adicionado");
    }

    [Theory]
    [InlineData(TaskStatus.Pendente)]
    [InlineData(TaskStatus.EmAndamento)]
    [InlineData(TaskStatus.Concluida)]
    public void UpdateStatus_WithDifferentStatuses_ShouldWork(TaskStatus newStatus)
    {
        // Arrange
        var task = new TaskItem { Status = TaskStatus.Pendente };
        const int userId = 10;

        // Act
        task.UpdateStatus(newStatus, userId);

        // Assert
        task.Status.Should().Be(newStatus);
        task.History.Should().HaveCount(1);

        if (newStatus == TaskStatus.Concluida)
        {
            task.ConcluidaEm.Should().NotBeNull();
        }
    }

    [Fact]
    public void TaskItem_WithNullCollections_ShouldInitializeEmptyCollections()
    {
        // Arrange & Act
        var task = new TaskItem();

        // Assert
        task.History.Should().NotBeNull();
        task.Comments.Should().NotBeNull();
        task.History.Should().BeEmpty();
        task.Comments.Should().BeEmpty();
    }
}