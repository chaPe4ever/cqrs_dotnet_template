using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Infrastructure.Persistence.Models;

namespace UserManagement.Api.Infrastructure.Persistence;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    
    public DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
            
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);
            
        // Configure list of roles as comma-separated string
        modelBuilder.Entity<User>()
            .Property(u => u.Roles)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            
        // Seed initial admin user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@example.com",
                PasswordHash = HashPassword("admin123"),
                Roles = new List<string> { "Admin" },
                CreatedAt = DateTime.UtcNow
            }
        );
    }
    
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
