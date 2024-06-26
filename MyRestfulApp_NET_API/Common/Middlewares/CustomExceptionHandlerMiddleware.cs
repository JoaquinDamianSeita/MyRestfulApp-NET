using MyRestfulApp_NET_API.Common.Exceptions;
using Newtonsoft.Json;

namespace MyRestfulApp_NET_API.Common.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ExternalApiException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Message = ex.Message
            };

            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Message = $"Internal Server Error: {ex.Message}"
            };

            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    private class ErrorResponse
    {
        public string? Message { get; set; }
    }
}
