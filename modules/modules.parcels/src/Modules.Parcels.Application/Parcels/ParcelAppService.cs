// src/ShippingManagement.Application/Parcels/ParcelAppService.cs

using Microsoft.AspNetCore.Authorization;
using Modules.Parcels;
using Modules.Parcels.Couriers;
using Modules.Parcels.Parcels;
using Modules.Parcels.Parcels.StatusHistory;
using ShippingManagement.Parcels.Dtos;
using ShippingManagement.Parcels.StatusHistory;
using ShippingManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace ShippingManagement.Parcels;

[Authorize(ShippingManagementPermissions.Parcels.Default)]
public class ParcelAppService : ParcelsAppService, IParcelAppService
{
    private readonly IParcelRepository              _parcelRepository;
    private readonly ParcelManager                  _parcelManager;
    private readonly IParcelStatusHistoryRepository _historyRepository;
    private readonly ICourierLookupService          _courierLookup;   // ✅ بدل ICourierProfileRepository
    private readonly ICurrentUser                   _currentUser;

    public ParcelAppService(
        IParcelRepository              parcelRepository,
        ParcelManager                  parcelManager,
        IParcelStatusHistoryRepository historyRepository,
        ICourierLookupService          courierLookup,
        ICurrentUser                   currentUser)
    {
        _parcelRepository  = parcelRepository;
        _parcelManager     = parcelManager;
        _historyRepository = historyRepository;
        _courierLookup     = courierLookup;
        _currentUser       = currentUser;
    }

    // ── GET ───────────────────────────────────────────────────
    public async Task<ParcelDto> GetAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    public async Task<PagedResultDto<ParcelDto>> GetListAsync(ParcelListFilterDto input)
    {
        var totalCount = await _parcelRepository.GetCountAsync(
            filter:            input.Filter,
            status:            input.Status,
            assignedCourierId: input.AssignedCourierId);

        var parcels = await _parcelRepository.GetListAsync(
            filter:            input.Filter,
            status:            input.Status,
            assignedCourierId: input.AssignedCourierId,
            sorting:           input.Sorting,
            skipCount:         input.SkipCount,
            maxResultCount:    input.MaxResultCount);

        return new PagedResultDto<ParcelDto>(
            totalCount,
            ObjectMapper.Map<List<Parcel>, List<ParcelDto>>(parcels));
    }

