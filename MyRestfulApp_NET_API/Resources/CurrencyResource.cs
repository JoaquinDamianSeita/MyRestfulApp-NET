using System.ComponentModel.DataAnnotations;

namespace MyRestfulApp_NET_API.Resources;

public class CurrencyResource
{
    [Required]
    public string? Id { get; set; }
    [Required]
    public string? Symbol { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public int DecimalPlaces { get; set; }
    public decimal ToDolar { get; set; }
}
