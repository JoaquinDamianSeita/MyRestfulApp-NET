using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MyRestfulApp_NET_API.Domain.Models;
using MyRestfulApp_NET_API.Domain.Repositories;
using MyRestfulApp_NET_API.Domain.Services.Communication;
using MyRestfulApp_NET_API.Resources;
using MyRestfulApp_NET_API.Services;
using Xunit;

namespace MyRestfulApp_NET_TEST.Services;
public class UserServiceTest
{
    [Fact]
    public async Task UserServiceGetUsersTestOk()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var users = new List<User>
        {
            new() { Id = 1, Name = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new() { Id = 2, Name = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        };
        mockUserRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(users);

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.GetUsers();

        Assert.NotNull(result);
        Assert.IsType<List<UserResource>>(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UserServiceSaveUserTestOk()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var user = new User
        {
            Id = 1, Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = System.Array.Empty<byte>(),
            PasswordSalt = System.Array.Empty<byte>()
        };
        mockUserRepository.Setup(repo => repo.CreateAsync(user));
        
        var userSaveResource = new UserSaveResource
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Password = "password"
        };

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.SaveUser(userSaveResource);

        Assert.NotNull(result);
        Assert.IsType<BasicMessageResponse>(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task UserServiceSaveUserTestError()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        
        var userSaveResource = new UserSaveResource
        {
            Name = "John",
            LastName = "Doe",
            Email = "test@mail.com"
        };

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.SaveUser(userSaveResource);

        Assert.NotNull(result);
        Assert.IsType<BasicMessageResponse>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task UserServiceUpdateUserTestOk()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var user = new User
        {
            Id = 1, Name = "John",
            LastName = "Doe",
            Email = "test@mail.com"
        };

        mockUserRepository.Setup(repo => repo.FindById(user.Id)).ReturnsAsync(user);
        mockUserRepository.Setup(repo => repo.UpdateAsync(user)).ReturnsAsync(user);

        var userUpdateResource = new UserUpdateResource
        {
            Name = "Jane",
            LastName = "Smith",
            Email = "test@mail.com"
        };

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.UpdateUser(user.Id, userUpdateResource);

        Assert.NotNull(result);
        Assert.IsType<UpdateUserMessageResponse>(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task UserServiceUpdateUserTestNotFound()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.FindById(1)).ReturnsAsync((User)null);

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.UpdateUser(1, new UserUpdateResource());

        Assert.NotNull(result);
        Assert.IsType<UpdateUserMessageResponse>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task UserServiceDeleteUserTestOk()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.FindById(1)).ReturnsAsync(new User { Id = 1 });

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.DeleteUser(1);

        Assert.NotNull(result); 
        Assert.IsType<BasicMessageResponse>(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task UserServiceDeleteUserTestNotFound()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.FindById(1)).ReturnsAsync((User)null);

        var userService = new UserService(mockUserRepository.Object);

        var result = await userService.DeleteUser(1);

        Assert.NotNull(result); 
        Assert.IsType<BasicMessageResponse>(result);
        Assert.False(result.Success);
    }
}
