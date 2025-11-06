using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class MessageReaction
{
    public long MessageId { get; set; }

    public string UserId { get; set; } = null!;

    public string Emoji { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public virtual Message Message { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
