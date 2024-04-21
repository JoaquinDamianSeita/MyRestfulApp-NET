using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRestfulApp_NET_API.Common.Exceptions;
using MyRestfulApp_NET_API.Common.Middlewares;
using Newtonsoft.Json;
using Xunit;
using Moq;

namespace MyRestfulApp_NET_TEST.Middlewares;

public class ErrorMessage
{
    public string Message { get; set; }
}

public class CustomExceptionHandlerMiddlewareTest
{
    [Fact]
    public async Task ExceptionMiddlewareExternalApiException()
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
        var objResponse = JsonConvert.DeserializeObject<ErrorMessage>(streamText);

        Assert.Equal("External API Error", objResponse.Message);

        Assert.Equal((int)StatusCodes.Status404NotFound, context.Response.StatusCode);
    }
}
