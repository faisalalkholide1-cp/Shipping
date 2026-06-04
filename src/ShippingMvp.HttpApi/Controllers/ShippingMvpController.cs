using ShippingMvp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShippingMvp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ShippingMvpController : AbpControllerBase
{
    protected ShippingMvpController()
    {
        LocalizationResource = typeof(ShippingMvpResource);
    }
}
