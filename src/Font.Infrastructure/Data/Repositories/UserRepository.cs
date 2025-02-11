using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Font.Core.Domain.User;
using Font.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Font.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FontDbContext _context;

        public UserRepository(FontDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.EmergencyContacts)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.Achievements)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.JournalEntries)
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.EmergencyContacts)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.Achievements)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.JournalEntries)
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.EmergencyContacts)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.Achievements)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.JournalEntries)
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.EmergencyContacts)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.Achievements)
                .Include(u => u.SobrietyProgress)
                    .ThenInclude(sp => sp.JournalEntries)
                .Include(u => u.Preferences)
                .ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
} 