using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class PmsRequisitionDetGsp
{
    public long RequisitionDetId { get; set; }

    public long? RequisitionId { get; set; }

    public long? ItemId { get; set; }

    public long? UomId { get; set; }

    public double? Qty { get; set; }

    public string? Comments { get; set; }

    public long? MirDetId { get; set; }

    public long? MachineId { get; set; }

    public double? AvailableQty { get; set; }

    public double? QtyToProcure { get; set; }

    public long? AlternateItemId { get; set; }

    public long? RqdforId { get; set; }

    public long? SinRetDetId { get; set; }

  public string? Status { get; set; }

  public virtual SetItemCd? Item { get; set; }

    public virtual PmsRequisition? Requisition { get; set; }
}
