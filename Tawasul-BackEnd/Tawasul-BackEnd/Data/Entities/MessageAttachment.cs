using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class MessageAttachment
{
    public long Id { get; set; }

    public long MessageId { get; set; }

    public string FilePath { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long SizeBytes { get; set; }

    public string? OriginalName { get; set; }

    public virtual Message Message { get; set; } = null!;
}
