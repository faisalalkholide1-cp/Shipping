using Couriers.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Couriers.EntityFrameworkCore;

public static class CouriersDbContextModelCreatingExtensions
{
    public static void ConfigureCouriers(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<CourierProfile>(b =>
        {
            b.ToTable("CourierProfiles");
            b.HasKey(x => x.Id);

            b.Property(x => x.UserId)
             .IsRequired();

            b.Property(x => x.FullName)
             .IsRequired()
             .HasMaxLength(128);

            b.Property(x => x.Phone)
             .IsRequired()
             .HasMaxLength(32);

            b.Property(x => x.Email)
             .IsRequired()
             .HasMaxLength(256);

            b.Property(x => x.Zone)
             .HasMaxLength(128);

            b.Property(x => x.Status)
             .IsRequired()
             .HasConversion<int>();

            b.Property(x => x.IsAvailable)
             .IsRequired();

            b.Property(x => x.DeliveredCount)
             .IsRequired()
             .HasDefaultValue(0);

            // Index للبحث السريع
            b.HasIndex(x => x.UserId).IsUnique();
            b.HasIndex(x => x.IsAvailable);
            b.HasIndex(x => x.Zone);
        });
    }
}
