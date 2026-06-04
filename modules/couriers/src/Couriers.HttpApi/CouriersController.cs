using Couriers.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Couriers;

public abstract class CouriersController : AbpControllerBase
{
    protected CouriersController()
    {
        LocalizationResource = typeof(CouriersResource);
    }
}
