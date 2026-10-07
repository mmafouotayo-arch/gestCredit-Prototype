using System.Text.Json;

namespace GestCredit.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            string correlationId = Guid.NewGuid().ToString();

            _logger.LogError(ex, "Erreur non gérée. CorrelationId: {CorrelationId}", correlationId);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var payload = new
            {
                error = ex.Message,
                correlationId
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}