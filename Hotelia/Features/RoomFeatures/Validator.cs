using FluentValidation;
using Hotelia.Features.RoomFeatures.CreateRoom;
using Hotelia.Features.RoomFeatures.UpdateRoom;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.RoomFeatures;

public class Validator : AbstractValidator<CreateRoomDto>
{
    public Validator()
    {
        RuleFor(r => r.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(r => r.RoomType).NotEmpty().WithMessage("Room type is not specified");
        RuleFor(r => r.RoomStatus).NotEmpty().WithMessage("Room status is not specified");
    }
}


public class UpdateRoomValidator : AbstractValidator<UpdateRoomDto>
{
    public UpdateRoomValidator()
    {
        RuleFor(r => r.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than 0");
    }
}