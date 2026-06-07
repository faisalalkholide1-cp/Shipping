// modules/couriers/src/Couriers.Application/EventHandlers/IParcelCourierResolver.cs

using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Couriers.Application.EventHandlers;

/// <summary>
/// يجلب CourierId من ParcelId — ينفَّذ في ShippingManagement
/// لأنه يعرف الـ Parcel Entity
/// </summary>
public interface IParcelCourierResolver : ITransientDependency
{
    Task<Guid?> GetCourierIdByParcelIdAsync(Guid parcelId);
}