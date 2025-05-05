using Hotelia.Shared.Common;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.RoomFeatures.DeleteRoom;

public class DeleteRoom : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app) 
        => app.MapDelete("api/hotel/{hotelId}/room/{roomId}",
            async ([FromRoute] Guid hotelId, [FromRoute] Guid roomId, HoteliaContext dbContext, CancellationToken cancellationToken) =>
            {
                var room = await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId & r.HotelId == hotelId,
                    cancellationToken);
                if (room is null)
                    return Results.NotFound();
                dbContext.Rooms.Remove(room);
                await dbContext.SaveChangesAsync(cancellationToken);
                return Results.NoContent();
            }).WithName("Delete Room")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<LoggingFilter<DeleteRoom>>()
            .WithTags("Room");
}