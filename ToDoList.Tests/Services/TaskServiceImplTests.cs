using Moq;
using ToDoList.Data.Converter.Contract;
using ToDoList.Exceptions;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoList.Services.Impl;

namespace ToDoList.Tests.Services;

public class TaskServiceImplTests
{
  private readonly Mock<IParser<TaskItemRequest, TaskItem>> _converterMock;
  private readonly Mock<ITaskItemRepository> _repositoryMock;

  private readonly TaskServiceImpl _service;

  public TaskServiceImplTests()
  {
    _converterMock = new Mock<IParser<TaskItemRequest, TaskItem>>();
    _repositoryMock = new Mock<ITaskItemRepository>();

    _service = new TaskServiceImpl(
        _converterMock.Object,
        _repositoryMock.Object);
  }

  [Fact]
  public void Create_DeveLancarException_QuandoEnviarNulo()
  {
    TaskItemRequest request = null;

    var result = () => _service.Create(request);

    Assert.Throws<ArgumentNullException>(result);
  }

  [Fact]
  public void Create_DeveCriarTaskComSucesso_QuandoEnviadoDadosValidos()
  {
    var request = new TaskItemRequest
    {
      Title = "Estudar c#",
      Description = "Fazer exercicios de LINQ",
      Priority = 2,
      DueDate = DateTime.Parse("2025-12-10T00:00:00")
    };

    var entity = new TaskItem
    {
      Title = request.Title,
      Description = request.Description,
      Priority = request.Priority
    };

    _converterMock
        .Setup(x => x.Parse(request))
        .Returns(entity);

    var result = _service.Create(request);

    Assert.NotEqual(Guid.Empty, result.Id);
    Assert.NotEqual(default, result.CreatedAt);
    Assert.Null(result.CompletedAt);

    Assert.Equal(request.Title, result.Title);
    Assert.Equal(request.Description, result.Description);
    Assert.Equal(request.Priority, result.Priority);

    _repositoryMock.Verify(
        x => x.Create(entity),
        Times.Once);
  }

  [Fact]
  public async Task Update_DeveLancarBusinessException_QuandoATaskJaEstiverConcluida()
  {
    var id = Guid.NewGuid();

    var task = new TaskItem
    {
      Id = id,
      Title = "Task antiga",
      CompletedAt = DateTime.UtcNow
    };

    task.Concluir();

    var request = new TaskItemRequest
    {
      Title = "Novo título"
    };

    _repositoryMock
        .Setup(x => x.FindById(id))
        .ReturnsAsync(task);

    var result = () => _service.Update(id, request);

    var exception =
        await Assert.ThrowsAsync<BusinessException>(result);

    Assert.Equal("Task já concluída.", exception.Message);
  }

  [Fact]
  public async Task Update_DeveAtualizarATask_QuandoATaskForValida()
  {
    var id = Guid.NewGuid();

    var task = new TaskItem
    {
      Id = id,
      Title = "Título antigo",
      Description = "Descrição antiga",
    };

    var request = new TaskItemRequest
    {
      Title = "Título novo",
      Description = "Descrição nova",
      Priority = 2
    };

    _repositoryMock
        .Setup(x => x.FindById(id))
        .ReturnsAsync(task);

    _repositoryMock
        .Setup(x => x.Update(It.IsAny<TaskItem>()))
        .ReturnsAsync(task);

    var result = await _service.Update(id, request);

    Assert.Equal("Título novo", result.Title);
    Assert.Equal("Descrição nova", result.Description);
    Assert.Equal(2, result.Priority);

    _repositoryMock.Verify(
        x => x.Update(task),
        Times.Once);
  }

  [Fact]
  public async Task Concluir_DeveLancarArgumentNullException_QuandoIdForNulo()
  {
    var id = Guid.Empty;

    var result = () => _service.Concluir(id);

    await Assert.ThrowsAsync<ArgumentNullException>(result);
  }

  [Fact]
  public async Task Concluir_DeveLancarKeyNotFoundException_QuandoNaoEncontrarTaskParaOIdInformado()
  {
    var id = Guid.NewGuid();

    _repositoryMock
        .Setup(x => x.FindById(id))
        .ReturnsAsync((TaskItem?)null);

    var result = () => _service.Concluir(id);

    await Assert.ThrowsAsync<KeyNotFoundException>(result);
  }

  [Fact]
  public async Task Concluir_DeveLancarBusinessException_QuandoATaskJaEstiverCompleta()
  {
    var id = Guid.NewGuid();

    var task = new TaskItem
    {
      Id = id,
      CompletedAt = DateTime.UtcNow
    };

    task.Concluir();

    _repositoryMock
        .Setup(x => x.FindById(id))
        .ReturnsAsync(task);

    var result = () => _service.Concluir(id);

    var exception =
        await Assert.ThrowsAsync<BusinessException>(result);

    Assert.Equal("Task já concluída", exception.Message);
  }

  [Fact]
  public async Task Concluir_DeveConcluirATask_QuandoTaskEstiverPendente()
  {
    var id = Guid.NewGuid();

    var task = new TaskItem
    {
      Id = id,
      CompletedAt = null
    };

    _repositoryMock
        .Setup(x => x.FindById(id))
        .ReturnsAsync(task);

    var result = await _service.Concluir(id);

    Assert.True(result.IsCompleted);
    Assert.NotNull(result.CompletedAt);
  }
}