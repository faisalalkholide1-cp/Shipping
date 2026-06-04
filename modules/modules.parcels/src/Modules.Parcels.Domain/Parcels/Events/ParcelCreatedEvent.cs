// modules/Parcels/Parcels.Domain/Parcels/Events/ParcelCreatedEvent.cs
using System;

namespace ShippingManagement.Parcels;

public record ParcelCreatedEvent(Guid ParcelId, string TrackingNumber);
public record CourierAssignedEvent(Guid ParcelId, Guid CourierId);
public record ParcelPickedUpEvent(Guid ParcelId);
public record ParcelInTransitEvent(Guid ParcelId);
public record ParcelDeliveredEvent(Guid ParcelId);
public record ParcelCancelledEvent(Guid ParcelId);