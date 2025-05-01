using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.Domain.ValueObjects;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.HotelFeatures.GetHotel;

public record HotelDto(string Name, Address Address, string ImageUrl, string Type, List<Room> Rooms)
{
    public static HotelDto MapToDto(Hotel hotel)
    {
        return new HotelDto(hotel.Name, hotel.Address, hotel.ImageUrl, hotel.HotelType.ToString(), hotel.Rooms);
    }
}

public static class GetHotel
{
    public static void GetHotelEndpoint(this IEndpointRouteBuilder app)
        => app.MapGet("api/hotel/{id}", async ([FromQuery] string id, HoteliaContext dbContext) =>
            {
                Guid.TryParse(id, out var hotelId);
                var result = dbContext.Hotels
                    .Include(h => h.Rooms)
                    .SingleOrDefault(h => h.Id == hotelId);
                if (result is null)
                    return Results.NotFound();
                var dto = HotelDto.MapToDto(result);
                return Results.Ok(dto);
            }).WithName("Get Hotel")
            .Produces<HotelDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

}
 



