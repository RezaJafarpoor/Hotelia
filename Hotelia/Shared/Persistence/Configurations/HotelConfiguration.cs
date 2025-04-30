using Hotelia.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotelia.Shared.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Name)
            .IsRequired();
        builder.OwnsOne(h => h.Address, owned =>
        {
            owned.Property(a => a.City).IsRequired();
            owned.HasIndex(a => a.City);
            owned.Property(a => a.Province).IsRequired();
            owned.Property(a => a.LocalAddress).IsRequired();
        });
        builder.Property(h => h.HotelType)
            .HasConversion<string>();
        builder.Property(h => h.HotelType).IsRequired();
        builder.HasMany(h =>h.Rooms)
            .WithOne()
            .HasForeignKey("HotelId")
            .OnDelete(DeleteBehavior.Cascade);

    }
}