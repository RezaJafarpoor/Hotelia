using FluentValidation;
using Hotelia.Features.IdentityFeatures.CreateUser;
using Hotelia.Features.IdentityFeatures.LoginUser;
using Microsoft.Identity.Client;

namespace Hotelia.Features.IdentityFeatures;

public class Validator : AbstractValidator<CreateUserDto>
{
    public Validator()
    {
        RuleFor(c => c.Email).NotNull().EmailAddress().NotEmpty().WithMessage("A valid email is required");
        RuleFor(c => c.Password).NotNull().NotEmpty().WithMessage("password is required");
    }
}


public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.Email).EmailAddress().WithMessage("A valid email is required");
        RuleFor(l => l.Password).NotEmpty().NotNull().WithMessage("password is required");
    }
}