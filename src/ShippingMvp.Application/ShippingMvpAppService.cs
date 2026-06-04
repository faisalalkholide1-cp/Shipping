using ShippingMvp.Localization;
using Volo.Abp.Application.Services;

namespace ShippingMvp;

/* Inherit your application services from this class.
 */
public abstract class ShippingMvpAppService : ApplicationService
{
    protected ShippingMvpAppService()
    {
        LocalizationResource = typeof(ShippingMvpResource);
    }
}
