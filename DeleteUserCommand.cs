using UserManagement.Api.Common.Interfaces;

namespace UserManagement.Api.Features.Users.Delete;

public record DeleteUserCommand(int Id) : IBaseCommand<bool>;
