using ToDoList.Controllers.Filter;
using ToDoList.Data.Converter.Contract;
using ToDoList.Data.Converter.Impl;
using ToDoList.Exceptions;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoList.Repositories.Impl;

namespace ToDoList.Services.Impl;

public class TaskServiceImpl : ITaskService
{

  private readonly IParser<TaskItemRequest, TaskItem> _converter;
  private readonly ITaskItemRepository _repository;

  public TaskServiceImpl(IParser<TaskItemRequest, TaskItem> taskItemConverter, ITaskItemRepository taskItemRepositoryImpl)
  {
    _converter = taskItemConverter;
    _repository = taskItemRepositoryImpl;
  }

  public TaskItem Create(TaskItemRequest request)
  {
    if(request == null) throw new ArgumentNullException("Task não deve ser nula");

    var taskEntity = _converter.Parse(request);
    taskEntity.Id = Guid.NewGuid();
    taskEntity.CreatedAt = DateTime.UtcNow;
    taskEntity.CompletedAt = null;

    _repository.Create(taskEntity);

    return taskEntity;
  }

  public void Delete(Guid id)
  {
    if(id == Guid.Empty) throw new ArgumentNullException("Id não deve ser nulo");
    _repository.Delete(id);
  }

  public async Task<PagedResult<TaskItem>> FindAll(TaskFilterRequest filterRequest)
  {
    ValidateFilter(filterRequest);
    return await _repository.FindAll(filterRequest);
  }

  public async Task<TaskItem> FindById(Guid id)
  {
    if(id == Guid.Empty) throw new ArgumentNullException("Id não deve ser nulo");

    var taskEntity = await _repository.FindById(id);
    if(taskEntity == null) throw new NotFoundException("Task não encontrada para o ID informado");
    return taskEntity;
  }

  public async Task<TaskItem> Update(Guid id, TaskItemRequest request)
  {
    if(request == null || id == Guid.Empty) throw new ArgumentNullException("Id não deve ser nulo.");
    var itemEntity = await _repository.FindById(id);
    if(itemEntity == null) throw new KeyNotFoundException("Task não encontrada para o id informado.");
    if(itemEntity.CompletedAt != null && itemEntity.IsCompleted) throw new BusinessException("Task já concluída.");
    
    itemEntity.Description = request.Description;
    itemEntity.Title = request.Title;
    itemEntity.Priority = request.Priority;
    itemEntity.DueDate = request.DueDate;

    itemEntity = await _repository.Update(itemEntity);
    return itemEntity;
  }

  public async Task<TaskItem> Concluir(Guid id)
  {
    if(id == Guid.Empty) throw new ArgumentNullException("Id não deve ser nulo");
    var itemEntity = await _repository.FindById(id);
    if(itemEntity == null) throw new KeyNotFoundException("Task não encontrada para o id informado");
    if(itemEntity.IsCompleted) throw new BusinessException("Task já concluída");
    itemEntity.Concluir();
    await _repository.Update(itemEntity);
    return itemEntity;
  }

  private static void ValidateFilter(
        TaskFilterRequest filter)
    {
        if (filter.Page < 1)
            throw new ArgumentException(
                "Page deve ser maior que zero.");

        if (filter.PageSize < 1)
            throw new ArgumentException(
                "PageSize deve ser maior que zero.");

        if (filter.PageSize > 50)
            throw new ArgumentException(
                "PageSize não pode ser maior que 50.");

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var validStatuses = new[]
            {
                "pending",
                "completed",
                "all"
            };

            if (!validStatuses.Contains(
                filter.Status.ToLower()))
            {
                throw new ArgumentException(
                    "Status inválido.");
            }
        }

        if (!string.IsNullOrWhiteSpace(filter.Direction))
        {
            var validDirections = new[]
            {
                "asc",
                "desc"
            };

            if (!validDirections.Contains(
                filter.Direction.ToLower()))
            {
                throw new ArgumentException(
                    "Direction inválido.");
            }
        }
    }
}