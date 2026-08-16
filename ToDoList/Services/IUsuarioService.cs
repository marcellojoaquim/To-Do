
using ToDoList.Models;

namespace ToDoList.Services;
public interface IUsuarioService
{
  Usuario Create(Usuario request);
  Task<Usuario?> FindById(Guid id);
  void Delete(Guid id);
}