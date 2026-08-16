using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

public class TaskItemRequest
{

  [Required]
  [StringLength(80, MinimumLength = 3)]
  public string Title {get; set;} = string.Empty;

  [StringLength(400)]
  public string? Description {get; set;}

  [Range(1, 3)]
  public int Priority {get; set;}

  public DateTime? DueDate {get; set;} 
  
}