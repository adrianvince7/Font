using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Font.Core.Domain.Chat;
using Font.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Font.Infrastructure.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly FontDbContext _context;

        public ChatRepository(FontDbContext context)
        {
            _context = context;
        }

        // ChatRoom operations
        public async Task<ChatRoom> GetChatRoomByIdAsync(Guid id)
        {
            return await _context.ChatRooms
                .Include(r => r.Messages)
                .Include(r => r.Participants)
                .Include(r => r.Settings)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(Guid userId)
        {
            return await _context.ChatRooms
                .Include(r => r.Messages)
                .Include(r => r.Participants)
                .Include(r => r.Settings)
                .Where(r => r.Participants.Any(p => p.UserId == userId))
                .OrderByDescending(r => r.Messages.Max(m => m.SentAt))
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatRoom>> GetPublicChatRoomsAsync()
        {
            return await _context.ChatRooms
                .Include(r => r.Participants)
                .Include(r => r.Settings)
                .Where(r => !r.IsPrivate)
                .ToListAsync();
        }

        public async Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom)
        {
            await _context.ChatRooms.AddAsync(chatRoom);
            await _context.SaveChangesAsync();
            return chatRoom;
        }

        public async Task UpdateChatRoomAsync(ChatRoom chatRoom)
        {
            _context.Entry(chatRoom).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatRoomAsync(Guid id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom != null)
            {
                _context.ChatRooms.Remove(chatRoom);
                await _context.SaveChangesAsync();
            }
        }

        // Message operations
        public async Task<ChatMessage> GetMessageByIdAsync(Guid id)
        {
            return await _context.ChatMessages
                .Include(m => m.Reactions)
                .Include(m => m.Attachments)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByChatRoomIdAsync(Guid chatRoomId, int skip = 0, int take = 50)
        {
            return await _context.ChatMessages
                .Include(m => m.Reactions)
                .Include(m => m.Attachments)
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<ChatMessage> AddMessageAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task UpdateMessageAsync(ChatMessage message)
        {
            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(Guid id)
        {
            var message = await _context.ChatMessages.FindAsync(id);
            if (message != null)
            {
                message.Delete();
                await _context.SaveChangesAsync();
            }
        }

        // Participant operations
        public async Task<ChatParticipant> GetParticipantAsync(Guid chatRoomId, Guid userId)
        {
            return await _context.ChatParticipants
                .FirstOrDefaultAsync(p => p.ChatRoomId == chatRoomId && p.UserId == userId);
        }

        public async Task<IEnumerable<ChatParticipant>> GetParticipantsByChatRoomIdAsync(Guid chatRoomId)
        {
            return await _context.ChatParticipants
                .Where(p => p.ChatRoomId == chatRoomId)
                .ToListAsync();
        }

        public async Task AddParticipantAsync(ChatParticipant participant)
        {
            await _context.ChatParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateParticipantAsync(ChatParticipant participant)
        {
            _context.Entry(participant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveParticipantAsync(Guid chatRoomId, Guid userId)
        {
            var participant = await GetParticipantAsync(chatRoomId, userId);
            if (participant != null)
            {
                _context.ChatParticipants.Remove(participant);
                await _context.SaveChangesAsync();
            }
        }

        // Reaction operations
        public async Task<IEnumerable<MessageReaction>> GetReactionsByMessageIdAsync(Guid messageId)
        {
            return await _context.MessageReactions
                .Where(r => r.MessageId == messageId)
                .ToListAsync();
        }

        public async Task AddReactionAsync(MessageReaction reaction)
        {
            await _context.MessageReactions.AddAsync(reaction);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReactionAsync(Guid messageId, Guid userId, string type)
        {
            var reaction = await _context.MessageReactions
                .FirstOrDefaultAsync(r => r.MessageId == messageId && r.UserId == userId && r.Type == type);

            if (reaction != null)
            {
                _context.MessageReactions.Remove(reaction);
                await _context.SaveChangesAsync();
            }
        }

        // Attachment operations
        public async Task<MessageAttachment> GetAttachmentByIdAsync(Guid id)
        {
            return await _context.MessageAttachments.FindAsync(id);
        }

        public async Task<IEnumerable<MessageAttachment>> GetAttachmentsByMessageIdAsync(Guid messageId)
        {
            return await _context.MessageAttachments
                .Where(a => a.MessageId == messageId)
                .ToListAsync();
        }

        public async Task AddAttachmentAsync(MessageAttachment attachment)
        {
            await _context.MessageAttachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttachmentAsync(Guid id)
        {
            var attachment = await _context.MessageAttachments.FindAsync(id);
            if (attachment != null)
            {
                _context.MessageAttachments.Remove(attachment);
                await _context.SaveChangesAsync();
            }
        }

        // Settings operations
        public async Task<ChatRoomSettings> GetChatRoomSettingsAsync(Guid chatRoomId)
        {
            return await _context.ChatRoomSettings
                .FirstOrDefaultAsync(s => s.ChatRoomId == chatRoomId);
        }

        public async Task UpdateChatRoomSettingsAsync(ChatRoomSettings settings)
        {
            _context.Entry(settings).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Search operations
        public async Task<IEnumerable<ChatMessage>> SearchMessagesAsync(Guid chatRoomId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Array.Empty<ChatMessage>();

            searchTerm = searchTerm.ToLower();
            return await _context.ChatMessages
                .Include(m => m.Reactions)
                .Include(m => m.Attachments)
                .Where(m => m.ChatRoomId == chatRoomId &&
                           m.Content.ToLower().Contains(searchTerm))
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatRoom>> SearchChatRoomsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Array.Empty<ChatRoom>();

            searchTerm = searchTerm.ToLower();
            return await _context.ChatRooms
                .Include(r => r.Participants)
                .Include(r => r.Settings)
                .Where(r => !r.IsPrivate &&
                           (r.Name.ToLower().Contains(searchTerm) ||
                            r.Description.ToLower().Contains(searchTerm)))
                .ToListAsync();
        }

        // Statistics
        public async Task<int> GetUnreadMessageCountAsync(Guid chatRoomId, Guid userId)
        {
            var participant = await GetParticipantAsync(chatRoomId, userId);
            if (participant?.LastReadAt == null)
                return 0;

            return await _context.ChatMessages
                .CountAsync(m => m.ChatRoomId == chatRoomId &&
                                m.SentAt > participant.LastReadAt);
        }

        public async Task<DateTime?> GetLastActivityAsync(Guid chatRoomId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatRoomId == chatRoomId)
                .MaxAsync(m => (DateTime?)m.SentAt);
        }

        public async Task<int> GetParticipantCountAsync(Guid chatRoomId)
        {
            return await _context.ChatParticipants
                .CountAsync(p => p.ChatRoomId == chatRoomId);
        }
    }
} 