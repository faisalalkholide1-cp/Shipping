using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ShippingMvp.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ShippingMvpDbContextFactory : IDesignTimeDbContextFactory<ShippingMvpDbContext>
{
    public ShippingMvpDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        ShippingMvpEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ShippingMvpDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new ShippingMvpDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ShippingMvp.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
