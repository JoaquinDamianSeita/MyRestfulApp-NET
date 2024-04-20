using MyRestfulApp_NET.Domain.Services.Communication;
using MyRestfulApp_NET.Resources;

namespace MyRestfulApp_NET.Domain.Services;

public interface IUserService
{
    Task<List<UserResource>> GetUsers();
    Task<BasicMessageResponse> SaveUser(UserSaveResource userSaveResource);
    Task<UpdateUserMessageResponse> UpdateUser(int userId, UserUpdateResource userUpdateResource);
    Task<BasicMessageResponse> DeleteUser(int userId);
}
