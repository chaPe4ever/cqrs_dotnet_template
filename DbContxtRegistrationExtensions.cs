// Infrastructure/Persistence/DbContextRegistrationExtensions.cs
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Test.Api.Infrastructure.Persistence;

public static class DbContextRegistrationExtensions
{
    public static IServiceCollection RegisterAllDbContexts(this IServiceCollection services, string connectionString = "ApplicationDb")
    {
        // Find all DbContext types in the assembly
        var dbContextTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DbContext)));
            
        // Register each DbContext
        foreach (var dbContextType in dbContextTypes)
        {
            // Use the generic method to register each DbContext with the correct type
            var method = typeof(DbContextRegistrationExtensions)
                .GetMethod(nameof(RegisterDbContext), BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(dbContextType);
                
            method?.Invoke(null, new object[] { services, connectionString });
        }
        
        return services;
    }
    
    private static IServiceCollection RegisterDbContext<TContext>(IServiceCollection services, string connectionString) 
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => 
            options.UseInMemoryDatabase(connectionString));
            
        return services;
    }
}
