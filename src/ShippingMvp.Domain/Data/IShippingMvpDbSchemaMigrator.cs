using System.Threading.Tasks;

namespace ShippingMvp.Data;

public interface IShippingMvpDbSchemaMigrator
{
    Task MigrateAsync();
}
