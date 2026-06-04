using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Modules.Parcels;

[DependsOn(
    typeof(ParcelsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ParcelsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ParcelsApplicationContractsModule).Assembly,
            ParcelsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ParcelsHttpApiClientModule>();
        });

    }
}
