using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class CmsApprovalHistory
{
    public long AutoGenId { get; set; }

    public long CostingId { get; set; }

    public double? SellPrice { get; set; }

    public double? ApprovedPrice { get; set; }

    public long? CommentsId { get; set; }

    public string StatusChangedBy { get; set; } = null!;

    public DateTime StatusChangedOn { get; set; }

    public string StatusChangedComp { get; set; } = null!;

    public string? Approvalstatus { get; set; }

    public virtual SetSetup? Comments { get; set; }

    public virtual CmsPreCosting Costing { get; set; } = null!;
}
