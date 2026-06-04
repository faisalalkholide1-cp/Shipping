using Volo.Abp.Reflection;

namespace Modules.Parcels.Permissions;

public class ParcelsPermissions
{
    public const string GroupName = "Parcels";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ParcelsPermissions));
    }
}
