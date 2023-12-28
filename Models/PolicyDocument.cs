using System;
using System.Collections.Generic;

namespace Hr_policy.Models;

public partial class PolicyDocument
{
    public IFormFile File { get; set; }
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public int TopicId { get; set; }

    public int PolicyId { get; set; }

    public string DocumentCaption { get; set; } = null!;

    public DateOnly DocumentDate { get; set; }

    public string KeywordsString { get; set; } = null!;

    public int DocumentTypeId { get; set; }

    public string Remarks { get; set; } = null!;

    public int SortOrder { get; set; }

    public string Extension { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public int FileSize { get; set; }

    public string FileName { get; set; } = null!;

    public string? FileHash { get; set; }

    public int Status { get; set; }

    public string InsertedBy { get; set; } = null!;

    public DateTime InsertedOn { get; set; }

    public string InsertedFrom { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime UpdatedOn { get; set; }

    public string UpdatedFrom { get; set; } = null!;

    public string ArchivedBy { get; set; } = null!;

    public DateTime ArchivedOn { get; set; }

    public string ArchivedFrom { get; set; } = null!;

    public string? HtmlContent { get; set; }

    public string? TextContent { get; set; }

    public string? WordFileName { get; set; }

    public string? WordFileUpdatedBy { get; set; }

    public DateTime? WordFileUpdatedOn { get; set; }

    public string? WordFileUpdatedFrom { get; set; }

    //public byte[]? FileUpload { get; set; }

    public virtual HrPolicy Policy { get; set; } = null!;

    public virtual PolicyTopic Topic { get; set; } = null!;
}
