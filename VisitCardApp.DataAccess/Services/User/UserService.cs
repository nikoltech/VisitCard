namespace VisitCardApp.DataAccess.Services.User
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;

    public class UserService
    {
        private readonly UserManager<AppUser> UserManager;
        private readonly IMemoryCache cache;
        public UserService(UserManager<AppUser> userManager, IMemoryCache memoryCache)
        {
            this.UserManager = userManager;
            this.cache = memoryCache;
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await this.UserManager.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserAsync(string id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            AppUser user = null;
            if (!cache.TryGetValue(id, out user))
            {
                user = await this.UserManager.Users.FirstOrDefaultAsync(p => p.Id.Equals(id));
                this.SetUserToCache(user);
            }

            return user;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            email = email ?? throw new ArgumentNullException(nameof(email));

            string id = null;
            AppUser user = null;
            if (cache.TryGetValue(email, out id))
            {
                cache.TryGetValue(id, out user);
            }

            if (user == null)
            {
                user = await this.UserManager.Users.FirstOrDefaultAsync(p => p.Email.Equals(email));
                this.SetUserToCache(user);
            }

            return user;
        }

        #region private methods
        private void SetUserToCache(AppUser user)
        {
            if (user != null)
            {
                // Set user to memory cache
                cache.Set(user.Id, user,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                // Save userId to memory cache by Email key
                cache.Set(user.Email, user.Id,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
        }
        #endregion
    }
}
