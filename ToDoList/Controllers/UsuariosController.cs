using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
  private readonly IUsuarioService _service;

  public UsuariosController(IUsuarioService service)
  {
    _service = service;
  }


  [HttpGet("{id}")]
  public async Task<IActionResult> FindById(Guid id)
  {
    var usuario = await _service.FindById(id);
    return Ok(usuario);
  }

  [HttpPost]
  public ActionResult<Usuario> Create([FromBody] Usuario usuario)
  {
    var result = _service.Create(usuario);
    return CreatedAtAction(nameof(FindById), new { id = result.Id }, result);
  }

}