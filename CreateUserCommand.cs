using UserManagement.Api.Common.Interfaces;
using UserManagement.Api.Features.Users;

namespace UserManagement.Api.Features.Users.Create;

public record CreateUserCommand(string Email, string Password, List<string> Roles) 
    : IBaseCommand<UserDto>;
