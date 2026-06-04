using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ShippingMvp.Data;

/* This is used if database provider does't define
 * IShippingMvpDbSchemaMigrator implementation.
 */
public class NullShippingMvpDbSchemaMigrator : IShippingMvpDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
