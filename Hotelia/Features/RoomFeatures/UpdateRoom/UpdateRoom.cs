using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.RoomFeatures.UpdateRoom;

public record UpdateRoomDto(
    int? Price,
    RoomType? RoomType,
    string? ImageUrl,
    List<RoomOption>? RoomOptions);

public class UpdateRoom : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPut("api/hotel/{hotelId}/room/{roomId}", async ([FromRoute]Guid hotelId,[FromRoute] Guid roomId,[FromBody] UpdateRoomDto dto,
            HoteliaContext dbContext, CancellationToken cancellationToken) =>
        {
            var room = await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId & r.HotelId == hotelId,
                cancellationToken);
            if (room is null)
                return Results.NotFound();
            room.Update(dto.Price, dto.RoomType, dto.ImageUrl, dto.RoomOptions);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        })
        .WithName("Update Room")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .AddEndpointFilter<LoggingFilter<UpdateRoom>>()
        .AddEndpointFilter<ValidationFilter<UpdateRoom>>()
        .WithTags("Room");
}