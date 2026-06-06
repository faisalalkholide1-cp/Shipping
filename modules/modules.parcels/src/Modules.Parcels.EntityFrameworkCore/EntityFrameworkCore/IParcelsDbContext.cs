using Microsoft.EntityFrameworkCore;
using ShippingManagement.Parcels;
using ShippingManagement.Parcels.StatusHistory;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Modules.Parcels.EntityFrameworkCore;

[ConnectionStringName(ParcelsDbProperties.ConnectionStringName)]
public interface IParcelsDbContext : IEfCoreDbContext
{
    DbSet<Parcel> Parcels { get; }
    DbSet<ParcelStatusHistory> ParcelStatusHistorys { get; }
}
