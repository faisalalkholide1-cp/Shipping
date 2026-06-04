using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Modules.Parcels.Parcels;

public class ParcelStatusHistory : CreationAuditedEntity<Guid>
{
    public Guid ParcelId { get; private set; }

    public ParcelStatus Status { get; private set; }

    public string? Note { get; private set; }

    protected ParcelStatusHistory()
    {
    }

    internal ParcelStatusHistory(
        Guid id,
        Guid parcelId,
        ParcelStatus status,
        string? note) : base(id)
    {
        ParcelId = parcelId;
        Status = status;
        SetNote(note);
    }

    private void SetNote(string? note)
    {
        if (note.IsNullOrWhiteSpace())
        {
            Note = null;
            return;
        }

        Note = Check.Length(
            note,
            nameof(note),
            ParcelConsts.MaxStatusHistoryNoteLength);
    }
}
