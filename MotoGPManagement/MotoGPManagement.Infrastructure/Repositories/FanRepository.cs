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
    public class FanRepository : BaseRepository<Fan>, IFanRepository
    {
        public FanRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Fan> GetFanWithPredictionsAsync(Guid fanId)
        {
            return await _dbSet
                .Include(f => f.Predictions.Where(p => p.Race.StartTime >= DateTime.UtcNow.AddDays(-7)))
                    .ThenInclude(p => p.Race)
                .Include(f => f.Predictions.Where(p => p.Race.StartTime >= DateTime.UtcNow.AddDays(-7)))
                    .ThenInclude(p => p.PredictedWinner)
                        .ThenInclude(r => r.CurrentTeam)
                .Include(f => f.FantasyTeams.Where(ft => ft.Season == DateTime.UtcNow.Year))
                    .ThenInclude(ft => ft.SelectedRiders)
                        .ThenInclude(sr => sr.Rider)
                .Include(f => f.Comments.Where(c => c.PostedAt >= DateTime.UtcNow.AddDays(-30)))
                    .ThenInclude(c => c.Race)
                .FirstOrDefaultAsync(f => f.Id == fanId);
        }

        public async Task<IEnumerable<RacePrediction>> GetFanPredictionsAsync(Guid fanId)
        {
            return await _context.RacePredictions
                .Where(rp => rp.FanId == fanId)
                .Include(rp => rp.Race)
                .Include(rp => rp.PredictedWinner)
                    .ThenInclude(r => r.CurrentTeam)
                .OrderByDescending(rp => rp.Race.StartTime)
                .ToListAsync();
        }

        public async Task<FantasyTeam> GetFanFantasyTeamAsync(Guid fanId, int season)
        {
            return await _context.FantasyTeams
                .Include(ft => ft.SelectedRiders)
                    .ThenInclude(sr => sr.Rider)
                        .ThenInclude(r => r.CurrentTeam)
                .Include(ft => ft.SelectedRiders)
                    .ThenInclude(sr => sr.Rider)
                        .ThenInclude(r => r.PerformanceStats.Where(ps => ps.Season == season))
                .FirstOrDefaultAsync(ft => ft.FanId == fanId && ft.Season == season);
        }

        public override async Task<Fan> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(f => f.FantasyTeams.Where(ft => ft.Season == DateTime.UtcNow.Year))
                .Include(f => f.Predictions.Where(p => p.Race.StartTime >= DateTime.UtcNow.AddDays(-7)))
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public override async Task<IEnumerable<Fan>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.FantasyTeams.Where(ft => ft.Season == DateTime.UtcNow.Year))
                .OrderByDescending(f => f.ReputationPoints)
                .ToListAsync();
        }
    }
}
