using System;
using System.Collections.Generic;

namespace Hr_policy.Models;

public partial class MasterRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
