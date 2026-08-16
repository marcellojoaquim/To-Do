using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Services.Impl;

public class UsuarioServiceImpl : IUsuarioService
{

  private readonly IUsuarioRepository _repository;

  public UsuarioServiceImpl(IUsuarioRepository usuarioRepository)
  {
    _repository = usuarioRepository;
  }

  public Usuario Create(Usuario request)
  {
    var usuario = _repository.Create(request);
    return usuario;
  }

  public void Delete(Guid id)
  {
    _repository.Delete(id);
  }

  public async Task<Usuario?> FindById(Guid id)
  {
    var usuario = await _repository.FindById(id);
    return usuario;
  }
}