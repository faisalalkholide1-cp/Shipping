using Modules.Parcels.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Modules.Parcels;

public abstract class ParcelsController : AbpControllerBase
{
    protected ParcelsController()
    {
        LocalizationResource = typeof(ParcelsResource);
    }
}
