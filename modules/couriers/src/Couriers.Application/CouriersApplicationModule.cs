using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Couriers;

[DependsOn(
    typeof(CouriersDomainModule),
    typeof(CouriersApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
    )]
public class CouriersApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<CouriersApplicationModule>();

        Configure<Volo.Abp.AspNetCore.Mvc.AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(
                typeof(CouriersApplicationModule).Assembly,
                opts =>
                {
                    opts.RootPath = "couriers"; // /api/app/couriers/...
                });
        });

    }
}
