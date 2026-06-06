using Localization.Resources.AbpUi;
using Couriers.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Couriers;

[DependsOn(
    typeof(CouriersApplicationContractsModule),
    //typeof(CouriersApplicationModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CouriersHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CouriersHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CouriersResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
