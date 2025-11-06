using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class ConversationMember
{
    public long ConversationId { get; set; }

    public string UserId { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public DateTime JoinedAtUtc { get; set; }

    public bool IsMuted { get; set; }

    public DateTime? MutedUntilUtc { get; set; }

    public string? InvitedByUserId { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual AspNetUser? InvitedByUser { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
