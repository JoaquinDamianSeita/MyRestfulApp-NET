using MyRestfulApp_NET.Resources;

namespace MyRestfulApp_NET.Domain.Services;

public interface ICurrencyService
{
    Task<List<CurrencyResource>> GetCurrencies(); 
}
