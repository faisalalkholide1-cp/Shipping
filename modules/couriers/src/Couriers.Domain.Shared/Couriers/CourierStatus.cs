namespace Couriers.Domain;

public enum CourierStatus
{
    Active = 0,  // نشط ويمكن تعيينه
    Inactive = 1,  // غير نشط
    Busy = 2,  // لديه طرود مفتوحة حالياً
}