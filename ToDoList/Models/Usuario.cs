namespace ToDoList.Models;

public class Usuario
{
  public Guid Id {get; set;}
  public string Nome {get; set;} 
  public ICollection<TaskItem> Tasks {get; set;} = new List<TaskItem>();
}