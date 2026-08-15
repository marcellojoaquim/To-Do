using ToDoList.Controllers.Filter;
using ToDoList.Models;

namespace ToDoList.Services;

public interface ITaskService
{
  TaskItem Create(TaskItemRequest request);
  Task<PagedResult<TaskItem>> FindAll(TaskFilterRequest filterRequest);
  Task<TaskItem?> FindById(Guid id);
  Task<TaskItem> Update(Guid id, TaskItemRequest request);
  void Delete(Guid id);
  Task<TaskItem> Concluir(Guid id);
}