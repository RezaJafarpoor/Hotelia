using Bogus;
using Hotelia.Shared.Domain.Entities;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.Domain.ValueObjects;
using Hotelia.Shared.Persistence;

namespace Hotelia.Shared.Seeds;

public class HotelSeeder(HoteliaContext dbContext) : ISeeder
{
    public async Task SeedAsync()
    {
        
        var hotelFaker = new Faker<Hotel>();
        hotelFaker.RuleFor(h => h.Id, Guid.NewGuid)
            .RuleFor(h => h.Address, f =>
                new Address(f.Address.City(), f.Address.County(), f.Address.SecondaryAddress()))
            .RuleFor(h => h.Name, f => f.Company.CompanyName())
            .RuleFor(h => h.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(h => h.CreatedAt, DateTime.UtcNow)
            .RuleFor(h => h.CreatedBy, "Reza")
            .RuleFor(h => h.HotelType, HotelType.FourStar);
        var options = new List<RoomOption>();
        options.Add(RoomOption.HasTv);
        options.Add(RoomOption.HasAirConditioner);
        options.Add(RoomOption.HasBathRoom);
        options.Add(RoomOption.HasGasOven);
        var roomFaker = new Faker<Room>();
        roomFaker.RuleFor(r => r.Price, f => f.Random.Int(100000, 10000000))
            .RuleFor(r => r.Type, RoomType.KingSizeBed)
            .RuleFor(r => r.RoomOptions, options);
        var rooms = roomFaker.Generate(20);
        var room2 = roomFaker.Generate(20); 
        var hotels = hotelFaker.Generate(2);
        hotels[0].Rooms = rooms;
        hotels[1].Rooms = room2;
        await dbContext.AddRangeAsync(hotels);
    }
}