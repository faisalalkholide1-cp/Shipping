namespace Couriers;

public static class CouriersDbProperties
{
    public static string DbTablePrefix { get; set; } = "Couriers";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Couriers";
}
