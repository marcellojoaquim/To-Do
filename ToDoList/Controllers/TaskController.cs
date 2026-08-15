using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Context;
using ToDoList.Models;

namespace ToDoList.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
  private readonly SQLiteContext _context;

  public TaskController(SQLiteContext context)
  {
    _context = context;
  }

  [HttpGet]
  public Task<ActionResult<IEnumerable<TaskItem>>> Get()
  {
    return Task.FromResult<ActionResult<IEnumerable<TaskItem>>>(Ok(_context.TaskItems.ToList()));
  }
}