﻿using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class PmsRequisitionHistory
{
    public long AutoGenId { get; set; }

    public long RequisitionId { get; set; }

    public long RequisitionDetId { get; set; }

    public double? PreviousQuantity { get; set; }

    public double? ApprovedQuantity { get; set; }

    public string? StatusChangedBy { get; set; }

    public DateTime StatusChangedDate { get; set; }

    public string? StatusChangedComp { get; set; }

    public string? Status { get; set; }

    public virtual PmsRequisition Requisition { get; set; } = null!;

    public virtual PmsRequisitionDetGsp RequisitionDet { get; set; } = null!;
}
