using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRestfulApp_NET_API.Common.Exceptions;
using MyRestfulApp_NET_API.Common.Middlewares;
using Newtonsoft.Json;
using Xunit;

namespace MyRestfulApp_NET_TEST.Middlewares;

public class ErrorResponse
    {
        public string Message { get; set; }
    }

public class CustomExceptionHandlerMiddlewareTest
{
    [Fact]
    public async Task ExceptionMiddlewareExternalApiExceptionTest()
    {
        var middleware = new CustomExceptionHandlerMiddleware((innerHttpContext) => 
        {
            throw new ExternalApiException("External API Error", StatusCodes.Status404NotFound);
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.Invoke(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var objResponse = JsonConvert.DeserializeObject<ErrorResponse>(streamText);

        Assert.Equal("External API Error", objResponse.Message);

        Assert.Equal((int)StatusCodes.Status404NotFound, context.Response.StatusCode);
    }

    [Fact]
    public async Task ExceptionMiddlewareInternalServerErrorExceptionTest()
    {
        var middleware = new CustomExceptionHandlerMiddleware((innerHttpContext) => 
        {
            throw new Exception("Internal API Error");
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.Invoke(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var objResponse = JsonConvert.DeserializeObject<ErrorResponse>(streamText);

        Assert.Equal("Internal Server Error: Internal API Error", objResponse.Message);

        Assert.Equal((int)StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }
}
