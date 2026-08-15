namespace ToDoList.Controllers.Filter;

public class TaskFilterRequest
{
    public string? Status { get; set; }

    public int? Priority { get; set; }

    public string? OrderBy { get; set; }

    public string? Direction { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}