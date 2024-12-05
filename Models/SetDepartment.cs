using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class SetDepartment
{
    public long DeptId { get; set; }

    public int MainId { get; set; }

    public long SubId { get; set; }

    public long DetId { get; set; }

    public long? GrpId { get; set; }

    public bool? DeleteTag { get; set; }

    public string? DeptAbbr { get; set; }

    public int? DeptSeqForUtility { get; set; }

    public string? Createby { get; set; }

    public DateTime? Createon { get; set; }

    public string? CreateComp { get; set; }

    public string? Modifyby { get; set; }

    public DateTime? Modifyon { get; set; }

    public string? ModifyComp { get; set; }

    public virtual SetSetup Det { get; set; } = null!;

    public virtual ICollection<PmsRequisitionCd> PmsRequisitionCds { get; set; } = new List<PmsRequisitionCd>();

    public virtual ICollection<PmsRequisition> PmsRequisitions { get; set; } = new List<PmsRequisition>();

    public virtual SetSetup Sub { get; set; } = null!;
}
