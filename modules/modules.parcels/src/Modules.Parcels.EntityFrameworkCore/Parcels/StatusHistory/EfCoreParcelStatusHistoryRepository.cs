using Microsoft.EntityFrameworkCore;
using Modules.Parcels.EntityFrameworkCore;
using ShippingManagement.Parcels.StatusHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Modules.Parcels.Parcels.StatusHistory
{
    public class EfCoreParcelStatusHistoryRepository : EfCoreRepository<IParcelsDbContext, ParcelStatusHistory, Guid>,
      IParcelStatusHistoryRepository
    {
        public EfCoreParcelStatusHistoryRepository(
            IDbContextProvider<IParcelsDbContext> dbContextProvider)
            : base(dbContextProvider) { }

        public async Task<List<ParcelStatusHistory>> GetListByParcelIdAsync(
            Guid parcelId,
            CancellationToken ct = default)
        {
            var db = await GetDbSetAsync();

            return await db
                .Where(h => h.ParcelId == parcelId)
                .OrderBy(h => h.ChangedAt)
                .ToListAsync(ct);
        }
    }
}