    // ── CREATE ────────────────────────────────────────────────
    [Authorize(ShippingManagementPermissions.Parcels.Create)]
    public async Task<ParcelDto> CreateAsync(CreateParcelDto input)
    {
        var parcel = await _parcelManager.CreateAsync(
            input.SenderName,    input.SenderPhone,
            input.ReceiverName,  input.ReceiverPhone,
            input.PickupAddress, input.DeliveryAddress,
            input.Weight,        input.Price);

        await _parcelRepository.InsertAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    // ── UPDATE ────────────────────────────────────────────────
    [Authorize(ShippingManagementPermissions.Parcels.Edit)]
    public async Task<ParcelDto> UpdateAsync(Guid id, UpdateParcelDto input)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.Update(
            input.SenderName,    input.SenderPhone,
            input.ReceiverName,  input.ReceiverPhone,
            input.PickupAddress, input.DeliveryAddress,
            input.Weight,        input.Price);

        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    // ── DELETE ────────────────────────────────────────────────
    [Authorize(ShippingManagementPermissions.Parcels.Delete)]
    public async Task DeleteAsync(Guid id)
        => await _parcelRepository.DeleteAsync(id);

    // ── WORKFLOW ──────────────────────────────────────────────
    [Authorize(ShippingManagementPermissions.Parcels.Assign)]
    public async Task<ParcelDto> AssignCourierAsync(Guid id, AssignCourierDto input)
    {
        // ✅ التحقق من توفر المندوب عبر ICourierLookupService
        var isAvailable = await _courierLookup.IsCourierAvailableAsync(input.CourierId);
        if (!isAvailable)
            throw new Volo.Abp.BusinessException("Couriers:CourierNotAvailable")
                .WithData("CourierId", input.CourierId);

        var parcel = await _parcelRepository.GetAsync(id);
        parcel.AssignCourier(input.CourierId, _currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> MarkPickedUpAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.MarkPickedUp(_currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> StartTransitAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.StartTransit(_currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> MarkOutForDeliveryAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.MarkOutForDelivery(_currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> MarkDeliveredAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.MarkDelivered(_currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> MarkReturnedAsync(Guid id, string reason)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.MarkReturned(reason, _currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Edit)]
    public async Task<ParcelDto> CancelAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        parcel.Cancel(_currentUser.Id);
        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    // ── STATUS HISTORY ────────────────────────────────────────
    public async Task<List<ParcelStatusHistoryDto>> GetStatusHistoryAsync(Guid id)
    {
        var history = await _historyRepository.GetListByParcelIdAsync(id);
        return ObjectMapper.Map<List<ParcelStatusHistory>, List<ParcelStatusHistoryDto>>(history);
    }

    // ── COURIER PORTAL ────────────────────────────────────────
    [Authorize(ShippingManagementPermissions.Parcels.Assign)]
    public async Task<PagedResultDto<ParcelDto>> GetMyParcelsAsync(MyParcelListFilterDto input)
    {
        // ✅ عبر ICourierLookupService — لا اعتماد مباشر على Couriers.Domain
        var userId     = _currentUser.GetId();
        var courierId  = await _courierLookup.FindCourierIdByUserIdAsync(userId)
            ?? throw new Volo.Abp.BusinessException("Couriers:ProfileNotFound");

        var totalCount = await _parcelRepository.GetCountAsync(
            assignedCourierId: courierId,
            status:            input.Status);

        var parcels = await _parcelRepository.GetListAsync(
            assignedCourierId: courierId,
            status:            input.Status,
            sorting:           input.Sorting ?? "creationTime desc",
            skipCount:         input.SkipCount,
            maxResultCount:    input.MaxResultCount);

        return new PagedResultDto<ParcelDto>(
            totalCount,
            ObjectMapper.Map<List<Parcel>, List<ParcelDto>>(parcels));
    }

    [Authorize(ShippingManagementPermissions.Parcels.Assign)]
    public async Task<CourierDashboardDto> GetMyDashboardAsync()
    {
        var userId    = _currentUser.GetId();
        var courierId = await _courierLookup.FindCourierIdByUserIdAsync(userId)
            ?? throw new Volo.Abp.BusinessException("Couriers:ProfileNotFound");

        var activeStatuses = new[]
        {
            ParcelStatus.Assigned,
            ParcelStatus.PickedUp,
            ParcelStatus.InTransit,
            ParcelStatus.OutForDelivery,
        };

        var activeParcels = 0;
        foreach (var status in activeStatuses)
            activeParcels += (int)await _parcelRepository.GetCountAsync(
                assignedCourierId: courierId, status: status);

        var allDelivered = await _parcelRepository.GetListAsync(
            assignedCourierId: courierId,
            status:            ParcelStatus.Delivered,
            sorting:           "creationTime desc",
            skipCount:         0,
            maxResultCount:    1000);

        var today          = DateTime.Today;
        var deliveredToday = allDelivered.Count(p =>
            p.DeliveredTime.HasValue &&
            p.DeliveredTime.Value.Date == today);

        var returned = (int)await _parcelRepository.GetCountAsync(
            assignedCourierId: courierId,
            status:            ParcelStatus.Returned);

        return new CourierDashboardDto
        {
            ActiveParcels   = activeParcels,
            DeliveredToday  = deliveredToday,
            TotalDelivered  = allDelivered.Count,
            ReturnedParcels = returned,
        };
    }

    // ── LOOKUP ────────────────────────────────────────────────
    /// <summary>قائمة المناديب المتاحين للـ dropdown</summary>
    public async Task<List<CourierLookupItemDto>> GetAvailableCouriersAsync()
        => await _courierLookup.GetAvailableCouriersAsync();
}