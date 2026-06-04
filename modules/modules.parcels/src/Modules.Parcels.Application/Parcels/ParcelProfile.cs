using Riok.Mapperly.Abstractions;
using ShippingManagement.Parcels;
using ShippingManagement.Parcels.Dtos;
using Volo.Abp.Mapperly;


namespace Modules.Parcels;

[Mapper]
public partial class ParcelMapper : MapperBase<Parcel, ParcelDto>
{
    public override partial ParcelDto Map(Parcel parcel);

    public override partial void Map(Parcel source, ParcelDto destination);

    //public override partial Parcel Map(CreateParcelDto source);

    //public override partial void Map(CreateParcelDto source, Parcel destination);



}