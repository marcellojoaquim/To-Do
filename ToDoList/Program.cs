using Microsoft.EntityFrameworkCore;
using ToDoList.Data.Converter.Contract;
using ToDoList.Data.Converter.Impl;
using ToDoList.Models;
using ToDoList.Models.Context;
using ToDoList.Repositories;
using ToDoList.Repositories.Impl;
using ToDoList.Services;
using ToDoList.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<ITaskItemRepository, TaskItemRepositoryImpl>();
builder.Services.AddScoped<ITaskService, TaskServiceImpl>();
builder.Services.AddScoped<IParser<TaskItemRequest, TaskItem>, TaskItemConverter>();

builder.Services.AddDbContext<SQLiteContext>(options => 
options.UseSqlite(
    "Data Source=todo.db"
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();