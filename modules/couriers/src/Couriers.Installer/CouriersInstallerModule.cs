using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Couriers;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CouriersInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CouriersInstallerModule>();
        });
    }
}
