using Microsoft.EntityFrameworkCore;
using MyRestfulApp_NET_API.Persistence.Contexts;

namespace MyRestfulApp_NET_TEST.Persistence;

public static class DbContextMocker
{
    public static AppDbContext GetDbContext(string name)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: name, b => b.EnableNullChecks(false))
            .Options;

        var dbContext = new AppDbContext(options);

        return dbContext;
    }
}
