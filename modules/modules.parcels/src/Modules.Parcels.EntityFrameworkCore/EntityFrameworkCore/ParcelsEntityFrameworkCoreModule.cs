using Microsoft.Extensions.DependencyInjection;
using Modules.Parcels.Parcels.StatusHistory;
using ShippingManagement.EntityFrameworkCore.Parcels;
using ShippingManagement.Parcels;
using ShippingManagement.Parcels.StatusHistory;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Modules.Parcels.EntityFrameworkCore;

[DependsOn(
    typeof(ParcelsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ParcelsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ParcelsDbContext>(options =>
        {
            options.AddDefaultRepositories<IParcelsDbContext>();
            options.AddRepository<Parcel, EfCoreParcelRepository>();
            options.AddRepository<ParcelStatusHistory, EfCoreParcelStatusHistoryRepository>();
        });
    }
}
