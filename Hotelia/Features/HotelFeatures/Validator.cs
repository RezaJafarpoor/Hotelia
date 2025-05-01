using FluentValidation;
using Hotelia.Features.HotelFeatures.CreateHotel;
using Hotelia.Features.HotelFeatures.UpdateHotel;
using Microsoft.Identity.Client;

namespace Hotelia.Features.HotelFeatures;


public sealed class Validator : AbstractValidator<CreateHotelDto>
{
    public Validator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Address.City).NotEmpty().WithMessage("City is required");
        RuleFor(c => c.Address.Province).NotEmpty().WithMessage("Province is required");
        RuleFor(c => c.Address.LocalAddress).NotEmpty().WithMessage("LocalAddress is required");
        RuleFor(c => c.Type).NotEmpty().WithMessage("Type of hotel is required");
    }
}

public class UpdateHotelDtoValidator : AbstractValidator<UpdateHotelDto>
{
    public UpdateHotelDtoValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Address.City).NotEmpty().WithMessage("City is required");
        RuleFor(c => c.Address.Province).NotEmpty().WithMessage("Province is required");
        RuleFor(c => c.Address.LocalAddress).NotEmpty().WithMessage("LocalAddress is required");
    }
}


