using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Packt.Shared;

public static class NorthwindContextExtensions
{
    /// <summary>
    ///  Adds NorthwindContext to the specified IServiceCollection. Uses the SqLite database provider.
    /// </summary>
    /// <value></value>
    /// <param name="services"></param>
    /// <param name="relativePath">Set to override the default of ".."</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static IServiceCollection AddNortwindContext(this IServiceCollection services, string relativePath = "..")
    {
        string databasePath = Path.Combine(relativePath, "Northwind.db");

        services.AddDbContext<NorthwindContext>(option =>
        {
            option.UseSqlite($"Data Source={databasePath}");

            option.LogTo(WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        });

        return services;
    }
}