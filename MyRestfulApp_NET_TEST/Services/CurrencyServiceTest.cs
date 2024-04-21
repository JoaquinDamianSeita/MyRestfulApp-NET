using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.HttpClients.Resources;
using Xunit;

namespace MyRestfulApp_NET_TEST.Services
{
    public class CurrencyServiceTest
    {
        [Fact]
        public async Task CurrencyServiceGetCurrenciesTest()
        {
            var mockMercadoLibreClient = new Mock<IMercadoLibreClient>();
            var currencies = new List<Currency>
            {
                new() { Id = "USD", Symbol = "$", Description = "US Dollar", DecimalPlaces = 2 },
                new() { Id = "EUR", Symbol = "€", Description = "Euro", DecimalPlaces = 2 }
            };
            mockMercadoLibreClient.Setup(client => client.GetCurrencies()).ReturnsAsync(currencies);
            mockMercadoLibreClient.Setup(client => client.GetCurrencyConversion("USD", "USD")).ReturnsAsync(new CurrencyConversion { Ratio = 1 });
            mockMercadoLibreClient.Setup(client => client.GetCurrencyConversion("EUR", "USD")).ReturnsAsync(new CurrencyConversion { Ratio = 1.2M });

            var currencyService = new CurrencyService(mockMercadoLibreClient.Object);

            var result = await currencyService.GetCurrencies();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.True(File.Exists("Data/currencies.json"));
            var jsonData = await File.ReadAllTextAsync("Data/currencies.json");
            Assert.Contains("USD", jsonData);

            Assert.True(File.Exists("Data/currency_conversions.csv"));
            var csvData = await File.ReadAllTextAsync("Data/currency_conversions.csv");
            Assert.Contains("1", csvData);
        }
    }
}
