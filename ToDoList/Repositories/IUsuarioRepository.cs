using ToDoList.Models;

namespace ToDoList.Repositories;
public interface IUsuarioRepository
{
  Usuario Create(Usuario request);
  Task<Usuario> FindById(Guid id);
  Task<Usuario> Update(Usuario request);
  void Delete(Guid id);

}