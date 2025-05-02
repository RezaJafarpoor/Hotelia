using Hotelia.Shared.Domain.DDD;
using Hotelia.Shared.Domain.Enums;
using System.ComponentModel;

namespace Hotelia.Shared.Domain.Entities;

public class Room : Entity<Guid>
{
    public int Price { get; set; }
    public RoomType Type { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public List<RoomOption> RoomOptions { get; set; } = new();
    public Guid HotelId { get; set; }
    
    public Room()
    {
        
    }

    private Room(int price, RoomType type, string? imageUrl, List<RoomOption>? roomOptions)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, "Price > 0");
        ArgumentNullException.ThrowIfNull(type, "RoomType != null");
        Price = price;
        Type = type;
        ImageUrl = imageUrl ?? ImageUrl;
        RoomOptions = roomOptions ?? RoomOptions;
    }

    public static Room Create(int price, RoomType type, string? imageUrl, List<RoomOption>? roomOptions)
        => new Room(price, type, imageUrl, roomOptions);


    public void Update(int? price, RoomType? roomType, string? imageUrl,
        List<RoomOption>? roomOptions)
    {
        if (price is null)
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero((int) price!, "Price > 0");
       
        Price = (int)price;
        Type = roomType ?? Type;
        ImageUrl = imageUrl ?? ImageUrl;
        RoomOptions = roomOptions ?? RoomOptions;
    }

    public Room WithOption(RoomOption option)
    {
        if(!RoomOptions.Contains(option)) 
            RoomOptions.Add(option);
        
        return this;
    }
    
    
}