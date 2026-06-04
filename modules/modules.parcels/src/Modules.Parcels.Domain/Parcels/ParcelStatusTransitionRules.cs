using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Modules.Parcels.Parcels;

internal static class ParcelStatusTransitionRules
{
    private static readonly IReadOnlyDictionary<ParcelStatus, ParcelStatus[]> AllowedTransitions =
        new ReadOnlyDictionary<ParcelStatus, ParcelStatus[]>(new Dictionary<ParcelStatus, ParcelStatus[]>
        {
            [ParcelStatus.Created] = [ParcelStatus.PickedUp, ParcelStatus.Cancelled],
            [ParcelStatus.PickedUp] = [ParcelStatus.InTransit, ParcelStatus.Cancelled],
            [ParcelStatus.InTransit] = [ParcelStatus.OutForDelivery, ParcelStatus.Cancelled],
            [ParcelStatus.OutForDelivery] = [ParcelStatus.Delivered, ParcelStatus.Cancelled],
            [ParcelStatus.Delivered] = [],
            [ParcelStatus.Cancelled] = []
        });

    public static bool IsAllowed(ParcelStatus current, ParcelStatus next)
    {
        return AllowedTransitions.TryGetValue(current, out var allowed)
               && Array.Exists(allowed, status => status == next);
    }

    public static IReadOnlyList<ParcelStatus> GetAllowedNextStatuses(ParcelStatus current)
    {
        return AllowedTransitions.TryGetValue(current, out var allowed)
            ? allowed
            : Array.Empty<ParcelStatus>();
    }
}
