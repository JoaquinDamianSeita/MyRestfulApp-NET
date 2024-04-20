using System.ComponentModel.DataAnnotations;

namespace MyRestfulApp_NET.Domain.Services.Communication;

public class BasicMessageResponse
{
    [Required]
    public bool Success { get; protected set; }
    [Required]
    public string Message { get; protected set; }
    [Required]
    public int ErrorCode { get; protected set; }

    public BasicMessageResponse(bool success, string message, int errorCode)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
    }
}
