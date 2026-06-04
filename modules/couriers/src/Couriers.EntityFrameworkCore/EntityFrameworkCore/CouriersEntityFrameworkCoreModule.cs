using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Couriers.EntityFrameworkCore;

[DependsOn(
    typeof(CouriersDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CouriersEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CouriersDbContext>(options =>
        {
            options.AddDefaultRepositories<ICouriersDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
