using System.ComponentModel.DataAnnotations;
using MyRestfulApp_NET.HttpClients.Resources;

namespace MyRestfulApp_NET.Resources;

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
