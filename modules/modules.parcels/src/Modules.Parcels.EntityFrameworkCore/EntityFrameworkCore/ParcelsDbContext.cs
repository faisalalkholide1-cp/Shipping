using Microsoft.EntityFrameworkCore;
using ShippingManagement.Parcels;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Modules.Parcels.EntityFrameworkCore;

[ConnectionStringName(ParcelsDbProperties.ConnectionStringName)]
public class ParcelsDbContext : AbpDbContext<ParcelsDbContext>, IParcelsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */
    public DbSet<Parcel> Parcels { get; set; }

    public ParcelsDbContext(DbContextOptions<ParcelsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureParcels();
        //builder.ParcelConfiguration()
    }
}
