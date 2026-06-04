using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Couriers;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CouriersDomainSharedModule)
)]
public class CouriersDomainModule : AbpModule
{

}
