using MediatR;
using UserManagement.Api.Features.Users;
using UserManagement.Api.Infrastructure.Persistence;

namespace UserManagement.Api.Features.Users.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto?>
{
    private readonly UserDbContext _dbContext;

    public UpdateUserCommandHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (user == null)
            return null;

        user.Email = request.Email;
        user.Roles = request.Roles;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserDto(
            user.Id,
            user.Email,
            user.Roles,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}
