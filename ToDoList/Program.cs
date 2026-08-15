using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Context;
using ToDoList.Repositories;
using ToDoList.Repositories.Impl;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<ITaskItemRepository, TaskItemRepositoryImpl>();
builder.Services.AddScoped<ITaskService>();

builder.Services.AddDbContext<SQLiteContext>(options => 
options.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();
app.Run();