using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Modules.Parcels;
using Modules.Parcels.Parcels;
using ShippingManagement.Parcels.Dtos;
using Volo.Abp;

namespace ShippingManagement.Parcels;

[AllowAnonymous]
public class TrackingAppService : ParcelsAppService, ITrackingAppService
{
    private readonly IParcelRepository _parcelRepository;

    public TrackingAppService(IParcelRepository parcelRepository)
    {
        _parcelRepository = parcelRepository;
    }

    public async Task<TrackingResultDto> GetByTrackingNumberAsync(string trackingNumber)
    {
        Check.NotNullOrWhiteSpace(trackingNumber, nameof(trackingNumber));

        var parcel = await _parcelRepository.FindByTrackingNumberAsync(trackingNumber.Trim());

        if (parcel == null)
        {
            throw new BusinessException(ParcelsErrorCodes.ParcelNotFound)
                .WithData("TrackingNumber", trackingNumber);
        }

        return new TrackingResultDto
        {
            TrackingNumber = parcel.TrackingNumber,
            Status = parcel.Status,
            ReceiverName = parcel.ReceiverName,
            DeliveryAddress = parcel.DeliveryAddress,
            CreationTime = parcel.CreationTime,
            LastModificationTime = parcel.LastModificationTime,
            History = BuildHistory(parcel)
        };
    }

    private List<ParcelStatusHistoryDto> BuildHistory(Parcel parcel)
    {
        var history = new List<ParcelStatusHistoryDto>
        {
            new()
            {
                Status = ParcelStatus.Created,
                CreationTime = parcel.CreationTime
            }
        };

        if (parcel.Status > ParcelStatus.Created)
        {
            history.Add(new ParcelStatusHistoryDto
            {
                Status = parcel.Status,
                CreationTime = parcel.LastModificationTime ?? parcel.CreationTime
            });
        }

        return history;
    }
}
