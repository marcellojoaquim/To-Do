using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Context;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Controllers.Filter;
using System.Threading.Tasks;

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
  public async Task<IActionResult> FindAll([FromHeader(Name = "X-User-Id")] Guid id, [FromQuery] TaskFilterRequest filterRequest)
  {
    var result = await _service.FindAll(id, filterRequest);
    return Ok(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<TaskItem>> FindById([FromHeader(Name = "X-User-Id")] Guid userId, Guid id)
  {
    var result = await _service.FindById(userId, id);
    return Ok(result);
  }

  [HttpPost]
  public async Task<ActionResult<TaskItemResponse>> Create([FromHeader(Name = "X-User-Id")] Guid id,TaskItemRequest request)
  {
    var result = await _service.Create(id, request);
    var response = new TaskItemResponse
    {
        Id = result.Id,
        Title = result.Title,
        Description = result.Description,
        Priority = result.Priority,
        DueDate = result.DueDate,
        CreatedAt = result.CreatedAt,
        CompletedAt = result.CompletedAt,
        IsCompleted = result.IsCompleted,
        UsuarioId = result.UsuarioId
    };

    return CreatedAtAction(nameof(FindById), new {id = result.Id}, response);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<TaskItem>> Update([FromHeader(Name = "X-User-Id")] Guid userId, Guid id, [FromBody] TaskItemRequest request)
  {
    var result = await _service.Update(userId, id, request);
    return Ok(result);
  }

  [HttpPatch("{id}/complete")]
  public async Task<IActionResult> Concluir([FromHeader(Name = "X-User-Id")] Guid userId, Guid id)
  {
    var result = await _service.Concluir(userId, id);
    return Ok(result);
  }

  [HttpDelete("{id}")]
  public IActionResult Delete([FromHeader(Name = "X-User-Id")] Guid userId, Guid id)
  {
    _service.Delete(userId, id);
    return NoContent();
  }
}