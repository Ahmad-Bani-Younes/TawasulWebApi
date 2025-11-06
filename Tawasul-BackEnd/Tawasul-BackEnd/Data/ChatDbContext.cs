using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tawasul_BackEnd.Data.Entities;

namespace Tawasul_BackEnd.Data;

public partial class ChatDbContext : DbContext
{
    public ChatDbContext()
    {
    }

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationMember> ConversationMembers { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }

    public virtual DbSet<MessageReaction> MessageReactions { get; set; }

    public virtual DbSet<UserMessageStatus> UserMessageStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AspNetUs__3214EC0710DA97BC");

            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PhotoUrl).HasMaxLength(400);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC07B8CE2E46");

            entity.HasIndex(e => new { e.DirectUserAId, e.DirectUserBId }, "UX_Conversations_DirectPair")
                .IsUnique()
                .HasFilter("([DirectUserAId] IS NOT NULL AND [DirectUserBId] IS NOT NULL)");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasMaxLength(450);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conversations_CreatedBy");
        });

        modelBuilder.Entity<ConversationMember>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.UserId });

            entity.HasIndex(e => e.UserId, "IX_ConversationMembers_User");

            entity.Property(e => e.InvitedByUserId).HasMaxLength(450);
            entity.Property(e => e.JoinedAtUtc).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.ConversationMembers)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK_CM_Conversation");

            entity.HasOne(d => d.InvitedByUser).WithMany(p => p.ConversationMemberInvitedByUsers)
                .HasForeignKey(d => d.InvitedByUserId)
                .HasConstraintName("FK_CM_InvitedBy");

            entity.HasOne(d => d.User).WithMany(p => p.ConversationMemberUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CM_User");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC07BF5C62C1");

            entity.HasIndex(e => new { e.ConversationId, e.CreatedAtUtc }, "IX_Messages_Conversation_CreatedAt").IsDescending(false, true);

            entity.HasIndex(e => new { e.SenderId, e.ClientGuid }, "UX_Messages_Sender_ClientGuid")
                .IsUnique()
                .HasFilter("([ClientGuid] IS NOT NULL)");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK_Messages_Conversation");

            entity.HasOne(d => d.ReplyToMessage).WithMany(p => p.InverseReplyToMessage)
                .HasForeignKey(d => d.ReplyToMessageId)
                .HasConstraintName("FK_Messages_ReplyTo");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Sender");
        });

        modelBuilder.Entity<MessageAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MessageA__3214EC076B6CA4D0");

            entity.HasIndex(e => e.MessageId, "IX_Attachments_Message");

            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.FilePath).HasMaxLength(400);
            entity.Property(e => e.OriginalName).HasMaxLength(255);

            entity.HasOne(d => d.Message).WithMany(p => p.MessageAttachments)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_Attachments_Message");
        });

        modelBuilder.Entity<MessageReaction>(entity =>
        {
            entity.HasKey(e => new { e.MessageId, e.UserId, e.Emoji });

            entity.Property(e => e.Emoji).HasMaxLength(32);
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageReactions)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_MR_Message");

            entity.HasOne(d => d.User).WithMany(p => p.MessageReactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MR_User");
        });

        modelBuilder.Entity<UserMessageStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMess__3214EC076A985185");

            entity.HasIndex(e => new { e.ConversationId, e.UserId, e.MessageId }, "IX_UMS_Conv_User").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.MessageId, e.UserIdHash }, "UX_UMS_Message_User").IsUnique();

            entity.Property(e => e.UserIdHash)
                .HasMaxLength(32)
                .HasComputedColumnSql("(CONVERT([varbinary](32),hashbytes('SHA2_256',[UserId])))", true);

            entity.HasOne(d => d.Conversation).WithMany(p => p.UserMessageStatuses)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UMS_Conv");

            entity.HasOne(d => d.Message).WithMany(p => p.UserMessageStatuses)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_UMS_Message");

            entity.HasOne(d => d.User).WithMany(p => p.UserMessageStatuses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UMS_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
