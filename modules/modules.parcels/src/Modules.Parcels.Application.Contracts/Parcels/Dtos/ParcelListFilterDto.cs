// src/ShippingManagement.Application.Contracts/Parcels/Dtos/ParcelListFilterDto.cs

using Modules.Parcels.Parcels;
using System;
using Volo.Abp.Application.Dtos;

namespace ShippingManagement.Parcels.Dtos;

public class ParcelListFilterDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }   // search keyword
    public ParcelStatus? Status { get; set; }
    public Guid? AssignedCourierId { get; set; }
}