using UserManagement.Api.Common.Interfaces;
using UserManagement.Api.Features.Users;

namespace UserManagement.Api.Features.Users.GetAll;

public record GetAllUsersQuery : IBaseQuery<List<UserDto>>;
