// modules/couriers/src/Couriers.Application/Couriers/CourierLookupService.cs

using Couriers.Domain;
using Modules.Parcels.Couriers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Couriers.Application;

/// <summary>
/// تنفيذ ICourierLookupService — الجسر بين Parcels و Couriers
/// موجود في Couriers.Application لأنه يعرف CourierProfile
/// </summary>
public class CourierLookupService : ICourierLookupService
{
    private readonly ICourierProfileRepository _repository;

    public CourierLookupService(ICourierProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid?> FindCourierIdByUserIdAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var profile = await _repository.FindByUserIdAsync(userId, ct);
        return profile?.Id;
    }

    public async Task<bool> IsCourierAvailableAsync(
        Guid courierId,
        CancellationToken ct = default)
    {
        var profile = await _repository.FindAsync(courierId, cancellationToken: ct);
        return profile is { IsAvailable: true, Status: CourierStatus.Active };
    }

    public async Task<List<CourierLookupItemDto>> GetAvailableCouriersAsync(
        CancellationToken ct = default)
    {
        var couriers = await _repository.GetAvailableListAsync(ct);

        return couriers.Select(c => new CourierLookupItemDto
        {
            Id = c.Id,
            FullName = c.FullName,
            Zone = c.Zone,
            IsAvailable = c.IsAvailable,
        }).ToList();
    }
}