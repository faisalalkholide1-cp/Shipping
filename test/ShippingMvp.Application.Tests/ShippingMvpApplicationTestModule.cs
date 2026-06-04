using Volo.Abp.Modularity;

namespace ShippingMvp;

[DependsOn(
    typeof(ShippingMvpApplicationModule),
    typeof(ShippingMvpDomainTestModule)
)]
public class ShippingMvpApplicationTestModule : AbpModule
{

}
