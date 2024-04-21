using MyRestfulApp_NET_API.Resources;

namespace MyRestfulApp_NET_API.Domain.Services;

public interface ICurrencyService
{
    Task<List<CurrencyResource>> GetCurrencies(); 
}
