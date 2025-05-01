using Hotelia.Shared.Common;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.HotelFeatures.DeleteHotel;

public class DeleteHotel : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapDelete("api/hotel/{id}", async (Guid id, HoteliaContext dbContext, CancellationToken cancellationToken) =>
        {
            var hotel = await dbContext.Hotels.SingleOrDefaultAsync(h => h.Id == id, cancellationToken);
            if (hotel is null)
                return Results.NotFound();
            dbContext.Hotels.Remove(hotel);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        }).WithName("Delete Hotel")
        .Produces<NotFound>(StatusCodes.Status404NotFound)
        .Produces<NoContent>(StatusCodes.Status204NoContent)
        .AddEndpointFilter<LoggingFilter<DeleteHotel>>();
}