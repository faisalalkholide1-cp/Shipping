using System;
using Couriers.Domain;
using Volo.Abp.Application.Dtos;

namespace Couriers.Application;

public class CourierProfileDto : FullAuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Zone { get; set; }
    public CourierStatus Status { get; set; }
    public bool IsAvailable { get; set; }
    public int DeliveredCount { get; set; }
}