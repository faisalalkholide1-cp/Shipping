// src/ShippingManagement.EntityFrameworkCore/Parcels/ParcelConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingManagement.Parcels;

namespace ShippingManagement.EntityFrameworkCore.Parcels;

public class ConfigureParcels : IEntityTypeConfiguration<Parcel>
{
    public void Configure(EntityTypeBuilder<Parcel> builder)
    {
        builder.ToTable("ShippingParcels");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrackingNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.TrackingNumber)
            .IsUnique();

        builder.Property(x => x.SenderName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SenderPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.ReceiverName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ReceiverPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.PickupAddress)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.DeliveryAddress)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.AssignedCourierId) 
            .IsRequired(false);
    }
}