using Microsoft.EntityFrameworkCore;
using MyRestfulApp_NET.Persistence.Contexts;

namespace MyRestfulApp_NET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost webHost = CreateHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var myDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var configuration = webHost.Services.GetRequiredService<IConfiguration>();

                myDbContext.Database.Migrate();

                DbInitializer.Initialize(myDbContext, configuration);
            }

            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
