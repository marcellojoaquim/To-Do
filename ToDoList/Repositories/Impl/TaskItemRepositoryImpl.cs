using ToDoList.Models;
using ToDoList.Models.Context;

namespace ToDoList.Repositories.Impl;

public class TaskItemRepositoryImpl : ITaskItemRepository
{

  private SQLiteContext _context;

  public TaskItemRepositoryImpl(SQLiteContext context)
  {
    _context = context;
  }

  public TaskItem Create(TaskItem request)
  {
    _context.TaskItems.Add(request);
    _context.SaveChanges();
    return request;
  }

  public List<TaskItem> FindAll()
  {
    return _context.TaskItems.ToList();
  }

  public async Task<TaskItem?> FindById(Guid id)
  {
    return await _context.TaskItems.FindAsync(id);
  }

  public async Task<TaskItem> Update(TaskItem request)
  {
    var taskEntity = await _context.TaskItems.FindAsync(request.Id);
    if (taskEntity == null) return null;
    try
    {
      _context.Entry(taskEntity).CurrentValues.SetValues(request);
      await _context.SaveChangesAsync();
    }
    catch (Exception)
    {
      throw;
    }
    return taskEntity;
  }

  public void Delete(Guid id)
  {
    var taskEntity = _context.TaskItems.SingleOrDefault(t => t.Id.Equals(id));
    if(taskEntity != null)
    {
    _context.TaskItems.Remove(taskEntity);
    _context.SaveChanges(); 
    }
  }

  public bool Exists(Guid id)
  {
    return _context.TaskItems.Any(t => t.Id.Equals(id));
  }
}