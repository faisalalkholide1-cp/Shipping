// src/ShippingManagement.Application.Contracts/Parcels/Dtos/UpdateParcelDto.cs

using System.ComponentModel.DataAnnotations;

namespace ShippingManagement.Parcels.Dtos;

public class UpdateParcelDto
{
    [Required]
    [MaxLength(100)]
    public string SenderName { get; set; }

    [Required]
    [MaxLength(20)]
    public string SenderPhone { get; set; }

    [Required]
    [MaxLength(100)]
    public string ReceiverName { get; set; }

    [Required]
    [MaxLength(20)]
    public string ReceiverPhone { get; set; }

    [Required]
    [MaxLength(250)]
    public string PickupAddress { get; set; }

    [Required]
    [MaxLength(250)]
    public string DeliveryAddress { get; set; }

    [Range(0.1, 9999)]
    public double Weight { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }
}