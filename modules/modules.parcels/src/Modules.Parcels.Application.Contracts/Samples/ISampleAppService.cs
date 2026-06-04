using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Modules.Parcels.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
