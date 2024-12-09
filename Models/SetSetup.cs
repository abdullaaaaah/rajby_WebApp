﻿using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class SetSetup
{
    public long SetsetupId { get; set; }

    public string SetsetupSegid { get; set; } = null!;

    public string Setsetupcode { get; set; } = null!;

    public string SetsetupName { get; set; } = null!;

    public string? SetsetupComm { get; set; }

    public string? SetSetupComm2 { get; set; }

    public string? SetSetupComm3 { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreateOn { get; set; }

    public string? CreateBy { get; set; }

    public string? CreateComp { get; set; }

    public DateTime? Modifyon { get; set; }

    public string? ModifyBy { get; set; }

    public string? ModifyComp { get; set; }

    public string? SetSetupComm4 { get; set; }

    public double? ForRajby { get; set; }

    public int? Sequence { get; set; }

    public string? SetSetupComm5 { get; set; }

    public string? OtherLanguage { get; set; }

    public long? Glid { get; set; }

    public bool? IsCotton { get; set; }

    public virtual ICollection<CmsApprovalHistory> CmsApprovalHistories { get; set; } = new List<CmsApprovalHistory>();

    public virtual ICollection<CmsPreCosting> CmsPreCostings { get; set; } = new List<CmsPreCosting>();

    public virtual ICollection<PmsRequisitionCd> PmsRequisitionCds { get; set; } = new List<PmsRequisitionCd>();

    public virtual ICollection<PmsRequisition> PmsRequisitions { get; set; } = new List<PmsRequisition>();

    public virtual ICollection<SetDepartment> SetDepartmentDets { get; set; } = new List<SetDepartment>();

    public virtual ICollection<SetDepartment> SetDepartmentSubs { get; set; } = new List<SetDepartment>();

    public virtual ICollection<SetItemCd> SetItemCdDets { get; set; } = new List<SetItemCd>();

    public virtual ICollection<SetItemCd> SetItemCdMains { get; set; } = new List<SetItemCd>();

    public virtual ICollection<SetItemCd> SetItemCdSubs { get; set; } = new List<SetItemCd>();

    public virtual ICollection<SetItemCd> SetItemCdUoms { get; set; } = new List<SetItemCd>();
}
