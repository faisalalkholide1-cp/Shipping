using Microsoft.EntityFrameworkCore;
using Modules.Parcels.Parcels;
using ShippingManagement.Parcels;
using ShippingManagement.Parcels.StatusHistory;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Modules.Parcels.EntityFrameworkCore;

public static class ParcelsDbContextModelCreatingExtensions
{
    public static void ConfigureParcels(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        // ── Parcel ────────────────────────────────────────────
        builder.Entity<Parcel>(b =>
        {
            b.ToTable(ParcelsDbProperties.DbTablePrefix + "Parcels",
                      ParcelsDbProperties.DbSchema);
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

            b.Property(x => x.Price)
             .HasColumnType("decimal(18,2)");

            b.Property(x => x.ReturnReason)
             .HasMaxLength(512);

            // Indexes
            b.HasIndex(x => new { x.TenantId, x.TrackingNumber }).IsUnique();
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.CreationTime);
            b.HasIndex(x => x.AssignedCourierId);

            // ── العلاقة مع StatusHistory ──────────────────────
            b.HasMany(x => x.StatusHistory)
             .WithOne()
             .HasForeignKey(h => h.ParcelId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── ParcelStatusHistory ───────────────────────────────
        builder.Entity<ParcelStatusHistory>(b =>
        {
            b.ToTable(ParcelsDbProperties.DbTablePrefix + "ParcelStatusHistories",
                      ParcelsDbProperties.DbSchema);

            b.HasKey(x => x.Id);

            b.Property(x => x.Status)
             .IsRequired()
             .HasConversion<int>();

            b.Property(x => x.Note)
             .HasMaxLength(512);

            b.Property(x => x.ChangedAt)
             .IsRequired();

            // Index للبحث السريع بالطرد
            b.HasIndex(x => x.ParcelId);
            b.HasIndex(x => x.ChangedAt);
        });
    }
}
