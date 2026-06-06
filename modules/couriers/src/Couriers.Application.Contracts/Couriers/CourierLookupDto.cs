using System;
using Couriers.Domain;
using Volo.Abp.Application.Dtos;

namespace Couriers.Application;

/// <summary>يُستخدم في dropdown تعيين المندوب في صفحة الطرود</summary>
public class CourierLookupDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Zone { get; set; }
    public bool IsAvailable { get; set; }
}