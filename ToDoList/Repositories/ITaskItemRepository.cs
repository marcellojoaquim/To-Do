using ToDoList.Controllers.Filter;
using ToDoList.Models;

namespace ToDoList.Repositories;
public interface ITaskItemRepository
{
  TaskItem Create(TaskItem request);
  Task<PagedResult<TaskItem>> FindAll(Guid id, TaskFilterRequest filterRequest);
  Task<TaskItem> FindById(Guid userId, Guid id);
  Task<TaskItem> Update(TaskItem request);
  void Delete(Guid userId, Guid id);
  bool Exists(Guid id);
}