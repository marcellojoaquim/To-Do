using ToDoList.Controllers.Filter;
using ToDoList.Models;

namespace ToDoList.Services;

public interface ITaskService
{
  Task<TaskItem> Create(Guid id, TaskItemRequest request);
  Task<PagedResult<TaskItem>> FindAll(Guid id, TaskFilterRequest filterRequest);
  Task<TaskItem?> FindById(Guid userId, Guid id);
  Task<TaskItem> Update(Guid userId, Guid id, TaskItemRequest request);
  void Delete(Guid userId, Guid id);
  Task<TaskItem> Concluir(Guid userId, Guid id);
}