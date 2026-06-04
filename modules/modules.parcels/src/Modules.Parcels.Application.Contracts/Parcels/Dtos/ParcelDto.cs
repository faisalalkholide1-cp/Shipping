// src/ShippingManagement.Application.Contracts/Parcels/Dtos/ParcelDto.cs

using Modules.Parcels.Parcels;
using System;
using Volo.Abp.Application.Dtos;

namespace ShippingManagement.Parcels.Dtos;

public class ParcelDto : FullAuditedEntityDto<Guid>
{
    public string TrackingNumber { get; set; }

    public string SenderName { get; set; }
    public string SenderPhone { get; set; }

    public string ReceiverName { get; set; }
    public string ReceiverPhone { get; set; }

    public string PickupAddress { get; set; }
    public string DeliveryAddress { get; set; }

    public double Weight { get; set; }
    public decimal Price { get; set; }

    public ParcelStatus Status { get; set; }

    public Guid? AssignedCourierId { get; set; }
}