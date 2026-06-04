using Microsoft.EntityFrameworkCore;
using Modules.Parcels.Parcels;
using ShippingManagement.Parcels;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Modules.Parcels.EntityFrameworkCore;

public static class ParcelsDbContextModelCreatingExtensions
{
    public static void ConfigureParcels(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Parcel>(b =>
        {
            b.ToTable(ParcelsDbProperties.DbTablePrefix + "Parcels", ParcelsDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.TrackingNumber)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxTrackingNumberLength);

            b.Property(x => x.SenderName)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxNameLength);

            b.Property(x => x.SenderPhone)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxPhoneLength);

            b.Property(x => x.ReceiverName)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxNameLength);

            b.Property(x => x.ReceiverPhone)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxPhoneLength);

            b.Property(x => x.PickupAddress)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxAddressLength);

            b.Property(x => x.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(ParcelConsts.MaxAddressLength);

            b.Property(x => x.Price).HasColumnType("decimal(18,2)");

            b.HasIndex(x => new { x.TenantId, x.TrackingNumber }).IsUnique();
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.CreationTime);
        });
    }
}
