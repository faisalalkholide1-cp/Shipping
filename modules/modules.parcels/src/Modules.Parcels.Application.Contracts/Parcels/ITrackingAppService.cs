using Modules.Parcels;
using System.Threading.Tasks;
using ShippingManagement.Parcels.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace ShippingManagement.Parcels;

[RemoteService(Name = ParcelsRemoteServiceConsts.RemoteServiceName)]
public interface ITrackingAppService : IApplicationService
{
    Task<TrackingResultDto> GetByTrackingNumberAsync(string trackingNumber);
}
