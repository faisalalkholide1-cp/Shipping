using Volo.Abp.Modularity;

namespace ShippingMvp;

public abstract class ShippingMvpApplicationTestBase<TStartupModule> : ShippingMvpTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
