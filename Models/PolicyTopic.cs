using System;
using System.Collections.Generic;

namespace Hr_policy.Models;

public partial class PolicyTopic
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    public int Status { get; set; }

    public string Remarks { get; set; } = null!;

    public string InsertedBy { get; set; } = null!;

    public DateTime InsertedOn { get; set; }

    public string InsertedFrom { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime UpdatedOn { get; set; }

    public string UpdatedFrom { get; set; } = null!;

    public virtual ICollection<HrPolicy> HrPolicies { get; set; } = new List<HrPolicy>();

    public virtual ICollection<PolicyDocument> PolicyDocuments { get; set; } = new List<PolicyDocument>();
}
