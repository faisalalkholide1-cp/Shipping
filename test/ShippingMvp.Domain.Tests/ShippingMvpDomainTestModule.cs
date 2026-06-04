using Volo.Abp.Modularity;

namespace ShippingMvp;

[DependsOn(
    typeof(ShippingMvpDomainModule),
    typeof(ShippingMvpTestBaseModule)
)]
public class ShippingMvpDomainTestModule : AbpModule
{

}
