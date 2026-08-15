using ToDoList.Controllers.Filter;
using ToDoList.Models;

namespace ToDoList.Repositories;
public interface ITaskItemRepository
{
  TaskItem Create(TaskItem request);
  Task<PagedResult<TaskItem>> FindAll(TaskFilterRequest filterRequest);
  Task<TaskItem> FindById(Guid id);
  Task<TaskItem> Update(TaskItem request);
  void Delete(Guid id);
  bool Exists(Guid id);
}