using Hotelia.Shared.Auth;
using Hotelia.Shared.Auth.Dtos;
using Hotelia.Shared.Common;
using Hotelia.Shared.EndpointFilters;
using Microsoft.AspNetCore.Mvc;

namespace Hotelia.Features.IdentityFeatures.CreateUser;

public record CreateUserDto(string Email, string Password);


public class CreateUser: IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("api/identity/SignUp", async ([FromBody]CreateUserDto dto, AuthService service) =>
        {
            var result = await service.CreateAccount(dto);
            return result.IsSuccessful ?
                Results.Ok(result.Data) :
                Results.BadRequest(result.Errors);
        })
        .WithName("Create User")
        .Produces<AuthResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .AddEndpointFilter<LoggingFilter<CreateUser>>()
        .AddEndpointFilter<ValidationFilter<CreateUserDto>>()
        .WithTags("Identity");
}