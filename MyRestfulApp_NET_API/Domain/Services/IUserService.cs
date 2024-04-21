using MyRestfulApp_NET_API.Domain.Services.Communication;
using MyRestfulApp_NET_API.Resources;

namespace MyRestfulApp_NET_API.Domain.Services;

public interface IUserService
{
    Task<List<UserResource>> GetUsers();
    Task<BasicMessageResponse> SaveUser(UserSaveResource userSaveResource);
    Task<UpdateUserMessageResponse> UpdateUser(int userId, UserUpdateResource userUpdateResource);
    Task<BasicMessageResponse> DeleteUser(int userId);
}
