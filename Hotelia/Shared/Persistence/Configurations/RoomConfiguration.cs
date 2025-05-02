using Hotelia.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotelia.Shared.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Price).IsRequired();

        
        builder.Property(r => r.Type).IsRequired().HasConversion<string>();
        // one to many is in  Hotel Configuration file 
    }
}