using MyRestfulApp_NET_API.Common;
using MyRestfulApp_NET_API.Domain.Models;
using MyRestfulApp_NET_API.Domain.Repositories;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Domain.Services.Communication;
using MyRestfulApp_NET_API.Resources;

namespace MyRestfulApp_NET_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResource>> GetUsers()
    {
        var users = await _userRepository.ListAsync();

        return users.Select(user => new UserResource
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email
        }).ToList();
    }

    public async Task<BasicMessageResponse> SaveUser(UserSaveResource userSaveResource)
    {
        if (userSaveResource.Password == null)
            return new BasicMessageResponse(false, "Password is required", 1);

        CreatePasswordHashHelper.CreatePasswordHash(userSaveResource.Password, out byte[] passwordHash, out byte[] passwordSalt);
        
        var user = new User
        {
            Name = userSaveResource.Name,
            LastName = userSaveResource.LastName,
            Email = userSaveResource.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _userRepository.CreateAsync(user);

        return new BasicMessageResponse(true, "User created successfully", 0);
    }

    public async Task<UpdateUserMessageResponse> UpdateUser(int userId, UserUpdateResource userUpdateResource)
    {
        var user = await _userRepository.FindById(userId);

        if (user == null)
            return new UpdateUserMessageResponse(false, "User not found.", 1, new UserResource { });
        
        user.Name = userUpdateResource.Name;
        user.LastName = userUpdateResource.LastName;
        user.Email = userUpdateResource.Email;

        var userUpdated = await _userRepository.UpdateAsync(user);

        if (userUpdated == null)
            return new UpdateUserMessageResponse(false, "An error occurred while updating the user.", 2, new UserResource { });

        var userUpdatedResource = new UserResource
        {
            Id = userUpdated.Id,
            Name = userUpdated.Name,
            LastName = userUpdated.LastName,
            Email = userUpdated.Email
        };

        return new UpdateUserMessageResponse(true, "User updated successfully!", 0, userUpdatedResource);
    }

    public async Task<BasicMessageResponse> DeleteUser(int userId)
    {
        var user = await _userRepository.FindById(userId);

        if (user == null)
            return new BasicMessageResponse(false, "User not found.", 1);

        _userRepository.Delete(user);

        return new BasicMessageResponse(true, "User deleted successfully!", 0);
    }
}
