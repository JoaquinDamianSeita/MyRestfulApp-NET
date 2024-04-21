using Microsoft.AspNetCore.Mvc;
using Moq;
using MyRestfulApp_NET_API.Controllers;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.HttpClients.Resources;
using System.Threading.Tasks;
using Xunit;

namespace MyRestfulApp_NET_TEST.Controllers;
public class BusquedaControllerTest
{
    [Fact]
    public async Task BusquedaControllerGetBusquedaTestOk()
    {
        var mockBusquedaService = new Mock<IBusquedaService>();
        mockBusquedaService.Setup(service => service.ObtenerInformacionTermino("test"))
            .ReturnsAsync(new SearchResult());

        var controller = new BusquedaController(mockBusquedaService.Object);

        var result = await controller.GetBusqueda("test");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var searchResult = Assert.IsType<SearchResult>(okResult.Value);
        Assert.NotNull(searchResult);
    }

    [Fact]
    public async Task BusquedaControllerGetBusquedaTestOk2()
    {
        var mockBusquedaService = new Mock<IBusquedaService>();
        var controller = new BusquedaController(mockBusquedaService.Object);

        var result = await controller.GetBusqueda(string.Empty);

        Assert.IsType<OkObjectResult>(result);
    }
}

