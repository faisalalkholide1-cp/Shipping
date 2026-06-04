// src/ShippingManagement.Application.Contracts/Parcels/Dtos/AssignCourierDto.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagement.Parcels.Dtos;

public class AssignCourierDto
{
    [Required]
    public Guid CourierId { get; set; }
}