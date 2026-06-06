using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Couriers.Domain;

/// <summary>
/// Domain Service — يتحكم في منطق إنشاء وتعديل المناديب
/// </summary>
public class CourierManager : DomainService 
{
    private readonly ICourierProfileRepository _repository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;

    public CourierManager(
        ICourierProfileRepository repository,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager)
    {
        _repository = repository;
        _userRepository = userRepository;
        _userManager = userManager;
    }

    /// <summary>
    /// إنشاء مستخدم جديد في ABP ثم ربطه بـ CourierProfile
    /// </summary>
    public async Task<CourierProfile> CreateAsync(
        string fullName,
        string phone,
        string email,
        string zone,
        string password)
    {
        // 1. تأكد أن الإيميل غير مستخدم
        var existing = await _userRepository.FindByNormalizedEmailAsync(email.ToUpperInvariant());
        if (existing != null)
            throw new BusinessException("Couriers:EmailAlreadyExists")
                .WithData("Email", email);

        // 2. أنشئ IdentityUser
        var user = new IdentityUser(GuidGenerator.Create(), email, email, CurrentTenant.Id);
        user.Name = fullName;
        await _userManager.CreateAsync(user, password);

        // 3. أضف Role "Courier"
        await _userManager.AddToRoleAsync(user, "Courier");

        // 4. أنشئ CourierProfile
        var profile = new CourierProfile(
            GuidGenerator.Create(),
            user.Id,
            fullName, phone, email, zone);

        return await _repository.InsertAsync(profile, autoSave: true);
    }

    /// <summary>
    /// تحديث حالة المندوب تلقائياً بناءً على الطرود المعينة له
    /// </summary>
    public async Task SetBusyAsync(Guid courierId)
    {
        var profile = await _repository.GetAsync(courierId);
        profile.SetStatus(CourierStatus.Busy);
        profile.SetAvailability(false);
        await _repository.UpdateAsync(profile, autoSave: true);
    }

    /// <summary>
    /// إعادة تفعيل المندوب بعد تسليم أو إلغاء الطرد
    /// </summary>
    public async Task SetAvailableAsync(Guid courierId, bool incrementDelivered = false)
    {
        var profile = await _repository.GetAsync(courierId);
        profile.SetStatus(CourierStatus.Active);
        profile.SetAvailability(true);
        if (incrementDelivered) profile.IncrementDelivered();
        await _repository.UpdateAsync(profile, autoSave: true);
    }
}