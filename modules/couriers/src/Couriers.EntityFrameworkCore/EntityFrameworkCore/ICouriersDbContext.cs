using Couriers.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Couriers.EntityFrameworkCore;

[ConnectionStringName(CouriersDbProperties.ConnectionStringName)]
public interface ICouriersDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
     DbSet<CourierProfile> CourierProfiles { get; }
}
