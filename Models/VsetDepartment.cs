using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class VsetDepartment
{
    public long DeptId { get; set; }

    public int MainId { get; set; }

    public long SubId { get; set; }

    public long DetId { get; set; }

    public long? GrpId { get; set; }

    public string DeptMain { get; set; } = null!;

    public string DeptSub { get; set; } = null!;

    public string DeptDet { get; set; } = null!;

    public string DeptGrp { get; set; } = null!;

    public string DeptAbbr { get; set; } = null!;

    public string Machine { get; set; } = null!;

    public long MachineId { get; set; }

    public int DeptSeqForUtility { get; set; }

    public bool? DeleteTag { get; set; }

    public long DeptGlid { get; set; }
}
