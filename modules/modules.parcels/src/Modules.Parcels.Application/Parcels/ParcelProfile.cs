using Riok.Mapperly.Abstractions;
using ShippingManagement.Parcels;
using ShippingManagement.Parcels.Dtos;
using ShippingManagement.Parcels.StatusHistory;
using Volo.Abp.Mapperly;


namespace Modules.Parcels;

[Mapper]
public partial class ParcelMapper : MapperBase<Parcel, ParcelDto>
{
    public override partial ParcelDto Map(Parcel parcel);

    public override partial void Map(Parcel source, ParcelDto destination);

}

[Mapper]
public partial class StatusHistoryMapper : MapperBase<ParcelStatusHistory, ParcelStatusHistoryDto>
{
    public override partial ParcelStatusHistoryDto Map(ParcelStatusHistory parcel);

    public override partial void Map(ParcelStatusHistory source, ParcelStatusHistoryDto destination);

}