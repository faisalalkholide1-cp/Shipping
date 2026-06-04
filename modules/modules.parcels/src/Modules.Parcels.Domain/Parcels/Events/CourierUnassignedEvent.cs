using System;

namespace ShippingManagement.Parcels;

//حدث إلغاء تعيين المندوب
public class CourierUnassignedEvent
{
    public Guid ParcelId { get; }
    public DateTime EventTime { get; }

    public CourierUnassignedEvent(Guid parcelId)
    {
        ParcelId = parcelId;
        EventTime = DateTime.UtcNow;
    }
}