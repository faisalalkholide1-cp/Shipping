using ShippingMvp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ShippingMvp.Permissions;

public class ShippingMvpPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ShippingMvpPermissions.GroupName);

        //var booksPermission = myGroup.AddPermission(ShippingMvpPermissions.Books.Default, L("Permission:Books"));
        //booksPermission.AddChild(ShippingMvpPermissions.Books.Create, L("Permission:Books.Create"));
        //booksPermission.AddChild(ShippingMvpPermissions.Books.Edit, L("Permission:Books.Edit"));
        //booksPermission.AddChild(ShippingMvpPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ShippingMvpPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ShippingMvpResource>(name);
    }
}
