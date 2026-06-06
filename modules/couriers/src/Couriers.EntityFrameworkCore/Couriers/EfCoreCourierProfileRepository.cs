using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Couriers.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Couriers.EntityFrameworkCore;

public class EfCoreCourierProfileRepository
    : EfCoreRepository<CouriersDbContext, CourierProfile, Guid>,
      ICourierProfileRepository
{
    public EfCoreCourierProfileRepository(
        IDbContextProvider<CouriersDbContext> dbContextProvider)
        : base(dbContextProvider) { }

    public async Task<CourierProfile?> FindByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var db = await GetDbSetAsync();
        return await db.FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task<List<CourierProfile>> GetAvailableListAsync(CancellationToken ct = default)
    {
        var db = await GetDbSetAsync();
        return await db
            .Where(x => x.IsAvailable && x.Status == CourierStatus.Active)
            .OrderBy(x => x.FullName)
            .ToListAsync(ct);
    }

    public async Task<List<CourierProfile>> GetListByZoneAsync(string zone, CancellationToken ct = default)
    {
        var db = await GetDbSetAsync();
        return await db
            .Where(x => x.Zone == zone)
            .OrderBy(x => x.FullName)
            .ToListAsync(ct);
    }

    public async Task<(List<CourierProfile> Items, int TotalCount)> GetPagedListAsync(
        string? filter = null,
        string? zone = null,
        CourierStatus? status = null,
        bool? isAvailable = null,
        int skipCount = 0,
        int maxResultCount = 10,
        string sorting = "FullName",
        CancellationToken ct = default)
    {
        var db = await GetDbSetAsync();

        var query = db.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(x =>
                x.FullName.Contains(filter) ||
                x.Phone.Contains(filter) ||
                x.Email.Contains(filter));

        if (!string.IsNullOrWhiteSpace(zone))
            query = query.Where(x => x.Zone == zone);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (isAvailable.HasValue)
            query = query.Where(x => x.IsAvailable == isAvailable.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(ct);

        return (items, total);
    }
}