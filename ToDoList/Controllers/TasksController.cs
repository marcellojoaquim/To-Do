using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Context;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
  private readonly ITaskService _service;

  public TasksController(ITaskService service)
  {
    _service = service;
  }

  [HttpGet]
  public List<TaskItem> FindAll()
  {
    return _service.FindAll();
  }
}