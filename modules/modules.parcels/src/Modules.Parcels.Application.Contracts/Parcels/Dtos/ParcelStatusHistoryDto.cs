using System;
using Volo.Abp.Application.Dtos;

namespace Modules.Parcels.Parcels.Dtos
{
    public class ParcelStatusHistoryDto : EntityDto<Guid>
    {
        public ParcelStatus Status { get; set; }
        public string? Note { get; set; }
        public Guid? ChangedByUserId { get; set; }
        public string? ChangedByName { get; set; } // اسم المستخدم — يُجلب من ABP
        public DateTime ChangedAt { get; set; }
    }
}
