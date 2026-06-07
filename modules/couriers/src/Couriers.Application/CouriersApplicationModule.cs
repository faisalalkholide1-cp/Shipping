using Modules.Parcels;
// modules/couriers/src/Couriers.Application/CouriersApplicationModule.cs

using Couriers.Application;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace Couriers;

[DependsOn(
    typeof(ParcelsDomainModule),
    typeof(ParcelsApplicationContractsModule),
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

        // ✅ تسجيل الـ API Controllers تلقائياً
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(
                typeof(CouriersApplicationModule).Assembly,
                opts => { opts.RootPath = "couriers"; });
        });

        // ✅ تسجيل ICourierLookupService
        // ABP يسجله تلقائياً عبر ITransientDependency
        // لكن نضيفه صريحاً للوضوح
        //context.Services.AddTransient<ICourierLookupService, CourierLookupService>();
    }
}