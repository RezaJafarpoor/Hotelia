using FluentValidation;
using FluentValidation.AspNetCore;
using Hotelia.Features.HotelFeatures.CreateHotel;
using Hotelia.Features.HotelFeatures.GetHotel;
using Hotelia.Shared.Common;
using Hotelia.Shared.Middlewares;
using Hotelia.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hotelia;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddScoped<ExceptionHandlingMiddleware>();
        services.AddScoped<RequestPerformanceMiddleware>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation();
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


    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var iEndpoint = typeof(IEndpoint);
        var endpoints = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && iEndpoint.IsAssignableFrom(t));
        
        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IEndpoint;
            instance?.RegisterEndpoint(app);
        }
    }
}