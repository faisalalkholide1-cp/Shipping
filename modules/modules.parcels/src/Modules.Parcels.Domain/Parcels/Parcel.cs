// modules/Parcels/Parcels.Domain/Parcels/Parcel.cs

using Modules.Parcels.Parcels;
using ShippingManagement.Parcels.StatusHistory;
using System;
using System.Collections.Generic;
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

    // ── التواريخ اللوجستية ───────────────────────────────────
    public DateTime? AssignedTime { get; private set; }
    public DateTime? PickedUpTime { get; private set; }
    public DateTime? OutForDeliveryTime { get; private set; }
    public DateTime? DeliveredTime { get; private set; }
    public DateTime? CancelledTime { get; private set; }
    public DateTime? ReturnedTime { get; private set; }
    public string? ReturnReason { get; private set; }

    // ── Status History ───────────────────────────────────────
    private readonly List<ParcelStatusHistory> _statusHistory = new();
    public IReadOnlyList<ParcelStatusHistory> StatusHistory
        => _statusHistory.AsReadOnly();

    // EF Core
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

        Weight = weight > 0
            ? weight
            : throw new BusinessException(ParcelsErrorCodes.InvalidParcelWeight);
        Price = price > 0
            ? price
            : throw new BusinessException(ParcelsErrorCodes.InvalidParcelPrice);

        Status = ParcelStatus.Created;

        // سجّل الحالة الأولى
        AddHistory(ParcelStatus.Created);

        AddLocalEvent(new ParcelCreatedEvent(Id, TrackingNumber));
    }

    // ── Status Transitions ───────────────────────────────────

    public Parcel AssignCourier(Guid courierId, Guid? changedBy = null)
    {
        if (Status != ParcelStatus.Created)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.Created);

        AssignedCourierId = courierId;
        Status = ParcelStatus.Assigned;
        AssignedTime = DateTime.UtcNow;

        AddHistory(ParcelStatus.Assigned, changedBy: changedBy);
        AddLocalEvent(new CourierAssignedEvent(Id, courierId));
        return this;
    }

    public Parcel UnassignCourier(Guid? changedBy = null)
    {
        if (Status != ParcelStatus.Assigned)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status);

        AssignedCourierId = null;
        AssignedTime = null;
        Status = ParcelStatus.Created;

        AddHistory(ParcelStatus.Created, note: "Courier unassigned", changedBy: changedBy);
        AddLocalEvent(new CourierUnassignedEvent(Id));
        return this;
    }

    public Parcel MarkPickedUp(Guid? changedBy = null)
    {
        if (Status != ParcelStatus.Assigned)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.Assigned);

        Status = ParcelStatus.PickedUp;
        PickedUpTime = DateTime.UtcNow;

        AddHistory(ParcelStatus.PickedUp, changedBy: changedBy);
        AddLocalEvent(new ParcelPickedUpEvent(Id));
        return this;
    }

    public Parcel StartTransit(Guid? changedBy = null)
    {
        if (Status != ParcelStatus.PickedUp)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.PickedUp);

        Status = ParcelStatus.InTransit;

        AddHistory(ParcelStatus.InTransit, changedBy: changedBy);
        AddLocalEvent(new ParcelInTransitEvent(Id));
        return this;
    }

    public Parcel MarkOutForDelivery(Guid? changedBy = null)
    {
        if (Status != ParcelStatus.InTransit)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.InTransit);

        Status = ParcelStatus.OutForDelivery;
        OutForDeliveryTime = DateTime.UtcNow;

        AddHistory(ParcelStatus.OutForDelivery, changedBy: changedBy);
        AddLocalEvent(new ParcelOutForDeliveryEvent(Id));
        return this;
    }

    public Parcel MarkDelivered(Guid? changedBy = null)
    {
        if (Status != ParcelStatus.OutForDelivery)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status)
                .WithData("ExpectedStatus", ParcelStatus.OutForDelivery);

        Status = ParcelStatus.Delivered;
        DeliveredTime = DateTime.UtcNow;

        AddHistory(ParcelStatus.Delivered, changedBy: changedBy);
        AddLocalEvent(new ParcelDeliveredEvent(Id));
        return this;
    }

    public Parcel MarkReturned(string reason, Guid? changedBy = null)
    {
        if (Status is not (ParcelStatus.InTransit or ParcelStatus.OutForDelivery))
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status);

        Status = ParcelStatus.Returned;
        ReturnedTime = DateTime.UtcNow;
        ReturnReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));

        AddHistory(ParcelStatus.Returned, note: reason, changedBy: changedBy);
        AddLocalEvent(new ParcelReturnedEvent(Id, reason));
        return this;
    }

    public Parcel Cancel(Guid? changedBy = null)
    {
        if (Status is ParcelStatus.Delivered or ParcelStatus.Returned)
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status);

        Status = ParcelStatus.Cancelled;
        CancelledTime = DateTime.UtcNow;

        AddHistory(ParcelStatus.Cancelled, changedBy: changedBy);
        AddLocalEvent(new ParcelCancelledEvent(Id));
        return this;
    }

    // ── Update ───────────────────────────────────────────────

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
        if (Status is not (ParcelStatus.Created or ParcelStatus.Assigned))
            throw new BusinessException(ParcelsErrorCodes.InvalidStatusTransition)
                .WithData("CurrentStatus", Status);

        SenderName = Check.NotNullOrWhiteSpace(senderName, nameof(senderName));
        SenderPhone = Check.NotNullOrWhiteSpace(senderPhone, nameof(senderPhone));
        ReceiverName = Check.NotNullOrWhiteSpace(receiverName, nameof(receiverName));
        ReceiverPhone = Check.NotNullOrWhiteSpace(receiverPhone, nameof(receiverPhone));
        PickupAddress = Check.NotNullOrWhiteSpace(pickupAddress, nameof(pickupAddress));
        DeliveryAddress = Check.NotNullOrWhiteSpace(deliveryAddress, nameof(deliveryAddress));

        Weight = weight > 0
            ? weight
            : throw new BusinessException(ParcelsErrorCodes.InvalidParcelWeight);
        Price = price > 0
            ? price
            : throw new BusinessException(ParcelsErrorCodes.InvalidParcelPrice);

        return this;
    }

    // ── Private Helper ───────────────────────────────────────

    private void AddHistory(
        ParcelStatus status,
        string? note = null,
        Guid? changedBy = null)
    {
        _statusHistory.Add(new ParcelStatusHistory(
            id: Guid.NewGuid(),
            parcelId: Id,
            status: status,
            note: note,
            changedByUserId: changedBy));
    }
}