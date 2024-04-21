using System.Threading.Tasks;
using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.HttpClients.Resources;
using Moq;
using Xunit;
using MyRestfulApp_NET_API.Common.Exceptions;

namespace MyRestfulApp_NET_TEST.Services
{
    public class BusquedaServiceTest
    {
        [Fact]
        public async Task BusquedaServiceObtenerInformacionTerminoTestOk()
        {
            var mockMercadoLibreClient = new Mock<IMercadoLibreClient>();
            var term = "example";
            var expectedResult = new SearchResult();
            mockMercadoLibreClient.Setup(client => client.GetTermInfo(term))
                .ReturnsAsync(expectedResult);

            var busquedaService = new BusquedaService(mockMercadoLibreClient.Object);

            var result = await busquedaService.ObtenerInformacionTermino(term);

            Assert.NotNull(result);
            Assert.IsType<SearchResult>(result);
        }

        [Fact]
        public async Task BusquedaServiceObtenerInformacionTerminoTestNotFound()
        {
            var mockMercadoLibreClient = new Mock<IMercadoLibreClient>();
            var term = "example";
            mockMercadoLibreClient.Setup(client => client.GetTermInfo(term))
                .ThrowsAsync(new ExternalApiException("Mercado Libre API HTTP Code NotFound with term example", 404));

            var busquedaService = new BusquedaService(mockMercadoLibreClient.Object);

            await Assert.ThrowsAsync<ExternalApiException>(() => busquedaService.ObtenerInformacionTermino(term));
        }
    }
}
