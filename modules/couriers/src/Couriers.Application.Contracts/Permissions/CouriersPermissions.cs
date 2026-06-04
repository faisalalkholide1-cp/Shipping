using Volo.Abp.Reflection;

namespace Couriers.Permissions;

public class CouriersPermissions
{
    public const string GroupName = "Couriers";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CouriersPermissions));
    }
}
