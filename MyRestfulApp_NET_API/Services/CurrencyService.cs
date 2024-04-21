using System.Text;
using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.Resources;
using Newtonsoft.Json;

namespace MyRestfulApp_NET_API.Domain.Services;

public class CurrencyService : ICurrencyService
{
    private readonly IMercadoLibreClient _mercadoLibreClient;
    private const string JsonFilePath = "Data/currencies.json";
    private const string CsvFilePath = "Data/currency_conversions.csv"; 

    public CurrencyService(IMercadoLibreClient mercadoLibreClient)
    {
        _mercadoLibreClient = mercadoLibreClient;
    }

    public async Task<List<CurrencyResource>> GetCurrencies()
    {
        var currencies = await _mercadoLibreClient.GetCurrencies();

        var currencyTasks = currencies.Select(async c =>
        {
            var currencyResource = new CurrencyResource
            {
                Id = c.Id,
                Symbol = c.Symbol,
                Description = c.Description,
                DecimalPlaces = c.DecimalPlaces
            };

            if (currencyResource.Id != null)
            {
                var conversionTask = _mercadoLibreClient.GetCurrencyConversion(currencyResource.Id, "USD");
                currencyResource.ToDolar = (await conversionTask).Ratio;
            }

            return currencyResource;
        });

        var currenciesResponse = await Task.WhenAll(currencyTasks);
        var currencyConversions = currenciesResponse.Select(c => c.ToDolar).ToList();

        var jsonTask = Task.Run(() => { WriteJson(currenciesResponse); });
        var csvTask = Task.Run(() => { WriteCsv(currencyConversions); });
        await Task.WhenAll(jsonTask, csvTask);

        return currenciesResponse.ToList();
    }

    private static void WriteJson(CurrencyResource[] currenciesResponse)
    {
        var json = JsonConvert.SerializeObject(currenciesResponse);
        File.WriteAllText(JsonFilePath, json);
    }

    private static void WriteCsv(List<decimal> conversions)
    {
        var csv = new StringBuilder();
        csv.Append(string.Join(",", conversions));
        csv.AppendLine();
        File.AppendAllText(CsvFilePath, csv.ToString());
    }
}
