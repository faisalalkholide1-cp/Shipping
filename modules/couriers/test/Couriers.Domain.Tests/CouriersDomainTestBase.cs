using Volo.Abp.Modularity;

namespace Couriers;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class CouriersDomainTestBase<TStartupModule> : CouriersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
