using Hotelia.Shared.Domain.DDD;

namespace Hotelia.Shared.Domain.Entities;

public class Reservation : Entity<Guid>
{
    
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public Room Room { get; set; }
    public Guid RoomId { get; set; }

    public Reservation()
    {
        
    }

    private Reservation(Guid userId, DateOnly start, DateOnly end, Guid roomId)
    {
        UserId = userId;
        Start= start;
        End = end;
        RoomId = roomId;
    }

    public static Reservation Create(Guid userId, DateOnly start, DateOnly end, Guid roomId) 
        => new(userId, start, end, roomId);
    
    
}