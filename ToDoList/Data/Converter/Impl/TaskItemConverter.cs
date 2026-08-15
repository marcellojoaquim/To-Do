using ToDoList.Data.Converter.Contract;
using ToDoList.Models;

namespace ToDoList.Data.Converter.Impl;

public class TaskItemConverter : IParser<TaskItemRequest, TaskItem>
{
  public TaskItem Parse(TaskItemRequest origin)
  {
    if(origin == null) return null;
    return new TaskItem
    {
      Id = Guid.NewGuid(),
      Title = origin.Title,
      Description = origin.Description,
      Priority = origin.Priority,
      DueDate = origin.DueDate,
      IsCompleted = false,
      CreatedAt = DateTime.Now,
      CompletedAt = null
    };
  }

  public List<TaskItem> Parse(List<TaskItemRequest> origin)
  {
    if(origin == null) return null;
    return origin.Select(item => Parse(item)).ToList();
  }
}
