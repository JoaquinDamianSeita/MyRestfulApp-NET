using MyRestfulApp_NET.HttpClients.Resources;

namespace MyRestfulApp_NET.Domain.HttpClients;

public interface IMercadoLibreClient
{
    Task<Country> GetCountryInfo(string countryCode);
    Task<SearchResult> GetTermInfo(string term);
    Task<List<Currency>> GetCurrencies();
    Task<CurrencyConversion> GetCurrencyConversion(string fromCurrency, string toCurrency);
}
