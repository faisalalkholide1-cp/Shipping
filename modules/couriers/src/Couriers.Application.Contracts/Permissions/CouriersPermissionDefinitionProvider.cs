using Couriers.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Couriers.Permissions;

public class CouriersPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CouriersPermissions.GroupName, L("Permission:Couriers"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CouriersResource>(name);
    }
}
