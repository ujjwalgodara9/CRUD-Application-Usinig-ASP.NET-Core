using System;
using System.Collections.Generic;

namespace Hr_policy.Models;

public partial class HrPolicy
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public int TopicId { get; set; }

    public string PolicyRefNo { get; set; } = null!;

    public string PolicyName { get; set; } = null!;

    public int SortOrder { get; set; }

    public string Remarks { get; set; } = null!;

    public int Status { get; set; }

    public string InsertedBy { get; set; } = null!;

    public DateTime InsertedOn { get; set; }

    public string InsertedFrom { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime UpdatedOn { get; set; }

    public string UpdatedFrom { get; set; } = null!;

    public string? ArchivedBy { get; set; }

    public DateTime? ArchivedOn { get; set; }

    public string? ArchivedFrom { get; set; }

    public virtual ICollection<PolicyDocument> PolicyDocuments { get; set; } = new List<PolicyDocument>();

    public virtual PolicyTopic Topic { get; set; } = null!;
}
