using ShippingManagement.Parcels.StatusHistory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Modules.Parcels.Parcels.StatusHistory
{
    public interface IParcelStatusHistoryRepository : IRepository<ParcelStatusHistory, Guid>
    {
        /// <summary>جلب كل سجلات تاريخ طرد معين مرتبة زمنياً</summary>
        Task<List<ParcelStatusHistory>> GetListByParcelIdAsync(
            Guid parcelId,
            CancellationToken ct = default);
    }
}
