using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couriers.Domain;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Couriers.Application;

public class CourierAppService : CouriersAppService, ICourierAppService
{
    private readonly ICourierProfileRepository _repository;
    private readonly CourierManager _courierManager;
    private readonly IIdentityUserRepository _userRepository;

    public CourierAppService(
        ICourierProfileRepository repository,
        CourierManager courierManager,
        IIdentityUserRepository userRepository)
    {
        _repository = repository;
        _courierManager = courierManager;
        _userRepository = userRepository;
    }

    // ── GET LIST ──────────────────────────────────────────────
    public async Task<PagedResultDto<CourierProfileDto>> GetListAsync(CourierListFilterDto input)
    {
        var (items, total) = await _repository.GetPagedListAsync(
            filter: input.Filter,
            zone: input.Zone,
            status: input.Status,
            isAvailable: input.IsAvailable,
            skipCount: input.SkipCount,
            maxResultCount: input.MaxResultCount,
            sorting: input.Sorting ?? "FullName");

        return new PagedResultDto<CourierProfileDto>(
            total,
            ObjectMapper.Map<List<CourierProfile>, List<CourierProfileDto>>(items));
    }

    // ── GET ───────────────────────────────────────────────────
    public async Task<CourierProfileDto> GetAsync(Guid id)
    {
        var profile = await _repository.GetAsync(id);
        return ObjectMapper.Map<CourierProfile, CourierProfileDto>(profile);
    }

    // ── CREATE ────────────────────────────────────────────────
    public async Task<CourierProfileDto> CreateAsync(CreateCourierDto input)
    {
        var profile = await _courierManager.CreateAsync(
            input.FullName,
            input.Phone,
            input.Email,
            input.Zone,
            input.Password);

        return ObjectMapper.Map<CourierProfile, CourierProfileDto>(profile);
    }

    // ── UPDATE ────────────────────────────────────────────────
    public async Task<CourierProfileDto> UpdateAsync(Guid id, UpdateCourierDto input)
    {
        var profile = await _repository.GetAsync(id);
        profile.Update(input.FullName, input.Phone, input.Email, input.Zone);
        await _repository.UpdateAsync(profile, autoSave: true);
        return ObjectMapper.Map<CourierProfile, CourierProfileDto>(profile);
    }

    // ── DELETE ────────────────────────────────────────────────
    public async Task DeleteAsync(Guid id)
    {
        var profile = await _repository.GetAsync(id);

        // حذف الـ IdentityUser أيضاً
        var user = await _userRepository.GetAsync(profile.UserId);
        await _userRepository.DeleteAsync(user, autoSave: true);

        await _repository.DeleteAsync(id, autoSave: true);
    }

    // ── LOOKUP (dropdown) ─────────────────────────────────────
    public async Task<List<CourierLookupDto>> GetAvailableLookupAsync()
    {
        var items = await _repository.GetAvailableListAsync();
        return ObjectMapper.Map<List<CourierProfile>, List<CourierLookupDto>>(items);
    }

    // ── SET AVAILABILITY ──────────────────────────────────────
    public async Task<CourierProfileDto> SetAvailabilityAsync(Guid id, bool isAvailable)
    {
        var profile = await _repository.GetAsync(id);
        profile.SetAvailability(isAvailable);
        if (!isAvailable) profile.SetStatus(CourierStatus.Inactive);
        else profile.SetStatus(CourierStatus.Active);
        await _repository.UpdateAsync(profile, autoSave: true);
        return ObjectMapper.Map<CourierProfile, CourierProfileDto>(profile);
    }

    // ── SET STATUS ────────────────────────────────────────────
    public async Task<CourierProfileDto> SetStatusAsync(Guid id, CourierStatus status)
    {
        var profile = await _repository.GetAsync(id);
        profile.SetStatus(status);
        await _repository.UpdateAsync(profile, autoSave: true);
        return ObjectMapper.Map<CourierProfile, CourierProfileDto>(profile);
    }
}