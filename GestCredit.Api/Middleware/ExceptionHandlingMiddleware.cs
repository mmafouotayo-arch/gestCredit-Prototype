using System.Text.Json;
using GestCredit.Api.Models;

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

            // Le détail complet reste dans les logs du serveur
            _logger.LogError(ex, "Erreur non gérée. CorrelationId: {CorrelationId}", correlationId);

            int statusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                TransitionInvalideException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            // Les erreurs 4xx portent un message métier destiné au client.
            // Une erreur 500 ne doit JAMAIS exposer de détail interne (SQL, chemins, pile d'appels).
            string message = statusCode == StatusCodes.Status500InternalServerError
                ? "Une erreur interne est survenue. Communiquez l'identifiant de corrélation au support."
                : ex.Message;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message, correlationId }));
        }
    }
}