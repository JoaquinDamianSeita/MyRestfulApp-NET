using System.Threading.Tasks;
using MyRestfulApp_NET_API.Domain.Models;
using MyRestfulApp_NET_API.Persistence.Contexts;
using MyRestfulApp_NET_API.Persistence.Repositories;
using Xunit;

namespace MyRestfulApp_NET_TEST.Persistence.Repositories;

public class UserRepositoryFixture
{
    public AppDbContext Context { get; }

    public UserRepositoryFixture()
    {
        // Aquí usamos el DbContext creado con DbContextMocker
        Context = DbContextMocker.GetDbContext("test_database");
    }
}

public class UserRepositoryTest : IClassFixture<UserRepositoryFixture>
{
    private readonly UserRepositoryFixture _fixture;

    public UserRepositoryTest(UserRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UserRepositoryFindByIdOk()
    {
        var userRepository = new UserRepository(_fixture.Context);
        var userId = 1;

        var userToAdd = new User { Id = userId, Name = "Usuario de prueba" };
        _fixture.Context.User.Add(userToAdd);
        await _fixture.Context.SaveChangesAsync();

        var user = await userRepository.FindById(userId);

        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public async Task UserRepositoryCreateAsyncOk()
    {
        var userRepository = new UserRepository(_fixture.Context);
        var userId = 1;

        var userToAdd = new User { Id = userId, Name = "Usuario de prueba" };
        await userRepository.CreateAsync(userToAdd);

        var user = await userRepository.FindById(userId);

        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public async Task UserRepositoryListAsyncOk()
    {
        var userRepository = new UserRepository(_fixture.Context);

        var users = await userRepository.ListAsync();

        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task UserRepositoryDeleteOk()
    {
        var userRepository = new UserRepository(_fixture.Context);
        var userId = 1;

        var userToAdd = new User { Id = userId, Name = "Usuario de prueba" };
        _fixture.Context.User.Add(userToAdd);
        await _fixture.Context.SaveChangesAsync();

        var user = await userRepository.FindById(userId);
        userRepository.Delete(user);

        user = await userRepository.FindById(userId);

        Assert.Null(user);
    }

    [Fact]
    public async Task UserRepositoryUpdateAsyncOk()
    {
        var userRepository = new UserRepository(_fixture.Context);
        var userId = 2;

        var userToAdd = new User { Id = userId, Name = "Usuario de prueba" };
        _fixture.Context.User.Add(userToAdd);
        await _fixture.Context.SaveChangesAsync();

        var user = await userRepository.FindById(userId);
        user.Name = "Usuario de prueba modificado";
        await userRepository.UpdateAsync(user);

        user = await userRepository.FindById(userId);

        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        Assert.Equal("Usuario de prueba modificado", user.Name);
    }
}
