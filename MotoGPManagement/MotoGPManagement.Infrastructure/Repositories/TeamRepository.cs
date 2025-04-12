using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoGPManagement.Core.Interfaces;
using MotoGPManagement.Core.Models;
using MotoGPManagement.Infrastructure.Data;

namespace MotoGPManagement.Infrastructure.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Team> GetTeamWithRidersAsync(Guid teamId)
        {
            return await _dbSet
                .Include(t => t.Riders)
                .Include(t => t.Sponsors)
                .Include(t => t.PerformanceHistory.Where(ph => ph.Season == DateTime.UtcNow.Year))
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<IEnumerable<Team>> GetTeamsBySeasonAsync(int season)
        {
            return await _dbSet
                .Include(t => t.Riders)
                .Include(t => t.PerformanceHistory.Where(ph => ph.Season == season))
                .Include(t => t.Sponsors.Where(s => s.SponsorshipStart.Year <= season && 
                    (!s.SponsorshipEnd.HasValue || s.SponsorshipEnd.Value.Year >= season)))
                .OrderByDescending(t => t.PerformanceHistory
                    .Where(ph => ph.Season == season)
                    .Select(ph => ph.TotalPoints)
                    .FirstOrDefault())
                .ToListAsync();
        }

        public async Task<TeamPerformanceStats> GetTeamStatsAsync(Guid teamId, int season)
        {
            return await _context.TeamPerformanceStats
                .FirstOrDefaultAsync(tps => tps.TeamId == teamId && tps.Season == season);
        }

        public override async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Riders)
                .Include(t => t.Sponsors.Where(s => !s.SponsorshipEnd.HasValue || s.SponsorshipEnd > DateTime.UtcNow))
                .Include(t => t.PerformanceHistory.Where(ph => ph.Season == DateTime.UtcNow.Year))
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}
