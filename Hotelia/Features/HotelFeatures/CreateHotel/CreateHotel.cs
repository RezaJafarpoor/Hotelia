using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.Domain.ValueObjects;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hotelia.Features.HotelFeatures.CreateHotel;

public record CreateHotelDto(string Name, Address Address, string ImageUrl, HotelType HotelType);


public  class CreateHotel :IEndpoint
{
    public  void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("api/hotel",
                async ([FromBody] CreateHotelDto dto, HoteliaContext dbContext, CancellationToken cancellationToken) =>
                {
                    var hotel = Hotel.Create(dto.Name, dto.Address, dto.ImageUrl, dto.HotelType);
                    dbContext.Hotels.Add(hotel);
                    return await dbContext.SaveChangesAsync(cancellationToken) > 0
                        ? Results.Created($"api/hotel/{hotel.Id}", null)
                        : Results.BadRequest();
                }).WithName("Create Hotel")
            .Produces<Created>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<LoggingFilter<CreateHotelDto>>()
            .AddEndpointFilter<ValidationFilter<CreateHotelDto>>()
            .WithTags("Hotel");
}