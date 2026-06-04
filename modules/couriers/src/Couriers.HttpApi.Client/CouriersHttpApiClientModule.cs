using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Couriers;

[DependsOn(
    typeof(CouriersApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CouriersHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CouriersApplicationContractsModule).Assembly,
            CouriersRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CouriersHttpApiClientModule>();
        });

    }
}
