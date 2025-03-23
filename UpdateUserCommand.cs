using UserManagement.Api.Common.Interfaces;
using UserManagement.Api.Features.Users;

namespace UserManagement.Api.Features.Users.Update;

public record UpdateUserCommand(int Id, string Email, List<string> Roles) 
    : IBaseCommand<UserDto?>;
