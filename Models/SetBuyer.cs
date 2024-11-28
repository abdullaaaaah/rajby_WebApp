using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class SetBuyer
{
    public string BuyerMainId { get; set; } = null!;

    public string BuyerSubId { get; set; } = null!;

    public string BuyerDetId { get; set; } = null!;

    public string BuyerGrpId { get; set; } = null!;

    public string BuyerId { get; set; } = null!;

    public string BuyerName { get; set; } = null!;

    public string? Comments { get; set; }

    public string? Createby { get; set; }

    public DateTime? Createon { get; set; }

    public string? Modifyby { get; set; }

    public DateTime? Modifyon { get; set; }

    public string? BuyerPriorId { get; set; }

    public int? CreditDays { get; set; }

    public bool? IsActive { get; set; }

    public double? OpDr { get; set; }

    public double? OpCr { get; set; }

    public bool? IsAsLocal { get; set; }

    public long? OpCurrencyId { get; set; }

    public double? OpFcrate { get; set; }

    public double? OpFcamountDr { get; set; }

    public double? OpFcamountCr { get; set; }

    public long? Glid { get; set; }

    public string? GlidcreateBy { get; set; }

    public DateTime? GlidcreateOn { get; set; }

    public string? GlidcreateComp { get; set; }

    public bool? IsEfs { get; set; }

    public string? Efsno { get; set; }

    public DateTime? EfsstartDt { get; set; }

    public DateTime? EfsendDt { get; set; }

    public virtual ICollection<CmsPreCosting> CmsPreCostings { get; set; } = new List<CmsPreCosting>();
}
