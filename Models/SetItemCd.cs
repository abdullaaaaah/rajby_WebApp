using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class SetItemCd
{
    public long ItemId { get; set; }

    public long MainId { get; set; }

    public long SubId { get; set; }

    public long DetId { get; set; }

    public long? GrpId { get; set; }

    public long? Lvl5Id { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public long? GenericId { get; set; }

    public long? Uomid { get; set; }

    public bool? DeleteTag { get; set; }

    public string? Createby { get; set; }

    public DateTime? Createon { get; set; }

    public string? CreateComp { get; set; }

    public string? Modifyby { get; set; }

    public DateTime? Modifyon { get; set; }

    public string? ModifyComp { get; set; }

    public string? SupplierId { get; set; }

    public string? GrpCode { get; set; }

    public bool? Financing { get; set; }

    public long? ItemStateId { get; set; }

    public int? UnitId { get; set; }

    public long? StoreId { get; set; }

    public string? OldCode { get; set; }

    public string? Lvl4Code { get; set; }

    public string? Lvl3Code { get; set; }

    public double? AddPcnt { get; set; }

    public string? Hscode { get; set; }

    public string? HssubCode { get; set; }

    public double? AddPcntEfs { get; set; }

    public long? Glid { get; set; }

    public string? GlidcreateBy { get; set; }

    public DateTime? GlidcreateOn { get; set; }

    public string? GlidcreateComp { get; set; }

    /// <summary>
    /// 1 for Include in Consumption and 2 for 
    /// </summary>
    public int? IsInclude { get; set; }

    public virtual SetSetup Det { get; set; } = null!;

    public virtual SetSetup Main { get; set; } = null!;

    public virtual ICollection<PmsRequisitionDetCd> PmsRequisitionDetCds { get; set; } = new List<PmsRequisitionDetCd>();

    public virtual ICollection<PmsRequisitionDetGsp> PmsRequisitionDetGsps { get; set; } = new List<PmsRequisitionDetGsp>();

    public virtual SetSetup Sub { get; set; } = null!;

    public virtual SetSetup? Uom { get; set; }
}
