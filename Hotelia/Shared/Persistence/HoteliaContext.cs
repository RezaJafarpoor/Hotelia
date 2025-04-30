using Hotelia.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hotelia.Shared.Persistence;

public class HoteliaContext(DbContextOptions<HoteliaContext> option) : DbContext(option)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}