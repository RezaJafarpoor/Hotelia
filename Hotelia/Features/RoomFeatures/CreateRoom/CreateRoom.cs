using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.RoomFeatures.CreateRoom;

public record CreateRoomDto(int Price, RoomType RoomType, RoomStatus RoomStatus, string? ImageUrl,List<RoomOption>? RoomOptions);

public class CreateRoom : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("api/hotel/{hotelId}/room", async ([FromRoute] Guid hotelId,[FromBody]CreateRoomDto dto, HoteliaContext dbContext, CancellationToken cancellationToken) =>
        {
            var room = Room.Create(dto.Price, dto.RoomType, dto.ImageUrl, dto.RoomOptions);
            var hotel = await dbContext.Hotels.Include(h => h.Rooms).SingleOrDefaultAsync(h => h.Id == hotelId, cancellationToken);
            if (hotel is null)
                return Results.BadRequest("Hotel Does Not Exist");
            hotel.AddRoom(room);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.Created($"api/hotel/room/{room.Id}", null);
        }).WithName("Create Room")
        .Produces<CreateRoomDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .AddEndpointFilter<LoggingFilter<CreateRoom>>()
        .AddEndpointFilter<ValidationFilter<CreateRoomDto>>();
}