using Microsoft.EntityFrameworkCore;
using MyRestfulApp_NET_API.Domain.Models;
using MyRestfulApp_NET_API.Domain.Repositories;
using MyRestfulApp_NET_API.Persistence.Contexts;

namespace MyRestfulApp_NET_API.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    protected readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindById(int id)
    {
        return await _context.User.FirstOrDefaultAsync(predicate => predicate.Id == id);
    }

    public async Task CreateAsync(User user)
    {
        var newUser = new User
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt
        };

        await _context.User.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> ListAsync()
    {
        return await _context.User.ToListAsync();
    }

    public void Delete(User user)
    {
        _context.User.Remove(user);
        _context.SaveChanges();
    }

    public async Task<User?> UpdateAsync(User user)
    {
        _context.User.Update(user);
        _context.SaveChanges();
        return await _context.User.FirstOrDefaultAsync(predicate => predicate.Id == user.Id);
    }
}
