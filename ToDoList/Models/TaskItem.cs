namespace ToDoList.Models;

public class TaskItem
{
  public Guid Id {get; set;}
  public string Title {get; set;} 
  public string? Description {get; set;}
  public int Priority {get; set;}
  public DateTime? DueDate {get; set;} 
  public DateTime CreatedAt {get; set;} 
  public DateTime? CompletedAt {get; set;}
  public bool IsCompleted {get; private set;}
  public Guid UsuarioId {get; set;}
  public Usuario Usuario {get; set;} = null!;

  public void Concluir()
  {
    IsCompleted = true;
    CompletedAt = DateTime.UtcNow;
  }  
  
}