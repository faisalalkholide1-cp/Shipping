using Microsoft.Extensions.Localization;
using ShippingMvp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ShippingMvp;

[Dependency(ReplaceServices = true)]
public class ShippingMvpBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ShippingMvpResource> _localizer;

    public ShippingMvpBrandingProvider(IStringLocalizer<ShippingMvpResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
