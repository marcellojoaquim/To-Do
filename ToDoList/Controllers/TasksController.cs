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
  public async Task<IActionResult> FindAll([FromQuery] TaskFilterRequest filterRequest)
  {
    var result = await _service.FindAll(filterRequest);
    return Ok(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<TaskItem>> FindById(Guid id)
  {
    var result = await _service.FindById(id);
    return Ok(result);
  }

  [HttpPost]
  public ActionResult<TaskItem> Create([FromHeader(Name = "")]TaskItemRequest request)
  {
    var result = _service.Create(request);
    return CreatedAtAction(nameof(FindById), new {id = result.Id}, result);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<TaskItem>> Update(Guid id, [FromBody] TaskItemRequest request)
  {
    var result = await _service.Update(id, request);
    return Ok(result);
  }

  [HttpPatch("{id}/complete")]
  public IActionResult Concluir(Guid id)
  {
    var result = _service.Concluir(id);
    return Ok(result);
  }

  [HttpDelete("{id}")]
  public IActionResult Delete(Guid id)
  {
    _service.Delete(id);
    return NoContent();
  }
}