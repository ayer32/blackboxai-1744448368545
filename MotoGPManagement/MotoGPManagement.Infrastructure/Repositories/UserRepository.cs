using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotoGPManagement.Core.Interfaces;
using MotoGPManagement.Core.Models;
using MotoGPManagement.Infrastructure.Data;

namespace MotoGPManagement.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) 
            : base(context)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Rider)
                    .ThenInclude(r => r.CurrentTeam)
                .Include(u => u.TeamManager)
                    .ThenInclude(tm => tm.Team)
                .Include(u => u.Fan)
                    .ThenInclude(f => f.FantasyTeams.Where(ft => ft.Season == DateTime.UtcNow.Year))
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.GetRolesAsync(user);
            }
            return new List<string>();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet.AnyAsync(u => u.Email == email);
        }

        public override async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(u => u.Rider)
                    .ThenInclude(r => r.CurrentTeam)
                .Include(u => u.TeamManager)
                    .ThenInclude(tm => tm.Team)
                .Include(u => u.Fan)
                    .ThenInclude(f => f.FantasyTeams.Where(ft => ft.Season == DateTime.UtcNow.Year))
                .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }
    }
}
