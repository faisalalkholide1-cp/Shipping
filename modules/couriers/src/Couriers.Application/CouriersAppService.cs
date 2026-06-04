using Couriers.Localization;
using Volo.Abp.Application.Services;

namespace Couriers;

public abstract class CouriersAppService : ApplicationService
{
    protected CouriersAppService()
    {
        LocalizationResource = typeof(CouriersResource);
        ObjectMapperContext = typeof(CouriersApplicationModule);
    }
}
