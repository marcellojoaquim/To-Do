using ToDoList.Models;
using ToDoList.Models.Context;
using ToDoList.Repositories;

public class UsuarioRepositoryImpl : IUsuarioRepository
{

  private SQLiteContext _context;

  public UsuarioRepositoryImpl(SQLiteContext context)
  {
    _context = context;
  }

  public Usuario Create(Usuario request)
  {
    throw new NotImplementedException();
  }

  public void Delete(Guid id)
  {
    throw new NotImplementedException();
  }

  public Task<Usuario> FindById(Guid id)
  {
    throw new NotImplementedException();
  }

  public Task<Usuario> Update(Usuario request)
  {
    throw new NotImplementedException();
  }
}