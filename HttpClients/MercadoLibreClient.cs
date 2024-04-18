using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
        try
        {
            var getCountryInfoUrl = _configuration.GetValue<string>("HttpClients:MercadoLibre:GetCountryInfo");
            var response = await _client.GetAsync($"{getCountryInfoUrl}/{countryCode}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Country>(jsonString);
            }
            else
            {
                // Handle error response
                return null;
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            return null;
        }
    }
}
