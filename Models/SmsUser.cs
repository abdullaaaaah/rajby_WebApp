using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class SmsUser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int EmpId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreateOn { get; set; }

    public int? CreateBy { get; set; }

    public string? CreateComp { get; set; }

    public DateTime? ModifyOn { get; set; }

    public string? ModifyBy { get; set; }

    public string? ModifyComp { get; set; }

    public string? UserGroup { get; set; }

    public string? UserType { get; set; }

    public string? EmailAddress { get; set; }

    public virtual ICollection<SetBuyer> Buyers { get; set; } = new List<SetBuyer>();
}
