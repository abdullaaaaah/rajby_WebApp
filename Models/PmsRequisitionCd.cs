﻿using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class PmsRequisitionCd
{
    public int? UnitId { get; set; }

    public long RequisitionId { get; set; }

    public string? DocPrefix { get; set; }

    public string? DocSuffix { get; set; }

    public string? DocId { get; set; }

    public DateTime? DocDt { get; set; }

    public long? DeptId { get; set; }

    public long? StoreId { get; set; }

    public string? Comments { get; set; }

    public string? Createby { get; set; }

    public DateTime? Createon { get; set; }

    public string? Createcomp { get; set; }

    public string? Modifyby { get; set; }

    public DateTime? Modifyon { get; set; }

    public string? ModifyComp { get; set; }

    public long? Mainid { get; set; }

    public long? ReqTypeId { get; set; }

    public long? StatusId { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public string? ApprovedComp { get; set; }

    public string? ApprovedBy2 { get; set; }

    public DateTime? ApprovedOn2 { get; set; }

    public string? ApprovedComp2 { get; set; }

    public long? SinRetId { get; set; }

    public long? ProjectId { get; set; }

    public virtual SetDepartment? Dept { get; set; }

    public virtual ICollection<PmsChemicalHistory> PmsChemicalHistories { get; set; } = new List<PmsChemicalHistory>();

    public virtual ICollection<PmsRequisitionDetCd> PmsRequisitionDetCds { get; set; } = new List<PmsRequisitionDetCd>();

    public virtual SetSetup? Store { get; set; }
}
