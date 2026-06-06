using Couriers.Application;
using Couriers.Domain;
using Riok.Mapperly.Abstractions;

using Volo.Abp.Mapperly;


namespace Modules.Couriers;

[Mapper]
public partial class CourierAutoMapperProfile : MapperBase<CourierProfile, CourierProfileDto>
{
    public override partial CourierProfileDto Map(CourierProfile parcel);

    public override partial void Map(CourierProfile source, CourierProfileDto destination);

}

[Mapper]
public partial class CourierLookupMapperProfile : MapperBase<CourierProfile, CourierLookupDto>
{
    public override partial CourierLookupDto Map(CourierProfile parcel);

    public override partial void Map(CourierProfile source, CourierLookupDto destination);
}