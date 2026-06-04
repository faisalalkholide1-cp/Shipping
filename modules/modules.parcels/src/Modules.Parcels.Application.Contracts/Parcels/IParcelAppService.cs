// src/ShippingManagement.Application.Contracts/Parcels/IParcelAppService.cs

using Modules.Parcels;
using System;
using System.Threading.Tasks;
using ShippingManagement.Parcels.Dtos;
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
}