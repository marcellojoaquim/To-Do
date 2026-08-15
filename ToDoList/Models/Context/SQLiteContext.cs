using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models.Context;

public class SQLiteContext : DbContext
{
  public SQLiteContext(DbContextOptions<SQLiteContext> options): base(options)
  {    
  }

  public DbSet<TaskItem> TaskItems {get; set;}
  public DbSet<Usuario> Usuarios {get; set;}

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Usuario>(entity =>
    {
      entity.HasKey(t => t.Id);
      entity.HasMany(t => t.Tasks)
      .WithOne(t => t.Usuario)
      .HasForeignKey(t => t.UsuarioId)
      .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<TaskItem>(entity =>
    {
      entity.HasKey(T => T.Id);
    });
  }

}