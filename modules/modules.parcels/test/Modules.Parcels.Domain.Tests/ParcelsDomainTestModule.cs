using Volo.Abp.Modularity;

namespace Modules.Parcels;

[DependsOn(
    typeof(ParcelsDomainModule),
    typeof(ParcelsTestBaseModule)
)]
public class ParcelsDomainTestModule : AbpModule
{

}
