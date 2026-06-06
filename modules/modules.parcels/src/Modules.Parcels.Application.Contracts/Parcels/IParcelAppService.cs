// src/ShippingManagement.Application.Contracts/Parcels/IParcelAppService.cs

using Modules.Parcels;
using ShippingManagement.Parcels.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ShippingManagement.Parcels;

[RemoteService(Name = ParcelsRemoteServiceConsts.RemoteServiceName)]
public interface IParcelAppService : IApplicationService
{
    // ── CRUD ─────────────────────────────────────────
    Task<ParcelDto> GetAsync(Guid id);
    Task<PagedResultDto<ParcelDto>> GetListAsync(ParcelListFilterDto input);
    Task<ParcelDto> CreateAsync(CreateParcelDto input);
    Task<ParcelDto> UpdateAsync(Guid id, UpdateParcelDto input);
    Task DeleteAsync(Guid id);

    // ── Workflow ──────────────────────────────────────
    Task<ParcelDto> AssignCourierAsync(Guid id, AssignCourierDto input);
    Task<ParcelDto> MarkPickedUpAsync(Guid id);
    Task<ParcelDto> StartTransitAsync(Guid id);
    Task<ParcelDto> MarkDeliveredAsync(Guid id);
    Task<ParcelDto> CancelAsync(Guid id);


    /// <summary>جلب تاريخ تغييرات حالة الطرد</summary>
    Task<List<ParcelStatusHistoryDto>> GetStatusHistoryAsync(Guid id);
}