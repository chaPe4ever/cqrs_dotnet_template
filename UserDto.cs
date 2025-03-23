namespace UserManagement.Api.Features.Users;

public record UserDto(
    int Id, 
    string Email, 
    List<string> Roles, 
    DateTime CreatedAt, 
    DateTime? UpdatedAt);
