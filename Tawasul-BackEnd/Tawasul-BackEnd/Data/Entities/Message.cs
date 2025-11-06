using System;
using System.Collections.Generic;

namespace Tawasul_BackEnd.Data.Entities;

public partial class Message
{
    public long Id { get; set; }

    public long ConversationId { get; set; }

    public string SenderId { get; set; } = null!;

    public string? Text { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public bool IsEdited { get; set; }

    public bool IsDeleted { get; set; }

    public long? ReplyToMessageId { get; set; }

    public Guid? ClientGuid { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual ICollection<Message> InverseReplyToMessage { get; set; } = new List<Message>();

    public virtual ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();

    public virtual ICollection<MessageReaction> MessageReactions { get; set; } = new List<MessageReaction>();

    public virtual Message? ReplyToMessage { get; set; }

    public virtual AspNetUser Sender { get; set; } = null!;

    public virtual ICollection<UserMessageStatus> UserMessageStatuses { get; set; } = new List<UserMessageStatus>();
}
