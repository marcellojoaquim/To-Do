using Microsoft.EntityFrameworkCore;
using ToDoList.Controllers.Filter;
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

  public async Task<PagedResult<TaskItem>> FindAll(Guid id, TaskFilterRequest filterRequest)
  {
    IQueryable<TaskItem> query = _context.TaskItems;

    query = query.Where(t => t.UsuarioId == id);

    if (!string.IsNullOrWhiteSpace(filterRequest.Status))
    {
      switch (filterRequest.Status.ToLower())
      {
        case "pending":
          query = query.Where(t => !t.IsCompleted);
          break;
        case "completed":
          query = query.Where(t => t.IsCompleted);
          break;
        case "all":
          break;
        default:
          throw new ArgumentException("Status inválido");
      }
    }

    if (filterRequest.Priority.HasValue)
    {
      query = query.Where(t => t.Priority == filterRequest.Priority.Value);
    }

    query = ApplyOrdering(query, filterRequest);

    var total = await query.CountAsync();

    var items = await query
      .Skip((filterRequest.Page - 1)*filterRequest.PageSize)
      .Take(filterRequest.PageSize)
      .ToListAsync();

    var totalPages = (int)Math.Ceiling((double) total / filterRequest.PageSize);

    return new PagedResult<TaskItem>
    {
      Items = items,
      Page = filterRequest.Page,
      PageSize = filterRequest.PageSize,
      TotalItems = total,
      TotalPages = totalPages
    };
  }

  public async Task<TaskItem?> FindById(Guid userId, Guid id)
  {
    return await _context.TaskItems.FirstOrDefaultAsync(t => t.UsuarioId == userId && t.Id == id);
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

  public void Delete(Guid userId, Guid id)
  {
    var taskEntity = _context.TaskItems.FirstOrDefault(t => t.UsuarioId == userId && t.Id == id);
    if (taskEntity != null)
    {
      _context.TaskItems.Remove(taskEntity);
      _context.SaveChanges();
    }
  }

  public bool Exists(Guid id)
  {
    return _context.TaskItems.Any(t => t.Id.Equals(id));
  }

  private static IQueryable<TaskItem> ApplyOrdering(
        IQueryable<TaskItem> query,
        TaskFilterRequest filter)
  {
    var descending =
        filter.Direction?.ToLower() == "desc";

    return filter.OrderBy?.ToLower() switch
    {
      "duedate" => descending
          ? query.OrderByDescending(x => x.DueDate)
          : query.OrderBy(x => x.DueDate),

      "priority" => descending
          ? query.OrderByDescending(x => x.Priority)
          : query.OrderBy(x => x.Priority),

      "createdat" => descending
          ? query.OrderByDescending(x => x.CreatedAt)
          : query.OrderBy(x => x.CreatedAt),

      _ => query.OrderBy(x => x.CreatedAt)
    };
  }
}