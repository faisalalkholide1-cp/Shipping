// modules/couriers/src/Couriers.Application/EventHandlers/ParcelEventHandlers.cs

using Couriers.Domain;
using ShippingManagement.Parcels;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Couriers.Application.EventHandlers;

/// <summary>
/// عند تعيين مندوب لطرد — يصبح Busy وغير متاح
/// </summary>
public class CourierAssignedEventHandler
    : ILocalEventHandler<CourierAssignedEvent>, ITransientDependency
{
    private readonly ICourierProfileRepository _repository;

    public CourierAssignedEventHandler(ICourierProfileRepository repository)
        => _repository = repository;

    public async Task HandleEventAsync(CourierAssignedEvent eventData)
    {
        var profile = await _repository.FindAsync(eventData.CourierId);
        if (profile is null) return;

        profile.SetStatus(CourierStatus.Busy);
        profile.SetAvailability(false);

        await _repository.UpdateAsync(profile, autoSave: true);
    }
}

/// <summary>
/// عند تسليم الطرد — يعود المندوب Active ويزداد عداد التسليم
/// </summary>
public class ParcelDeliveredEventHandler
    : ILocalEventHandler<ParcelDeliveredEvent>, ITransientDependency
{
    private readonly ICourierProfileRepository _repository;
    private readonly IParcelCourierResolver _resolver;

    public ParcelDeliveredEventHandler(
        ICourierProfileRepository repository,
        IParcelCourierResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public async Task HandleEventAsync(ParcelDeliveredEvent eventData)
    {
        var courierId = await _resolver.GetCourierIdByParcelIdAsync(eventData.ParcelId);
        if (courierId is null) return;

        var profile = await _repository.FindAsync(courierId.Value);
        if (profile is null) return;

        profile.SetStatus(CourierStatus.Active);
        profile.SetAvailability(true);
        profile.IncrementDelivered();

        await _repository.UpdateAsync(profile, autoSave: true);
    }
}

/// <summary>
/// عند إلغاء الطرد — يعود المندوب Active ومتاح
/// </summary>
public class ParcelCancelledEventHandler
    : ILocalEventHandler<ParcelCancelledEvent>, ITransientDependency
{
    private readonly ICourierProfileRepository _repository;
    private readonly IParcelCourierResolver _resolver;

    public ParcelCancelledEventHandler(
        ICourierProfileRepository repository,
        IParcelCourierResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public async Task HandleEventAsync(ParcelCancelledEvent eventData)
    {
        var courierId = await _resolver.GetCourierIdByParcelIdAsync(eventData.ParcelId);
        if (courierId is null) return;

        var profile = await _repository.FindAsync(courierId.Value);
        if (profile is null) return;

        profile.SetStatus(CourierStatus.Active);
        profile.SetAvailability(true);

        await _repository.UpdateAsync(profile, autoSave: true);
    }
}

/// <summary>
/// عند إرجاع الطرد — يعود المندوب Active ومتاح
/// </summary>
public class ParcelReturnedEventHandler
    : ILocalEventHandler<ParcelReturnedEvent>, ITransientDependency
{
    private readonly ICourierProfileRepository _repository;
    private readonly IParcelCourierResolver _resolver;

    public ParcelReturnedEventHandler(
        ICourierProfileRepository repository,
        IParcelCourierResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public async Task HandleEventAsync(ParcelReturnedEvent eventData)
    {
        var courierId = await _resolver.GetCourierIdByParcelIdAsync(eventData.ParcelId);
        if (courierId is null) return;

        var profile = await _repository.FindAsync(courierId.Value);
        if (profile is null) return;

        profile.SetStatus(CourierStatus.Active);
        profile.SetAvailability(true);

        await _repository.UpdateAsync(profile, autoSave: true);
    }
}

/// <summary>
/// عند إلغاء تعيين المندوب — يعود Active ومتاح
/// </summary>
public class CourierUnassignedEventHandler
    : ILocalEventHandler<CourierUnassignedEvent>, ITransientDependency
{
    private readonly ICourierProfileRepository _repository;
    private readonly IParcelCourierResolver _resolver;

    public CourierUnassignedEventHandler(
        ICourierProfileRepository repository,
        IParcelCourierResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public async Task HandleEventAsync(CourierUnassignedEvent eventData)
    {
        var courierId = await _resolver.GetCourierIdByParcelIdAsync(eventData.ParcelId);
        if (courierId is null) return;

        var profile = await _repository.FindAsync(courierId.Value);
        if (profile is null) return;

        profile.SetStatus(CourierStatus.Active);
        profile.SetAvailability(true);

        await _repository.UpdateAsync(profile, autoSave: true);
    }
}