using Hotelia.Shared.Auth;
using Hotelia.Shared.Auth.Dtos;
using Hotelia.Shared.Common;
using Hotelia.Shared.EndpointFilters;
using Microsoft.AspNetCore.Mvc;

namespace Hotelia.Features.IdentityFeatures.LoginUser;

public record LoginDto(string Email, string Password);

public class LoginUser : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("api/identity/login", async ([FromBody]LoginDto dto, AuthService service) =>
        {
            var response = await service.Login(dto);
            return response.IsSuccessful ?
                Results.Ok(response.Data) :
                Results.BadRequest(response.Errors);
        }).WithName("Login User")
        .Produces<AuthResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .AddEndpointFilter<LoggingFilter<LoginUser>>()
        .AddEndpointFilter<ValidationFilter<LoginDto>>()
        .WithTags("Identity");
}