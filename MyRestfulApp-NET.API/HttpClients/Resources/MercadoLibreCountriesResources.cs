namespace MyRestfulApp_NET.HttpClients.Resources;

public class Country
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Locale { get; set; }
    public string? CurrencyId { get; set; }
    public string? DecimalSeparator { get; set; }
    public string? ThousandsSeparator { get; set; }
    public string? TimeZone { get; set; }
    public GeoInformation? GeoInformation { get; set; }
    public List<State>? States { get; set; }
}

public class GeoInformation
{
    public Location? Location { get; set; }
}

public class Location
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public class State
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}
