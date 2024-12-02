using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class SetUserAccessBuyer
{
    public int UserId { get; set; }

    public string BuyerId { get; set; } = null!;
}
