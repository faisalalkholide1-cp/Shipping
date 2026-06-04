using Modules.Parcels.Localization;
using ShippingManagement.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Modules.Parcels.Permissions;

public class ParcelsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
             ShippingManagementPermissions.GroupName,
             L("Permission:ShippingManagement")
         );

        // ── Parcels ──────────────────────────────────
        var parcels = group.AddPermission(
            ShippingManagementPermissions.Parcels.Default,
            L("Permission:Parcels")
        );
        parcels.AddChild(ShippingManagementPermissions.Parcels.Create, L("Permission:Parcels.Create"));
        parcels.AddChild(ShippingManagementPermissions.Parcels.Edit, L("Permission:Parcels.Edit"));
        parcels.AddChild(ShippingManagementPermissions.Parcels.Delete, L("Permission:Parcels.Delete"));
        parcels.AddChild(ShippingManagementPermissions.Parcels.Assign, L("Permission:Parcels.Assign"));
        parcels.AddChild(ShippingManagementPermissions.Parcels.Deliver, L("Permission:Parcels.Deliver"));

        // ── Couriers ─────────────────────────────────
        var couriers = group.AddPermission(
            ShippingManagementPermissions.Couriers.Default,
            L("Permission:Couriers")
        );
        couriers.AddChild(ShippingManagementPermissions.Couriers.Create, L("Permission:Couriers.Create"));
        couriers.AddChild(ShippingManagementPermissions.Couriers.Edit, L("Permission:Couriers.Edit"));
        couriers.AddChild(ShippingManagementPermissions.Couriers.Delete, L("Permission:Couriers.Delete"));

        // ── Tracking ──────────────────────────────────
        group.AddPermission(
            ShippingManagementPermissions.Tracking.Default,
            L("Permission:Tracking")
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ParcelsResource>(name);
    }
}
