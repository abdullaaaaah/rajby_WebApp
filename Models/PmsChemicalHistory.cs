using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class PmsChemicalHistory
{
    public long AutoGenId { get; set; }

    public long RequisitionId { get; set; }

    public long RequisitionDetId { get; set; }

    public double? PreviousQuantity { get; set; }

    public double? ApprovedQuantity { get; set; }

    public string? StatusChangedBy { get; set; }

    public string? StatusChangedComp { get; set; }

    public DateTime StatusChangedDate { get; set; }

    public string? Status { get; set; }

    public virtual PmsRequisitionCd Requisition { get; set; } = null!;

    public virtual PmsRequisitionDetCd RequisitionDet { get; set; } = null!;
}
