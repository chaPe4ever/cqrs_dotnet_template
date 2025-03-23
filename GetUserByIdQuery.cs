using UserManagement.Api.Common.Interfaces;
using UserManagement.Api.Features.Users;

namespace UserManagement.Api.Features.Users.GetById;

public record GetUserByIdQuery(int Id) : IBaseQuery<UserDto?>;
