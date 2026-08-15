using System.Net;
using System.Text.Json;
using ToDoList.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(
                "Erro de negócio: {Message}",
                ex.Message);

            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro não tratado.");

            await HandleGenericExceptionAsync(context);
        }
    }

    private static async Task HandleBusinessExceptionAsync(
        HttpContext context,
        BusinessException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)HttpStatusCode.Conflict,
            message = exception.Message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static async Task HandleGenericExceptionAsync(
        HttpContext context)
    {
        context.Response.StatusCode =
            (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)HttpStatusCode.InternalServerError,
            message = "Ocorreu um erro interno."
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}