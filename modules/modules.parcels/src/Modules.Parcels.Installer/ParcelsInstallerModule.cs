using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Modules.Parcels;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ParcelsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ParcelsInstallerModule>();
        });
    }
}
