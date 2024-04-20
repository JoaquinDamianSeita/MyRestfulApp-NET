using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MyRestfulApp_NET.Domain.Models;

namespace MyRestfulApp_NET.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get ; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
        User = Set<User>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<User>().HasKey(p => p.Id);
        modelBuilder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(p => p.Name).IsRequired().HasMaxLength(256);
        modelBuilder.Entity<User>().Property(p => p.LastName).IsRequired().HasMaxLength(256);
        modelBuilder.Entity<User>().Property(p => p.Email).IsRequired().HasMaxLength(256);
        modelBuilder.Entity<User>().Property(p => p.PasswordHash).IsRequired();
        modelBuilder.Entity<User>().Property(p => p.PasswordSalt).IsRequired();
        modelBuilder.Entity<User>().Property(p => p.CreatedAt).IsRequired();
        modelBuilder.Entity<User>().Property(p => p.UpdatedAt);
    }
}