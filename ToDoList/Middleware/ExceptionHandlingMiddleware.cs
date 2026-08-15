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

      await HandleExceptionAsync(
        context,
        HttpStatusCode.Conflict,
        ex.Message
      );
    }
    catch (NotFoundException ex)
    {
      _logger.LogWarning("Recurso não encontrado Exception");
      await HandleExceptionAsync(
        context,
        HttpStatusCode.NotFound,
        ex.Message
      );
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning("Recurso não encontrado Exception");
      await HandleExceptionAsync(
        context,
        HttpStatusCode.BadRequest,
        ex.Message
      );
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning("Erro no parametro");
      await HandleExceptionAsync(
        context,
        HttpStatusCode.BadRequest,
        ex.Message
      );
    }
    catch (Exception ex)
    {
      _logger.LogError(
          ex,
          "Erro não tratado.");

      await HandleExceptionAsync(
        context,
        HttpStatusCode.BadRequest,
        ex.Message);
    }
  }

  private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}