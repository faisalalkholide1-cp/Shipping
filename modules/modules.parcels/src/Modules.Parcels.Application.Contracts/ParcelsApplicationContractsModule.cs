using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Modules.Parcels;

[DependsOn(
    typeof(ParcelsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ParcelsApplicationContractsModule : AbpModule
{

}
