namespace MyRestfulApp_NET.Common.Exceptions;

public class ExternalApiException : Exception
{
    public ExternalApiException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
    public int StatusCode { get; }
}
