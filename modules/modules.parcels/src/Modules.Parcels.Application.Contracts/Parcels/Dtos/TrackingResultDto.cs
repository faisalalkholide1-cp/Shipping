using System;
using System.Collections.Generic;
using Modules.Parcels.Parcels;
using Volo.Abp.Application.Dtos;

namespace ShippingManagement.Parcels.Dtos;

public class TrackingResultDto
{
    public string TrackingNumber { get; set; } = null!;

    public ParcelStatus Status { get; set; }

    public string ReceiverName { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;

    public DateTime CreationTime { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public List<ParcelStatusHistoryDto> History { get; set; } = [];
}

public class ParcelStatusHistoryDto : EntityDto<Guid>
{
    public ParcelStatus Status { get; set; }

    public string? Note { get; set; }

    public DateTime CreationTime { get; set; }
}
