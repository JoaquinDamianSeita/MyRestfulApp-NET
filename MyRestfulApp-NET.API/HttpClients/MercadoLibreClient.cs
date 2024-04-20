using System.Web;
using MyRestfulApp_NET.Common.Exceptions;
using MyRestfulApp_NET.HttpClients.Resources;

namespace MyRestfulApp_NET.Domain.HttpClients;

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

    public async Task<SearchResult> GetTermInfo(string term)
    {
        var getTermInfoUrl = _configuration.GetValue<string>("HttpClients:MercadoLibre:GetTermInfo");
        var uriBuilder = new UriBuilder(getTermInfoUrl);

        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryParams["q"] = term;
        uriBuilder.Query = queryParams.ToString();
        
        var finalUrl = uriBuilder.Uri.ToString().Replace("http://", "");

        var response = await _client.GetAsync(finalUrl);
        
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsAsync<SearchResult>();
        
        throw new ExternalApiException($"Mercado Libre API HTTP Code {response.StatusCode} with term {term}", StatusCodes.Status404NotFound);
    }

    public async Task<List<Currency>> GetCurrencies()
    {
        var getCurrenciesInfoUrl = _configuration.GetValue<string>("HttpClients:MercadoLibre:GetCurrencies");
        var response = await _client.GetAsync($"{getCurrenciesInfoUrl}");

        if (response.IsSuccessStatusCode)
        {
            var currenciesList =  await response.Content.ReadAsAsync<List<Currency>>();
            return currenciesList.OrderBy(c => c.Id).ToList();
        }
        
        throw new ExternalApiException($"Mercado Libre API HTTP Code {response.StatusCode}",  StatusCodes.Status404NotFound);
    }

    public async Task<CurrencyConversion> GetCurrencyConversion(string fromCurrency, string toCurrency)
    {
        var getCurrencyConversionInfoUrl = _configuration.GetValue<string>("HttpClients:MercadoLibre:GetCurrencyConversion");

        var uriBuilder = new UriBuilder(getCurrencyConversionInfoUrl);

        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryParams["from"] = fromCurrency;
        queryParams["to"] = toCurrency;
        uriBuilder.Query = queryParams.ToString();
        
        var finalUrl = uriBuilder.Uri.ToString().Replace("http://", "");

        var response = await _client.GetAsync(finalUrl);
        
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsAsync<CurrencyConversion>();

        return new CurrencyConversion{};
    }
}
