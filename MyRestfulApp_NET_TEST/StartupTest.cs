using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API;
using Xunit;

namespace MyRestfulApp_NET_TEST;

public class StartupTest
{
    [Fact]
    public void ConfigureServices_RegistersDependenciesCorrectly()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);

        var startup = new Startup(configuration);

        startup.ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();
        var paisesService = serviceProvider.GetService<IPaisesService>();
        Assert.NotNull(paisesService);
    }
}
