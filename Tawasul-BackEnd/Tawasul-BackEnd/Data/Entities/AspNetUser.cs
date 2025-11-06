using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public string? DisplayName { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTimeOffset? LastSeenAt { get; set; }

    public bool IsOnline { get; set; }

    public virtual ICollection<ConversationMember> ConversationMemberInvitedByUsers { get; set; } = new List<ConversationMember>();

    public virtual ICollection<ConversationMember> ConversationMemberUsers { get; set; } = new List<ConversationMember>();

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<MessageReaction> MessageReactions { get; set; } = new List<MessageReaction>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<UserMessageStatus> UserMessageStatuses { get; set; } = new List<UserMessageStatus>();
}
