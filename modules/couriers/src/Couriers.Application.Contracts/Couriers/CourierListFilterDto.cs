using System;
using Couriers.Domain;
using Volo.Abp.Application.Dtos;

namespace Couriers.Application;

public class CourierListFilterDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Zone { get; set; }
    public CourierStatus? Status { get; set; }
    public bool? IsAvailable { get; set; }
}