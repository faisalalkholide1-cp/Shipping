using Microsoft.EntityFrameworkCore;
using ShippingManagement.Parcels;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Modules.Parcels.EntityFrameworkCore;

[ConnectionStringName(ParcelsDbProperties.ConnectionStringName)]
public interface IParcelsDbContext : IEfCoreDbContext
{
    DbSet<Parcel> Parcels { get; }
}
