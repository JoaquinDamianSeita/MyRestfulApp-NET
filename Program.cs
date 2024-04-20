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

                myDbContext.Database.Migrate();
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
