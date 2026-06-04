namespace Modules.Parcels;

public static class ParcelsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Parcels";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Parcels";
}
