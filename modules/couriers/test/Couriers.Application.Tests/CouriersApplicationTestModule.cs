using Volo.Abp.Modularity;

namespace Couriers;

[DependsOn(
    typeof(CouriersApplicationModule),
    typeof(CouriersDomainTestModule)
    )]
public class CouriersApplicationTestModule : AbpModule
{

}
