using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Features.Users;
using UserManagement.Api.Infrastructure.Persistence;
using UserManagement.Api.Infrastructure.Persistence.Models;

namespace UserManagement.Api.Features.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly UserDbContext _dbContext;

    public CreateUserCommandHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new Exception($"User with email {request.Email} already exists");
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            Roles = request.Roles,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserDto(
            user.Id,
            user.Email,
            user.Roles,
            user.CreatedAt,
            user.UpdatedAt
        );
    }

    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
