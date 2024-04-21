using Microsoft.AspNetCore.Mvc;
using Moq;
using MyRestfulApp_NET_API.Controllers;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyRestfulApp_NET_TEST.Controllers;
public class CurrenciesControllerTest
{
    [Fact]
    public async Task CurrenciesControllerGetCurrenciesTestOk()
    {
        var mockCurrencyService = new Mock<ICurrencyService>();
        mockCurrencyService.Setup(service => service.GetCurrencies())
            .ReturnsAsync(new List<CurrencyResource>());

        var controller = new CurrenciesController(mockCurrencyService.Object);

        var result = await controller.GetCurrencies();

        var actionResult = Assert.IsType<ActionResult<List<CurrencyResource>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var users = Assert.IsType<List<CurrencyResource>>(okResult.Value);
        Assert.NotNull(users);
    }
}

