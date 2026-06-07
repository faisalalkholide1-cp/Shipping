// src/ShippingManagement.Application/Parcels/ParcelCourierResolver.cs

using Couriers.Application.EventHandlers;
using ShippingManagement.Parcels;
using System;
using System.Threading.Tasks;

namespace ShippingManagement.Application.Parcels;

/// <summary>
/// تنفيذ IParcelCourierResolver في ShippingManagement
/// يجلب AssignedCourierId من جدول Parcels
/// </summary>
public class ParcelCourierResolver : IParcelCourierResolver
{
    private readonly IParcelRepository _parcelRepository;

    public ParcelCourierResolver(IParcelRepository parcelRepository)
        => _parcelRepository = parcelRepository;

    public async Task<Guid?> GetCourierIdByParcelIdAsync(Guid parcelId)
    {
        var parcel = await _parcelRepository.FindAsync(parcelId);
        return parcel?.AssignedCourierId;
    }
}