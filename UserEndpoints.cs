using FluentValidation;
using MediatR;
using UserManagement.Api.Features.Users.Create;
using UserManagement.Api.Features.Users.Delete;
using UserManagement.Api.Features.Users.GetAll;
using UserManagement.Api.Features.Users.GetById;
using UserManagement.Api.Features.Users.Update;

namespace UserManagement.Api;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
    {
        // Create
        group.MapPost("/", async (CreateUserCommand command, IMediator mediator) =>
        {
            try
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/users/{result.Id}", result);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    ));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // Get by ID
        group.MapGet("/{id}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserByIdQuery(id));
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        // Get all
        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllUsersQuery());
            return Results.Ok(result);
        });

        // Update
        group.MapPut("/{id}", async (int id, UpdateUserCommand command, IMediator mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            try
            {
                var result = await mediator.Send(command);
                return result != null ? Results.Ok(result) : Results.NotFound();
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    ));
            }
        });

        // Delete
        group.MapDelete("/{id}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteUserCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        });

        return group;
    }
}
