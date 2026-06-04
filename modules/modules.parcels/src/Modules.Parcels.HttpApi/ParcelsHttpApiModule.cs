using Localization.Resources.AbpUi;
using Modules.Parcels.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Parcels;

[DependsOn(
    typeof(ParcelsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ParcelsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ParcelsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ParcelsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
