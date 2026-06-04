// modules/Parcels/Parcels.Domain/Parcels/Parcel.cs
using Modules.Parcels.Parcels;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ShippingManagement.Parcels;

public class Parcel : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public string TrackingNumber { get; private set; }
    public string SenderName { get; private set; }
    public string SenderPhone { get; private set; }

    public string ReceiverName { get; private set; }
    public string ReceiverPhone { get; private set; }

    public string PickupAddress { get; private set; }
    public string DeliveryAddress { get; private set; }

    public double Weight { get; private set; }
    public decimal Price { get; private set; }

    public ParcelStatus Status { get; private set; }
    public Guid? AssignedCourierId { get; private set; }

    // ── التواريخ اللوجستية المضافة لحساب مؤشرات الأداء ──
    public DateTime? AssignedTime { get; private set; }
    public DateTime? PickedUpTime { get; private set; }
    public DateTime? OutForDeliveryTime { get; private set; }
    public DateTime? DeliveredTime { get; private set; }
    public DateTime? CancelledTime { get; private set; }
    public DateTime? ReturnedTime { get; private set; }
    public string? ReturnReason { get; private set; }

    // Required by EF Core
    protected Parcel() { }

    internal Parcel(
        Guid id,
        string trackingNumber,
        string senderName,
        string senderPhone,
        string receiverName,
        string receiverPhone,
        string pickupAddress,
        string deliveryAddress,
        double weight,
        decimal price) : base(id)
    {
        TrackingNumber = Check.NotNullOrWhiteSpace(trackingNumber, nameof(trackingNumber));
        SenderName = Check.NotNullOrWhiteSpace(senderName, nameof(senderName));
        SenderPhone = Check.NotNullOrWhiteSpace(senderPhone, nameof(senderPhone));
        ReceiverName = Check.NotNullOrWhiteSpace(receiverName, nameof(receiverName));
        ReceiverPhone = Check.NotNullOrWhiteSpace(receiverPhone, nameof(receiverPhone));
        PickupAddress = Check.NotNullOrWhiteSpace(pickupAddress, nameof(pickupAddress));
        DeliveryAddress = Check.NotNullOrWhiteSpace(deliveryAddress, nameof(deliveryAddress));

        // تصحيح كود الخطأ ليكون معبراً عن الحقل المتأثر بدلاً من ParcelNotFound
        Weight = weight > 0 ? weight : throw new BusinessException(ParcelsErrorCodes.InvalidParcelWeight);
        Price = price > 0 ? price : throw new BusinessException(ParcelsErrorCodes.InvalidParcelPrice);

        Status = ParcelStatus.Created;

        // Raise domain event
        AddLocalEvent(new ParcelCreatedEvent(Id, TrackingNumber));
    }

    // ── Status Transitions ───────────────────────────────────────

    public Parcel AssignCourier(Guid courierId)
    {
        if (Status != ParcelStatus.Created)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.Created);

        AssignedCourierId = courierId;
        Status = ParcelStatus.Assigned;
        AssignedTime = DateTime.UtcNow; // تسجيل وقت التعيين

        AddLocalEvent(new CourierAssignedEvent(Id, courierId));
        return this;
    }

    // ميزة مضافة: إلغاء تعيين المندوب وإعادة الطرد متاحاً للتعيين مجدداً
    public Parcel UnassignCourier()
    {
        if (Status != ParcelStatus.Assigned)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("Message", "Can only unassign a courier if the parcel is not picked up yet.")
                .WithData("CurrentStatus", Status);

        AssignedCourierId = null;
        AssignedTime = null;
        Status = ParcelStatus.Created;

        AddLocalEvent(new CourierUnassignedEvent(Id));
        return this;
    }

    public Parcel MarkPickedUp()
    {
        if (Status != ParcelStatus.Assigned)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.Assigned);

        Status = ParcelStatus.PickedUp;
        PickedUpTime = DateTime.UtcNow; // تسجيل وقت الاستلام الفعلي

        AddLocalEvent(new ParcelPickedUpEvent(Id));
        return this;
    }

    public Parcel StartTransit()
    {
        if (Status != ParcelStatus.PickedUp)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.PickedUp);

        Status = ParcelStatus.InTransit;
        AddLocalEvent(new ParcelInTransitEvent(Id));
        return this;
    }

    // ميزة مضافة: نقل الطرد إلى مرحلة التوصيل النهائي مع المندوب
    public Parcel MarkOutForDelivery()
    {
        if (Status != ParcelStatus.InTransit)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.InTransit);

        Status = ParcelStatus.OutForDelivery;
        OutForDeliveryTime = DateTime.UtcNow; // تسجيل وقت الخروج للتوصيل

        AddLocalEvent(new ParcelOutForDeliveryEvent(Id));
        return this;
    }

    public Parcel MarkDelivered()
    {
        // تم تحديث الشرط ليتوقع أن الطرد خرج للتوصيل الفعلي أولاً
        if (Status != ParcelStatus.OutForDelivery)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.OutForDelivery);

        Status = ParcelStatus.Delivered;
        DeliveredTime = DateTime.UtcNow; // تسجيل وقت التوصيل النهائي لعميل

        AddLocalEvent(new ParcelDeliveredEvent(Id));
        return this;
    }

    // ميزة مضافة: معالجة فشل التوصيل وتحويل الطرد إلى مرتجع للمخازن
    public Parcel MarkReturned(string reason)
    {
        if (Status is not (ParcelStatus.InTransit or ParcelStatus.OutForDelivery))
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("Message", "Can only return parcels that are currently in transit or out for delivery.")
                .WithData("CurrentStatus", Status);

        Status = ParcelStatus.Returned;
        ReturnedTime = DateTime.UtcNow;
        ReturnReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));

        AddLocalEvent(new ParcelReturnedEvent(Id, reason));
        return this;
    }

    public Parcel Cancel()
    {
        // حماية منطقية: لا يمكن إلغاء طرد تم توصيله أو إرجاعه بالفعل ومغلق مسبقاً
        if (Status is ParcelStatus.Delivered or ParcelStatus.Returned)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("Message", "Cannot cancel a parcel that is already delivered or returned.")
                .WithData("CurrentStatus", Status);

        Status = ParcelStatus.Cancelled;
        CancelledTime = DateTime.UtcNow; // تسجيل وقت الإلغاء

        AddLocalEvent(new ParcelCancelledEvent(Id));
        return this;
    }

    // ── Update Info ──────────────────────────────────────────────

    public Parcel Update(
        string senderName,
        string senderPhone,
        string receiverName,
        string receiverPhone,
        string pickupAddress,
        string deliveryAddress,
        double weight,
        decimal price)
    {
        // حماية البيانات الأمنية: قفل ميزة التعديل تماماً فور بدء تحرك الشحنة حمايةً للمصداقية المالية واللوجستية
        if (Status is not (ParcelStatus.Created or ParcelStatus.Assigned))
        {
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("Message", "For safety and security, parcel details cannot be modified once the courier has picked it up.")
                .WithData("CurrentStatus", Status);
        }

        SenderName = Check.NotNullOrWhiteSpace(senderName, nameof(senderName));
        SenderPhone = Check.NotNullOrWhiteSpace(senderPhone, nameof(senderPhone));
        ReceiverName = Check.NotNullOrWhiteSpace(receiverName, nameof(receiverName));
        ReceiverPhone = Check.NotNullOrWhiteSpace(receiverPhone, nameof(receiverPhone));
        PickupAddress = Check.NotNullOrWhiteSpace(pickupAddress, nameof(pickupAddress));
        DeliveryAddress = Check.NotNullOrWhiteSpace(deliveryAddress, nameof(deliveryAddress));

        Weight = weight > 0 ? weight : throw new BusinessException(ParcelsErrorCodes.InvalidParcelWeight);
        Price = price > 0 ? price : throw new BusinessException(ParcelsErrorCodes.InvalidParcelPrice);

        return this;
    }
}