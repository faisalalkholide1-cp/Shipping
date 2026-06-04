using Volo.Abp.Modularity;

namespace ShippingMvp;

/* Inherit from this class for your domain layer tests. */
public abstract class ShippingMvpDomainTestBase<TStartupModule> : ShippingMvpTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
