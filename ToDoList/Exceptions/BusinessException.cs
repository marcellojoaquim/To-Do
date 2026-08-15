namespace ToDoList.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {
    }
}