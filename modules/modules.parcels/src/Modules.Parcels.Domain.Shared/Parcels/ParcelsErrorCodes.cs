namespace ShippingManagement.Parcels;

public static class ParcelsErrorCodes
{
    public const string ParcelNotFound = "Parcels:ParcelNotFound";

    public const string InvalidStatusTransition = "Parcels:InvalidStatusTransition";

    public const string CourierAlreadyAssigned = "Parcels:CourierAlreadyAssigned";

    public const string CannotModifyParcel = "Parcels:CannotModifyParcel";

    public const string TrackingNumberGenerationFailed = "Parcels:TrackingNumberGenerationFailed";

    public const string InvalidParcelWeight = "Parcels:0003";

    public const string InvalidParcelPrice = "Parcels:0004";

}
