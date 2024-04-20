using MyRestfulApp_NET.Domain.Models;

namespace MyRestfulApp_NET.Domain.Repositories;

public interface IUserRepository
{
    Task<List<User>> ListAsync();
    Task<User?> FindById(int id);
    Task CreateAsync(User user);
    Task<User?> UpdateAsync(User user);
    void Delete(User user);
}