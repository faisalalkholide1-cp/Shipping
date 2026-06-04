using ShippingMvp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ShippingMvp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ShippingMvpEntityFrameworkCoreModule),
    typeof(ShippingMvpApplicationContractsModule)
)]
public class ShippingMvpDbMigratorModule : AbpModule
{
}
