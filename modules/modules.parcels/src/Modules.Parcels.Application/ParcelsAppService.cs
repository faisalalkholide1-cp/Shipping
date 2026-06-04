using Modules.Parcels.Localization;
using Volo.Abp.Application.Services;

namespace Modules.Parcels;

public abstract class ParcelsAppService : ApplicationService
{
    protected ParcelsAppService()
    {
        LocalizationResource = typeof(ParcelsResource);
        ObjectMapperContext = typeof(ParcelsApplicationModule);
    }
}
