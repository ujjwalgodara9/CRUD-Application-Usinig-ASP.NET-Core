using System;
using System.Collections.Generic;

namespace Hr_policy.Models;

public partial class User
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public int RoleId { get; set; }

    public bool Status { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string Email { get; set; } = null!;

    public virtual MasterRole Role { get; set; } = null!;
}
