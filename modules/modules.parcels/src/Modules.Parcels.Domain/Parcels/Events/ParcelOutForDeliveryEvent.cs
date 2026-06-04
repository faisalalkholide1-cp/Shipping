using System;

namespace ShippingManagement.Parcels;

//حدث خروج الطرد للتوصيل
public class ParcelOutForDeliveryEvent
{
    public Guid ParcelId { get; }
    public DateTime EventTime { get; }

    public ParcelOutForDeliveryEvent(Guid parcelId)
    {
        ParcelId = parcelId;
        EventTime = DateTime.UtcNow;
    }
}