using MyRestfulApp_NET.Common.Exceptions;

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

public interface IMercadoLibreClient
{
    Task<Country> GetCountryInfo(string countryCode);
}

public class MercadoLibreClient : IMercadoLibreClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public MercadoLibreClient(HttpClient client, IConfiguration configuration)
    {
        _configuration = configuration;
        client.BaseAddress = new Uri(_configuration.GetValue<string>("HttpClients:MercadoLibre:Url"));
        client.Timeout = TimeSpan.FromSeconds(60);
        _client = client;
    }

    public async Task<Country> GetCountryInfo(string countryCode)
    {
        var getCountryInfoUrl = _configuration.GetValue<string>("HttpClients:MercadoLibre:GetCountryInfo");
        var response = await _client.GetAsync($"{getCountryInfoUrl}/{countryCode}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsAsync<Country>();
        
        throw new ExternalApiException($"Mercado Libre API HTTP Code {response.StatusCode} with country code {countryCode}",  StatusCodes.Status404NotFound);
    }
}
