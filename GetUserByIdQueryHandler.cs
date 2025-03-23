using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Features.Users;
using UserManagement.Api.Infrastructure.Persistence;

namespace UserManagement.Api.Features.Users.GetById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly UserDbContext _dbContext;

    public GetUserByIdQueryHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
            return null;

        return new UserDto(
            user.Id,
            user.Email,
            user.Roles,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}
