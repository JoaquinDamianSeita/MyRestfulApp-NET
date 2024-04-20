using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyRestfulApp_NET.Common;

public class ValidationErrorResponse : IActionResult
{
    [Required]
    public string? Message { get; set; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var result = new BadRequestObjectResult(new { Message });
        await result.ExecuteResultAsync(context);
    }
}
