using System.Net;
using System.Text.Json;
using FluentValidation;

namespace WebApi.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var errorResponse = new
        {
            type = "ValidationError",
            title = "Хатогии санҷиши маълумот",
            status = 400,
            errors = errors
        };

        logger.LogWarning("Хатогии валидатсия: {Errors}", 
            JsonSerializer.Serialize(errorResponse));

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        }));
    }

    private async Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            type = "ServerError",
            title = "Хатогии дохилии сервер",
            status = 500,
            message = "Дар сервер хатогӣ ба миён омад. Лутфан баъдтар кӯшиш кунед."
        };

        logger.LogError(exception, "Хатогии номаълум: {Message}", exception.Message);

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        }));
    }
}
