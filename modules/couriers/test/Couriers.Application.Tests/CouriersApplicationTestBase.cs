using Volo.Abp.Modularity;

namespace Couriers;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CouriersApplicationTestBase<TStartupModule> : CouriersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
