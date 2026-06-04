// src/ShippingManagement.Domain/Parcels/IParcelRepository.cs

using Modules.Parcels.Parcels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ShippingManagement.Parcels;

public interface IParcelRepository : IRepository<Parcel, Guid>
{
    Task<bool> TrackingNumberExistsAsync(string trackingNumber);

    Task<Parcel?> FindByTrackingNumberAsync(string trackingNumber);

    Task<List<Parcel>> GetListAsync(
        string? filter = null,
        ParcelStatus? status = null,
        Guid? assignedCourierId = null,
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = 10);

    Task<long> GetCountAsync(
        string? filter = null,
        ParcelStatus? status = null,
        Guid? assignedCourierId = null);
}