// src/ShippingManagement.EntityFrameworkCore/Parcels/EfCoreParcelRepository.cs

using Microsoft.EntityFrameworkCore;
using Modules.Parcels.EntityFrameworkCore;
using Modules.Parcels.Parcels;
using ShippingManagement.Parcels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ShippingManagement.EntityFrameworkCore.Parcels;

public class EfCoreParcelRepository
    : EfCoreRepository<IParcelsDbContext, Parcel, Guid>,
      IParcelRepository
{
    public EfCoreParcelRepository(
        IDbContextProvider<IParcelsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    // ── Find by Tracking Number ───────────────────────────────────────────

    public async Task<Parcel?> FindByTrackingNumberAsync(string trackingNumber)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .FirstOrDefaultAsync(p => p.TrackingNumber == trackingNumber);
    }

    public async Task<bool> TrackingNumberExistsAsync(string trackingNumber)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .AnyAsync(p => p.TrackingNumber == trackingNumber);
    }

    // ── GetList with filtering ────────────────────────────────────────────

    public async Task<List<Parcel>> GetListAsync(
        string? filter = null,
        ParcelStatus? status = null,
        Guid? assignedCourierId = null,
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = 10)
    {
        var query = await BuildQueryAsync(filter, status, assignedCourierId);

        return await query
            .OrderBy(sorting.IsNullOrWhiteSpace()
                ? nameof(Parcel.TrackingNumber)
                : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    // ── GetCount with filtering ───────────────────────────────────────────

    public async Task<long> GetCountAsync(
        string? filter = null,
        ParcelStatus? status = null,
        Guid? assignedCourierId = null)
    {
        var query = await BuildQueryAsync(filter, status, assignedCourierId);

        return await query.LongCountAsync();
    }

    // ── Shared Query Builder ──────────────────────────────────────────────

    private async Task<IQueryable<Parcel>> BuildQueryAsync(
        string? filter,
        ParcelStatus? status,
        Guid? assignedCourierId)
    {
        var dbSet = await GetDbSetAsync();

        return dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                p => p.TrackingNumber.Contains(filter!) ||
                     p.SenderName.Contains(filter!) ||
                     p.ReceiverName.Contains(filter!) ||
                     p.SenderPhone.Contains(filter!) ||
                     p.ReceiverPhone.Contains(filter!))
            .WhereIf(
                status.HasValue,
                p => p.Status == status!.Value)
            .WhereIf(
                assignedCourierId.HasValue,
                p => p.AssignedCourierId == assignedCourierId!.Value);
    }
}