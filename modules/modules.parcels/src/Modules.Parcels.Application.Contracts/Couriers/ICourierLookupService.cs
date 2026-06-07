// modules/Parcels/Parcels.Application.Contracts/Couriers/ICourierLookupService.cs

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Modules.Parcels.Couriers;

/// <summary>
/// Anti-Corruption Layer — واجهة محايدة يستخدمها Parcels Module
/// للتواصل مع Couriers Module بدون اعتماد مباشر
/// يُنفَّذ في Couriers.Application
/// </summary>
public interface ICourierLookupService : ITransientDependency
{
    /// <summary>
    /// جلب CourierProfile.Id عبر IdentityUser.Id
    /// يُستخدم في GetMyParcelsAsync لمعرفة المندوب الحالي
    /// </summary>
    Task<Guid?> FindCourierIdByUserIdAsync(
        Guid userId,
        CancellationToken ct = default);

    /// <summary>
    /// التحقق من وجود المندوب وأنه متاح قبل التعيين
    /// </summary>
    Task<bool> IsCourierAvailableAsync(
        Guid courierId,
        CancellationToken ct = default);

    /// <summary>
    /// جلب قائمة المناديب المتاحين للـ dropdown في صفحة الطرود
    /// </summary>
    Task<List<CourierLookupItemDto>> GetAvailableCouriersAsync(
        CancellationToken ct = default);
}

/// <summary>DTO مبسط — لا يعتمد على Couriers.Domain</summary>
public class CourierLookupItemDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Zone { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}