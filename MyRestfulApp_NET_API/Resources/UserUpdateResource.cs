using System.ComponentModel.DataAnnotations;

namespace MyRestfulApp_NET_API.Resources;

public class UserUpdateResource
{
    [Required]
    [MaxLength(256)]
    public string? Name { get; set; }
    [Required]
    [MaxLength(256)]
    public string? LastName { get; set; }
    [Required]
    [MaxLength(256)]
    public string? Email { get; set; }
}
