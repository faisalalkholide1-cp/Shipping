using Couriers.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Couriers.Permissions;

public class CouriersPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CouriersPermissions.GroupName, L("Permission:CouriersManagement"));

        var couriers = myGroup.AddPermission(
            CouriersPermissions.Couriers.Default,
            L("Permission:CouriersManagement")
        );
        couriers.AddChild(CouriersPermissions.Couriers.Create, L("Permission:Couriers.Create"));
        couriers.AddChild(CouriersPermissions.Couriers.Edit, L("Permission:Couriers.Edit"));
        couriers.AddChild(CouriersPermissions.Couriers.Delete, L("Permission:Couriers.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CouriersResource>(name);
    }

}
