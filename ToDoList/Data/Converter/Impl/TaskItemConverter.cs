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
      Title = origin.Title,
      Description = origin.Description,
      Priority = origin.Priority,
      DueDate = origin.DueDate,
    };
  }

  public List<TaskItem> Parse(List<TaskItemRequest> origin)
  {
    if(origin == null) return null;
    return origin.Select(item => Parse(item)).ToList();
  }
}
