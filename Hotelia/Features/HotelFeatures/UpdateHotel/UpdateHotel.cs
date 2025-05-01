using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.ValueObjects;

namespace Hotelia.Features.HotelFeatures.UpdateHotel;


public record UpdateHotelDto(string Name, Address Address, string ImageUrl, List<Room> Rooms);

public class UpdateHotel : IEndpoint
{
    public void RegisterEndpoint(IEndpointRouteBuilder app)
        => app.MapPut("api/hotel", async () =>
        {
            
        });
}