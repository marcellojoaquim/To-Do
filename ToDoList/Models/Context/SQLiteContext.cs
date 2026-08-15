using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models.Context;

public class SQLiteContext : DbContext
{
  public SQLiteContext(DbContextOptions<SQLiteContext> options): base(options)
  {    
  }

  public DbSet<TaskItem> TaskItems {get; set;}
}