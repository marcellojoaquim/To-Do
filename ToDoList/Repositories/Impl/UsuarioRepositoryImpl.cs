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
    _context.Add(request);
    _context.SaveChanges();
    return request;
  }

  public void Delete(Guid id)
  {
    var result = _context.Set<Usuario>().Find(id);
    _context.Remove(result);
    _context.SaveChanges();
  }

  public async Task<Usuario?> FindById(Guid id)
  {
    return await _context.Usuarios.FindAsync(id);
  }
}