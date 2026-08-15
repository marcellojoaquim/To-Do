using ToDoList.Models;

namespace ToDoList.Services;

public interface ITaskService
{
  TaskItem Create(TaskItemRequest request);
  List<TaskItem> FindAll();
  Task<TaskItem> FindById(Guid id);
  Task<TaskItem> Update(Guid id, TaskItemRequest request);
  void Delete(Guid id);
}