using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class Conversation
{
    public long Id { get; set; }

    public byte Type { get; set; }

    public string? Title { get; set; }

    public string CreatedByUserId { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public string? DirectUserAId { get; set; }

    public string? DirectUserBId { get; set; }

    public DateTime? ExpiresAtUtc { get; set; }

    public byte ExpiryAction { get; set; }

    public virtual ICollection<ConversationMember> ConversationMembers { get; set; } = new List<ConversationMember>();

    public virtual AspNetUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<UserMessageStatus> UserMessageStatuses { get; set; } = new List<UserMessageStatus>();
}
