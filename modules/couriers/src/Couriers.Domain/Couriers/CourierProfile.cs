using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Couriers.Domain;

/// <summary>
/// بيانات إضافية للمندوب — مرتبطة بـ IdentityUser عبر UserId
/// </summary>
public class CourierProfile : FullAuditedAggregateRoot<Guid>
{
    /// <summary>معرف المستخدم من جدول AbpUsers</summary>
    public Guid UserId { get; private set; }
    public string FullName { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string Zone { get; private set; }
    public CourierStatus Status { get; private set; }
    public bool IsAvailable { get; private set; }
    public int DeliveredCount { get; private set; }

    private CourierProfile() { }

    public CourierProfile(Guid id, Guid userId, string fullName,
                          string phone, string email, string zone)
    {
        Id = id;
        UserId = userId;
        FullName = fullName;
        Phone = phone;
        Email = email;
        Zone = zone;
        Status = CourierStatus.Active;
        IsAvailable = true;
        DeliveredCount = 0;
    }

    public void Update(string fullName, string phone, string email, string zone)
    {
        FullName = fullName;
        Phone = phone;
        Email = email;
        Zone = zone;
    }

    public void SetAvailability(bool isAvailable) => IsAvailable = isAvailable;
    public void SetStatus(CourierStatus status) => Status = status;
    public void IncrementDelivered() => DeliveredCount++;
}