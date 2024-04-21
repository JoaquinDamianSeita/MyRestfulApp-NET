using MyRestfulApp_NET_API.Common;
using MyRestfulApp_NET_API.Domain.Models;

namespace MyRestfulApp_NET_API.Persistence.Contexts
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context, IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("IsLocalEnvironment") || context.User.Any())
              return;

            CreatePasswordHashHelper.CreatePasswordHash("password", out byte[] passwordHash, out byte[] passwordSalt);

            var users = new User[]
            {
                new() {
                    Name = "Jose",
                    LastName = "Martinez",
                    Email = "jose.martinez@mail.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new() {
                    Name = "Maria",
                    LastName = "Gonzalez",
                    Email = "maria.gonzalez@mail.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new() {
                    Name = "Carlos",
                    LastName = "Perez",
                    Email = "carlos.perez@mail.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            context.User.AddRange(users);
            context.SaveChanges();
        }
    }
}
