using System.Threading.Tasks;
using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.HttpClients.Resources;
using Moq;
using Xunit;
using MyRestfulApp_NET_API.Common.Exceptions;

namespace MyRestfulApp_NET_TEST.Services
{
    public class PaisesServiceTest
    {
        [Fact]
        public async Task PaisesServiceObtenerInformacionPaisTestOk()
        {
            var mockMercadoLibreClient = new Mock<IMercadoLibreClient>();
            var countryCode = "example";
            var expectedResult = new Country();
            mockMercadoLibreClient.Setup(client => client.GetCountryInfo(countryCode))
                .ReturnsAsync(expectedResult);

            var paisesService = new PaisesService(mockMercadoLibreClient.Object);

            var result = await paisesService.ObtenerInformacionPais(countryCode);

            Assert.NotNull(result);
            Assert.IsType<Country>(result);
        }

        [Fact]
        public async Task PaisesServiceObtenerInformacionPaisTestNotFound()
        {
            var mockMercadoLibreClient = new Mock<IMercadoLibreClient>();
            var countryCode = "example";
            mockMercadoLibreClient.Setup(client => client.GetCountryInfo(countryCode))
                .ThrowsAsync(new ExternalApiException("Mercado Libre API HTTP Code NotFound with countryCode example", 404));

            var paisesService = new PaisesService(mockMercadoLibreClient.Object);

            await Assert.ThrowsAsync<ExternalApiException>(() => paisesService.ObtenerInformacionPais(countryCode));
        }
    }
}
