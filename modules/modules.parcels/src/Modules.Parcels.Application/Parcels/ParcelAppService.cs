// src/ShippingManagement.Application/Parcels/ParcelAppService.cs

using Microsoft.AspNetCore.Authorization;
using Modules.Parcels;
using ShippingManagement.Parcels.Dtos;
using ShippingManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ShippingManagement.Parcels;

[Authorize(ShippingManagementPermissions.Parcels.Default)]
public class ParcelAppService : ParcelsAppService, IParcelAppService
{
    private readonly IParcelRepository _parcelRepository;
    private readonly ParcelManager _parcelManager;

    public ParcelAppService(
        IParcelRepository parcelRepository,
        ParcelManager parcelManager)
    {
        _parcelRepository = parcelRepository;
        _parcelManager = parcelManager;
    }

    // ── GET ───────────────────────────────────────────────────────────────

    public async Task<ParcelDto> GetAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    public async Task<PagedResultDto<ParcelDto>> GetListAsync(ParcelListFilterDto input)
    {
        var totalCount = await _parcelRepository.GetCountAsync(
            filter: input.Filter,
            status: input.Status,
            assignedCourierId: input.AssignedCourierId
        );

        var parcels = await _parcelRepository.GetListAsync(
            filter: input.Filter,
            status: input.Status,
            assignedCourierId: input.AssignedCourierId,
            sorting: input.Sorting,
            skipCount: input.SkipCount,
            maxResultCount: input.MaxResultCount
        );

        return new PagedResultDto<ParcelDto>(
            totalCount,
            ObjectMapper.Map<List<Parcel>, List<ParcelDto>>(parcels)
        );
    }

    // ── CREATE ────────────────────────────────────────────────────────────

    [Authorize(ShippingManagementPermissions.Parcels.Create)]
    public async Task<ParcelDto> CreateAsync(CreateParcelDto input)
    {
        var parcel = await _parcelManager.CreateAsync(
            input.SenderName, input.SenderPhone,
            input.ReceiverName, input.ReceiverPhone,
            input.PickupAddress, input.DeliveryAddress,
            input.Weight, input.Price
        );

        await _parcelRepository.InsertAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    // ── UPDATE ────────────────────────────────────────────────────────────

    [Authorize(ShippingManagementPermissions.Parcels.Edit)]
    public async Task<ParcelDto> UpdateAsync(Guid id, UpdateParcelDto input)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.Update(
            input.SenderName, input.SenderPhone,
            input.ReceiverName, input.ReceiverPhone,
            input.PickupAddress, input.DeliveryAddress,
            input.Weight, input.Price
        );

        await _parcelRepository.UpdateAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    // ── DELETE ────────────────────────────────────────────────────────────

    [Authorize(ShippingManagementPermissions.Parcels.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _parcelRepository.DeleteAsync(id);
    }

    // ── WORKFLOW ──────────────────────────────────────────────────────────

    [Authorize(ShippingManagementPermissions.Parcels.Assign)]
    public async Task<ParcelDto> AssignCourierAsync(Guid id, AssignCourierDto input)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.AssignCourier(input.CourierId);

        await _parcelRepository.UpdateAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> MarkPickedUpAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.MarkPickedUp();

        await _parcelRepository.UpdateAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> StartTransitAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.StartTransit();

        await _parcelRepository.UpdateAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Deliver)]
    public async Task<ParcelDto> MarkDeliveredAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.MarkDelivered();

        await _parcelRepository.UpdateAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    public async Task<ParcelDto> MarkOutForDeliveryAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.MarkOutForDelivery(); // استدعاء المنطق من الكيان مباشرة

        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    public async Task<ParcelDto> MarkReturnedAsync(Guid id, string reason)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.MarkReturned(reason);

        await _parcelRepository.UpdateAsync(parcel);
        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

    [Authorize(ShippingManagementPermissions.Parcels.Edit)]
    public async Task<ParcelDto> CancelAsync(Guid id)
    {
        var parcel = await _parcelRepository.GetAsync(id);

        parcel.Cancel();

        await _parcelRepository.UpdateAsync(parcel);

        return ObjectMapper.Map<Parcel, ParcelDto>(parcel);
    }

}