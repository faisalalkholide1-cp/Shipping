// modules/Parcels/Parcels.Domain/Parcels/ParcelManager.cs
using Modules.Parcels.Parcels;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
namespace ShippingManagement.Parcels;

public class ParcelManager : DomainService
{
    private readonly IParcelRepository _parcelRepository;

    public ParcelManager(IParcelRepository parcelRepository)
    {
        _parcelRepository = parcelRepository;
    }

    public async Task<Parcel> CreateAsync(
        string senderName, string senderPhone,
        string receiverName, string receiverPhone,
        string pickupAddress, string deliveryAddress,
        double weight, decimal price)
    {
        var trackingNumber = await GenerateTrackingNumberAsync();

        return new Parcel(
            GuidGenerator.Create(),
            trackingNumber,
            senderName, senderPhone,
            receiverName, receiverPhone,
            pickupAddress, deliveryAddress,
            weight, price
        );
    }

    private async Task<string> GenerateTrackingNumberAsync()
    {
        string trackingNumber;
        do
        {
            trackingNumber = "SHP" + Clock.Now.ToString("yyyyMMdd")
                             + RandomHelper.GetRandom(100000, 999999);
        }
        while (await _parcelRepository.TrackingNumberExistsAsync(trackingNumber));

        return trackingNumber;
    }
}