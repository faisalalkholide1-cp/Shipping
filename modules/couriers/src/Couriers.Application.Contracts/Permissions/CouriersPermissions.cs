using Volo.Abp.Reflection;

namespace Couriers.Permissions;

public class CouriersPermissions
{
    public const string GroupName = "CouriersManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CouriersPermissions));
    }

    public static class Couriers
    {
        public const string Default = GroupName + ".Couriers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
