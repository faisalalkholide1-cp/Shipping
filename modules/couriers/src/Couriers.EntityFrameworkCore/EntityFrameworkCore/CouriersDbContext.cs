using Couriers.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Couriers.EntityFrameworkCore;

[ConnectionStringName(CouriersDbProperties.ConnectionStringName)]
public class CouriersDbContext : AbpDbContext<CouriersDbContext>, ICouriersDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */
    public DbSet<CourierProfile> CourierProfiles { get; set; }

    public CouriersDbContext(DbContextOptions<CouriersDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCouriers();
    }
}
