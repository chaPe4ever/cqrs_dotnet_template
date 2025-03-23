using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api;
using UserManagement.Api.Common.Behaviors;
using UserManagement.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.RegisterAllDbContexts();

// MediatR with validation
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed the database (for InMemory)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // Create all DbContexts automatically
    var dbContextTypes = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DbContext)));
        
    foreach (var contextType in dbContextTypes)
    {
        // Get the context instance and ensure database is created
        var context = services.GetService(contextType) as DbContext;
        context?.Database.EnsureCreated();
    }
}

// Endpoints
app.MapGroup("/api/users")
    .MapUsersEndpoints()
    .WithTags("Users")
    .WithOpenApi();

app.Run();
