using Hotelia.Shared.Domain.DDD;
using Hotelia.Shared.Domain.Enums;

namespace Hotelia.Shared.Domain.Entities;

public class Room : Aggregate<Guid>
{
    public int Price { get; set; }
    public RoomType Type { get; set; }
    public string? ImageUrl { get; set; }
    public RoomStatus RoomStatus { get; set; }
    public List<RoomOption> RoomOptions { get; set; } = [];
    public List<Reservation> Reservations { get; set; } = [];
    public Guid HotelId { get; set; }
    
    public Room()
    {
        
    }

    private Room(int price, RoomType type, string? imageUrl, RoomStatus roomStatus, List<RoomOption>? roomOptions)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, "Price > 0");
        ArgumentNullException.ThrowIfNull(type, "RoomType != null");
        Price = price;
        Type = type;
        ImageUrl = imageUrl ?? ImageUrl;
        RoomStatus = roomStatus;
        RoomOptions = roomOptions ?? RoomOptions;
    }

    public static Room Create(int price, RoomType type, string? imageUrl, RoomStatus roomStatus, List<RoomOption>? roomOptions)
        => new Room(price, type, imageUrl, roomStatus, roomOptions);


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

    public bool AddReservation(Reservation reservation)
    {
        var isAvailable = !Reservations.Any(r => reservation.Start < r.End && reservation.End > r.Start);
        if (isAvailable is false)
            return false;
        Reservations.Add(reservation);
        return true;
    }
    
}