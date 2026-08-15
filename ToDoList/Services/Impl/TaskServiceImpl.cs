using ToDoList.Data.Converter.Impl;
using ToDoList.Models;
using ToDoList.Repositories.Impl;

namespace ToDoList.Services.Impl;

public class TaskServiceImpl : ITaskService
{

  private readonly TaskItemConverter _converter;
  private readonly TaskItemRepositoryImpl _repository;

  public TaskServiceImpl(TaskItemConverter taskItemConverter, TaskItemRepositoryImpl taskItemRepositoryImpl)
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
    taskEntity.IsCompleted = false;
    taskEntity.CompletedAt = null;

    _repository.Create(taskEntity);

    return taskEntity;
  }

  public void Delete(Guid id)
  {
    if(id == null) throw new ArgumentNullException("Id não deve ser nulo");
    _repository.Delete(id);
  }

  public List<TaskItem> FindAll()
  {
    return _repository.FindAll();
  }

  public async Task<TaskItem> FindById(Guid id)
  {
    if(id == null) throw new ArgumentNullException("Id não deve ser nulo");

    var taskEntity = await _repository.FindById(id);
    return taskEntity;
  }

  public async Task<TaskItem> Update(Guid id, TaskItemRequest request)
  {
    if(request == null || id == null) throw new ArgumentNullException("Id não deve ser nulo");
    var itemEntity = await _repository.FindById(id);
    if(itemEntity == null) throw new KeyNotFoundException("Task não encontrada para o id informado");
    // if(itemEntity.CompletedAt != null && itemEntity.IsCompleted) throw new implementar business exception
    
    itemEntity.Description = request.Description;
    itemEntity.Title = request.Title;
    itemEntity.Priority = request.Priority;
    itemEntity.DueDate = request.DueDate;

    itemEntity = await _repository.Update(itemEntity);
    return itemEntity;
  }
}