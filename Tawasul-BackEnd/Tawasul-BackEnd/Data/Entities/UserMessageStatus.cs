using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class UserMessageStatus
{
    public long Id { get; set; }

    public long MessageId { get; set; }

    public string UserId { get; set; } = null!;

    public long ConversationId { get; set; }

    public bool HasSeen { get; set; }

    public DateTime? SeenAtUtc { get; set; }

    public byte[]? UserIdHash { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
