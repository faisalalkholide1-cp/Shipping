using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Modules.Parcels;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ParcelsDomainSharedModule)
)]
public class ParcelsDomainModule : AbpModule
{

}
