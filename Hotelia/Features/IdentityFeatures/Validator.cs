using FluentValidation;
using Hotelia.Features.IdentityFeatures.CreateUser;

namespace Hotelia.Features.IdentityFeatures;

public class Validator : AbstractValidator<CreateUserDto>
{
    public Validator()
    {
        RuleFor(c => c.Email).NotNull().EmailAddress().NotEmpty().WithMessage("A valid email is required");
        RuleFor(c => c.Password).NotNull().NotEmpty().WithMessage("password is required");
    }
}