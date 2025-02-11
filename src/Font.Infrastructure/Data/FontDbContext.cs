using Font.Core.Domain.User;
using Font.Core.Domain.Meeting;
using Font.Core.Domain.Chat;
using Microsoft.EntityFrameworkCore;

namespace Font.Infrastructure.Data
{
    public class FontDbContext : DbContext
    {
        public FontDbContext(DbContextOptions<FontDbContext> options) : base(options)
        {
        }

        // User Domain
        public DbSet<User> Users { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<SobrietyProgress> SobrietyProgresses { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }

        // Meeting Domain
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        public DbSet<RecurrencePattern> RecurrencePatterns { get; set; }

        // Chat Domain
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }
        public DbSet<MessageReaction> MessageReactions { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<ChatRoomSettings> ChatRoomSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Domain Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();

                entity.HasMany(e => e.EmergencyContacts)
                    .WithOne()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.SobrietyProgress)
                    .WithOne()
                    .HasForeignKey<SobrietyProgress>("UserId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Preferences)
                    .WithOne()
                    .HasForeignKey<UserPreferences>("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EmergencyContact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Relationship).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<SobrietyProgress>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Achievements)
                    .WithOne()
                    .HasForeignKey("SobrietyProgressId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.JournalEntries)
                    .WithOne()
                    .HasForeignKey("SobrietyProgressId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Meeting Domain Configuration
            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Tags).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

                entity.HasMany(e => e.Participants)
                    .WithOne()
                    .HasForeignKey("MeetingId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.RecurrencePattern)
                    .WithOne()
                    .HasForeignKey<RecurrencePattern>("MeetingId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Chat Domain Configuration
            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasMany(e => e.Messages)
                    .WithOne()
                    .HasForeignKey(m => m.ChatRoomId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Participants)
                    .WithOne()
                    .HasForeignKey(p => p.ChatRoomId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Settings)
                    .WithOne()
                    .HasForeignKey<ChatRoomSettings>(s => s.ChatRoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasMany(e => e.Reactions)
                    .WithOne()
                    .HasForeignKey(r => r.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Attachments)
                    .WithOne()
                    .HasForeignKey(a => a.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChatRoomSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BannedWords).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
                entity.Property(e => e.AllowedAttachmentTypes).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
            });
        }
    }
} 