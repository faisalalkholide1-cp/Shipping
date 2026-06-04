using Volo.Abp.Reflection;

namespace ShippingManagement.Permissions;

public static class ShippingManagementPermissions
{
    public const string GroupName = "ShippingManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ShippingManagementPermissions));
    }

    public static class Parcels
    {
        public const string Default = GroupName + ".Parcels";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Assign = Default + ".Assign";
        public const string Deliver = Default + ".Deliver";
    }

    public static class Couriers
    {
        public const string Default = GroupName + ".Couriers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Tracking
    {
        public const string Default = GroupName + ".Tracking";
    }
}