using Volo.Abp.Settings;

namespace ShippingMvp.Settings;

public class ShippingMvpSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ShippingMvpSettings.MySetting1));
    }
}
