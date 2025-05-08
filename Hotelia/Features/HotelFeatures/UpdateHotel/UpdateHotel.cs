using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.ValueObjects;
using Hotelia.Shared.EndpointFilters;
using Hotelia.Shared.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelia.Features.HotelFeatures.UpdateHotel;


public record UpdateHotelDto(string? Name, Address? Address, string? ImageUrl);

public class UpdateHotel : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPut("api/hotel/{id}", async ([FromQuery]Guid id,[FromBody]UpdateHotelDto dto, HoteliaContext dbContext,CancellationToken cancellationToken) =>
        {
            
            var hotel = await dbContext.Hotels.SingleOrDefaultAsync(h => h.Id == id, cancellationToken);
            if (hotel is null)
                return Results.NotFound();
            hotel.Update(dto.Name,dto.Address,dto.ImageUrl);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.NoContent();

        }).WithName("Update Hotel")
        .Produces<NotFound>(StatusCodes.Status404NotFound)
        .Produces<NoContent>(StatusCodes.Status204NoContent)
        .AddEndpointFilter<LoggingFilter<UpdateHotel>>()
        .AddEndpointFilter<ValidationFilter<UpdateHotelDto>>()
        .WithTags("Hotel");
    
    
}