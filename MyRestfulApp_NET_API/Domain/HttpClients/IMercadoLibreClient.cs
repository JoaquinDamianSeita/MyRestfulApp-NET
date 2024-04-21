using MyRestfulApp_NET_API.HttpClients.Resources;

namespace MyRestfulApp_NET_API.Domain.HttpClients;

public interface IMercadoLibreClient
{
    Task<Country> GetCountryInfo(string countryCode);
    Task<SearchResult> GetTermInfo(string term);
    Task<List<Currency>> GetCurrencies();
    Task<CurrencyConversion> GetCurrencyConversion(string fromCurrency, string toCurrency);
}
