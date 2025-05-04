using FluentValidation;
using FluentValidation.AspNetCore;
using Hotelia.Features.HotelFeatures.CreateHotel;
using Hotelia.Features.HotelFeatures.GetHotel;
using Hotelia.Shared.Auth;
using Hotelia.Shared.Common;
using Hotelia.Shared.Documents;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Middlewares;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Hotelia;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Swagger();
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

    private static void Swagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.UseInlineDefinitionsForEnums();
            option.SchemaFilter<EnumAsStringSchemaFilter>();
        });
    }

    private static void Auth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<HoteliaContext>()
            .AddDefaultTokenProviders();

        services.AddOptions<JwtConfig>().Bind(configuration.GetSection("JwtConfig"));
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
            if (jwtConfig is null)
                throw new SecurityException("Jwt Config is not provided check the appsettings.jeson");
            option.TokenValidationParameters = new TokenValidationParameters
            {
                RoleClaimType = ClaimTypes.Role,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromMinutes(jwtConfig.ExpiryInMinute),
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
            };
        });
    }

}