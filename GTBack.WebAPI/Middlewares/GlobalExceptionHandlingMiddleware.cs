using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public GlobalExceptionHandlingMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
_logger.LogError(e,e.Message);
context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

ProblemDetails problem = new()
{
    Status = (int)HttpStatusCode.InternalServerError,
    Type = e.Message,
    Title = "Server Error",
    Detail = "An Internal Error"
};
string json = JsonSerializer.Serialize(problem);
context.Response.ContentType = "application/json";
await context.Response.WriteAsync(json);
        }
    }
    
}