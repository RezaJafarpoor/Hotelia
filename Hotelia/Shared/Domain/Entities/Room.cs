using Hotelia.Shared.Domain.DDD;
using Hotelia.Shared.Domain.Enums;

namespace Hotelia.Shared.Domain.Entities;

public class Room : Entity<Guid>
{
    public int Price { get; set; }
    public RoomType Type { get; set; }
    public RoomStatus Status { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public List<RoomOption> RoomOptions { get; set; } = new();
    public Guid HotelId { get; set; }
    
    public Room()
    {
        
    }

    private Room(int price, RoomType type, string? imageUrl)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, "Price > 0");
        ArgumentNullException.ThrowIfNull(type, "RoomType != null");
        Price = price;
        Type = type;
        Status = RoomStatus.Available;
        ImageUrl = imageUrl ?? ImageUrl;
    }

    public static Room Create(int price, RoomType type, string? imageUrl)
        => new Room(price, type, imageUrl);


    public Room WithOption(RoomOption option)
    {
        if(!RoomOptions.Contains(option)) 
            RoomOptions.Add(option);
        
        return this;
    }

    public void ChangeStatus(RoomStatus status)
        => Status = status;

    public bool IsAvailable()
        => Status == RoomStatus.Available;
    
}