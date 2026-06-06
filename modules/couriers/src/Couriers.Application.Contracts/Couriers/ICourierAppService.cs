using Couriers.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Couriers.Application;

public interface ICourierAppService : IApplicationService
{
    Task<PagedResultDto<CourierProfileDto>> GetListAsync(CourierListFilterDto input);
    Task<CourierProfileDto> GetAsync(Guid id);
    Task<CourierProfileDto> CreateAsync(CreateCourierDto input);
    Task<CourierProfileDto> UpdateAsync(Guid id, UpdateCourierDto input);
    Task DeleteAsync(Guid id);

    /// <summary>للاستخدام في dropdown تعيين المندوب</summary>
    Task<List<CourierLookupDto>> GetAvailableLookupAsync();

    Task<CourierProfileDto> SetAvailabilityAsync(Guid id, bool isAvailable);
    Task<CourierProfileDto> SetStatusAsync(Guid id, CourierStatus status);
}