using Volo.Abp.Modularity;

namespace Modules.Parcels;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ParcelsApplicationTestBase<TStartupModule> : ParcelsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
