using Microsoft.AspNetCore.Mvc;
using Moq;
using MyRestfulApp_NET_API.Controllers;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Domain.Services.Communication;
using MyRestfulApp_NET_API.HttpClients.Resources;
using System.Threading.Tasks;
using Xunit;

namespace MyRestfulApp_NET_TEST.Controllers;

public class PaisesControllerTest
{
    [Fact]
    public async Task PaisesControllerGetPaisTestOk()
    {
        var pais = "USA";
        var mockPaisesService = new Mock<IPaisesService>();
        mockPaisesService.Setup(service => service.ObtenerInformacionPais(pais))
            .ReturnsAsync(new Country());

        var controller = new PaisesController(mockPaisesService.Object);

        var result = await controller.GetPais(pais);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var countryInfo = Assert.IsType<Country>(okResult.Value);
        Assert.NotNull(countryInfo);
    }

    [Fact]
    public async Task PaisesControllerGetPaisTestUnauthorized()
    {
        var pais = "BR";
        var mockPaisesService = new Mock<IPaisesService>();
        mockPaisesService.Setup(service => service.ObtenerInformacionPais(pais))
            .ReturnsAsync(new Country());

        var controller = new PaisesController(mockPaisesService.Object);

        var result = await controller.GetPais(pais);

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var basicMessageResponse = Assert.IsType<BasicMessageResponse>(unauthorizedResult.Value);
        Assert.Equal(1, basicMessageResponse.ErrorCode);
    }
}
