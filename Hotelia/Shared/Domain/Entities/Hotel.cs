using Hotelia.Shared.Domain.DDD;
using Hotelia.Shared.Domain.Enums;
using Hotelia.Shared.Domain.ValueObjects;

namespace Hotelia.Shared.Domain.Entities;

public class Hotel : Aggregate<Guid>
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = default!;
    public string ImageUrl { get; set; } = string.Empty;
    public HotelType HotelType { get; set; }
    public List<Room> Rooms { get; set; } = [];

    public Hotel()
    {
        
    }

    private Hotel(string name, Address address, string? imageUrl, HotelType type)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, "name != null");
        ArgumentNullException.ThrowIfNull(address, "address != null");
        ArgumentNullException.ThrowIfNull(type,"type != null");
        Name = name;
        Address = address;
        ImageUrl = imageUrl ?? ImageUrl;
        HotelType = type;
    }

    public static Hotel Create(string name, Address address, string? imageUrl, HotelType type) 
        => new(name, address, imageUrl, type);

    public  void Update(string? name, Address? address, string? imageUrl)
    {
        Name = name ?? Name;
        Address = address ?? Address;
        ImageUrl = imageUrl ?? ImageUrl;
    }

    public void AddRoom(Room room)
        => Rooms.Add(room);

    public void RemoveRoom(Room room)
        => Rooms.Remove(room);
}