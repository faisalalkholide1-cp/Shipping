using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShippingMvp.Data;
using Volo.Abp.DependencyInjection;

namespace ShippingMvp.EntityFrameworkCore;

public class EntityFrameworkCoreShippingMvpDbSchemaMigrator
    : IShippingMvpDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreShippingMvpDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ShippingMvpDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ShippingMvpDbContext>()
            .Database
            .MigrateAsync();
    }
}
