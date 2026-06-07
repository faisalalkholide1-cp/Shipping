// modules/modules.parcels/src/Modules.Parcels.Application.Contracts/Parcels/Dtos/CourierPortalDtos.cs

using Modules.Parcels.Parcels;
using Volo.Abp.Application.Dtos;

namespace ShippingManagement.Parcels.Dtos;

/// <summary>فلتر طرود المندوب الحالي</summary>
public class MyParcelListFilterDto : PagedAndSortedResultRequestDto
{
    public ParcelStatus? Status { get; set; }
}

/// <summary>إحصائيات لوحة تحكم المندوب</summary>
public class CourierDashboardDto
{
    public int ActiveParcels { get; set; }
    public int DeliveredToday { get; set; }
    public int TotalDelivered { get; set; }
    public int ReturnedParcels { get; set; }
}