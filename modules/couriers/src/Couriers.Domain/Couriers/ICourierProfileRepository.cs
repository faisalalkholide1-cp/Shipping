using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Couriers.Domain;

public interface ICourierProfileRepository : IRepository<CourierProfile, Guid>
{
    Task<CourierProfile?> FindByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<List<CourierProfile>> GetAvailableListAsync(CancellationToken ct = default);
    Task<List<CourierProfile>> GetListByZoneAsync(string zone, CancellationToken ct = default);
    Task<(List<CourierProfile> Items, int TotalCount)> GetPagedListAsync(
        string? filter = null,
        string? zone = null,
        CourierStatus? status = null,
        bool? isAvailable = null,
        int skipCount = 0,
        int maxResultCount = 10,
        string sorting = "FullName",
        CancellationToken ct = default);
}