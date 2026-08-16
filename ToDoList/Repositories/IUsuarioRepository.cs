using ToDoList.Models;

namespace ToDoList.Repositories;
public interface IUsuarioRepository
{
  Usuario Create(Usuario request);
  Task<Usuario> FindById(Guid id);
  void Delete(Guid id);

}