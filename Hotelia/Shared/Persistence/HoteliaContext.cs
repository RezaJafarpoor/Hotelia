using Hotelia.Shared.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hotelia.Shared.Persistence;

public class HoteliaContext(DbContextOptions<HoteliaContext> option) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(option)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}