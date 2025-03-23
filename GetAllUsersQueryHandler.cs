using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Features.Users;
using UserManagement.Api.Infrastructure.Persistence;

namespace UserManagement.Api.Features.Users.GetAll;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly UserDbContext _dbContext;

    public GetAllUsersQueryHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Select(u => new UserDto(
                u.Id,
                u.Email,
                u.Roles,
                u.CreatedAt,
                u.UpdatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}
