using Volo.Abp.Modularity;

namespace Modules.Parcels;

[DependsOn(
    typeof(ParcelsApplicationModule),
    typeof(ParcelsDomainTestModule)
    )]
public class ParcelsApplicationTestModule : AbpModule
{

}
