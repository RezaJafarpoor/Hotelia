using Hotelia.Shared.Middlewares;
using Hotelia.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Shared;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddScoped<ExceptionHandlingMiddleware>();
        services.AddScoped<RequestPerformanceMiddleware>();
    }
    

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HoteliaContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("sqlServer"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
    }
}