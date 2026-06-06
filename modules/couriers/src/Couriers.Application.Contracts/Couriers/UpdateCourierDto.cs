using System;
using Couriers.Domain;
using Volo.Abp.Application.Dtos;

namespace Couriers.Application;

public class UpdateCourierDto
{
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Zone { get; set; }
}