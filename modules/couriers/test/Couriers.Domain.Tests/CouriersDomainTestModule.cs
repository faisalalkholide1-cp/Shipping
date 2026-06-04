using Volo.Abp.Modularity;

namespace Couriers;

[DependsOn(
    typeof(CouriersDomainModule),
    typeof(CouriersTestBaseModule)
)]
public class CouriersDomainTestModule : AbpModule
{

}
