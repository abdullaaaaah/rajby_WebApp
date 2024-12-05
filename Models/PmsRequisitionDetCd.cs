using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class PmsRequisitionDetCd
{
    public long RequisitionDetId { get; set; }

    public long? RequisitionId { get; set; }

    public long? ItemId { get; set; }

    public long? Uomid { get; set; }

    public double? Qty { get; set; }

    public string? Comments { get; set; }

    public long? PackingId { get; set; }

    public double? KgConversion { get; set; }

    public double? QtyInKg { get; set; }

    public double? RateKg { get; set; }

    public long? MirDetId { get; set; }

    public long? MachineId { get; set; }

    public double? AvailableQty { get; set; }

    public double? QtyToProcure { get; set; }

    public long? AlternateItemId { get; set; }

    public long? RqdforId { get; set; }

    public long? SinRetDetId { get; set; }

    public string? Status { get; set; }

    public virtual SetItemCd? Item { get; set; }

    public virtual ICollection<PmsChemicalHistory> PmsChemicalHistories { get; set; } = new List<PmsChemicalHistory>();

    public virtual ICollection<PmsRequisitionHistory> PmsRequisitionHistories { get; set; } = new List<PmsRequisitionHistory>();

    public virtual PmsRequisitionCd? Requisition { get; set; }
}
