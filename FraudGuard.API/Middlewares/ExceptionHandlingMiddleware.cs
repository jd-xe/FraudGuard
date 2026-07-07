using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FraudGuard.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Intenta ejecutar el flujo normal de la petición
            await _next(context);
        }
        catch (Exception)
        {
            // Si cualquier regla, repositorio o controlador explota, lo capturamos aquí
            // Respuesta basada en el estándar moderno RFC 9457
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = "Ocurrió un problema inesperado al procesar la transacción.",
                Instance = context.Request.Path,
                Type = "about:blank"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}