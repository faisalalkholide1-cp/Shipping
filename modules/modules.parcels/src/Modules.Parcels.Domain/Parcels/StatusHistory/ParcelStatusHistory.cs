// modules/Parcels/Parcels.Domain/Parcels/StatusHistory/ParcelStatusHistory.cs

using Modules.Parcels.Parcels;
using System;
using Volo.Abp.Domain.Entities;

namespace ShippingManagement.Parcels.StatusHistory;

/// <summary>
/// سجل تاريخ تغييرات حالة الطرد
/// جدول منفصل مرتبط بـ Parcel عبر ParcelId
/// </summary>
public class ParcelStatusHistory : Entity<Guid>
{
    public Guid ParcelId { get; private set; }
    public ParcelStatus Status { get; private set; }
    public string? Note { get; private set; }
    public Guid? ChangedByUserId { get; private set; }
    public DateTime ChangedAt { get; private set; }

    // EF Core
    private ParcelStatusHistory() { }

    public ParcelStatusHistory(
        Guid id,
        Guid parcelId,
        ParcelStatus status,
        string? note,
        Guid? changedByUserId)
    {
        Id = id;
        ParcelId = parcelId;
        Status = status;
        Note = note;
        ChangedByUserId = changedByUserId;
        ChangedAt = DateTime.UtcNow;
    }
}