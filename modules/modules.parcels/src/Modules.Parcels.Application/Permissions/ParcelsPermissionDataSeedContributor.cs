using System.Threading.Tasks;
using ShippingManagement.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Modules.Parcels.Permissions;

public class ParcelsPermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public const string AdminRoleName = "admin";

    private readonly IPermissionDataSeeder _permissionDataSeeder;

    public ParcelsPermissionDataSeedContributor(IPermissionDataSeeder permissionDataSeeder)
    {
        _permissionDataSeeder = permissionDataSeeder;
    }

    [UnitOfWork]
    public virtual Task SeedAsync(DataSeedContext context)
    {
        return _permissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            AdminRoleName,
            ShippingManagementPermissions.GetAll());
    }
}
