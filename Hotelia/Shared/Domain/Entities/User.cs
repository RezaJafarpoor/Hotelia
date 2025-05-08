using Microsoft.AspNetCore.Identity;

namespace Hotelia.Shared.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public List<Reservation> Reservations { get; set; } = [];
}