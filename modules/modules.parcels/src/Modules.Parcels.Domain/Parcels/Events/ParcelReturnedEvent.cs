using System;

namespace ShippingManagement.Parcels;

//حدث إرجاع الطرد / فشل التوصيل
public class ParcelReturnedEvent
{
    public Guid ParcelId { get; }
    public string Reason { get; }
    public DateTime EventTime { get; }

    public ParcelReturnedEvent(Guid parcelId, string reason)
    {
        ParcelId = parcelId;
        Reason = reason;
        EventTime = DateTime.UtcNow;
    }
}