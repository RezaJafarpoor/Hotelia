using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.RoomFeatures.GetRoom;

public record RoomDto(
    int Price,
    RoomType RoomType,
    RoomStatus RoomStatus,
    string? ImageUrl,
    List<RoomOption>? RoomOptions);

public class GetRoom : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapGet("api/hotel/{hotelId}/room/{roomId}", async ([FromRoute] Guid hotelId, [FromRoute] Guid roomId,
                HoteliaContext dbContext, CancellationToken cancellationToken) =>
            {
                var room = await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId & r.HotelId == hotelId,
                    cancellationToken);
                if (room is null)
                    return Results.NotFound();
                var dto = MapToRoomDto(room);
                return Results.Ok(dto);
            }).WithName("Get Room")
            .Produces<RoomDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .AddEndpointFilter<LoggingFilter<GetRoom>>()
            .AddEndpointFilter<ValidationFilter<GetRoom>>();
    
    private  RoomDto MapToRoomDto(Room room) =>
        new(room.Price, room.Type, room.Status, room.ImageUrl, room.RoomOptions);
        
    
}