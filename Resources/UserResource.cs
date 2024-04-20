using System.ComponentModel.DataAnnotations;

namespace MyRestfulApp_NET.Resources;

public class UserResource
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Email { get; set; }
}