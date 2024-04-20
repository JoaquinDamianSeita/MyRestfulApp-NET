using System.ComponentModel.DataAnnotations;

namespace MyRestfulApp_NET.Resources;

public class UserSaveResource
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
    [Required]
    public string? Password { get; set; }
}