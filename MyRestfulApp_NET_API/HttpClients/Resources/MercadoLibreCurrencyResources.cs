using Newtonsoft.Json;

namespace MyRestfulApp_NET_API.HttpClients.Resources;

public class Currency
{
    public string? Id { get; set; }
    public string? Symbol { get; set; }
    public string? Description { get; set; }

    [JsonProperty(propertyName: "decimal_places")]
    public int DecimalPlaces { get; set; }
}

public class CurrencyConversion
{
    public decimal Ratio { get; set; }
}
