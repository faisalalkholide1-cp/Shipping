using Couriers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Modules.Parcels;

[DependsOn(
    typeof(CouriersApplicationContractsModule),
    typeof(ParcelsDomainModule),
    typeof(ParcelsApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
    )]
public class ParcelsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<ParcelsApplicationModule>();
    }
}
