using Volo.Abp.Modularity;

namespace Modules.Parcels;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ParcelsDomainTestBase<TStartupModule> : ParcelsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
