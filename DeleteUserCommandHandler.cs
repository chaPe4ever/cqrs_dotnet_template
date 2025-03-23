using MediatR;
using UserManagement.Api.Infrastructure.Persistence;

namespace UserManagement.Api.Features.Users.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserDbContext _dbContext;

    public DeleteUserCommandHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (user == null)
            return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
